using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using UnityEditor;

public class LockOn : MonoBehaviour
{
    [SerializeField] float lockOnRadius = 12;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float minViewAngle = 0f;
    [SerializeField] float maxViewAngle = 360f;
    
    [SerializeField] Transform lockOnImage;

    [SerializeField] CinemachineFreeLook playerCam; // 기본 플레이어Cam
    [SerializeField] CinemachineFreeLook enemyCam; // 타겟 바라보게 enemyCam연결

    ILockOnTarget currentTarget;
    Vector3 currentTargetPosition;

    public bool isFindTarget = false;
    private Transform cameraTransform;

    void Awake()
    {
        cameraTransform = Camera.main.transform;
        enemyCam.GetRig(0).m_LookAt = this.transform;
        enemyCam.GetRig(1).m_LookAt = this.transform;
        enemyCam.GetRig(2).m_LookAt = this.transform;

        enemyCam.m_LookAt = this.transform;
        enemyCam.m_Follow = this.transform;

        enemyCam.Priority = 0;
    }

    private void Start()
    {
        lockOnImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isFindTarget && currentTarget != null)
        {
            if (isTargetRange())
            {
                LookAtTarget();

                // 플레이어 입력에 따라 enemyCam의 카메라가 회전하도록 설정
                float horizontal = Input.GetAxis("Mouse X");
                float vertical = Input.GetAxis("Mouse Y");

                enemyCam.m_XAxis.Value += horizontal;
                enemyCam.m_YAxis.Value += vertical;
            }
            else
            {
                ResetTarget();
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, lockOnRadius);
    //}

    //LockOn 버튼을 누를 때 한번만 호출되는 함수
    public void TryLockOnTarget()
    {
        if (!isFindTarget)
        {
            FindLockOnTarget();
        }
        else
        {
            ResetTarget();
        }
    }

    public void FindLockOnTarget()
    {
        Collider[] findTarget = Physics.OverlapSphere(transform.position, lockOnRadius, targetLayer);
        List<ILockOnTarget> potentialTargets = new List<ILockOnTarget>();

        foreach (Collider collider in findTarget)
        {
            ILockOnTarget target = collider.GetComponent<ILockOnTarget>();

            if (target != null)
            {
                Vector3 targetDir = target.GetTransform().position - transform.position;
                float viewAngle = Vector3.Angle(targetDir, cameraTransform.forward);

                if (viewAngle > minViewAngle && viewAngle < maxViewAngle)
                {
                    RaycastHit hit;

                    if (Physics.Linecast(transform.position,
                        target.GetTransform().position, out hit, targetLayer))
                    {
                        potentialTargets.Add(target);
                    }
                }
            }
        }

        if (potentialTargets.Count > 0)
        {
            LockOnTarget(potentialTargets);
        }
    }

    private void LockOnTarget(List<ILockOnTarget> potentialTargets)
    {
        float shortDistance = Mathf.Infinity;


        foreach(var target in potentialTargets)
        {
            float distanceFromTarget = Vector3.Distance(transform.position, target.GetTransform().position);

            if(distanceFromTarget < shortDistance)
            {
                Debug.Log("LockOnTarget for문");
                shortDistance = distanceFromTarget;
                currentTarget = target;
            }
        }


        if(currentTarget != null)
        {
            Debug.Log("enemy캠 조절, FindTarget()으로 이동");
            currentTargetPosition = currentTarget.GetTransform().position;            

            enemyCam.m_LookAt = currentTarget.GetTransform();

            // enemyCam의 위치와 회전을 playerCam과 동일하게 설정
            SynchronizeCameras();

            enemyCam.Priority = 11;

            FindTarget();

            // 타겟 사망시 이미지 끄게 이벤트 구독
            currentTarget.Ondeath += HandleTargetDeath;
        }
    }

    private void LookAtTarget()
    {
        if(currentTarget == null)
        {            
            return;
        }

        currentTargetPosition = currentTarget.GetTransform().position;

        // 락온 이미지 높이
        float heightOffset = currentTarget.IsBoss ? 4.5f : 1f;

        lockOnImage.position = Camera.main.WorldToScreenPoint(currentTargetPosition + new Vector3(0, heightOffset, 0)); ;

        Vector3 dir = (currentTargetPosition - transform.position).normalized;
        dir.y = transform.position.y;

        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * 20f);
    }

    private void FindTarget()
    {
        isFindTarget = true;
        lockOnImage.gameObject.SetActive(true);
    }

    private bool isTargetRange()
    {
        float distance = (transform.position - currentTargetPosition).magnitude;

        if(distance > lockOnRadius)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void HandleTargetDeath()
    {
        // 타겟 조준 중 타겟사망시.
        ResetTarget();
    }

    public void ResetTarget()
    {
        Debug.Log("ResetTarget()합니다.");

        if(currentTarget != null )
        {
            currentTarget.Ondeath -= HandleTargetDeath;
        }

        isFindTarget = false;
        currentTarget = null;
        lockOnImage.gameObject.SetActive(false);

        enemyCam.Priority = 0;

        // enemyCam의 LookAt을 플레이어로 재설정
        enemyCam.m_LookAt = this.transform;

    }

    private void SynchronizeCameras()
    {
        // enemyCam의 위치와 회전을 playerCam과 동일하게 설정
        enemyCam.transform.position = playerCam.transform.position;
        enemyCam.transform.rotation = playerCam.transform.rotation;

        // 필요에 따라 추가적으로 카메라의 상태를 동기화
        enemyCam.m_XAxis.Value = playerCam.m_XAxis.Value;
        enemyCam.m_YAxis.Value = playerCam.m_YAxis.Value;
    }

}

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

    //[SerializeField] List<EnemyAttributesManager> targetEnemy = new List<EnemyAttributesManager>();
    [SerializeField] Transform lockOnImage;

    [SerializeField] CinemachineFreeLook playerCam; // 기본 플레이어Cam
    [SerializeField] CinemachineFreeLook enemyCam; // 타겟 바라보게 enemyCam연결

    EnemyAttributesManager currentTarget;
    Vector3 currentTargetPosition;

    public bool isFindTarget = false;
    private Transform cameraTransform;
    // Start is called before the first frame update
    void Awake()
    {
        cameraTransform = Camera.main.transform;
        enemyCam.GetRig(0).m_LookAt = this.transform;
        enemyCam.GetRig(1).m_LookAt = this.transform;
        enemyCam.GetRig(2).m_LookAt = this.transform;
    }

    private void Start()
    {
        lockOnImage.gameObject.SetActive(false);
        enemyCam.Priority = 0;
    }
    //여기서부터
    private void Update()
    {
        if (isFindTarget && currentTarget != null)
        {
            if (isTargetRange())
            {
                Debug.Log("Update문 if 트루");
                LookAtTarget();

                // 플레이어 입력에 따라 enemyCam의 카메라가 회전하도록 설정
                float horizontal = Input.GetAxis("Mouse X");
                float vertical = Input.GetAxis("Mouse Y");

                enemyCam.m_XAxis.Value += horizontal;
                enemyCam.m_YAxis.Value += vertical;
            }
            else
            {
                Debug.Log("Update문 if FALSE");
                ResetTarget();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, lockOnRadius);
    }

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
        List<EnemyAttributesManager> potentialTargets = new List<EnemyAttributesManager>();

        foreach (Collider collider in findTarget)
        {
            EnemyAttributesManager target = collider.GetComponent<EnemyAttributesManager>();

            if (target != null)
            {
                Vector3 targetDir = target.transform.position - transform.position;

                float viewAngle = Vector3.Angle(targetDir, cameraTransform.forward);

                if (viewAngle > minViewAngle && viewAngle < maxViewAngle)
                {
                    RaycastHit hit;

                    if (Physics.Linecast(transform.position,
                        target.transform.position, out hit, targetLayer))
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

    private void LockOnTarget(List<EnemyAttributesManager> potentialTargets)
    {
        float shortDistance = Mathf.Infinity;


        foreach(var target in potentialTargets)
        {
            float distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);

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
            currentTargetPosition = currentTarget.transform.position;
            enemyCam.Priority = 11;
            enemyCam.m_LookAt = currentTarget.transform;            
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

        currentTargetPosition = currentTarget.transform.position;
        lockOnImage.position = Camera.main.WorldToScreenPoint(currentTargetPosition + new Vector3(0, 1f, 0)); ;

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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LockOn : MonoBehaviour
{
    [SerializeField] float lockOnRadius = 12;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float minViewAngle = 0f;
    [SerializeField] float maxViewAngle = 360f;

    [SerializeField] List<EnemyAttributesManager> targetEnemy = new List<EnemyAttributesManager>();
    [SerializeField] Transform lockOnImage;

    [SerializeField] CinemachineFreeLook playerCam; // 기본 플레이어Cam
    [SerializeField] CinemachineFreeLook enemyCam; // 타겟 바라보게 enemyCam연결

    EnemyAttributesManager currentTarget;
    Vector3 currentTargetPosition;

    public bool isFindTarget = false;
    private bool isLockOn = false;
    private Transform cameraTransform;
    // Start is called before the first frame update
    void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        lockOnImage.gameObject.SetActive(false);
        enemyCam.Priority = 0;
    }

    private void Update()
    {
        if (isFindTarget)
        {
            if (isTargetRange())
            {
                LookAtTarget();
            }
            else
            {
                ResetTarget();
            }
        }
    }

    public void FindLockOnTarget()
    {
        Collider[] findTarget = Physics.OverlapSphere(transform.position, lockOnRadius, targetLayer);

        if (findTarget.Length <= 0)
        {
            return;
        }

        for (int i = 0; i < findTarget.Length; i++)
        {
            EnemyAttributesManager target = findTarget[i].GetComponent<EnemyAttributesManager>();

            if (target != null)
            {
                Vector3 targetDir = target.transform.position - transform.position;

                float viewAngle = Vector3.Angle(targetDir, cameraTransform.forward);

                if (viewAngle > minViewAngle && viewAngle < maxViewAngle)
                {
                    RaycastHit hit;
                    
                    if(Physics.Linecast(transform.position,
                        target.transform.position,out hit, targetLayer))
                    {
                        targetEnemy.Add(target);
                    }
                }
                else
                {
                    ResetTarget();
                }
            }
        }

        LockOnTarget();
    }

    private void LockOnTarget()
    {
        float shortDistance = Mathf.Infinity;

        if(targetEnemy != null)
        {
            for (int i = 0; i < targetEnemy.Count; i++)
            {
                float distanceFromTarget = Vector3.Distance(transform.position, targetEnemy[i].transform.position);

                if(distanceFromTarget < shortDistance)
                {
                    shortDistance = distanceFromTarget;
                    currentTarget = targetEnemy[i];
                }
            }

            enemyCam.Priority = 11;
            enemyCam.m_LookAt = targetEnemy[0].transform;

        }

        if(currentTarget != null)
        {
            FindTarget();
        }

    }

    private void LookAtTarget()
    {
        if(currentTarget == null)
        {
            ResetTarget();
            return;
        }

        currentTargetPosition = currentTarget.transform.position;

        lockOnImage.position = Camera.main.WorldToScreenPoint(currentTargetPosition);

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

    public void ResetTarget()
    {
        isFindTarget = false;
        targetEnemy.Clear();
        lockOnImage.gameObject.SetActive(false);
        enemyCam.Priority = 0;
    }
}

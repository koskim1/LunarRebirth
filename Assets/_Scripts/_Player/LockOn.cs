using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    [SerializeField] float lockOnRadius = 12;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float minViewAngle = 0f;
    [SerializeField] float maxViewAngle = 360f;

    [SerializeField] List<EnemyAttributesManager> targetEnemy = new List<EnemyAttributesManager>();

    [SerializeField] Transform lockOnImage;

    EnemyAttributesManager currentTarget;
    Vector3 currentTargetPosition;

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
    }

    private void Update()
    {
        if (isLockOn)
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
    }

    private void FindTarget()
    {
        isLockOn = true;

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

    private void ResetTarget()
    {
        isLockOn = false;
        targetEnemy.Clear();
        lockOnImage.gameObject.SetActive(false);
    }
}

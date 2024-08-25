using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    [SerializeField] float lockOnRadius = 6;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float minViewAngle = -70f;
    [SerializeField] float maxViewAngle = 70f;

    [SerializeField] List<EnemyAttributesManager> targetEnemy = new List<EnemyAttributesManager>();


    private bool isLockOn;
    private Transform cameraTransform;
    // Start is called before the first frame update
    void Awake()
    {
        cameraTransform = Camera.main.transform;
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
                // target null일때 없애야하는거 아닌가?
                else
                {
                    ResetTarget();
                }

            }
        }
    }

    private void ResetTarget()
    {
        targetEnemy.Clear();
    }
}

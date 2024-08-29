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

    [SerializeField] CinemachineFreeLook playerCam; // �⺻ �÷��̾�Cam
    [SerializeField] CinemachineFreeLook enemyCam; // Ÿ�� �ٶ󺸰� enemyCam����

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
    //���⼭����
    private void Update()
    {
        if (isFindTarget && currentTarget != null)
        {
            if (isTargetRange())
            {
                Debug.Log("Update�� if Ʈ��");
                LookAtTarget();

                // �÷��̾� �Է¿� ���� enemyCam�� ī�޶� ȸ���ϵ��� ����
                float horizontal = Input.GetAxis("Mouse X");
                float vertical = Input.GetAxis("Mouse Y");

                enemyCam.m_XAxis.Value += horizontal;
                enemyCam.m_YAxis.Value += vertical;
            }
            else
            {
                Debug.Log("Update�� if FALSE");
                ResetTarget();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, lockOnRadius);
    }

    //LockOn ��ư�� ���� �� �ѹ��� ȣ��Ǵ� �Լ�
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
                Debug.Log("LockOnTarget for��");
                shortDistance = distanceFromTarget;
                currentTarget = target;
            }
        }


        if(currentTarget != null)
        {
            Debug.Log("enemyķ ����, FindTarget()���� �̵�");
            currentTargetPosition = currentTarget.transform.position;
            enemyCam.Priority = 11;
            enemyCam.m_LookAt = currentTarget.transform;            
            FindTarget();

            // Ÿ�� ����� �̹��� ���� �̺�Ʈ ����
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
        // Ÿ�� ���� �� Ÿ�ٻ����.
        ResetTarget();
    }

    public void ResetTarget()
    {
        Debug.Log("ResetTarget()�մϴ�.");

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

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 20;
    private Transform target;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }        

        // ��ǥ�� ���� �̵�.
        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        // ��ǥ�� ���� ������ �ı�.
        AttributesManager targetAttributes = target.GetComponent<EnemyAttributesManager>();
        if(targetAttributes != null)
        {
            targetAttributes.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}

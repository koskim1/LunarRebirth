using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private FireballController _fireballController;
    private void Start()
    {
        _fireballController = GetComponent<FireballController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyAttributesManager target = collision.gameObject.GetComponent<EnemyAttributesManager>();
        if (target != null && tag == "Enemy")
        {
            target.TakeDamage(35);
        }
    }
}

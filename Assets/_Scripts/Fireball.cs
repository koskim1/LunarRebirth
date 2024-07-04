using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject impactVFX;

    private PlayerAttributesManager playerAttributesManager;

    private void Start()
    {
        playerAttributesManager = GetComponent<PlayerAttributesManager>();
    }

    private void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.collider.tag == "Enemy")
        {            
            EnemyAttributesManager enemy = collisionInfo.collider.GetComponent<EnemyAttributesManager>();
            if (enemy != null)
            {
                var impact = Instantiate(impactVFX, collisionInfo.contacts[0].point, Quaternion.identity) as GameObject;

                Destroy(impact, 2f);

                enemy.TakeDamage(35);
                Destroy(gameObject);
            }
        }
        else
        {
            var impact = Instantiate(impactVFX, collisionInfo.contacts[0].point, Quaternion.identity) as GameObject;

            Destroy(impact, 2f);
            Destroy(gameObject);
        }
    }
}

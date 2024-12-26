using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireball : MonoBehaviour
{
    public GameObject impactVFX;
    private void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.CompareTag("Player"))
        {
            PlayerAttributesManager player = collisionInfo.collider.GetComponent<PlayerAttributesManager>();
            if (player != null)
            {
                var impact = Instantiate(impactVFX, collisionInfo.contacts[0].point, Quaternion.identity) as GameObject;
                Destroy(impact, 2f);
                PlayerAttributesManager.Instance.TakeDamage(45);
                AudioManager.Instance.PlaySoundFXClip(AudioManager.Instance.fireballExplosionSFX, transform, .5f);
                Destroy(gameObject);
            }
        }
        else
        {
            var impact = Instantiate(impactVFX, collisionInfo.contacts[0].point, Quaternion.identity) as GameObject;
            AudioManager.Instance.PlaySoundFXClip(AudioManager.Instance.fireballExplosionSFX, transform, .5f);
            Destroy(impact, 2f);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject impactVFX;

    // �÷��̾ �ڽ��� �� ���̾�� ������ ���԰� �����ؾ���
    // �÷��̾� Fireball�̶�, EnemyFireball�� ������ �����ϱ����.
    private void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.collider.CompareTag("Enemy"))
        {            
            EnemyAttributesManager enemy = collisionInfo.collider.GetComponent<EnemyAttributesManager>();
            Boss boss = collisionInfo.collider.GetComponent<Boss>();
            if (enemy != null)
            {
                var impact = Instantiate(impactVFX, collisionInfo.contacts[0].point, Quaternion.identity) as GameObject;
                AudioManager.Instance.PlaySoundFXClip(AudioManager.Instance.fireballExplosionSFX, transform, .5f);
                Destroy(impact, 2f);

                enemy.TakeDamage(55);
                Destroy(gameObject);
            }
            else
            {
                var impact = Instantiate(impactVFX, collisionInfo.contacts[0].point, Quaternion.identity) as GameObject;
                AudioManager.Instance.PlaySoundFXClip(AudioManager.Instance.fireballExplosionSFX, transform, .5f);
                Destroy(impact, 2f);

                boss.TakeDamage(55);
                Destroy(gameObject);
            }
        }
        else if (collisionInfo.collider.CompareTag("Player")){
            PlayerAttributesManager player = collisionInfo.collider.GetComponent<PlayerAttributesManager>();
            if (player != null)
            {
                var impact = Instantiate(impactVFX, collisionInfo.contacts[0].point, Quaternion.identity) as GameObject;
                AudioManager.Instance.PlaySoundFXClip(AudioManager.Instance.fireballExplosionSFX, transform, .5f);
                Destroy(impact, 2f);
                PlayerAttributesManager.Instance.TakeDamage(40);
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

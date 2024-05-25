using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private AttributesManager _attributesManager;
    private float _hitCooldown = 0.53f;
    private Dictionary<GameObject, float> _hitTimes = new Dictionary<GameObject, float>();

    private void Start()
    {
        // �θ� ��ü�� �ʿ��� ��ü���� AttributesManager�� �����ɴϴ�.
        _attributesManager = GetComponentInParent<AttributesManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_attributesManager == null) return;

        // �浹�� ��ü�� AttributesManager�� �����ɴϴ�.
        var targetAttributesManager = other.GetComponent<AttributesManager>();
        if (targetAttributesManager != null)
        {          
            if(_hitTimes.TryGetValue(other.gameObject, out var lastHitTime))
            {
                // ��ٿ� �ð� ���� ���浹�ϸ� ����
                // �ִϸ��̼��� ���� �ֵθ��� �ٽ� �ö�ö� ������ 2���Ǵ� ���� fix
                if(Time.time - lastHitTime < _hitCooldown)
                {
                    return; // Cooldown �ð� ���� ���浹�ϸ� ����
                }
            }
            // ��ųʸ��� ��ü �߰� �� �ð� ������Ʈ.
            _attributesManager.DealDamage(other.gameObject);
            _hitTimes[other.gameObject] = Time.time;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �浹�� ������ ��� ��ü�� ��ųʸ����� ����.
        _hitTimes.Remove(other.gameObject);
    }
}

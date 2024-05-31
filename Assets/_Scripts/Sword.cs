using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private AttributesManager _attributesManager;
    private float _hitCooldown = 0.15f;
    private Dictionary<GameObject, float> _hitTimes = new Dictionary<GameObject, float>();
    private PlayerAnimation _playerAnimation;
    private Animator _animator;
    private BoxCollider _boxCollider;

    public bool _canDealDamage = false;

    

    private void Start()
    {
        // �θ� ��ü�� �ʿ��� ��ü���� AttributesManager�� �����ɴϴ�.
        _attributesManager = GetComponentInParent<AttributesManager>();
        //_playerAnimation = GetComponent<PlayerAnimation>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        
    }

    // ���� ������ ��ü�� isTrigger �����ִ��� �׻� Ȯ��~~
    private void OnTriggerEnter(Collider other)
    {
        if(_attributesManager == null || !_canDealDamage) return;

        // �浹�� ��ü�� AttributesManager�� �����ɴϴ�.
        var targetAttributesManager = other.GetComponent<AttributesManager>();
        if (targetAttributesManager != null)
        {
            if (_hitTimes.TryGetValue(other.gameObject, out var lastHitTime))
            {
                // ��ٿ� �ð� ���� ���浹�ϸ� ����
                // �ִϸ��̼��� ���� �ֵθ��� �ٽ� �ö�ö� ������ 2���Ǵ� ���� fix
                if (Time.time - lastHitTime < _hitCooldown)
                {
                    Debug.Log("Cooldown in effect. Skipping damage.");
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

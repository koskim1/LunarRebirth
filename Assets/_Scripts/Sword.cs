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
        // 부모 객체나 필요한 객체에서 AttributesManager를 가져옵니다.
        _attributesManager = GetComponentInParent<AttributesManager>();
        //_playerAnimation = GetComponent<PlayerAnimation>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        
    }

    // 무기 프리펩 교체시 isTrigger 켜져있는지 항상 확인~~
    private void OnTriggerEnter(Collider other)
    {
        if(_attributesManager == null || !_canDealDamage) return;

        // 충돌한 객체의 AttributesManager를 가져옵니다.
        var targetAttributesManager = other.GetComponent<AttributesManager>();
        if (targetAttributesManager != null)
        {
            if (_hitTimes.TryGetValue(other.gameObject, out var lastHitTime))
            {
                // 쿨다운 시간 내에 재충돌하면 무시
                // 애니메이션이 검을 휘두르고 다시 올라올때 공격이 2번되던 현상 fix
                if (Time.time - lastHitTime < _hitCooldown)
                {
                    Debug.Log("Cooldown in effect. Skipping damage.");
                    return; // Cooldown 시간 내에 재충돌하면 무시
                }
            }
            // 딕셔너리에 객체 추가 및 시간 업데이트.
            _attributesManager.DealDamage(other.gameObject);
            _hitTimes[other.gameObject] = Time.time;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 충돌이 끝나면 대상 객체를 딕셔너리에서 삭제.
        _hitTimes.Remove(other.gameObject);
    }
}

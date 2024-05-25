using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private AttributesManager _attributesManager;

    private void Start()
    {
        // 부모 객체나 필요한 객체에서 AttributesManager를 가져옵니다.
        _attributesManager = GetComponentInParent<AttributesManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 객체의 AttributesManager를 가져옵니다.
        var targetAttributesManager = other.GetComponent<AttributesManager>();
        if (targetAttributesManager != null)
        {
            // 대상에게 데미지를 입힙니다.
            _attributesManager.DealDamage(other.gameObject);
        }
    }
}

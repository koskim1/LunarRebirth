using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private AttributesManager _attributesManager;

    private void Start()
    {
        // �θ� ��ü�� �ʿ��� ��ü���� AttributesManager�� �����ɴϴ�.
        _attributesManager = GetComponentInParent<AttributesManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ��ü�� AttributesManager�� �����ɴϴ�.
        var targetAttributesManager = other.GetComponent<AttributesManager>();
        if (targetAttributesManager != null)
        {
            // ��󿡰� �������� �����ϴ�.
            _attributesManager.DealDamage(other.gameObject);
        }
    }
}

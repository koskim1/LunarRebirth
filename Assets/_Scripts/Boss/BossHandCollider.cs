using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandCollider : MonoBehaviour
{
    private BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 페이즈 별로 데미지 입히게 수정
            PlayerAttributesManager.Instance.TakeDamage(1);
        }
        
    }
}

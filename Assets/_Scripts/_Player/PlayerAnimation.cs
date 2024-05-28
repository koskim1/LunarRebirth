using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private Sword _sword;
    private BoxCollider _swordCollider;

    private bool isAttack = false;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        _sword = GetComponentInChildren<Sword>();
        _swordCollider = _sword.GetComponent<BoxCollider>();

        _swordCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (!isAttack)
        {
            Debug.Log("Attack ½ÇÇà");
            isAttack = true;
            _sword._canDealDamage = true;
            animator.SetBool("isAttack", true);
        }
    }


    public void EnableDamage()
    {
        _sword._canDealDamage = true;
        _swordCollider.enabled = true;
    }

    public void DisableDamage()
    {
        _sword._canDealDamage = false;
        isAttack = false;
        animator.SetBool("isAttack", false);
        _swordCollider.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private bool isAttack = false;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
            Debug.Log("Attack 실행");
            isAttack = true;
            animator.SetBool("isAttack", true);

            // 공격이 끝나면 isAttack를 false로 설정
            Invoke("ResetAttack", 0.1f); // 여기서 0.1f는 공격 애니메이션 길이에 따라 조절 가능
        }
    }

    private void ResetAttack()
    {
        Debug.Log("ResetAttack 실행");
        isAttack = false;
        animator.SetBool("isAttack", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private Sword _sword;
    private BoxCollider _swordCollider;
    private PlayerController _playerController;
    private HealthBar _healthBar;

    private bool isAttack = false;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        _sword = GetComponentInChildren<Sword>();
        _swordCollider = _sword.GetComponent<BoxCollider>();
        _playerController = FindObjectOfType<PlayerController>();
        _healthBar = GetComponentInChildren<HealthBar>();

        _swordCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _playerController.canMove)
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

    public void Dead()
    {
        if (isDead) return;

        isDead = true;
        animator.SetBool("isDead", true);
        // EnemyAnimation도 마찬가지지만 완전 원본 애니메이션 이름을 적어야 함
        StartCoroutine(DestroyAfterAnimation("Die01_SwordAndShield"));
    }

    private IEnumerator DestroyAfterAnimation(string clipName)
    {
        float clipLength = GetAnimationClipLength(animator, clipName);
        clipLength *= 4f;

        yield return new WaitForSeconds(clipLength);        

        SceneManagers.Instance.LoadMainRoom();
        isDead = false;
        animator.SetBool("isDead", false);
        PlayerAttributesManager.Instance.deathCount++;
        _healthBar.SetHealth(PlayerAttributesManager.Instance.maxHealth);
    }

    private float GetAnimationClipLength(Animator animator, string clipName)
    {
        foreach(AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if(clip.name == clipName)
            {
                return clip.length;
            }
        }
        return 0f;
    }
}

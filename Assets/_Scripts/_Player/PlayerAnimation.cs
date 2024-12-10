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
    private PlayerHealthBar _healthBar;

#pragma warning disable 414
    private bool isAttack = false;
#pragma warning restore 414
    private bool isDead = false;

    [SerializeField] private float attackSpeed = 1.0f;
    [SerializeField] private int attackIndex = 0;
    [SerializeField] private float attackResetTime = 0.3f;
    private float lastAttackTime;

    [SerializeField] private AudioClip[] attackSoundClip;
    void Start()
    {
        animator = GetComponent<Animator>();
        _sword = GetComponentInChildren<Sword>();
        _swordCollider = _sword.GetComponent<BoxCollider>();
        _playerController = FindObjectOfType<PlayerController>();
        _healthBar = FindObjectOfType<PlayerHealthBar>();

        _swordCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _playerController.canMove)
        {
            Attack();       
        }

        if (Time.time - lastAttackTime > attackResetTime)
        {
            ResetAttack();
        }
    }

    private void Attack()
    {
        isAttack = true;
        lastAttackTime = Time.time;

        _sword._canDealDamage = true;

        PlayAttackAnimation();
        attackIndex++;
    }

    public void IncreaseAttackSpeed(float amount)
    {
        attackSpeed += amount;
    }

    private void PlayAttackAnimation()
    {
        animator.speed = attackSpeed;

        if (attackIndex == 0)
        {
            animator.SetTrigger("Attacking");            
        }
        else if (attackIndex == 1)
        {
            animator.SetTrigger("Attacking");            
        }
        else if (attackIndex == 2)
        {
            animator.SetTrigger("Attacking");            
        }
        else
        {
            ResetAttack();
        }
    }

    private void ResetAttack()
    {
        attackIndex = 0;

        animator.speed = 1.0f;
    }

    // 애니메이션 이벤트로 관리.
    public void EnableDamage()
    {
        _sword._canDealDamage = true;
        _swordCollider.enabled = true;
    }

    // 애니메이션 이벤트로 관리.
    public void DisableDamage()
    {
        _sword._canDealDamage = false;
        isAttack = false;
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
    public void PlaySwordSwingSound()
    {
        AudioManager.Instance.PlayRandomSoundFXClip(attackSoundClip, transform, 0.3f);
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

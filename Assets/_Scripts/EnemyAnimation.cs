using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;
    private CapsuleCollider capsuleCollider;
    private HealthBar healthBar;
    private Sword _sword;
    private BoxCollider _swordCollider;

    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        healthBar = GetComponentInChildren<HealthBar>();        
        _sword = GetComponentInChildren<Sword>();
        if (_sword != null)
        {
            _swordCollider = _sword.GetComponent<BoxCollider>();
        }
    }
    public void Dead()
    {
        if (isDead) return;
        isDead = true;

        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        animator.SetBool("isDead", true);
        capsuleCollider.enabled = false;
        healthBar.gameObject.SetActive(false);
        yield return new WaitForSeconds(8f);

        Destroy(gameObject);
    }

    private float GetAnimationClipLength(Animator animator, string clipName)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }
        return 0f;
    }

    // 애니메이션 이벤트로 관리.
    public void EnableDamage()
    {
        if(_sword != null)
        {
            _sword._canDealDamage = true;
            _swordCollider.enabled = true;
        }
    }

    // 애니메이션 이벤트로 관리.
    public void DisableDamage()
    {
        if(_sword != null)
        {
            _sword._canDealDamage = false;
            _swordCollider.enabled = false;
        }        
    }

}

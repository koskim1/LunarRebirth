using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Dead()
    {
        animator.SetBool("isDead", true);
        StartCoroutine(DestroyAfterAnimation("Die01_SwordAndShield"));
    }

    private IEnumerator DestroyAfterAnimation(string clipName)
    {
        float clipLength = GetAnimationClipLength(animator, clipName);

        clipLength *= 4f;

        yield return new WaitForSeconds(clipLength);

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
}

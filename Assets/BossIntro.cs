using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntro : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        AudioManager.Instance.PlaySoundFXClip(AudioManager.Instance.bossIntroSFX, transform, .8f);
    }
}

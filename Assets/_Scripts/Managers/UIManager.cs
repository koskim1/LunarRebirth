using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject BossUI;
    public GameObject GoingToBossRoomText;
    public GameObject AfterBossDeadText;
    public AudioMixer AudioMixer;

    [SerializeField] private AudioClip clickSound;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else if (Instance != null)
        {
            Destroy(gameObject);
        }

        if(BossUI != null)
        {
            BossUI.SetActive(false);
        }
        if(GoingToBossRoomText != null)
        {
            GoingToBossRoomText.SetActive(false);
        }
        if(AfterBossDeadText != null)
        {
            AfterBossDeadText.SetActive(false);
        }
    }

    public void SetVolume(float volume)
    {
        AudioMixer.SetFloat("Volume", volume);
    }

    public void PlayClickSound()
    {
        AudioManager.Instance.PlaySoundFXClip(clickSound, transform, 1f);
    }
}

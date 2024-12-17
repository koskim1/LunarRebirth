using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;

    public GameObject MainMenuUI;
    public GameObject TitleImage;

    //public GameObject InGameUI;
    public Button startGameBtn;
    //public Button goBackToGameBtn;
    public Button optionBtn;
    public Button quitBtn;
    public Button continueBtn;

    public AudioMixer audioMixer;

    public GameObject mainMenu;
    public GameObject optionMenu;

    public AudioSource mainMenuAudioSource;
    public AudioClip clickSound;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
        
        if (startGameBtn != null)
        {
            Debug.Log("startGameBtn != null");
            startGameBtn.onClick.AddListener(SceneManagers.Instance.LoadIntroScene);
        }
        
        optionBtn.onClick.AddListener(OpenOptionMenu);
        quitBtn.onClick.AddListener(Application.Quit);
        continueBtn.onClick.AddListener(SceneManagers.Instance.LoadMainRoom);

        //UIManager.Instance.gameObject.SetActive(false);
        continueBtn.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(PlayerAttributesManager.Instance != null)
        {
            if(PlayerAttributesManager.Instance.deathCount != 0)
            {
                continueBtn.gameObject.SetActive(true);
            }
            else
            {
                continueBtn.gameObject.SetActive(false);
            }
        }   
    }
    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // 오브젝트 생성
        AudioSource audioSource = Instantiate(mainMenuAudioSource, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float audioLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, audioLength);
    }
    public void OpenOptionMenu()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
        TitleImage.SetActive(false);

        PlaySoundFXClip(clickSound, transform, .5f);
    }

    public void GoBackToMain()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
        TitleImage.SetActive(true);
        PlaySoundFXClip(clickSound, transform, .5f);
    }

    public void QuitBtn()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
}

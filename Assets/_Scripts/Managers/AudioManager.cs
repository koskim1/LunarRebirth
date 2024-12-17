using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    public AudioSource soundFXObject;
    public AudioSource BGM;

    public AudioClip[] enemyHitSounds;
    public AudioClip fireballSFX;
    public AudioClip fireballExplosionSFX;
    public AudioClip levelUpSFX;
    public AudioClip gainMLPSFX;
    public AudioClip bossIntroSFX;
    public AudioClip bossAttackSFX;
    public AudioClip bossDeadSFX;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMainBGM(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // 오브젝트 생성
        AudioSource audioSource = Instantiate(BGM, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // 오브젝트 생성
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        
        audioSource.clip = audioClip;
        
        audioSource.volume = volume;

        audioSource.Play();

        float audioLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, audioLength);
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        int rand = Random.Range(0, audioClip.Length);
        // 오브젝트 생성
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip[rand];

        audioSource.volume = volume;

        audioSource.Play();

        float audioLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, audioLength);
    }

}

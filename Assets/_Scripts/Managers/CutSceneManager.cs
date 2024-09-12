using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class CutSceneManager : MonoBehaviour
{
    public PlayableDirector introTimeline;
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainRoom" && PlayerAttributesManager.Instance.deathCount == 0)
        {
            PlayIntroCutScene();
        }
    }

    public void PlayIntroCutScene()
    {
        if(introTimeline != null)
        {
            introTimeline.Play();
        }
        else
        {
            Debug.LogError("introTimeline ¹Ì¼³Á¤");
        }
    }
}

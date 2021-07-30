using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroVideo : MonoBehaviour
{
    VideoPlayer videoPlayer;    

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Play();
        videoPlayer.loopPointReached += CheckOver;         
    } 
 
    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("FoodScape");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)    	
        {        	
        	videoPlayer.Stop();
            LoadNextScene();
        }            
    }
}

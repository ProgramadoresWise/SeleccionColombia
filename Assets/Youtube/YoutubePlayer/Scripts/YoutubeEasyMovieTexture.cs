using UnityEngine;
using System.Collections;

public class YoutubeEasyMovieTexture : MonoBehaviour
{

    public string youtubeVideoIdOrUrl;
    public int videoQuatily = 720;

    void Start()
    {
        LoadYoutubeInTexture();
    }

    public void LoadYoutubeInTexture()
    {
        //ALERT If you are using EasyMovieTexture uncomment the line..
        MediaPlayerCtrl mpc = gameObject.GetComponent<MediaPlayerCtrl>();
        //Debug.Log(YoutubeVideo.Instance.RequestVideo(youtubeVideoIdOrUrl, 720));
        if (mpc)
        {
            mpc.m_strFileName = YoutubeVideo.Instance.RequestVideo(youtubeVideoIdOrUrl, videoQuatily);
        }
        else
        {
            Debug.LogError("No carga el componente");
        }
    }
}

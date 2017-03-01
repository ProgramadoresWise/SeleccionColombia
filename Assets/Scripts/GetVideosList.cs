using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using JsonFx;

public class GetVideosList : MonoBehaviour
{
    public GameObject loadingScreen;
    public Sprite noImage;
    public string databaseLink = "http://192.169.155.181/~fcf2waysports/2waysports/Colombia/VideosVR/php/getVideosList.php";

    // Use this for initialization
    IEnumerator Load()
    {
        loadingScreen.SetActive(true);

        List<VideosListItem> videosList = new List<VideosListItem>();
        //string url = "http://localhost/";
        WWW www = new WWW(databaseLink);
        yield return www;
        if (www.error == null)
        {
            detailsVideoVR[] jvrvlist = JsonFx.Json.JsonReader.Deserialize<detailsVideoVR[]>(www.text);
            foreach (detailsVideoVR d in jvrvlist)
            {
                VideosListItem videosListItem = new VideosListItem() { name = d.nameVideoVR, linkThumbnail = d.linkThumbnailVideoVR, linkVideo = d.linkVideoVR, noThumbnail = noImage };
                videosList.Add(videosListItem);
            }
            loadingScreen.SetActive(false);
            VideosListManager.singleton.FillVideosList(videosList);
        }
        else
        {
            Debug.LogError("ERROR: " + www.error);
            loadingScreen.SetActive(false);
        }
    }

    public void StartLoading()
    {
        StartCoroutine(Load());
    }
}

public class detailsVideoVR
{
    public string nameVideoVR, linkVideoVR, linkThumbnailVideoVR;
}
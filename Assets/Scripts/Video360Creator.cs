using System.Collections;
using UnityEngine;

public class Video360Creator : MonoBehaviour
{
    public static Video360Creator singleton;
    public GameObject video360Prefab;
    public GameObject loadingScreen;

    void Awake ()
    {
        Video360Creator.singleton = this;
    }

    public void StartVideo (string videoIndex)
    {
        int index;
        if (!int.TryParse (videoIndex, out index))
            return;

        // Screen.autorotateToPortrait = Screen.autorotateToPortraitUpsideDown = false;
        //Screen.autorotateToLandscapeLeft = Screen.autorotateToLandscapeRight = true;

        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Screen.orientation = ScreenOrientation.AutoRotation;
        VideosListManager.singleton.cameraVR.SetActive (false);
        //foreach (GameObject go in GameObject.FindGameObjectsWithTag("VideosListCamera"))
        //    go.SetActive(false);

        foreach (GameObject go in GameObject.FindGameObjectsWithTag ("360VideoParent"))
        Destroy (go);

        StartCoroutine (WaitUntilLandscapeComplete (index));
    }

    IEnumerator WaitUntilLandscapeComplete (int videoIndex)
    {
        loadingScreen.SetActive(true);

        // while (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        // {
        //     yield return null;
        //     loadingScreen.SetActive(false);
        // }

        GameObject v3600p = Instantiate (video360Prefab, Vector3.zero, Quaternion.identity) as GameObject;

        DreamHouse360Video dh360v = v3600p.GetComponentInChildren<DreamHouse360Video> ();


        if (dh360v)
        {
            dh360v.videoIdOrUrl = VideosListManager.singleton.videosList[videoIndex].linkVideo;
			dh360v.tittleText.text = VideosListManager.singleton.videosList[videoIndex].name;
        }
        else
        {
            Debug.LogError ("¡Algo salió mal!");
//            DebugConsole.Log ("¡Algo salió mal!", "error");
            loadingScreen.SetActive(false);
        }

        Screen.autorotateToPortrait = Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.AutoRotation;
        yield return null;
        loadingScreen.SetActive(false);
    }

    public void GetBack ()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag ("360VideoParent"))
        Destroy (go);

        Screen.autorotateToLandscapeLeft = Screen.autorotateToLandscapeRight = false;
        Screen.orientation = ScreenOrientation.Portrait;

        VideosListManager.singleton.cameraVR.SetActive (true);
        //VideosListManager.singleton.ShowMenu();

#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.AutoRotation;
#endif

        StartCoroutine (WaitUntilPortraitComplete ());

        //foreach (GameObject go in GameObject.FindGameObjectsWithTag("VideosListCamera"))
        //    go.SetActive(true);

    }

    IEnumerator WaitUntilPortraitComplete ()
    {
        while (Screen.orientation != ScreenOrientation.Portrait)
        {
            yield return null;
        }
        // Screen.autorotateToPortrait = Screen.autorotateToPortraitUpsideDown = true;

        VideosListManager.singleton.ShowMenu ();
        yield return null;
    }

}
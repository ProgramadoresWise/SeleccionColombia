using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class DreamHouse360Video : MonoBehaviour
{
    //[System.NonSerialized]
    public MediaPlayerCtrl mpc; //clase contenedora de eventos de reproduccion
    [System.NonSerialized]
    public bool inputDown, loading, isPlaying, startDelayed = true;
    public Transform mainCamera;
    public float delay = 1f, touchRotationMultiplier = 10f;
    public string videoIdOrUrl;
    public int videoQuatily = 720;
    public Text vrButtonText, playPauseText, timeText, tempText, tittleText;
    public GameObject loadingText, controlPanel, playImage, pauseImage;
    public Transform _360Objects;
    public Slider seekBar;
    public float controlPanelTime = 3f;

    int lastSeek;
    Vector2 startTouchPosition, startTouchRotation;

    IEnumerator showcontrolPanel;
    IEnumerator changeSeekValue;
    IEnumerator checkState;

    IEnumerator showcontrolPanel_ ()
    {
        controlPanel.SetActive (true);
        while (inputDown)
        {
            yield return new WaitForSeconds (0.01f);
        }
        yield return new WaitForSeconds (controlPanelTime - 0.01f);
        HideControlPanel ();
    }

    IEnumerator changeSeekValue_ (int value)
    {
        yield return new WaitForSeconds (0.1f);
        mpc.SeekTo (value);
    }

    IEnumerator checkState_ ()
    {
        for (;;)
        {
            if (!mpc)
                break;

            if (!loading && mpc.GetCurrentState () == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING && lastSeek == mpc.GetSeekPosition ())
                SetLoadingState (true);

            if (loading && mpc.GetCurrentState () == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING && lastSeek != mpc.GetSeekPosition ())
                SetLoadingState (false);

            CheckPlayPauseStateButton ();
            lastSeek = mpc.GetSeekPosition ();

            if (mpc.GetCurrentState () == MediaPlayerCtrl.MEDIAPLAYER_STATE.END)
                Video360Creator.singleton.GetBack ();

            yield return new WaitForSeconds (0.1f);
        }
    }

    IEnumerator startLoadDelayed (float delay)
    {
        yield return new WaitForSeconds (delay);
        LoadVideo ();
    }

    IEnumerator timeUpdate ()
    {
        for (;;)
        {
            if (mpc)
            {
                System.TimeSpan elapsed = System.TimeSpan.FromSeconds ((int) (mpc.GetSeekPosition () / 1000));
                System.TimeSpan duration = System.TimeSpan.FromSeconds ((int) (mpc.GetDuration () / 1000));
                timeText.text = string.Format ("{0}:{1}", (elapsed.Minutes < 10 ? "0" : "") + elapsed.Minutes.ToString (), (elapsed.Seconds < 10 ? "0" : "") + elapsed.Seconds.ToString () + " / ");
                timeText.text += string.Format ("{0}:{1}", (duration.Minutes < 10 ? "0" : "") + duration.Minutes.ToString (), (duration.Seconds < 10 ? "0" : "") + duration.Seconds.ToString ());
            }
            //Borrar
            tempText.text = Input.deviceOrientation.ToString ();
            yield return new WaitForSeconds (0.1f);
        }
    }

    // Use this for initialization
    void Start ()
    {
        //controlPanel.SetActive(false);
        if (vrButtonText)
            vrButtonText.text = GvrViewer.Instance.VRModeEnabled ? "360º" : "VR";
        //mpc = gameObject.GetComponent<MediaPlayerCtrl>();

        if (startDelayed)
            StartCoroutine (startLoadDelayed (delay));
        else
            LoadVideo ();

        if (checkState != null)
            StopCoroutine (checkState);
        checkState = checkState_ ();
        StartCoroutine (checkState);
        StartCoroutine (timeUpdate ());
        ShowControlPanel ();
    }

    void Update ()
    {
        if (Input.GetMouseButtonDown (0))
        {
            inputDown = true;
            ShowControlPanel ();
        }

        if (Input.GetMouseButtonUp (0))
        {
            inputDown = false;
        }

        if (!inputDown && mpc && seekBar)
            seekBar.value = mpc.GetSeekPosition ();

        if (Input.GetMouseButtonDown (0))
        {
            startTouchPosition = Input.mousePosition;
            startTouchRotation = new Vector2 (_360Objects.localRotation.x, _360Objects.localRotation.y);

        }

        if (!GvrViewer.Instance.VRModeEnabled && Input.GetMouseButton (0))
        {
            float dtx = Input.mousePosition.x - startTouchPosition.x,
                dty = Input.mousePosition.y - startTouchPosition.y;
            _360Objects.RotateAround (_360Objects.localPosition, _360Objects.up, (dtx * touchRotationMultiplier * Mathf.Deg2Rad));
            _360Objects.RotateAround (_360Objects.localPosition, mainCamera.right, -(dty * touchRotationMultiplier * Mathf.Deg2Rad));
            //_360Objects.RotateAround(_360Objects.localPosition, Vector3.right, -(dty * touchRotationMultiplier * Mathf.Deg2Rad));
            startTouchPosition = Input.mousePosition;
        }

    }

    public void SetLoadingState (bool value)
    {
        loading = value;
        loadingText.SetActive (value);
    }

    public void ShowControlPanel ()
    {
        if (showcontrolPanel != null)
            StopCoroutine (showcontrolPanel);
        showcontrolPanel = showcontrolPanel_ ();
        StartCoroutine (showcontrolPanel);
    }

    public void HideControlPanel ()
    {
        controlPanel.SetActive (false);
    }

    public void LoadVideo ()
    {
        if (videoIdOrUrl == string.Empty || videoQuatily <= 0)
            return;
        LoadVideo (videoIdOrUrl, videoQuatily);
    }

    public void LoadVideo (string youtubeVideoId)
    {
        if (youtubeVideoId == string.Empty || videoQuatily <= 0)
            return;
        LoadVideo (youtubeVideoId, videoQuatily);
    }

    public void LoadVideo (string youtubeVideoId, int youtubeVideoQuality)
    {
        if (youtubeVideoId == string.Empty || youtubeVideoQuality <= 0)
            return;

        if (mpc)
        {
            mpc.m_strFileName = YoutubeVideo.Instance.RequestVideo (youtubeVideoId, videoQuatily);

            loading = true;
            loadingText.SetActive (true);

            mpc.OnReady += delegate ()
            {
                seekBar.maxValue = mpc.GetDuration ();
                SetLoadingState (false);
                isPlaying = true;
            };
        }
    }

    public void VREnabledToggle ()
    {

        //Screen.orientation = (!GvrViewer.Instance.VRModeEnabled || Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight || Screen.orientation == ScreenOrientation.Landscape) ? Screen.orientation : ScreenOrientation.LandscapeLeft;

        Screen.autorotateToPortrait = Screen.autorotateToPortraitUpsideDown = GvrViewer.Instance.VRModeEnabled;
        if (!GvrViewer.Instance.VRModeEnabled)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            Screen.orientation = ScreenOrientation.AutoRotation;
        }

        GvrViewer.Instance.VRModeEnabled = !GvrViewer.Instance.VRModeEnabled;
        if (vrButtonText)
            vrButtonText.text = GvrViewer.Instance.VRModeEnabled ? "360º" : "VR";

        if (GvrViewer.Instance.VRModeEnabled)
            _360Objects.rotation = Quaternion.identity;
        //Screen.autorotateToPortrait = Screen.autorotateToPortraitUpsideDown = !GvrViewer.Instance.VRModeEnabled;
    }

    public void PlayPauseToggle ()
    {
        if (!mpc || mpc.GetCurrentState () == MediaPlayerCtrl.MEDIAPLAYER_STATE.NOT_READY)
            return;

        if (!isPlaying)
        {
            mpc.Play ();
            isPlaying = true;
        }
        else
        {
            mpc.Pause ();
            isPlaying = false;
        }

        //isPlaying = mpc.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING;
        CheckPlayPauseStateButton ();
    }

    public void CheckPlayPauseStateButton ()
    {
        //playPauseText.text = (mpc.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING && playPauseText) ? "Play" : "Pause";

        if (mpc.GetCurrentState () != MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING && playPauseText)
        {
            playImage.SetActive (true);
            pauseImage.SetActive (false);
        }
        else
        {
            playImage.SetActive (false);
            pauseImage.SetActive (true);
        }
    }

    public void SeekBarValueChanged (float value)
    {
        if (!inputDown || !mpc)
            return;

        if (changeSeekValue != null)
            StopCoroutine (changeSeekValue);
        changeSeekValue = changeSeekValue_ ((int) value);
        StartCoroutine (changeSeekValue);
    }

    public void ShowVideosList ()
    {
        Debug.Log ("Mostrar video");
        HideControlPanel ();
        Video360Creator.singleton.GetBack ();
    }

    //public void HideVideosList()
    //{
    //    Debug.Log("Ocultar video");
    //    HideControlPanel();
    //}

    public void CheckState ()
    {
        if (!mpc)
            return;
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{

    RectTransform rt;
    public float heightFactor = 0.7f;
    public Image image;
    public Text text;

    // Use this for initialization
    void Start ()
    {
        rt = GetComponent<RectTransform> ();
    }

    // Update is called once per frame
    void Update ()
    {
        // float width = Screen.width;
        // float height = width * heightFactor;

        // rt.sizeDelta = new Vector2 (width, height);
    }

    public void SendValue ()
    {
        Video360Creator.singleton.StartVideo (name);
//		Video360Creator.singleton.
        VideosListManager.singleton.HideMenu ();
    }
}
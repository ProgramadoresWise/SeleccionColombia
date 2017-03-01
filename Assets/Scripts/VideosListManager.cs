using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideosListManager : MonoBehaviour
{
    public static VideosListManager singleton;
    public List<VideosListItem> videosList;
    public GameObject buttonPrefab;
    public RectTransform content;
    public GameObject menu, camera;

    void Awake ()
    {
        VideosListManager.singleton = this;
    }

    public void FillVideosList (List<VideosListItem> list)
    {
        ClearContent();

        VideosListManager.singleton.videosList = list;

        float buttonWidth = Screen.width;
        float buttonHeight = buttonWidth * 0.7f;
        float currentHeight = 0;
        int countFe = 0;
        
        foreach (VideosListItem i in list)
        {
            GameObject b = Instantiate (buttonPrefab) as GameObject;
            RectTransform brt = b.GetComponent<RectTransform> ();
            ButtonInfo bi = b.GetComponent<ButtonInfo> ();
            //
            b.name = countFe.ToString ();
            brt.SetParent (content, false);
            brt.anchoredPosition = new Vector2 (0, currentHeight);
            bi.text.text = i.name;
            i.buttonImage = bi.image;
            currentHeight -= buttonHeight;
            countFe++;
        }

        content.sizeDelta = new Vector2 (0, -currentHeight);
    }

    IEnumerator _GetThumbnail (string url, Image img)
    {
        WWW www = new WWW (url);
        yield return www;

        if (www.error == null && www.texture.GetPixels ().Length > 64)
        {
            img.sprite = Sprite.Create (www.texture, new Rect (0, 0, www.texture.width, www.texture.height), new Vector2 (0, 0));
        }
    }

    public void GetThumbnail (string url, Image img)
    {
        StartCoroutine (_GetThumbnail (url, img));
    }

    public void HideMenu ()
    {
        menu.SetActive (false);
    }

    public void ShowMenu ()
    {
        menu.SetActive (true);
    }

    public void ClearContent()
    {
        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
        }
    }
}

[System.Serializable]
public class VideosListItem
{
    public string name, linkVideo, linkThumbnail;
    public Sprite noThumbnail;
    private Image _buttonImage;
    public Image buttonImage
    {
        get
        {
            return _buttonImage;
        }
        set
        {
            _buttonImage = value;
            _buttonImage.sprite = noThumbnail;
            VideosListManager.singleton.GetThumbnail (linkThumbnail, _buttonImage);
        }
    }
}
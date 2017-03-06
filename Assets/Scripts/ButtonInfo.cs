using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using MaterialUI;

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
        if( User.main.GetMyEmailVerif( ) == 1 )
        {
            Video360Creator.singleton.StartVideo (name);
    //		Video360Creator.singleton.
            VideosListManager.singleton.HideMenu ();
        }
        else
        {
            DataApp.main.DisableLoading ();
            SnackbarManager.Show("Debes verificar tu cuenta desde tu E-mail",3f, "Re-Enviar Correo", () => {
                DataApp.main.EnableLoading();
                SnackbarManager.OnSnackbarCompleted();
                RegisterLoginManager.main.SendEmailto( User.main.GetMyEmail() , User.main.GetMyToken(), true );
            });
        }
    }
}
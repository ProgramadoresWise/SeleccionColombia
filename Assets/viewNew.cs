using System.Collections;
using MaterialUI;
using UnityEngine;
using UnityEngine.UI;


//[System.Serializable]
using System.Net.Mime;

public class viewNew : MonoBehaviour
{
    public NewsManager managerNews;

    [HideInInspector]
    public int idNew;
    [HideInInspector]
    public int isvideo;
    public string linkVideo;
    public Text tituloNew;
    public Text descNew;
    public Text dateNew;
    public Image imageNew;
    public Text selectFunction;
    public GameObject videoButton;

    public int noticiaJugador;
    public string nImportante;

    float h;
    [HideInInspector]
    public float sizeheight;
    [HideInInspector]
    public float sizewidth;
    [HideInInspector]
    public float _w, _h;

    public IEnumerator LoadImageInit (bool upd)
    {

        //		imageNew.sprite = ImgLoadManager.main.NewsImg(imageNew,idNew.ToString(),false);
        //		yield return StartCoroutine( ImgLoadManager.main.NewsImg_d( imageNew,idNew.ToString(), resultSprite =>{
        //			imageNew.sprite  = resultSprite;
        //			_w = imageNew.sprite.texture.width;
        //			_h = imageNew.sprite.texture.height;
        //			Rect dim = new Rect (0, 0, imageNew.sprite.texture.width, imageNew.sprite.texture.height);
        //			imageNew.sprite = Sprite.Create (imageNew.sprite.texture, dim, Vector2.zero);
        //			h = this.GetComponent<RectTransform> ().sizeDelta.x;
        //			imageNew.GetComponent<LayoutElement> ().preferredHeight = RecalculateHeigth ((int)imageNew.sprite.texture.height);
        //			sizewidth = RecalculateSize ((int)_h , "w");
        //			sizeheight = RecalculateSize ((int)_h , "h");
        //			managerNews.canCreate = true;
        //		}, upd));
        //		yield return new WaitForEndOfFrame();
        imageNew.sprite = ImgLoadManager.main.NewsImg (imageNew, idNew.ToString (), upd);

        Rect newdim = new Rect (0, 0, imageNew.sprite.texture.width, imageNew.sprite.texture.height);
        imageNew.sprite = Sprite.Create (imageNew.sprite.texture, newdim, Vector2.zero);
        h = Screen.height;
        imageNew.GetComponent<LayoutElement> ().preferredHeight = RecalculateHeigth ((int) imageNew.sprite.texture.height);
        yield return new WaitForEndOfFrame ();

        GetComponent<scrollComponent> ().IsDownload = true;
        GetComponent<scrollComponent> ().enabled = false;
        //		managerNews.canCreate = true;

        //		if( this.noticiaJugador != 0){
        //			if(nImportante != "TRUE" ){
        //				this.gameObject.SetActive(false);
        //			}
        //		}	

    }

    public float RecalculateHeigth (int myHeight)
    {
        float newH = myHeight;

        switch (myHeight)
        {
            case 512:
                newH = (h == 480) ? newH = 280 : (h == 800) ? newH = 280 : ((int) h == 854) ? newH = 260 : (h == 1024) ? newH = 260 : (h == 1280) ? newH = 260 : (h == 2560) ? newH = 250 : newH = myHeight;
                break;
            case 256:
                newH = (h == 480) ? newH = 280 : (h == 800) ? newH = 280 : ((int) h == 854) ? newH = 260 : (h == 1024) ? newH = 260 : (h == 1280) ? newH = 260 : (h == 2560) ? newH = 250 : newH = myHeight;
                break;
        }
        Debug.Log ("H ES : " + h + "RETORNANDO " + newH);
        return newH;
    }

    //	public float RecalculateSize (int oldH ){
    //
    //		float newH = _h;
    //		Debug.Log("mi h es: " + newH + "y mi s: es "+ Screen.width);
    //
    //		switch (Screen.width) {
    //		case  480:
    //			
    //			if (oldH == 512) {
    //				newH = 280;
    //			} else if (oldH == 256) {
    //				newH = 565;
    //			} 
    //			break;	
    //		case  600:
    //			if (oldH == 512) {
    //				newH = 460;
    //			} else if (oldH == 256) {
    //				newH = 705;
    //			} 			
    //			break;
    //		case 800:
    //			if (oldH == 512) {
    //				newH = 600;
    //			} else if (oldH == 256) {
    //				newH = 935;
    //			} 			
    //			break;
    //		case 1440:
    //			if (oldH == 512) {
    //				newH = 1140;
    //			} else if (oldH == 256) {
    //				newH = 1700;
    //			} 			
    //			break;
    //		case 720:
    //			if (oldH == 512) {
    //				newH = 570;
    //			} else if (oldH == 256) {
    //				newH = 850;
    //			}			
    //			break;
    //		}
    //
    //			return newH;
    //	}

    public void FunctionNew ()
    {
        if (isvideo == 1)
        {
            if (User.main.GetMyEmailVerif () == 1)
            {
                NavigatorManager.main.popUpInfoPanelToDesactived = MediaPlayerManager.main._panelVideos;
                NavigatorManager.main.enablePopUpInfoPanel = true;
                MediaPlayerManager.main.videoURL = linkVideo;
                //StartCoroutine (MediaPlayerManager.main.selectVideo (720, tituloNew.text));
                InAppBrowser.OpenURL(linkVideo);
            }
            else
            {
				DataApp.main.DisableLoading ();
                SnackbarManager.Show ("Debes verificar tu cuenta desde tu E-mail", 3f, "Re-Enviar Correo", () =>
                {
                    DataApp.main.EnableLoading ();
                    SnackbarManager.OnSnackbarCompleted ();
                    RegisterLoginManager.main.SendEmailto (User.main.GetMyEmail (), User.main.GetMyToken (), true);
                });
            }
        }
        else
        {
            DataApp.main.EnableLoading ();
            NavigatorManager.main.panelsPrincipales[NavigatorManager.main.actualPanel]._enablePopUpInfoPanel (0);
            StartCoroutine (managerNews.bigView.BigNews (this));
            //			StartCoroutine  ( DataApp.main._DisbleLoading() );
        }
    }

    public void FunctionNewPlayer ()
    {
        if (isvideo == 1)
        {
            if (User.main.GetMyEmailVerif () == 1)
            {
                NavigatorManager.main.popUpInfoSubPanelToDesactived = MediaPlayerManager.main._panelVideos;
                NavigatorManager.main.enableSubPopUpInfoPanel = true;
                MediaPlayerManager.main.videoURL = linkVideo;
                //StartCoroutine (MediaPlayerManager.main.selectVideo (720, tituloNew.text));
                InAppBrowser.OpenURL(linkVideo);
            }
            else
            {
				DataApp.main.DisableLoading ();
                SnackbarManager.Show ("Debes verificar tu cuenta desde tu E-mail", 3f, "Re-Enviar Correo", () =>
                {
                    DataApp.main.EnableLoading ();
                    SnackbarManager.OnSnackbarCompleted ();
                    RegisterLoginManager.main.SendEmailto (User.main.GetMyEmail (), User.main.GetMyToken (), true);
                });
            }
        }
        else
        {
            DataApp.main.EnableLoading ();
            NavigatorManager.main.panelsPrincipales[NavigatorManager.main.actualPanel]._enablePopUpInfoSubPanel (1);
            StartCoroutine (PerfilJugador.main.bigView.BigNews (this));
            //			StartCoroutine  ( DataApp.main._DisbleLoading() );
        }
    }

}
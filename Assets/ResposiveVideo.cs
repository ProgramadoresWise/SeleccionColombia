using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResposiveVideo : MonoBehaviour {

	public CanvasGroup canvasV, canvasH;

	// Use this for initialization
	void OnEnable () {
		DataApp.main.EnableLoading();
		StartCoroutine ( DataApp.main._DisbleLoading());
		Screen.orientation = ScreenOrientation.AutoRotation;
	}


	void Update( ){
		if( Screen.orientation == ScreenOrientation.Landscape  || Screen.orientation == ScreenOrientation.LandscapeLeft   || Screen.orientation == ScreenOrientation.LandscapeRight){
			this.GetComponent<RectTransform>().offsetMin = new Vector2( this.GetComponent<RectTransform>().offsetMin.x , 0 );
			this.GetComponent<RectTransform>().offsetMax = new Vector2( this.GetComponent<RectTransform>().offsetMax.x , 0 );
			changesCanvasGroups(false);
		}else if( Screen.orientation == ScreenOrientation.Portrait  || Screen.orientation == ScreenOrientation.PortraitUpsideDown ){
			this.GetComponent<RectTransform>().offsetMin = new Vector2( this.GetComponent<RectTransform>().offsetMin.x , 420 );
			this.GetComponent<RectTransform>().offsetMax = new Vector2( this.GetComponent<RectTransform>().offsetMax.x , 420*-1 );
			changesCanvasGroups(true);
		}

	}



	void changesCanvasGroups( bool isvertical ){
		if( isvertical ){
			canvasV.alpha = 1;
			canvasH.alpha =0 ;

			canvasV.blocksRaycasts = true;
			canvasH.blocksRaycasts = false;
		}else{
			canvasV.alpha = 0;
			canvasH.alpha = 1;

			canvasV.blocksRaycasts = false;
			canvasH.blocksRaycasts = true;
		}
	
	}

	// Update is called once per frame
	void OnDisable () {
		Screen.orientation = ScreenOrientation.Portrait;
	}


	void OnApplicationFocus( bool hasFocus ) {
		if( hasFocus && MediaPlayerManager.main.vMedia.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED ){
				MediaPlayerManager.main.PlayAndPause();
				Debug.Log("REANUDAR VIDEO");
		}else if( !hasFocus && MediaPlayerManager.main.vMedia.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING ){
				MediaPlayerManager.main.PlayAndPause();
				Debug.Log("PAUSAR VIDEO");
			}
	}
}

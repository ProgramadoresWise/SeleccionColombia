using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Text;

public class MediaPlayerManager : MonoBehaviour {



	public static MediaPlayerManager main;



	public Text timeVideo, tittleVideo;
	public Text timeVideoH, tittleVideoH;
	public Image pausePlayv;
	public Sprite s_Play, s_Pause;
	public MediaPlayerCtrl vMedia;
	public GameObject _panelVideos, controlPanel, loading;
	public string videoURL;
	public bool m_bFinish = false;

	bool inputDown, tooglePlay;
	// Use this for initialization


	Texture2D tex= null;

	void OnEnable() {
		vMedia.OnEnd += OnEnd;
		PlayAndPause();
	}


	void OnDisable (){
		vMedia.End();
		m_bFinish = false;
		vMedia.m_strFileName = YoutubeVideo.Instance.RequestVideo( videoURL, 720 );
		NavigatorManager.main.enablePopUpInfoPanel = false;
	}
		

	public IEnumerator selectVideo( int quality , string _name){
		DataApp.main.EnableLoading();
		yield return new WaitForSeconds(1);
		loading.SetActive(true);
		tooglePlay = false;
		m_bFinish = false;
		tittleVideo.text = _name;
		tittleVideoH.text = _name;
		vMedia.m_strFileName = YoutubeVideo.Instance.RequestVideo( videoURL, quality );
		_panelVideos.SetActive(true);
		yield return new WaitForSeconds(1);
		PlayAndPause();
	}

	void getState( ){

		timeVideo.text = secondsVideos((double) vMedia.GetSeekPosition()).ToString( );
		timeVideoH.text = secondsVideos((double) vMedia.GetSeekPosition()).ToString( );
		switch( vMedia.GetCurrentState( )){
		case MediaPlayerCtrl.MEDIAPLAYER_STATE.READY:
				loading.SetActive(false);
				StartCoroutine(showcontrolPanel_());
			break;
		case MediaPlayerCtrl.MEDIAPLAYER_STATE.NOT_READY:
				loading.SetActive(true);
			break;
		case MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED:
				pausePlayv.sprite = s_Play;
			break;
		case MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING:
				DataApp.main.DisableLoading();
				pausePlayv.sprite = s_Pause;
				loading.SetActive(false);
			break;
		case MediaPlayerCtrl.MEDIAPLAYER_STATE.END:
				DataApp.main.EnableLoading();
				loading.SetActive(false);
				Exit();
			break;
		}
	}

	void Update ( ){

		if( _panelVideos.gameObject.activeSelf ){
			getState( );
			if( Input.GetMouseButtonDown(0) ){
				controlPanel.SetActive(true);
				inputDown = true;
				StopAllCoroutines();
			}
			if( Input.GetMouseButtonUp(0) ){
				inputDown = false;
				StartCoroutine(showcontrolPanel_());
			}
		}
	}

	public string secondsVideos ( double miliseconds ) {
		float sec  =  ( float ) TimeSpan.FromMilliseconds(miliseconds).TotalSeconds;
		float min = 0;
		if( sec > 60 )
			min = sec / 60;
		float resultSeconds = sec; 
		while ( resultSeconds >= 60 ){
			resultSeconds -= 60;  }
		return min.ToString("00") +":"+ resultSeconds.ToString("00");
	}

	IEnumerator showcontrolPanel_()  {
		yield return new WaitForSeconds(5f);
		controlPanel.SetActive(false);
	}


	public void Exit ( ){
		DataApp.main.EnableLoading();
		_panelVideos.SetActive(false);
		vMedia.Stop( );
		OnEnd();
		tex = null;
	}



	public void PlayAndPause( ){
		if( tooglePlay ){
			pausePlayv.sprite = s_Play;
			vMedia.Pause();
		}else{
			pausePlayv.sprite = s_Pause;
			vMedia.Play();}
		tooglePlay =! tooglePlay;
	}

	public void Stop(){
		DataApp.main.EnableLoading();
		StartCoroutine ( DataApp.main._DisbleLoading());
		tooglePlay = false;
		pausePlayv.sprite = s_Play;
		vMedia.Stop();
		tex = null;
	}

	public void OnEnd ( ){
		print( "on end llamado" );
		DataApp.main.EnableLoading();
		StartCoroutine ( DataApp.main._DisbleLoading());
		vMedia.End();
		_panelVideos.SetActive(false);
		 m_bFinish = true;
		tex = null;
	}








}

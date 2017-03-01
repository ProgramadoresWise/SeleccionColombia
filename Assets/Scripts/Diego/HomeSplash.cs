using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MaterialUI;

public class HomeSplash : MonoBehaviour {

	public static HomeSplash main;


	public Loading loadingmain;
	public Image splash;
	public List<Sprite> spritesSplash;
	public RawImage [] pagesSplash;


	bool update;


	public Image barLoad;


	void Awake( ){
		Font.GetOSInstalledFontNames();
	}

	void Start(){
		StartCoroutine(Init());
	}

	IEnumerator Init () {
		
		yield return StartCoroutine( loadMesajes());
		yield return StartCoroutine( UpdateSplash());
		yield return StartCoroutine( NavigatorManager.main.runEventInBackGround(0,2));

		for( int i =0; i < spritesSplash.Count; i++) {
			yield return new WaitForSeconds(.03f);
			splash.sprite = spritesSplash[i];
		}
		
		yield return new WaitForSeconds(.5f);

		for( float i = 1.0f; i >= 0; i-=0.05f*Time.deltaTime) {
			
			splash.color = new Color(i, i, i, splash.color.a );
		}

//		yield return StartCoroutine( NavigatorManager.main.runEventInBackGround(0,0));
		selectInitPanel();

		yield return null;
	}

	void selectInitPanel( ){
		
		if( DataApp.main.IsRegistered() ){
			NavigatorManager.main.actualPanel = 0;
			NavigatorManager.main.actualSubPanel = 0;

		}else{
			NavigatorManager.main.actualPanel = 14;
			NavigatorManager.main.actualSubPanel  = 0;
		}

		NavigatorManager.main.HistorialClear();
		NavigatorManager.main.saveIndex(NavigatorManager.main.actualPanel,NavigatorManager.main.actualSubPanel,"Inicio");
		Screen.fullScreen = false;
	}

	void clear( )
	{
		spritesSplash.Clear();
		Destroy( HomeSplash.main );
	}


	IEnumerator UpdateSplash( ){
		WWW updNew = new WWW(DataApp.main.host + DataApp.main.urlActualizarInfo + (int)idActualizacion.splash);
		yield return updNew;
		if( string.IsNullOrEmpty( updNew.error ) ){
			StartCoroutine (getDatasSplash( int.Parse(updNew.text)));
		}else{
			DataApp.main.popUpInformative(true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
		}

	}

	public IEnumerator getDatasSplash( int NumAct){
		int ind=0;
		if( DataApp.main.GetMyInfoInt("Splash") !=  NumAct){
			foreach(RawImage img in pagesSplash  ){
				ind++;
				yield return new WaitForEndOfFrame();
				img.texture = ImgLoadManager.main.SplashImg(img,ind.ToString(),true);
			}
		}else{
			foreach(RawImage img in pagesSplash  ){
				ind++;
				yield return new WaitForEndOfFrame();
				img.texture = ImgLoadManager.main.SplashImg(img,ind.ToString(),false);
			}
		}

		DataApp.main.SetMyInfo("Splash",NumAct.ToString(),1);

	}


	#region LAODING MENSAJES

	[SerializeField]
	private DataRowList DataMgsList;
	[HideInInspector]
	public List<string> msgs;

	IEnumerator loadMesajes( ){

		Debug.Log("VALIDANDO ACTUALIZACION");
		WWW updNew = new WWW(DataApp.main.host + DataApp.main.urlActualizarInfo + (int)idActualizacion.infopopuop);
		yield return updNew;
		if( string.IsNullOrEmpty( updNew.error ) ){
			StartCoroutine (getDatasMsgs( int.Parse(updNew.text), DataApp.main.host + "MensajeLoading/msgload.php"));
		}else{
			DataApp.main.popUpInformative(true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
		}

	}



	public IEnumerator getDatasMsgs( int NumAct, string url ){


		if( DataApp.main.GetMyInfoInt("MsgLoading") !=  NumAct){
			yield return StartCoroutine( GetJsonDataScript.getJson.GetPhpData( url));
			DataApp.main.SetMyInfo("JsonMsgLoading", GetJsonDataScript.getJson.GetDataConsult(), 3);
		}else{
			string jsonGuardado = DataApp.main.GetMyInfoString("JsonMsgLoading");
			yield return StartCoroutine(GetJsonDataScript.getJson.GetLocalData(jsonGuardado));
		}

		if( GetJsonDataScript.getJson._state == "Successful" ){
			DataMgsList  = GetJsonDataScript.getJson.GetData( DataMgsList , "msg");
			saveMesages();
			DataApp.main.SetMyInfo("MsgLoading",NumAct.ToString(),1);
		}else if( GetJsonDataScript.getJson._state == "Warning_01" ){
			print("w1");
		}else if( GetJsonDataScript.getJson._state == "Warning_02" ){
			print("w2");
		}else if (GetJsonDataScript.getJson._state == "Warning_03"){
			DataApp.main.SetMyInfo("JsonMsgLoading", "", 3);
			DataApp.main.SetMyInfo("MsgLoading", "0", 1);
			StartCoroutine(getDatasMsgs(NumAct,url));
		}

		GetJsonDataScript.getJson._state = "";


	}

	void saveMesages( ){
		int indx = 0;
		foreach( DataRow obj in DataMgsList.dataList){
			msgs.Add(obj.GetValueToKey("msg"));
			indx++;
		}
		loadingmain.msgs = msgs;

	}


	#endregion
}

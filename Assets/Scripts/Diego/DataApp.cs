using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using System.Security.Cryptography;

public enum idActualizacion {

	tablas = 1,
	calendario = 2,
	noticias = 3,
	patrocinadores = 4,
	convocados = 5,
	alineacion = 6,

	userRanking = 7,
	userPrediction = 8,

	jugadores = 9,
	tuElijes =10,
	infopopuop =11,
	splash =12
}

public class DataApp : MonoBehaviour {

	public enum DateState : int{ WAITING_DATE =1,WAITING_TIME =2,WAITING_DAY_MATCH =3 ,IMMINENT_MATCH=4, IN_MATCH =5};

	public static DataApp main;
	public string host = "";
	public string urlActualizarInfo = "";
	public popUpInfo _PopUpInformative;
	public GameObject loadingPrincipal , popUpRegistroInfo;
	public bool IsActivePopUpInfo;
	public List<string> Directorys;

	public int versionApp;



	void Awake(){
		CreateDirectorys();
		if( IsRegistered() ){
			StartCoroutine(setMyVersion(versionApp));
		}
	}

	void Start(){
		if( IsRegistered() ){
			User.main.reloadDataInUser( );
		}
	}


	IEnumerator setMyVersion(int version ){
		WWW getVersion = new WWW(DataApp.main.host + "/Registro/ReadVersion.php?userID=" + GetMyID());
		yield return getVersion;

		//si la version de la bd es diferente a la del app entonces actualice la version en la bd
		if(int.Parse(getVersion.text) != version) {
			WWW v = new WWW(DataApp.main.host + "/Registro/registerandlogin.php?indata=changeVersion&iduser=" + GetMyID() + "&version=" + version);
			yield return v;
		}


		/*Gender sexo;
		if (User.main.GetMyGender() == "Femenino"){
			sexo = Gender.Female;
		}else{
			sexo = Gender.Male;
		}
		Analytics.SetUserGender(sexo);   */

	}

	// CREAR LOS DIRECTORIOS NECESARIOS EN LA APP PARA EL CACHE
	public void CreateDirectorys( ){


		foreach( string dir in Directorys){
			if(!Directory.Exists( Application.persistentDataPath +"/"+ dir ))
			 	Directory.CreateDirectory(Application.persistentDataPath+"/"+ dir );
		}
	}





	 // DATA DEL ID DEL USUARIO
	public void SetMyID (string id){
		Debug.Log("REGISTRADOO MI PERRO");
		PlayerPrefs.SetInt ("MyID", int.Parse(id));
	}

	public int GetMyID (){
		return PlayerPrefs.HasKey("MyID") ? PlayerPrefs.GetInt ("MyID"): 0;
	}

	public bool IsRegistered(){
		
		return PlayerPrefs.HasKey("MyID");
	}





	/// DATA PARA GUARDARRRR ACTUALIZACION Y LLAVESS  GET -----  SET

	//variableType = 1 INT, 2 FLOAT, 3 String 
	public void SetMyInfo(string nameKey, string value, int variableType){
		if (variableType == 1){
			PlayerPrefs.SetInt(nameKey, int.Parse(value)); }
		
		if (variableType == 2){
			PlayerPrefs.SetFloat(nameKey, float.Parse(value)); }
		
		if (variableType == 3){
			PlayerPrefs.SetString(nameKey, value); }
	}

	public int GetMyInfoInt(string nameKey)	{
		return PlayerPrefs.HasKey(nameKey) ? PlayerPrefs.GetInt(nameKey) : 0;
	}

	public float GetMyInfoFloat(string nameKey){
		return PlayerPrefs.HasKey(nameKey) ? PlayerPrefs.GetFloat(nameKey) : 0;
	}

	public string GetMyInfoString(string nameKey){
		return PlayerPrefs.HasKey(nameKey) ? PlayerPrefs.GetString(nameKey) : " ";
	}



	/// CHEQUEARR SI HAY INTERNETTT EN LA APP


	public IEnumerator CheckInternet  ( System.Action<bool> hasInternet ){
		bool result = false;
		string chcInternet = "http://2WAYSPORTS.COM/2waysports/Ecuador/Barcelona/ConexionInternet/isConection.php?conection=validarConexion";
		WWW getData = new WWW( chcInternet );
		yield return getData;
		if ( getData.text == "ConexionEstablecida") {
			result = true; }
		hasInternet( result );
	}



	public IEnumerator Consult  ( string link, Dictionary<string ,string > addFields, System.Action<string> Rresult ){
		
		string result = null;
		string url = link;
		WWWForm form = new WWWForm();
		foreach ( var field in addFields ){
			form.AddField(field.Key.Trim(),field.Value.Trim());  }
		WWW newConsult = new WWW( WWW.UnEscapeURL(url) , form );
		yield return newConsult;
		if( !string.IsNullOrEmpty(newConsult.error)  )
			result = newConsult.error;
		else
			result = newConsult.text;
			Rresult( result );

	}
		


	#region VERSION APP
	public void SetMyversion (string v){
		PlayerPrefs.SetString ("MyVersion", v);
	}

	public string GetMyversion (){
		return PlayerPrefs.HasKey("MyVersion") ? PlayerPrefs.GetString ("MyVersion"): "";
	}

	public bool IsVersionRegister(){
		return PlayerPrefs.HasKey("MyVersion");
	}
	#endregion



	public void DeleteVar( string _name){
		PlayerPrefs.DeleteKey (_name);
	}

	public void FinalSesion( ){
		EnableLoading();
		StartCoroutine(_Finalsesion( ));
		User.main.DeleteDataUser();
	}

	IEnumerator _Finalsesion ( ){
		EnableLoading();
		yield return new WaitForEndOfFrame();
		SceneManager.LoadSceneAsync("SeleccionColombia");
//		NavigatorManager.main.saveIndex( 0,0,"Inicio" );
//		DisableLoading();
	}




	public void SimpleActivePopUpInformative( bool act){
		_PopUpInformative.gameObject.SetActive(act);
		IsActivePopUpInfo = act;
	}


	public void popUpInformative ( bool active, string title, string msg ){
		_PopUpInformative.gameObject.SetActive(active);
		if(active){
			_PopUpInformative.titlePopup.text = title;
			_PopUpInformative.msgPopUp.text = msg;
		}
		DisableLoading();
	} 


	public IEnumerator _DisbleLoading( ){
		yield return new WaitForSeconds(1);
		loadingPrincipal.SetActive(false);
	}



	public void ActivePopUpRegistroInfo( bool act){
		popUpRegistroInfo.SetActive(act);
		IsActivePopUpInfo = act;
	}


	public void EnableLoading( ){
		loadingPrincipal.SetActive(true);
	}


	public void DisableLoading( ){
		loadingPrincipal.SetActive(false);
	}


}

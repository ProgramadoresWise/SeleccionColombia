using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using MaterialUI;
using System;

public class NewsManager: MonoBehaviour {

	public static NewsManager main;
	public static NewsManager mainHome;

	private string getUrlTotalNews = "Barcelona/noticiasV1/newsdata.php";
//	[HideInInspector]
	public GameObject ContentScrollList;
//	[HideInInspector]
	public GameObject newPrefab;
//	[HideInInspector]
	public GameObject enableComponentsReset;
//	[HideInInspector]
	public  List<viewNew> newsListActual;
//	[HideInInspector]
	public List <GameObject> newsList;
//	[HideInInspector]
	public BigViewNew bigView;

//	[HideInInspector]
	public Button BackNewsBtn;
//	[HideInInspector]
	public GameObject failLoadNews;

	bool reload;
	public bool update;

	public bool canCreate;


	public DataRowList DatanewsList;



	public int stateCount, temporalCount = 0;

	void Update ( ){
		if( Input.GetMouseButtonUp(0) &&  ContentScrollList.gameObject.GetComponent<RectTransform>().anchoredPosition.y < -230 ){
			if(DatanewsList.dataList.Count == stateCount)
				reloadNews();
			else
				StartCoroutine (createNew());
		}



		if (ContentScrollList.GetComponent<RectTransform> ().anchoredPosition.y > 1000)
			BackNewsBtn.gameObject.SetActive (true);
		else
			BackNewsBtn.gameObject.SetActive (false);
	}

	public void LoadNews( bool enable) {
		NavigatorManager.main.susccesfullEventInback = enable;
		failLoadNews.SetActive(false);

		if(!reload)
			DataApp.main.EnableLoading();

//		StartCoroutine ( DataApp.main.CheckInternet( internet =>{
//			if(internet){
				StartCoroutine (UpdateNews());
//			}else{
//				failLoadNews.SetActive(true);
//				DataApp.main.popUpInformative(true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
//			}
//		}));
	}



	public void reloadNews( ){
		StartCoroutine(UpdateReloadNews());
	}


	IEnumerator UpdateReloadNews( ){

		Debug.Log("VALIDANDO ACTUALIZACION");
		WWW updNew = new WWW(DataApp.main.host + DataApp.main.urlActualizarInfo + (int)idActualizacion.noticias);
		yield return updNew;
		if( string.IsNullOrEmpty( updNew.error ) ){
			if( DataApp.main.GetMyInfoInt("News") ==  int.Parse(updNew.text) ){
				ToastManager.Show("No hay articulos nuevos",2f,null);
				reload = true;
			}
			StartCoroutine (getDatasNew( int.Parse(updNew.text), DataApp.main.host + "Noticias/noticias.php"));
		}else{
			failLoadNews.SetActive(true);
			DataApp.main.popUpInformative(true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
		}

	}




	 IEnumerator UpdateNews( )
	 {
		WWW updNew = new WWW(DataApp.main.host + DataApp.main.urlActualizarInfo + (int)idActualizacion.noticias);
		yield return updNew;
		if( string.IsNullOrEmpty( updNew.error ) ){
			StartCoroutine (getDatasNew( int.Parse(updNew.text), DataApp.main.host + "Noticias/noticias.php"));
		}else{
			failLoadNews.SetActive(true);
			DataApp.main.popUpInformative(true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
		}
		
	}



	public IEnumerator getDatasNew( int NumAct, string url ){

	
			if( DataApp.main.GetMyInfoInt("News") !=  NumAct){
				update = true;
				DataApp.main.EnableLoading();
				yield return StartCoroutine( GetJsonDataScript.getJson.GetPhpData( url));
				DataApp.main.SetMyInfo("JsonNews", GetJsonDataScript.getJson.GetDataConsult(), 3);
				if( DataApp.main.IsRegistered())
					ToastManager.Show("Nuevos articulos Agregados",2f,null);
			}else{
//				if(!reload){
					DataApp.main.EnableLoading();
					string jsonGuardado = DataApp.main.GetMyInfoString("JsonNews");
					yield return StartCoroutine(GetJsonDataScript.getJson.GetLocalData(jsonGuardado));
//				}else{
//					DataApp.main.DisableLoading();
//					yield break;
//				}
			}

			if( GetJsonDataScript.getJson._state == "Successful" ){
				DatanewsList  = GetJsonDataScript.getJson.GetData( DatanewsList , "idnoticia", "linknoticia", "titulonoticia", "descnoticia", "dateNoticia", "isvideo", "noticiaJugador", "noticiaImportante");
				StartCoroutine( createNew());
				DataApp.main.SetMyInfo("News",NumAct.ToString(),1);
				reload =true;
			}else if( GetJsonDataScript.getJson._state == "Warning_01" ){
				print("w1");
				reload =false;
				update = false;
			}else if( GetJsonDataScript.getJson._state == "Warning_02" ){
				print("w2");
				reload =false;
				update = false;
			}else if (GetJsonDataScript.getJson._state == "Warning_03"){
				Debug.Log("Mensaje: Datos con error.");
				DataApp.main.SetMyInfo("JsonNews", "", 3);
				DataApp.main.SetMyInfo("News", "0", 1);
				reload =false;
				update = false;
				StartCoroutine(getDatasNew(NumAct,url));
			}
			
			GetJsonDataScript.getJson._state = "";
		

	}



	void ClearListPrefabs ( ){
		foreach(GameObject g in newsList ){
			Destroy (g);
		}
		newsListActual.Clear();
	}


	public IEnumerator createNew(){
		DataApp.main.EnableLoading();
//		ClearListPrefabs();
		int index = stateCount;

//		NavigatorManager.main.susccesfullEventInback = true;

		if(index >= 8 )
			DataApp.main.DisableLoading();
		
		for( int i = index; i <  DatanewsList.dataList.Count ; i++){
			if( index < DatanewsList.dataList.Count){
				canCreate = true;
				
				yield return new WaitUntil (() => canCreate );
				yield return new WaitForSecondsRealtime(.1f);
//				yield return new WaitForEndOfFrame();
				GameObject nNew = Instantiate( newPrefab ); 
				nNew.transform.SetParent(ContentScrollList.transform, false);
				canCreate =! canCreate;
				newsList.Add( nNew );
				if(index == 8 )
					DataApp.main.DisableLoading();
				
				nNew.gameObject.SetActive(true);
				nNew.name = DatanewsList.dataList[i].GetValueToKey("idnoticia");
				nNew.GetComponent<viewNew>().idNew = int.Parse(DatanewsList.dataList[i].GetValueToKey("idnoticia"));
				nNew.GetComponent<viewNew> ().tituloNew.text = DatanewsList.dataList[i].GetValueToKey("titulonoticia");
				nNew.GetComponent<viewNew> ().descNew.text = DatanewsList.dataList[i].GetValueToKey("descnoticia");
				nNew.GetComponent<viewNew> ().dateNew.text = "                        "+ DatanewsList.dataList[i].GetValueToKey("dateNoticia");
				nNew.GetComponent<viewNew> ().linkVideo = DatanewsList.dataList[i].GetValueToKey("linknoticia");
				nNew.GetComponent<viewNew> ().isvideo =  int.Parse(DatanewsList.dataList[i].GetValueToKey("isvideo"));
				nNew.GetComponent<viewNew> ().selectFunction.text = selFunction (int.Parse(DatanewsList.dataList[i].GetValueToKey("isvideo")));
				nNew.GetComponent<viewNew> ().videoButton.SetActive( actBtnVideo (int.Parse(DatanewsList.dataList[i].GetValueToKey("isvideo"))));
				nNew.GetComponent<viewNew> ().noticiaJugador = int.Parse(DatanewsList.dataList[i].GetValueToKey("noticiaJugador"));
				nNew.GetComponent<viewNew> ().nImportante = DatanewsList.dataList[i].GetValueToKey("noticiaImportante");
				newsListActual.Add(nNew.GetComponent<viewNew>());
//				StartCoroutine (  nNew.GetComponent<viewNew> ().LoadImageInit ( update ));
			}else{
				ToastManager.Show("No hay mas noticias para cargar.",2f,null);
				DataApp.main.DisableLoading();
//				NavigatorManager.main.susccesfullEventInback = false;
				yield break;
			}
			index++;
			stateCount = index;
		}

		canCreate = false;

		DataApp.main.DisableLoading();
		NavigatorManager.main.susccesfullEventInback = false;

		foreach( GameObject nt in newsList){
			if( nt.GetComponent<viewNew>().noticiaJugador != 0  && nt.GetComponent<viewNew>().nImportante != "TRUE"){
				nt.SetActive(false);
			}
		}


	}

	public string selFunction(int opc ){
		string func = null;
		func = ( opc == 1)? func = "Ver video > " : func = "Leer más > ";
		return func;
	}


	public bool actBtnVideo (int opc ){
		bool gm = false;
		gm = ( opc == 1)? gm = true : gm = false;
		return gm;
	}

	public void ResetPosContent( ){
		StartCoroutine (enumResetPosContent ());
	}
		
	IEnumerator enumResetPosContent(){
		enableComponentsReset.SetActive(true);
		float restPosY = ContentScrollList.GetComponent<RectTransform> ().anchoredPosition.y;
		while (restPosY > 0 ){
			if (restPosY <= 20f) {
				restPosY = 0;
				break;
			}
			restPosY-=ContentScrollList.GetComponent<RectTransform> ().anchoredPosition.y/3f;
			yield return new WaitForSeconds (.00001f);
			ContentScrollList.GetComponent<RectTransform> ().anchoredPosition = new Vector2(ContentScrollList.GetComponent<RectTransform> ().anchoredPosition.x, restPosY ) ;
			BackNewsBtn.gameObject.SetActive (false);
		}
		enableComponentsReset.SetActive(false);
	}

	public  string ToAntiCache( string url){
		string r = "";
		r += UnityEngine.Random.Range(1000000,9000000).ToString();
		r += UnityEngine.Random.Range(1000000,9000000).ToString();
		string result="";
		if(url.Substring(url.Length -4,4   ) == ".php" || url.Substring(url.Length -4,4   ) == ".png"|| url.Substring(url.Length -4,4   ) == ".jpg"){
			result = url + "?key=" + r;
		}else{
			result = url + "&key=" + r;
		}
		return result;
	}



}

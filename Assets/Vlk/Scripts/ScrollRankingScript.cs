using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Globalization;

public class ScrollRankingScript : MonoBehaviour {

	public GameObject prefabContentRanking;

	//public ViewUserRankingResultadosScript objResultView;
	public ViewUserScrollResultadosScript objResultView;

	public GameObject ScrollContent;

	public Dropdown rankingOptions;

	public Text fechaText;
	public Text infNoPuntajeText;

	public GameObject BlockPanel;

	[SerializeField]
	private Text shareRankingText;// = new GetJsonDataScript ();

//	[SerializeField]
//	private ContentRankingListScript rankingList;

	[SerializeField]
	private DataRowList rankingList;

	[SerializeField]
	private ContentRankingListScript userRanking;

	[SerializeField]
	private bool rankingIsLoad = false;

	//private bool imageRankingIsLoad = false;

	private string urlPhpPronostico = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/UserPronostico.php?";

	private string urlPhpUltimoPartido = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/GetDate.php?";

	private string urlPhpHistorialRanking = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/RankingHistory.php";

	private string urlHistorialRankingImg = "http://2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/HistoryRankingImg/";

	private float initContentPosY = 0f;

	private bool isRefresh = false;

	//public ScrollRect scroll;

	//----------------------

	public List<GameObject> rankingsData = new List<GameObject>();

	[SerializeField]
	private ContentRankingScript userDataRankingHeader;

	[SerializeField]
	private ContentRankingScript userDataRankingFooter;
	[SerializeField]
	private ContentRankingScript userDataRankingFooter2;

	[SerializeField]
	public DateClassList fechaList;

	[SerializeField]
	private DatesRankingHistoryListScript historialRankingList;
	public DatesRankingHistoryListScript _historialRankingList {get{ return historialRankingList; } set{ historialRankingList = value; }}

	private int hieghtScrollContent = 103;

	// Use this for initialization
	void Start () {

		initContentPosY = ScrollContent.GetComponent<RectTransform> ().position.y - 80;
	}
	
	// Update is called once per frame
	void Update () {

		if(ScrollContent.GetComponent<RectTransform> ().position.y < initContentPosY && !isRefresh && !Input.GetMouseButton(0)){

			isRefresh = true;

			ChangeToRanking();
		}
	}

	public void UpdateRankingIsLoad(){

		rankingIsLoad = false;
	}

	private IEnumerator ValidateStatusRanking (){

		//string url = "http://2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/UserPronosticoSColombia.php?indata=buscarRankingPronostico";
		string url = DataApp.main.host + "PollaTricolor/Php/GetTopRankings.php?" + "action=GetTopRankings";

		yield return StartCoroutine (GetJsonDataScript.getJson.GetPhpData (url));

		if (GetJsonDataScript.getJson._state == "Successful") {

			rankingList = GetJsonDataScript.getJson.GetData (rankingList, "userID", "nameText", "points");

			StartCoroutine (ShowRanking ());

		} else if (GetJsonDataScript.getJson._state == "Warning_01") {

			StartCoroutine(SetUserDataRanking ());
		
		} else if (GetJsonDataScript.getJson._state == "Warning_02") {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}

		Debug.Log ("Json Lista Ranking: " + JsonUtility.ToJson(rankingList));
	}

	private IEnumerator ValidateStatusRankingFecha (){

//		WWW getData = new WWW (urlPhpUltimoPartido + "indata=getNextDate" + "&idDate=" + 2);
//		yield return getData;

		WWW getData = new WWW (DataApp.main.host + "PollaTricolor/Php/GetFechasPartidos.php?" + "action=GetFechaPartido" + "&idDate=" + 2);
		yield return getData;

		if (string.IsNullOrEmpty (getData.error)) {

			if (getData.text != "PredictingUncreated") {

				fechaList = JsonUtility.FromJson<DateClassList> (getData.text);
				StartCoroutine (GetUsersPorFecha ());
			} else {

				Debug.Log("¡No hay datos!");
			}
		} else {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}

		Debug.Log ("GETDATA: " + getData.text);
	}

	private IEnumerator ValidateStatusRankingHistorialRanking (/*string action*/){

		WWW getData = new WWW (urlPhpHistorialRanking + "?indata=getDatesRankingHistorial");
		yield return getData;

		if (string.IsNullOrEmpty (getData.error)) {

			if (getData.text != "PredictingUncreated") {

				historialRankingList = JsonUtility.FromJson<DatesRankingHistoryListScript> (getData.text);

				//UpdateDropDownRankings ();
				//Debug.Log ("AQUI!!!");
				StartCoroutine(GetHistoryRankingImgInf ());

			} else {

				Debug.Log("¡No hay datos!");
			}
		} else {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}

		Debug.Log ("GET Historial Ranking DATA: " + getData.text);
	}

	private IEnumerator GetHistoryRankingImgInf(){

		//Debug.Log ("AQUI!!!");

		foreach (HistoryDate history in historialRankingList.dataList) {

			Debug.Log (urlHistorialRankingImg + history.indexPosition + "_HistoryShields" + ".png");

			WWW img = new WWW(urlHistorialRankingImg + history.indexPosition + "_HistoryShields" + ".png");
			yield return img;

			if (string.IsNullOrEmpty (img.error)) {

				Rect size = new Rect (0, 0, img.texture.width, img.texture.height);
				history._imgInf = Sprite.Create (img.texture, size, Vector2.zero);
			} else {

				history._imgInf = Resources.Load <Sprite> ("Various/Default_HistoryShields");
				Debug.Log ("Load Image Failed Set DefaultSilhouette" + history.indexPosition + "-" + history.date);
			}
		}

		UpdateDropDownRankings ();

		//yield return null;
	}

	private void UpdateDropDownRankings (){

		rankingOptions.options.Clear ();

		rankingOptions.value = 0;

		rankingOptions.options.Add (new Dropdown.OptionData ("<b>RANKING GENERAL</b> \nPRONÓSTICO"));

		//rankingOptions.options.Add (new Dropdown.OptionData ("<b>RANKING ÚLTIMO PARTIDO</b> \nPRONÓSTICO"));

		string optionText;

		DateTime date;

		CultureInfo ci = new CultureInfo ("es-ES");

		foreach (HistoryDate obj in historialRankingList.dataList) {

			if (obj.name != "none") {

				date = DateTime.Parse (obj.date);

				string mes = date.ToString ("MMMM", ci);
				string dia = date.ToString ("dddd", ci);
				string diaNum = date.ToString ("dd", ci).ToUpper ();

				mes = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (mes);

				dia = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (dia);

				optionText = "VS\n\n" + dia + " " + diaNum + " de " + mes;

				//rankingOptions.options.Add (new Dropdown.OptionData (optionText.ToUpper ()));
				rankingOptions.options.Add (new Dropdown.OptionData (optionText.ToUpper (), obj._imgInf));
				//rankingOptions.options.Add (new Dropdown.OptionData (namePlayer, player._imgPhoto));

			} else {
			
				break;
			}
		}

		StartCoroutine (DestroyCurrentRankingData ());

		//UpdateDataScript.updateData.StopUpdatePanel ();
	}

	private IEnumerator GetUsersPorFecha(){

		DateTime date = DateTime.Parse (fechaList.dataList [0].fecha);
		//DateTime date = DateTime.Parse (historialRankingList.dataList[rankingOptions.value-1].date);
		//DateTime nextDate = DateTime.Parse (fechaList.dataList [0].fecha + " " + fechaList.dataList [0].hora);

		CultureInfo ci = new CultureInfo("es-ES");
		string mes =  date.ToString("MMMM",ci);
		string dia =  date.ToString("dddd",ci);
		string diaNum = date.ToString("dd",ci).ToUpper();

//		string url = "http://2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/UserPronosticoSColombia.php?" + "indata=PronosticoRankingFecha" + "&buscarFecha=" + date.Year + "-" + date.Month + "-" + date.Day;
		//string url = urlPhpPronostico + "indata=PronosticoRankingFecha" + "&buscarFecha=" + date.Year + "-" + date.Month + "-" + date.Day;
		string url = DataApp.main.host + "PollaTricolor/Php/GetTopRankings.php?" + "action=GetTopRankingsFecha" + "&buscarFecha=" + date.Year + "-" + date.Month + "-" + date.Day;

//		Debug.Log("LA URL ES: " + url); 

		yield return StartCoroutine (GetJsonDataScript.getJson.GetPhpData (url));

		if (GetJsonDataScript.getJson._state == "Successful") {

			rankingList = GetJsonDataScript.getJson.GetData (rankingList, "userID", "nameText", "points");

			StartCoroutine (ShowRanking ());

		} else if (GetJsonDataScript.getJson._state == "Warning_01") {

			StartCoroutine(SetUserDataRanking ());

		} else if (GetJsonDataScript.getJson._state == "Warning_02") {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}

		Debug.Log ("Json Lista Ranking: " + JsonUtility.ToJson(rankingList));

		Debug.Log ("FECHA PARTIDO ANTERIOR: " + date.Year + "-" + date.Month + "-" + date.Day);
	}

	private IEnumerator ShowRanking(){

		//ShowUpdatePopup ();

		ScrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector3 (0, 0, 0);

		ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 0);

		int index = 0;

		int rankingPos = 0;

		foreach (DataRow obj in rankingList.dataList) {

			ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, ScrollContent.GetComponent<RectTransform> ().sizeDelta.y + hieghtScrollContent);

			if (index < rankingsData.Count ) {

				rankingsData[index].gameObject.SetActive(true);

				rankingsData[index].GetComponent<ContentRankingScript> ().userPhoto.GetComponent<LoadImgRankingScript> ().loadImgRanking (obj.GetValueToKey("userID") + ".jpg");
				rankingsData[index].GetComponent<ContentRankingScript> ().nameText.text = obj.GetValueToKey("nameText");
				rankingsData [index].GetComponent<ContentRankingScript> ().pointsText.text = obj.GetValueToKey ("points");
				rankingsData[index].GetComponent<ContentRankingScript> ().userID = int.Parse(obj.GetValueToKey("userID"));

			} else {

				//obj.gameObject.SetActive(false);
				GameObject panelData = Instantiate (prefabContentRanking);
				//panelData.GetComponent<CanvasGroup> ().alpha = 0f;
				panelData.transform.SetParent (ScrollContent.transform);
				rankingsData.Add (panelData);
				panelData.GetComponent<RectTransform>().anchoredPosition = new Vector3 (0, index * -hieghtScrollContent, 0);
				panelData.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);
				yield return new WaitForEndOfFrame (); 
				panelData.GetComponent<ContentRankingScript> ().userPhoto.GetComponent<LoadImgRankingScript> ().loadImgRanking (obj.GetValueToKey("userID") + ".jpg");
				panelData.GetComponent<ContentRankingScript> ().nameText.text = obj.GetValueToKey("nameText");
				panelData.GetComponent<ContentRankingScript> ().pointsText.text = obj.GetValueToKey("points");
				panelData.GetComponent<ContentRankingScript> ().userID = int.Parse(obj.GetValueToKey("userID"));
			}

			rankingPos = index + 1;
			rankingsData[index].GetComponent<ContentRankingScript> ().objResultView = objResultView;
			rankingsData[index].GetComponent<ContentRankingScript> ().indexPosition = rankingPos;

			rankingsData[index].GetComponent<ContentRankingScript> ().posRankingText.gameObject.SetActive (true);
			rankingsData[index].GetComponent<ContentRankingScript> ().posRankingText.text = rankingPos.ToString ();

//			if (rankingPos == 1 || rankingPos == 2 || rankingPos == 3) {
//
//				rankingsData[index].GetComponent<ContentRankingScript> ().cup.gameObject.SetActive (true);
//				rankingsData[index].GetComponent<ContentRankingScript> ().cupRankingText.text = rankingPos.ToString ();
//
//				if (rankingPos == 1) {
//					rankingsData[index].GetComponent<ContentRankingScript> ().cup.color = new Color32 (250, 157, 0, 255);
//
//				} else if (rankingPos == 2) {
//					rankingsData[index].GetComponent<ContentRankingScript> ().cup.color = new Color32 (170, 170, 170, 255);
//
//				} else if (rankingPos == 3) {
//					rankingsData[index].GetComponent<ContentRankingScript> ().cup.color = new Color32 (124, 55, 24, 255);
//
//				}
//			} else {
//				rankingsData[index].GetComponent<ContentRankingScript> ().posRankingText.gameObject.SetActive (true);
//				rankingsData[index].GetComponent<ContentRankingScript> ().posRankingText.text = rankingPos.ToString ();
//
//			}

			yield return rankingsData [index];

			index++;
		}

		for(index=index; index < rankingsData.Count; index++){

			rankingsData[index].gameObject.SetActive(false);
		}

		if (ScrollContent.GetComponent<RectTransform> ().sizeDelta.y < 808) {

			ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 808);
		}

		//EndUpdateData ("none");

		StartCoroutine(SetUserDataRanking ());
	}

	private IEnumerator SetUserDataRanking(){

		//ShowUpdatePopup ();

		string urlPhp = "";

		if(rankingOptions.value == 0){

			//urlPhp = urlPhpPronostico + "indata=buscarUserRanking&buscarUserID=" + DataApp.main.GetMyID();
			urlPhp = DataApp.main.host + "PollaTricolor/Php/GetUsersRankings.php?" + "action=GetRankingUser&buscarUserID=" + DataApp.main.GetMyID();

		} else if(rankingOptions.value == 1){
		//} else if(rankingOptions.value > 0){
			
			DateTime date = DateTime.Parse (fechaList.dataList [0].fecha);

			//DateTime date = DateTime.Parse (historialRankingList.dataList[rankingOptions.value-1].date);

			string fechaEncuentro = "&buscarFecha=" + date.Year + "-" + date.Month + "-" + date.Day;

			Debug.Log ("FECHA:" + fechaEncuentro);

			//urlPhp = urlPhpPronostico + "indata=buscarUserRankingPorFecha" + fechaEncuentro + "&buscarUserID=" + DataApp.main.GetMyID();
			urlPhp = DataApp.main.host + "PollaTricolor/Php/GetUsersRankings.php?" + "action=GetRankingUserFecha" + fechaEncuentro + "&buscarUserID=" + DataApp.main.GetMyID();
		}

		WWW getData = new WWW(urlPhp);

		yield return getData;

		if (string.IsNullOrEmpty (getData.error)) {

			if (getData.text != "PredictingUncreated") {

				userRanking = JsonUtility.FromJson<ContentRankingListScript> (getData.text);
				StartCoroutine (ShowUserRanking ());
			} else {
				
				StartCoroutine(NoHayDatos());
			}
		} else {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}

		SetSubtitleRankingInf ();

		Debug.Log (getData.text);
	}

	private IEnumerator ShowUserRanking(){
	
		ContentRanking obj = userRanking.dataList[0];
		userDataRankingFooter.posRankingText.text = obj.userID.ToString ();
		userDataRankingFooter.nameText.text = User.main.GetMyName();
		userDataRankingFooter.pointsText.text = obj.points.ToString ();
		//userDataRankingFooter.userPhoto.GetComponent<LoadImgRankingScript> ().loadImgRanking (DataApp.main.GetMyID() + ".jpg");
		userDataRankingFooter.userPhoto.sprite = ImgLoadManager.main.UsersImg(userDataRankingFooter.userPhoto, DataApp.main.GetMyID().ToString(), false);

		Debug.Log (obj.userID.ToString () + " / " + obj.nameText);

		yield return null;

		EndUpdateData ("none");
	}

	private IEnumerator NoHayDatos(){

		//ShowUpdatePopup ();

		infNoPuntajeText.text = "No se ha generado puntake de la ultima fecha";

		//ContentRanking obj = userRanking.dataList[0];

		userDataRankingHeader.posRankingText.text = "0";
		//userDataRankingHeader.nameText.text = obj.nameText;
		userDataRankingHeader.pointsText.text = "0";

		//userDataRankingHeader.userPhoto.GetComponent<LoadImgRankingScript> ().loadImgRanking (1 + ".jpg");

		userDataRankingFooter.posRankingText.text = "0";
		//userDataRankingFooter.nameText.text = obj.nameText;
		userDataRankingFooter.pointsText.text = "0";

		userDataRankingFooter2.posRankingText.text = "0";
		//userDataRankingFooter.nameText.text = obj.nameText;
		userDataRankingFooter2.pointsText.text = "0";

		//userDataRankingFooter.userPhoto.GetComponent<LoadImgRankingScript> ().loadImgRanking (obj.userID + ".jpg");

		//Debug.Log (obj.userID.ToString ());

		yield return null;

		EndUpdateData ("none");
	}

	private void SetSubtitleRankingInf (){

		if(rankingOptions.value == 0){

			fechaText.text = "Ranking general";

		} else if(rankingOptions.value == 1){

			//DateTime date = DateTime.Parse (fechaList.dataList [0].fecha);

			DateTime nextDate = DateTime.Parse (fechaList.dataList [0].fecha);

			CultureInfo ci = new CultureInfo ("es-ES");
			//DateTime pastMonth = Date.AddMonths(0);
			string mes = nextDate.ToString ("MMMM", ci);
			string dia = nextDate.ToString ("dddd", ci);
			string diaNum = nextDate.ToString ("dd", ci).ToUpper ();

			mes = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (mes);

			dia = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (dia);

			fechaText.text = dia + " " + diaNum + " de " + mes;
		}
	}

	public void ChangeToRanking(){

		ShowUpdatePopup ();

		fechaText.text = "";
		infNoPuntajeText.text = "";

		//SetDropdownNameSelected ();

		StartCoroutine(DestroyCurrentRankingData ());
	}

	private void SetDropdownNameSelected () {
	
		if(rankingOptions.value == 0){

			rankingOptions.captionText.text = "<b>RANKING GENERAL</b> \nPRONÓSTICO";

		} else if(rankingOptions.value > 0){

			DateTime date = DateTime.Parse (historialRankingList.dataList [rankingOptions.value-1].date);

			CultureInfo ci = new CultureInfo ("es-ES");
			string mes = date.ToString ("MMMM", ci);
			string dia = date.ToString ("dddd", ci);
			string diaNum = date.ToString ("dd", ci).ToUpper ();

			mes = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (mes);

			dia = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (dia);

			String optionText = dia + " " + diaNum + " de " + mes;

			//rankingOptions.captionText.text = "<b>RANKING PARTIDO</b> \n" + optionText.ToUpper ();
			rankingOptions.captionText.text = "VS \n\n" + optionText.ToUpper ();

			//shareRankingText.text = historialRankingList.dataList [rankingOptions.value-1].name.ToUpper ();
		}
	}

	private IEnumerator DestroyCurrentRankingData(){

		/*foreach(GameObject obj in rankingsData){

			//Destroy (obj);

			obj.SetActive (false);

			//yield return obj;
		}*/

		//rankingsData.Clear ();

		//shareRankingText.text = rankingOptions.captionText.text;

		if(rankingOptions.value == 0){

			Debug.Log ("ESTADO: " + "GENERAL");

			shareRankingText.text = "GENERAL";

			StartCoroutine(ValidateStatusRanking ());

		} else if(rankingOptions.value == 1){
		//} else if(rankingOptions.value > 0){

			Debug.Log ("ESTADO: " + "ULTIMO PARTIDO");
			
			shareRankingText.text = "ÚLTIMO PARTIDO";

			StartCoroutine(ValidateStatusRankingFecha ());

//			DateTime date = DateTime.Parse (historialRankingList.dataList [rankingOptions.value-1].date);
//
//			CultureInfo ci = new CultureInfo ("es-ES");
//			string mes = date.ToString ("MMMM", ci);
//			string dia = date.ToString ("dddd", ci);
//			string diaNum = date.ToString ("dd", ci).ToUpper ();
//
//			mes = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (mes);
//
//			dia = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (dia);
//
//			String optionText = dia + " " + diaNum + " de " + mes;
//
//			shareRankingText.text = optionText.ToUpper ();
//
//			StartCoroutine (GetUsersPorFecha ());
		}

		yield return null;
	}

	public void RunRankingPanel (){
	
		if (rankingIsLoad == false) {

			ShowUpdatePopup ();

			//StartCoroutine(ValidateStatusRankingHistorialRanking ());

			StartCoroutine(ValidateStatusRanking ());
			//StartCoroutine (DestroyCurrentRankingData ());
		} else {
		
			if(rankingOptions.value == 0){

				shareRankingText.text = "GENERAL";

			//} else if(rankingOptions.value == 1){
			} else if(rankingOptions.value > 0){

				//shareRankingText.text = "ÚLTIMO PARTIDO";

				shareRankingText.text = historialRankingList.dataList [rankingOptions.value-1].name.ToUpper ();
			}
		}
	}

//	private void InitValidation ( ){ 
//		//RegistrationUserNClub.Metodo = PlayerPrefs.GetString("Metodo");
//		if(DataApp.main.IsRegistered( )){   
//			//_id = DataApp.main.GetMyID( );
//			if (RegisterAndLogin.isActive == 1) {
//
//				RunRankingPanel ();
//
//			} else {
//
//				BlockPanel.SetActive (true);
//			}
//
//		}else{
//			//PopUpInitValidation.SetActive(true);
//		}
//	}

	void OnEnable()
	{
		//shareRankingText.text = rankingOptions.captionText.text;
		//InitValidation();
		RunRankingPanel ();

	}

//	void OnEnable() {
//
//		if (rankingIsLoad == false) {
//
//			ShowUpdatePopup ();
//
//			StartCoroutine(ValidateStatusRanking ());
//		}
//	}

	public void EndUpdateData(string state){

		isRefresh = false;
		rankingIsLoad = true;
		HideUpdatePopup ();
	}

	private void ShowUpdatePopup(){

		GameObject eventManager = GameObject.Find ("EventManager");
		eventManager.GetComponent<NavigationTabPanelsScript> ().ShowUpdateDataPanel();
	}

	private void HideUpdatePopup(){

		GameObject eventManager = GameObject.Find ("EventManager");
		eventManager.GetComponent<NavigationTabPanelsScript> ().HideUpdateDataPanel();
	}

	private void ShowMessagePopup(string message, int indexTabContinue){

		GameObject eventManager = GameObject.Find ("EventManager");
		eventManager.GetComponent<NavigationTabPanelsScript> ().ShowPopup (message, indexTabContinue);
	}
}

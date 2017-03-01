﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Globalization;

public class ViewUserScrollResultadosScript : MonoBehaviour {

	//private string[] titleGols = new [] {"PRIMER","SEGUNDO","TERCER","CUARTO","QUITO","SEXTO","SÉPTIMO","OCTAVO","NOVENO","DÉCIMO","ONCEAVO","DOCEAVO","TRECEAVO","CATORCEAVO","QUINCEAVO","DIECISEISAVO","DIECISIETEAVO","DIECIOCHOAVO","DIECINUEVEAVO","VEINTEAVO"};
	private string[] titleGols = new [] {"Primer","Segundo","Tercer","Cuarto","Quinte","Sexto","Séptimo","Octavo","Noveno","Décimo","Onceavo","Doceavo","Treceavo","Catorceavo","Aquinceavo","Dieciseisavo","Diecisieteavo","Dieciochoavo","Diecinueveavo","Veinteavo"};

	public ContentRankingScript updateUserInf;

	private string signOfPoints = "+";

	public GameObject prefabContentGolPanel;
	//public GameObject prefabTitleContent;
	//public GameObject prefabResultContent;
	public GameObject ScrollContent;

	public GameObject titleMarcador;
	public GameObject marcadorPanel;

	[Header("Escudos:")]

	[SerializeField]
	private Image EscudoEquipoHeader;
	[SerializeField]
	private Image EscudoOponenteHeader;
	[SerializeField]
	private Image EscudoEquipoLeft;
	[SerializeField]
	private Image EscudoOponenteLeft;
	[SerializeField]
	private Image EscudoEquipoRight;
	[SerializeField]
	private Image EscudoOponenteRight;
	[Space(20)]

	public Text BarcelonaGolesLeft;
	public Text BarcelonaGolesRight;
	public Text OponenteGolesLeft;
	public Text OponenteGolesRight;
	public Text marcadorPoints;
	//public Image marcadorBall;

	//public RawImage imageNews1;
	//public RawImage imageNews2;
	//public RawImage imageNews3;

	private WWW www;
	private string urlTemp1;

	public Text totalPoints;
	private int totalPnts = 0;

	public bool resultIsLoad = false;

	private string urlPhpPronostico = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/UserPronostico.php?";
	//private string urlPhpPronostico = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/UserPronosticoTest.php?";
	private string urlPhpUltimoPartido = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/GetDate.php?";

	public GameObject textInfoHeader;

	public ScrollRankingScript updateRanking;


	[SerializeField]
	public GameObject alignmentDatasPanel;

	[SerializeField]
	public List<AlignmentResultData> alignmentResultDataList;

	[SerializeField]
	public DateClassList fechaList;

	private DateTime date;

	public Text fechaText;

	private RawImage loadImg;

	[SerializeField]
	private ContentResultListScript resultList;

	[SerializeField]
	private DataRowList alignmentResultList;

	[SerializeField]
	private DataRowList scoreResultList;

	public List<GameObject> resultsData = new List<GameObject>();

	private int idUsuario = 0;

	public Text fechaPronosticoText;

	private int heightContent = 491;

	public void RunViewUserRankingResult(int userID, DateClassList fecha){

		Debug.Log ("El ID del usuario recibido es: " + userID + " y la fecha: " + fecha);

		gameObject.SetActive (true);

		DateTime newDate = DateTime.Parse (fecha.dataList[0].fecha);

		date = newDate;

		idUsuario = userID;

		CultureInfo ci = new CultureInfo ("es-ES");
		//DateTime pastMonth = Date.AddMonths(0);
		string mes = date.ToString ("MMMM", ci);
		string dia = date.ToString ("dddd", ci);
		string diaNum = date.ToString ("dd", ci).ToUpper ();

		fechaText.text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (dia) + " " + diaNum + " de " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase (mes);

		//StartCoroutine (GetResultados ());
		//StartCoroutine (ClearAndEnablePrediction (fecha));
		resultIsLoad = true;
		StartCoroutine (ValidateStatusResultados (fecha));
	}

	private IEnumerator ValidateStatusResultados (DateClassList fecha){

		fechaList = fecha;
		setupDate();

		yield return null;
	}

	private void setupDate(){

		DateTime newDate = DateTime.Parse (fechaList.dataList [0].fecha);

		date = newDate;
		//DateTime nextDate = DateTime.Parse (fechaList.dataList [0].fecha + " " + fechaList.dataList [0].hora);

		CultureInfo ci = new CultureInfo ("es-ES");
		//DateTime pastMonth = Date.AddMonths(0);
		string mes = date.ToString ("MMMM", ci);
		string dia = date.ToString ("dddd", ci);
		string diaNum = date.ToString ("dd", ci).ToUpper ();

		fechaText.text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (dia) + " " + diaNum + " de " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase (mes);

		StartCoroutine (ClearAndEnablePrediction ("NuevaFecha"));
	}

	private IEnumerator ExistenResultados(){
		
		string fechaEncuentro = "&buscarFecha=" + date.Year + "-" + date.Month + "-" + date.Day;

		WWW getData = new WWW (urlPhpPronostico + "indata=buscarResultado" + fechaEncuentro + "&buscarUserID=" + DataApp.main.GetMyID());
		yield return getData;

		if (string.IsNullOrEmpty (getData.error)) {

			if (getData.text != "PredictingUncreated") {

				if (resultIsLoad) {

					HideUpdatePopup ();

				} else {

					StartCoroutine (GetResultados ());;
				}
				//EndUpdateData ("none");


			} else {

					resultIsLoad = false;
					StartCoroutine (ClearAndEnablePrediction ("Limpiar"));
			}
		} else {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}
	}

	private IEnumerator ClearAndEnablePrediction(string action){

		foreach (GameObject objPanel in resultsData) {

			Destroy (objPanel);
		}

		resultsData.Clear ();

		yield return resultsData;

		//fechaText.text = "";

		titleMarcador.SetActive (false);
		marcadorPanel.SetActive (false);

		totalPoints.text = "";


		if (action == "NuevaFecha") {

			StartCoroutine (GetResultados ());

		} else if (action == "Limpiar") {

			ShowMessagePopup ("Aún no hay resultados. Cuando termine el partido podrás consultarlos", 1);

		}

		//StartCoroutine (ValidateStatusResultados (fecha));
	}

	private IEnumerator GetResultados (){

		//string fechaEncuentro = "&buscarFecha=" + "2016-10-14";
		string fechaEncuentro = "&buscarFecha=" + date.Year + "-" + date.Month + "-" + date.Day;

		string url = UrlsAdmin.self._ftpUrl + "PollaTricolor/Php/GetUserResults.php" + "?action=GetGoalsUserResult" + fechaEncuentro + "&buscarUserID=" + idUsuario;//DataApp.main.GetMyID ();

		Debug.Log ("LA URLLLLLLLLLL ES: " + url);

		//WWW getData = new WWW (urlPhpPronostico + "indata=buscarResultado" + fechaEncuentro + "&buscarUserID=" + DataApp.main.GetMyID());
		WWW getData = new WWW (url);

		yield return getData;

		if (string.IsNullOrEmpty (getData.error)) {

			if (getData.text != "PredictingUncreated") {

				resultList = JsonUtility.FromJson<ContentResultListScript> (getData.text);
				//StartCoroutine (ShowResultados ());
				StartCoroutine (GetAlingnmentResult ());

			} else {
				Debug.Log ("AQUI!!!!!GerR");
				ShowMessagePopup ("Aún no hay resultados. Cuando termine el partido podrás consultarlos", 1);
			}
		} else {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}

		Debug.Log (getData.text);
	}

	private IEnumerator GetAlingnmentResult(){

		string fechaEncuentro = "&buscarFecha=" + date.Year + "-" + date.Month + "-" + date.Day;
	
		string url = UrlsAdmin.self._ftpUrl + "PollaTricolor/Php/GetUserResults.php" + "?action=GetAlignmentUserResult" + fechaEncuentro + "&buscarUserID=" + idUsuario;//DataApp.main.GetMyID ();

		Debug.Log ("LA URL ES: " + url);

		yield return StartCoroutine (GetJsonDataScript.getJson.GetPhpData (url));

		if (GetJsonDataScript.getJson._state == "Successful") {

			alignmentResultList = GetJsonDataScript.getJson.GetData (alignmentResultList, "AlinPlayer1", "AlinRealPlayer1", "PtnAlign1", "AlinPlayer2", "AlinRealPlayer2", "PtnAlign2", "AlinPlayer3", "AlinRealPlayer3", "PtnAlign3", "AlinPlayer4", "AlinRealPlayer4", "PtnAlign4", "AlinPlayer5", "AlinRealPlayer5", "PtnAlign5", "AlinPlayer6", "AlinRealPlayer6", "PtnAlign6", "AlinPlayer7", "AlinRealPlayer7", "PtnAlign7", "AlinPlayer8", "AlinRealPlayer8", "PtnAlign8", "AlinPlayer9", "AlinRealPlayer9", "PtnAlign9", "AlinPlayer10", "AlinRealPlayer10", "PtnAlign10", "AlinPlayer11", "AlinRealPlayer11", "PtnAlign11", "AcertoTodo");

			//StartCoroutine (ShowResultados ());

			StartCoroutine (GetScoreDate());

		} else if (GetJsonDataScript.getJson._state == "Warning_01") {

			// adasdasdasdasdasd

		} else if (GetJsonDataScript.getJson._state == "Warning_02") {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}
	}

	private IEnumerator GetScoreDate(){
	
		string fechaEncuentro = "&buscarFecha=" + date.Year + "-" + date.Month + "-" + date.Day;

		string url = UrlsAdmin.self._ftpUrl + "PollaTricolor/Php/GetUserResults.php" + "?action=GetScoreFecha" + fechaEncuentro + "&buscarUserID=" + idUsuario;//DataApp.main.GetMyID ();

		Debug.Log ("LA URL ES: " + url);

		yield return StartCoroutine (GetJsonDataScript.getJson.GetPhpData (url));

		if (GetJsonDataScript.getJson._state == "Successful") {

			scoreResultList = GetJsonDataScript.getJson.GetData (scoreResultList, "ScoreResultado");

			StartCoroutine (ShowResultados ());

		} else if (GetJsonDataScript.getJson._state == "Warning_01") {

			// adasdasdasdasdasd

		} else if (GetJsonDataScript.getJson._state == "Warning_02") {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}
	}

	private IEnumerator ShowResultados(){

		int index = 0;

		totalPnts = 0;

		//ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (728, 160);
		ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 742);

		if (resultList.dataList [0].autor_Gol != "none" || resultList.dataList[0].autor_Gol_Result != "none") {

			ContentResultListScript golesNoPredecidos = new ContentResultListScript();

			foreach(ContentResult obj in resultList.dataList){

				if (obj.min_Gol != -2) {

					GameObject resultData = Instantiate (prefabContentGolPanel);

					Debug.Log ("CREO: " + index);

					resultData.transform.SetParent (ScrollContent.transform);
					resultsData.Add (resultData);
					resultData.GetComponent<RectTransform>().anchoredPosition = new Vector3 (0, -742 + index * -heightContent, 0);
					resultData.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);
					yield return new WaitForEndOfFrame ();

					ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, ScrollContent.GetComponent<RectTransform> ().sizeDelta.y + heightContent);

					resultData.GetComponent<ContentGolScript> ().SetDataGolPanel ("setNumGol", titleGols [index] + " Gol", titleGols [index] + " Gol", 0);
					resultData.GetComponent<ContentGolScript> ().SetDataGolPanel ("setAnotador", obj.autor_Gol, obj.autor_Gol_Result, obj.autor_Gol_Points);
					resultData.GetComponent<ContentGolScript> ().SetDataGolPanel ("setMinuto", obj.min_Gol.ToString (), obj.min_Gol_Result.ToString (), obj.min_Gol_Points);
					resultData.GetComponent<ContentGolScript> ().SetDataGolPanel ("setTipoGol", obj.tipo_Gol, obj.tipo_Gol_Result, obj.tipo_Gol_Points);
					resultData.GetComponent<ContentGolScript> ().SetDataGolPanel ("setAsistencia", obj.autor_PaseGol, obj.autor_PaseGol_Result, obj.autor_PaseGol_Points);

					//totalPnts = (obj.prediction_Points != 0) ? totalPnts * obj.prediction_Points : totalPnts;
					totalPnts = (obj.prediction_Points > 1) ? totalPnts + obj.prediction_Points : totalPnts;

					index++;

				} else {
				
					golesNoPredecidos.dataList.Add (obj);
				}

				yield return null;
			}

			if(golesNoPredecidos.dataList.Count > 0){

				golesNoPredecidos.SortMinNegativos ();

				foreach(ContentResult obj in golesNoPredecidos.dataList){

					ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, ScrollContent.GetComponent<RectTransform> ().sizeDelta.y + heightContent);

					GameObject resultData = Instantiate (prefabContentGolPanel);

					resultData.transform.SetParent (ScrollContent.transform);
					resultsData.Add (resultData);
					resultData.GetComponent<RectTransform>().anchoredPosition = new Vector3 (0, -742 + index * -heightContent, 0);
					resultData.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);
					yield return new WaitForEndOfFrame (); //WaitForSeconds(0.1f);

					resultData.GetComponent<ContentGolScript> ().SetDataGolPanel ("setNumGol", titleGols[index] + " Gol", titleGols[index] + " GSol", 0);
					resultData.GetComponent<ContentGolScript> ().SetDataGolPanel ("setAnotador", obj.autor_Gol, obj.autor_Gol_Result, obj.autor_Gol_Points);
					resultData.GetComponent<ContentGolScript> ().SetDataGolPanel ("setMinuto", obj.min_Gol.ToString(), obj.min_Gol_Result.ToString(), obj.min_Gol_Points);
					resultData.GetComponent<ContentGolScript> ().SetDataGolPanel ("setTipoGol", obj.tipo_Gol, obj.tipo_Gol_Result, obj.tipo_Gol_Points);
					resultData.GetComponent<ContentGolScript> ().SetDataGolPanel ("setAsistencia", obj.autor_PaseGol, obj.autor_PaseGol_Result, obj.autor_PaseGol_Points);

					//totalPnts = (obj.prediction_Points != 0) ? totalPnts*obj.prediction_Points : totalPnts;
					totalPnts = (obj.prediction_Points > 1) ? totalPnts + obj.prediction_Points : totalPnts;

					index++;

					yield return null;
				}
			}
		}

		titleMarcador.SetActive (true);
		marcadorPanel.SetActive (true);

		ContentResult marcador = resultList.dataList [0];

		string oponente = fechaList.dataList [0].equipo.Replace (".", "");

		if (marcador.esLocal == 1) {

			EscudoEquipoHeader.sprite = ImgLoadManager.main.teamImg (EscudoEquipoHeader, "COLOMBIA", false);
			EscudoOponenteHeader.sprite = ImgLoadManager.main.teamImg (EscudoOponenteHeader, oponente.ToUpper(), false);

			EscudoEquipoLeft.sprite = ImgLoadManager.main.teamImg (EscudoEquipoLeft, "COLOMBIA", false);
			EscudoOponenteLeft.sprite = ImgLoadManager.main.teamImg (EscudoOponenteLeft, oponente.ToUpper(), false);

			EscudoEquipoRight.sprite = ImgLoadManager.main.teamImg (EscudoEquipoRight, "COLOMBIA", false);
			EscudoOponenteRight.sprite = ImgLoadManager.main.teamImg (EscudoOponenteRight, oponente.ToUpper(), false);

			BarcelonaGolesLeft.text = marcador.goles_Barcelona.ToString ();
			OponenteGolesLeft.text = marcador.goles_Oponente.ToString ();

			BarcelonaGolesRight.text = marcador.goles_Barcelona_Result.ToString ();
			OponenteGolesRight.text = marcador.goles_Oponente_Result.ToString ();

		} else {

			EscudoEquipoHeader.sprite = ImgLoadManager.main.teamImg (EscudoEquipoHeader, oponente.ToUpper(), false);
			EscudoOponenteHeader.sprite = ImgLoadManager.main.teamImg (EscudoOponenteHeader, "COLOMBIA", false);

			EscudoEquipoLeft.sprite = ImgLoadManager.main.teamImg (EscudoEquipoLeft, oponente.ToUpper(), false);
			EscudoOponenteLeft.sprite = ImgLoadManager.main.teamImg (EscudoOponenteLeft, "COLOMBIA", false);

			EscudoEquipoRight.sprite = ImgLoadManager.main.teamImg (EscudoEquipoRight, oponente.ToUpper(), false);
			EscudoOponenteRight.sprite = ImgLoadManager.main.teamImg (EscudoOponenteRight, "COLOMBIA", false);
		
			BarcelonaGolesLeft.text = marcador.goles_Oponente.ToString ();
			OponenteGolesLeft.text = marcador.goles_Barcelona.ToString ();

			BarcelonaGolesRight.text = marcador.goles_Oponente_Result.ToString ();
			OponenteGolesRight.text = marcador.goles_Barcelona_Result.ToString ();
		}

		if (marcador.goles_Barcelona_Points == 500 && marcador.goles_Oponente_Points == 500) {

			if (marcador.goles_Barcelona == 0 && marcador.goles_Oponente == 0) {

				if(marcador.IsFirst == 1){

					//totalPnts = (totalPnts == 1) ? totalPnts * 10 : totalPnts;
					totalPnts = (totalPnts == 0) ? totalPnts + marcador.goles_Barcelona_Points+10 : totalPnts * marcador.goles_Barcelona_Points+10;

					marcadorPoints.text = signOfPoints + marcador.goles_Barcelona_Points;

				} else {

					//totalPnts = (totalPnts == 1) ? totalPnts * 5 : totalPnts;
					totalPnts = (totalPnts == 0) ? totalPnts + marcador.goles_Barcelona_Points : totalPnts * marcador.goles_Barcelona_Points;

					marcadorPoints.text = signOfPoints + marcador.goles_Barcelona_Points;
				}


			} else {

				marcadorPoints.text = signOfPoints + marcador.goles_Barcelona_Points;
			
				//totalPnts = (totalPnts == 1) ? totalPnts * marcador.goles_Barcelona_Points : totalPnts;
				totalPnts = (totalPnts == 0) ? totalPnts + marcador.goles_Barcelona_Points : totalPnts * marcador.goles_Barcelona_Points;
			}
		}

		//totalPoints.text = (totalPnts == 1) ? "0" : totalPnts.ToString ();

		totalPoints.text = scoreResultList.dataList [0].GetValueToKey ("ScoreResultado");

		if (ScrollContent.GetComponent<RectTransform> ().sizeDelta.y < 732) {

			ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 732);
		}

		SetFechaCuandoSePronostico ();

		StartCoroutine ("showMatch");

		updateRanking.UpdateRankingIsLoad ();

		SetAlignmentPlayerInScroll ();
	}

	void SetAlignmentPlayerInScroll (){

		for(int i = 0; i < 11; i++){

			alignmentResultDataList [i]._leftPlayerName.text = alignmentResultList.dataList[0].GetValueToKey ("AlinPlayer" + (i+1));
			alignmentResultDataList [i]._rightPlayerName.text = alignmentResultList.dataList[0].GetValueToKey ("AlinRealPlayer" + (i+1));

			if(alignmentResultList.dataList[0].GetValueToKey ("PtnAlign" + (i+1)) != "0"){
				
				alignmentResultDataList [i]._points.text =  signOfPoints + alignmentResultList.dataList[0].GetValueToKey ("PtnAlign" + (i+1));

			} else {

				alignmentResultDataList [i]._points.text = "";
			}
		}

		alignmentDatasPanel.SetActive (true);

		EndUpdateData ("none");
		//textInfoHeader.GetComponent<tituloInfo> ().estaregistrado ("none");
		//textInfoHeader.GetComponent<tituloInfo> ().GetRankingDataUser();

		UpdateDataScript.updateData.StopUpdatePanel ();
	}

	private void SetFechaCuandoSePronostico (){

		DateTime nextDate = DateTime.Parse (resultList.dataList [0].fechaPronostico);

		CultureInfo ci = new CultureInfo ("es-ES");
		//DateTime pastMonth = Date.AddMonths(0);
		string mes = nextDate.ToString ("MMMM", ci);
		string dia = nextDate.ToString ("dddd", ci);
		string diaNum = nextDate.ToString ("dd", ci).ToUpper ();

		mes = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (mes);

		dia = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (dia);

		fechaPronosticoText.text = "<b>Fecha y hora de tu Pronóstico: </b>" + dia.ToLower() + " " + diaNum + " de " + mes.ToLower() + " a las " + nextDate.Hour + "h, " + nextDate.Minute + "m, " + nextDate.Second + "s";

	}


	IEnumerator showMatch()
	{
		urlTemp1 = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/pronosticoResultados.png";

		www = new WWW (ToAntiCache (urlTemp1));
		yield return www;

		//loadImg = imageNews1;

		//imageNews1.texture = www.texture;
		//imageNews2.texture = www.texture;
		//imageNews3.texture = www.texture;
		//imageNews3.gameObject.SetActive (true);
	}

	public  string ToAntiCache( string url)
	{
		string r = "";
		r += UnityEngine.Random.Range(1000000,9000000).ToString();
		r += UnityEngine.Random.Range(1000000,9000000).ToString();
		string result="";
		if(url.Substring(url.Length -4,4   ) == ".php" || url.Substring(url.Length -4,4   ) == ".png"|| url.Substring(url.Length -4,4   ) == ".jpg")
		{
			result = url + "?key=" + r;
		}
		else
		{
			result = url + "&key=" + r;
		}
		return result;
	}

	/*
	void OnEnable() {

		//if (resultIsLoad == false) {

//			ShowUpdatePopup ();
			UpdateDataScript.updateData.RunUpdatePanel();

			StartCoroutine(ValidateStatusResultados ());
		//}
	}
	*/
	void OnDisable() {

		CancelInvoke("RotateImg");
	}

	public void EndUpdateData(string state){
		
		resultIsLoad = true;
		//HideUpdatePopup ();
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

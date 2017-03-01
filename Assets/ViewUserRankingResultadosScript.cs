using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Globalization;

public class ViewUserRankingResultadosScript : MonoBehaviour {

	//private string[] titleGols = new [] {"PRIMER","SEGUNDO","TERCER","CUARTO","QUITO","SEXTO","SÉPTIMO","OCTAVO","NOVENO","DÉCIMO","ONCEAVO","DOCEAVO","TRECEAVO","CATORCEAVO","QUINCEAVO","DIECISEISAVO","DIECISIETEAVO","DIECIOCHOAVO","DIECINUEVEAVO","VEINTEAVO"};
	private string[] titleGols = new [] {"Primer","Segundo","Tercer","Cuarto","Quinte","Sexto","Séptimo","Octavo","Noveno","Décimo","Onceavo","Doceavo","Treceavo","Catorceavo","Aquinceavo","Dieciseisavo","Diecisieteavo","Dieciochoavo","Diecinueveavo","Veinteavo"};

	public ContentRankingScript updateUserInf;

	public GameObject prefabContentGolPanel;
	//public GameObject prefabTitleContent;
	//public GameObject prefabResultContent;
	public GameObject ScrollContent;

	public GameObject titleMarcador;
	public GameObject marcadorPanel;

	public Text BarcelonaGolesLeft;
	public Text BarcelonaGolesRight;
	public Text OponenteGolesLeft;
	public Text OponenteGolesRight;
	public Text marcadorPoints;
	//public Image marcadorBall;

//	public RawImage imageNews1;
//	public RawImage imageNews2;
//	public RawImage imageNews3;

	private WWW www;
	private string urlTemp1;

	public Text totalPoints;
	private int totalPnts = 0;

	public bool resultIsLoad = false;

	private string urlPhpPronostico = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/UserPronostico.php?";
	//private string urlPhpPronostico = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/UserPronosticoTest.php?";
	private string urlPhpUltimoPartido = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/GetDate.php?";

	public GameObject textInfoHeader;

	[SerializeField]
	public DateClassList fechaList;

	private DateTime date;

	public Text fechaText;

	private RawImage loadImg;

	[SerializeField]
	private ContentResultListScript resultList;

	public List<GameObject> resultsData = new List<GameObject>();

	private int idUsuario = 0;

	private int heightContent = 491;

	public Text fechaPronosticoText;

	void Start(){

		//loadImg = imageNews1;
	}

	public void RunViewUserRankingResult(int userID, string fecha){

		Debug.Log ("El ID del usuario recibido es: " + userID + " y la fecha: " + fecha);

		gameObject.SetActive (true);

		DateTime newDate = DateTime.Parse (fecha);

		date = newDate;

		idUsuario = userID;

		CultureInfo ci = new CultureInfo ("es-ES");
		//DateTime pastMonth = Date.AddMonths(0);
		string mes = date.ToString ("MMMM", ci);
		string dia = date.ToString ("dddd", ci);
		string diaNum = date.ToString ("dd", ci).ToUpper ();

		fechaText.text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (dia) + " " + diaNum + " de " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase (mes);

		//StartCoroutine (GetResultados ());
		StartCoroutine (ClearAndEnablePrediction ("NuevaFecha"));
	}

	private IEnumerator ValidateStatusResultados (){

		WWW getData = new WWW (urlPhpUltimoPartido + "indata=getNextDate" + "&idDate=" + 2);
		yield return getData;

		if (string.IsNullOrEmpty (getData.error)) {

			if (getData.text != "PredictingUncreated") {

				fechaList = JsonUtility.FromJson<DateClassList> (getData.text);
				//StartCoroutine (GetPredictions ());
				setupDate();

			} else {

				Debug.Log("¡No hay datos!");
			}

		} else {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}

		Debug.Log ("GETDATA: " + getData.text);
	}

	private void setupDate(){

		DateTime newDate = DateTime.Parse (fechaList.dataList [0].fecha);

		if (date != newDate) {

			date = newDate;
			//DateTime nextDate = DateTime.Parse (fechaList.dataList [0].fecha + " " + fechaList.dataList [0].hora);

			CultureInfo ci = new CultureInfo ("es-ES");
			//DateTime pastMonth = Date.AddMonths(0);
			string mes = date.ToString ("MMMM", ci);
			string dia = date.ToString ("dddd", ci);
			string diaNum = date.ToString ("dd", ci).ToUpper ();

			fechaText.text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (dia) + " " + diaNum + " de " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase (mes);

			if (resultIsLoad) {

				StartCoroutine (ClearAndEnablePrediction ("NuevaFecha"));

			} else {

				StartCoroutine (GetResultados ());
			}

		} else {

			if (resultIsLoad) {

				HideUpdatePopup ();

			} else {

				StartCoroutine (GetResultados ());
			}

		}
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

//		imageNews1.texture = loadImg.texture;
//		imageNews2.texture = loadImg.texture;
//		imageNews3.texture = loadImg.texture;
//		imageNews3.gameObject.SetActive (false);

		titleMarcador.SetActive (false);
		marcadorPanel.SetActive (false);

		totalPoints.text = "";

		//marcadorBall.gameObject.SetActive (false);

		//marcadorPoints.text = "";

		if (action == "NuevaFecha") {

			StartCoroutine (GetResultados ());

		} else if (action == "Limpiar") {

			ShowMessagePopup ("Aún no hay resultados. Cuando termine el partido podrás consultarlos", 1);

		}
	}

	private IEnumerator GetResultados (){

		//string fechaEncuentro = "&buscarFecha=" + "2016-10-14";
		string fechaEncuentro = "&buscarFecha=" + date.Year + "-" + date.Month + "-" + date.Day;

		//WWW getData = new WWW (urlPhpPronostico + "indata=buscarResultado&buscarUserID=1");
		WWW getData = new WWW (urlPhpPronostico + "indata=buscarResultado" + fechaEncuentro + "&buscarUserID=" + idUsuario);
		yield return getData;

		if (string.IsNullOrEmpty (getData.error)) {

			if (getData.text != "PredictingUncreated") {

				resultList = JsonUtility.FromJson<ContentResultListScript> (getData.text);
				StartCoroutine (ShowResultados ());
			} else {
				Debug.Log ("AQUI!!!!!GerR");
				ShowMessagePopup ("Aún no hay resultados. Cuando termine el partido podrás consultarlos", 1);
				//ShowMessagePopup ("Los resultados del último partido no se han generado, cuando culmine el partido podrá consultarlos", 1);
				//HideUpdatePopup ();
				//dataIsValidated = true;
				//predecirBtn.SetActive (true);
				//StartCoroutine (gameObject.GetComponent<GetFootballPlayersDataScript> ().GetFootballPlayersData ());
			}
		} else {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}

		Debug.Log (getData.text);

		//EndUpdateData ("none");
	}

	private IEnumerator ShowResultados(){

		/*int goals = resultList.dataList.Count;*/

		int index = 0;

		totalPnts = 0;

		ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (140, 160);

		if (resultList.dataList [0].autor_Gol != "none" || resultList.dataList[0].autor_Gol_Result != "none") {

			ContentResultListScript golesNoPredecidos = new ContentResultListScript();

			foreach(ContentResult obj in resultList.dataList){

				//				GameObject resultData = Instantiate (prefabContentGolPanel);
				//
				//				Debug.Log ("CREO: " + index);
				//
				//				resultData.transform.SetParent (ScrollContent.transform);
				//				resultsData.Add (resultData);
				//				resultData.GetComponent<RectTransform>().anchoredPosition = new Vector3 (0, -140 + index * -heightContent, 0);
				//				resultData.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);
				//				yield return new WaitForEndOfFrame ();

				if (obj.min_Gol != -2) {

					GameObject resultData = Instantiate (prefabContentGolPanel);

					Debug.Log ("CREO: " + index);

					resultData.transform.SetParent (ScrollContent.transform);
					resultsData.Add (resultData);
					resultData.GetComponent<RectTransform>().anchoredPosition = new Vector3 (0, -140 + index * -heightContent, 0);
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

				//				resultData.GetComponent<ContentGolScript> ().SetDataGolPanel ("setNumGol", titleGols[index] + " GOL", titleGols[index] + " GOL", 0);
				//				resultData.GetComponent<ContentGolScript> ().SetDataGolPanel ("setAnotador", obj.autor_Gol, obj.autor_Gol_Result, obj.autor_Gol_Points);
				//				resultData.GetComponent<ContentGolScript> ().SetDataGolPanel ("setMinuto", obj.min_Gol.ToString(), obj.min_Gol_Result.ToString(), obj.min_Gol_Points);
				//				resultData.GetComponent<ContentGolScript> ().SetDataGolPanel ("setTipoGol", obj.tipo_Gol, obj.tipo_Gol_Result, obj.tipo_Gol_Points);
				//				resultData.GetComponent<ContentGolScript> ().SetDataGolPanel ("setAsistencia", obj.autor_PaseGol, obj.autor_PaseGol_Result, obj.autor_PaseGol_Points);
				//
				//				totalPnts = (obj.prediction_Points != 0) ? totalPnts*obj.prediction_Points : totalPnts;
				//
				//				index++;

				yield return null;
			}

			if(golesNoPredecidos.dataList.Count > 0){

				golesNoPredecidos.SortMinNegativos ();

				foreach(ContentResult obj in golesNoPredecidos.dataList){

					ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, ScrollContent.GetComponent<RectTransform> ().sizeDelta.y + heightContent);

					GameObject resultData = Instantiate (prefabContentGolPanel);

					resultData.transform.SetParent (ScrollContent.transform);
					resultsData.Add (resultData);
					resultData.GetComponent<RectTransform>().anchoredPosition = new Vector3 (0, -140 + index * -heightContent, 0);
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

		if (marcador.esLocal == 1) {

			BarcelonaGolesLeft.text = marcador.goles_Barcelona.ToString ();
			OponenteGolesLeft.text = marcador.goles_Oponente.ToString ();

			BarcelonaGolesRight.text = marcador.goles_Barcelona_Result.ToString ();
			OponenteGolesRight.text = marcador.goles_Oponente_Result.ToString ();

		} else {

			BarcelonaGolesLeft.text = marcador.goles_Oponente.ToString ();
			OponenteGolesLeft.text = marcador.goles_Barcelona.ToString ();

			BarcelonaGolesRight.text = marcador.goles_Oponente_Result.ToString ();
			OponenteGolesRight.text = marcador.goles_Barcelona_Result.ToString ();
		}

		//		BarcelonaGolesLeft.text = marcador.goles_Barcelona.ToString();
		//		OponenteGolesLeft.text = marcador.goles_Oponente.ToString();
		//
		//		BarcelonaGolesRight.text = marcador.goles_Barcelona_Result.ToString();
		//		OponenteGolesRight.text = marcador.goles_Oponente_Result.ToString();

		if (marcador.goles_Barcelona_Points == 20 && marcador.goles_Oponente_Points == 20) {

			//marcadorBall.gameObject.SetActive (true);

			if (marcador.goles_Barcelona == 0 && marcador.goles_Oponente == 0) {

				if(marcador.IsFirst == 1){

					//totalPnts = (totalPnts == 1) ? totalPnts * 10 : totalPnts;
					totalPnts = (totalPnts == 0) ? totalPnts + marcador.goles_Barcelona_Points : totalPnts * marcador.goles_Barcelona_Points;

					marcadorPoints.text = "x" + marcador.goles_Barcelona_Points;

				} else {

					//totalPnts = (totalPnts == 1) ? totalPnts * 5 : totalPnts;
					totalPnts = (totalPnts == 0) ? totalPnts + (marcador.goles_Barcelona_Points/2) : totalPnts * (marcador.goles_Barcelona_Points/2);

					marcadorPoints.text = "x" + (marcador.goles_Barcelona_Points/2);
				}


			} else {

				marcadorPoints.text = "x" + marcador.goles_Barcelona_Points;

				//totalPnts = (totalPnts == 1) ? totalPnts * marcador.goles_Barcelona_Points : totalPnts;
				totalPnts = (totalPnts == 0) ? totalPnts + marcador.goles_Barcelona_Points : totalPnts * marcador.goles_Barcelona_Points;
			}

			//			Debug.Log (marcador.goles_Barcelona_Points +" / " + marcador.goles_Oponente_Points);
			//			Debug.Log (marcador.goles_Barcelona +" / " + marcador.goles_Oponente);
			//			Debug.Log (marcador.IsFirst);
		}

		totalPoints.text = (totalPnts == 1) ? "0" : totalPnts.ToString ();

		if (ScrollContent.GetComponent<RectTransform> ().sizeDelta.y < 732) {

			ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 732);
		}

		SetFechaCuandoSePronostico ();

		StartCoroutine ("showMatch");

		//updateRanking.UpdateRankingIsLoad ();

		EndUpdateData ("none");

		//textInfoHeader.GetComponent<tituloInfo> ().estaregistrado ("none");
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

//		loadImg = imageNews1;
//
//		imageNews1.texture = www.texture;
//		imageNews2.texture = www.texture;
//		imageNews3.texture = www.texture;
//		imageNews3.gameObject.SetActive (true);
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

	/*void OnEnable() {

		//if (resultIsLoad == false) {

		//			ShowUpdatePopup ();
		UpdateDataScript.updateData.RunUpdatePanel();

		StartCoroutine(ValidateStatusResultados ());
		//}
	}*/

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

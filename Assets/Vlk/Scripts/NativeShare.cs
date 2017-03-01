using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;
using System.Globalization;

/*
 * https://github.com/ChrisMaire/unity-native-sharing 
 */

public class NativeShare : MonoBehaviour {

	[Header("Paneles a Compartir:")]

	[SerializeField]
	private GameObject sharePredictionPanel;
	[SerializeField]
	private GameObject shareRankingPanel;

	[Header("Datos Ranking:")]

	[SerializeField]
	private Text originRankinName;
	[SerializeField]
	private Text originPosRanking;
	[SerializeField]
	private Text originScoreRanking;

	[SerializeField]
	private Text userRankinName;
	[SerializeField]
	private Text posRanking;
	[SerializeField]
	private Text scoreRanking;

	[Space(10)]

	public string ScreenshotName = "screenshot.png";
	public Camera CamComps;

	private string urlPhpPronostico = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/UserPronostico.php?";
	private string urlPhpUltimoPartido = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/GetDate.php?";

	public GameObject prefabShowData;
	public GameObject dataContent;

	public Text nameBarcelona;
	public Text goalsBarcelona;

	public Text nameOponente;
	public Text goalsOponente;

	public Text userName;

	public Text fechaPronosticoText;

	[SerializeField]
	private Image userPhoto;

	[SerializeField]
	private List<Text> alignmentList;

	public List<GameObject> panelsData = new List<GameObject>();

	private string msgData;

	public GameObject maskShareRanking;

	[SerializeField]
	private DataRowList predictionsList;


	public ScrollPrediccionScript predictions;

	[SerializeField]
	private DateClassList fechaList;

	private DateTime date;

	private string shareType;

	public void ShareScreenshotWithText(string text, string typ)  { //+++++++++++++++++++++++++++

		shareType = typ;

		StartCoroutine (espera(text));
    }

	private IEnumerator espera(string text){ //++++++++++++++++++++++++++++

		UpdateDataScript.updateData.RunUpdatePanel ();

		maskShareRanking.SetActive (true);

		yield return new WaitForSeconds(3.0f);

		UpdateDataScript.updateData.StopUpdatePanel ();

		yield return new WaitForSeconds(.1f);

		Texture2D tex = new Texture2D( Screen.width , Screen.height );
		tex.ReadPixels(new Rect(0,0,Screen.width, Screen.height),0,0);
		tex.Apply();

		string destination = Path.Combine(Application.persistentDataPath,"rankingPronostico" + ".png");
		File.WriteAllBytes(destination, tex.EncodeToJPG());

		if (shareType == "Native") {

//			Share ("", destination, "");

		} else if (shareType == "Facebook") {

			//UM_ShareUtility.FacebookShare ("Mira mi Ranking en PRONOSTICO BSC", tex);
			maskShareRanking.SetActive (false);

		} else if (shareType == "Twitter") {

			//UM_ShareUtility.TwitterShare ("Mira mi Ranking en PRONOSTICO BSC", tex);
			maskShareRanking.SetActive (false);
		}
	}

	private IEnumerator ShowPredictions(){//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		foreach(GameObject obj in panelsData){

			Destroy (obj);
		}

		panelsData.Clear ();

		int goals = predictionsList.dataList.Count;


		int index = 0;

		if (predictionsList.dataList [0].GetValueToKey("autor_Gol") != "none") {
		
			foreach(DataRow obj in predictionsList.dataList){

				GameObject panelData = Instantiate (prefabShowData);
				panelData.transform.SetParent (dataContent.transform);
				panelsData.Add (panelData);
				panelData.GetComponent<RectTransform>().localPosition = new Vector3 (panelData.GetComponent<RectTransform>().localPosition.x, panelData.GetComponent<RectTransform>().localPosition.y, 0);
				panelData.GetComponent<RectTransform>().anchoredPosition = new Vector3 (0, index * -30, 0);
				panelData.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);
				yield return new WaitForEndOfFrame ();
				panelData.GetComponent<ShareContentPanelScript> ().anotador.text = obj.GetValueToKey("autor_Gol");
				panelData.GetComponent<ShareContentPanelScript> ().forma.text = obj.GetValueToKey("tipo_Gol");
				panelData.GetComponent<ShareContentPanelScript> ().asistencia.text = obj.GetValueToKey("autor_PaseGol");
				panelData.GetComponent<ShareContentPanelScript> ().min.text = obj.GetValueToKey("min_Gol") + "\"";

				index++;
			}
		}

		if (int.Parse(predictionsList.dataList [0].GetValueToKey("esLocal")) == 1) {

			nameBarcelona.text = "BARCELONA";
			nameOponente.text = fechaList.dataList [0].equipo.Remove(fechaList.dataList [0].equipo.Length - 1);

			goalsBarcelona.text = predictionsList.dataList [0].GetValueToKey("goles_Barcelona");
			goalsOponente.text = predictionsList.dataList [0].GetValueToKey("goles_Oponente");

		} else {

			nameBarcelona.text = fechaList.dataList [0].equipo.Remove(fechaList.dataList [0].equipo.Length - 1);
			nameOponente.text = "BARCELONA";
		
			goalsBarcelona.text = predictionsList.dataList[0].GetValueToKey("goles_Oponente");
			goalsOponente.text = predictionsList.dataList[0].GetValueToKey("goles_Barcelona");
		}

		SetFechaCuandoSePronostico ();

		SetAlignmentShare ();
	}

	private void SetAlignmentShare(){

		int index = 0;

		foreach(string obj in predictions._alignmentShareList){

			alignmentList [index].text = obj;

			index++;
		}
	
		StartCoroutine(GetUserData());
	}

	private void SetFechaCuandoSePronostico (){ //+++++++++++++++++++++++++++++++++++++++++++++++++++++

		DateTime nextDate = DateTime.Parse (predictionsList.dataList [0].GetValueToKey("fechaPronostico"));

		CultureInfo ci = new CultureInfo ("es-ES");
		string mes = nextDate.ToString ("MMMM", ci);
		string dia = nextDate.ToString ("dddd", ci);
		string diaNum = nextDate.ToString ("dd", ci).ToUpper ();

		mes = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (mes);

		dia = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (dia);

		fechaPronosticoText.text = "Fecha y hora de este Pronóstico: " + dia.ToLower() + " " + diaNum + " de " + mes.ToLower() + " a las " + nextDate.Hour + "h, " + nextDate.Minute + "m, " + nextDate.Second + "s";

	}

	private IEnumerator GetUserData(){//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	
		WWW getData = new WWW (urlPhpPronostico + "indata=getUserName" + "&buscarUserID=" + DataApp.main.GetMyID());
		yield return getData;

		if (string.IsNullOrEmpty (getData.error)) {

			if (getData.text != "PredictingUncreated") {

				userName.text = getData.text;

				//userPhoto.sprite = "N"; //Aqui Cargar User Photo

			} else {

				UpdateDataScript.updateData.RunPopup ("No se encontro un nombre para este usuario.", 0);
			}
		} else {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}

		Debug.Log (getData.text);

		StartCoroutine (ShareNative ( msgData ));
	}

	public void SharePrediction (string msg, string typ ){//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		UpdateDataScript.updateData.RunUpdatePanel ();

		shareRankingPanel.SetActive (false);
		sharePredictionPanel.SetActive (true);

		shareType = typ;
		
		msgData = msg;

		fechaList = predictions._fechaList;
		predictionsList = predictions._predictionsList;
		StartCoroutine (ShowPredictions ());
	}

	public void ShareRanking (string msg, string typ ){//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		UpdateDataScript.updateData.RunUpdatePanel ();

		sharePredictionPanel.SetActive (false);
		shareRankingPanel.SetActive (true);

		shareType = typ;

		msgData = msg;

		ShowRankingData ();
	}

	private void ShowRankingData (){

		userRankinName.text = originRankinName.text;
		posRanking.text = originPosRanking.text;
		scoreRanking.text = originScoreRanking.text;

		//userPhoto.sprite = "N"; //Aqui Cargar User Photo

		StartCoroutine (ShareNative (msgData));
	}

	Texture2D ShareImageCompuesta(Camera cam) { //+++++++++++++++++++++++++++++++++++++++++++++
		
		RenderTexture currentRT = RenderTexture.active;
		RenderTexture.active = cam.targetTexture;
		cam.Render();
		Rect rt = new Rect (0, 0, cam.targetTexture.width, cam.targetTexture.height);
		Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
		image.ReadPixels(rt, 0, 0);
		image.Apply();
		RenderTexture.active = currentRT;
		return image;
	}


	IEnumerator ShareNative(string text ){//++++++++++++++++++++++++++++++++++++++++++++++++++
		Debug.Log ("COMPARTIENDO");

		UpdateDataScript.updateData.RunUpdatePanel ();

		yield return new WaitForSeconds(3.0f);

		string destination = Path.Combine(Application.persistentDataPath,System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
		print(destination);

		File.WriteAllBytes(destination, ShareImageCompuesta(CamComps).EncodeToJPG());

		Texture2D tex = new Texture2D(2, 2);
		tex.LoadImage(ShareImageCompuesta(CamComps).EncodeToJPG());

		if (shareType == "Native") {
			
//			Share ("", destination, "");

		} else if (shareType == "Facebook") {

			//UM_ShareUtility.FacebookShare ("Mira mi Ranking en PRONOSTICO BSC", tex);

		} else if (shareType == "Twitter") {


			//UM_ShareUtility.TwitterShare ("Mira mi Ranking en PRONOSTICO BSC", tex);
		}

		UpdateDataScript.updateData.StopUpdatePanel ();
	}



}

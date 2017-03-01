using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ContentRankingScript : MonoBehaviour {

	private string urlPhp = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/ViewUserResults.php";
	//private string urlPhpUltimoPartido = "2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/ViewUserResults.php?indata=GetDateOldResult&userID=62985";

	//public ViewUserRankingResultadosScript objResultView;
	public ViewUserScrollResultadosScript objResultView;

	public int indexPosition;

	public int userID;
	
	public Image cup;
	public Text cupRankingText;
	public Text posRankingText;

	public Image userPhoto;
	public Text nameText;

	public Text pointsText;

	public Button viewUserResultBtn;

	public void ViewUserResult(){

		UpdateDataScript.updateData.RunUpdatePanel();

		StartCoroutine (ValideDateResult());
	}

	/*private IEnumerator ValideDateResult(){

		string url = UrlsAdmin.self._ftpUrl + "PollaTricolor/Php/ViewUserResults.php?action=GetDateOldResult" + "&userID=" + userID;
		Debug.Log ("LA URL DE VER RESULTADOS ES: " + url);
		//WWW getData = new WWW (urlPhp + "?indata=GetDateOldResult" + "&userID=" + userID);
		WWW getData = new WWW (url);

		yield return getData;

		if (string.IsNullOrEmpty (getData.error)) {

			if (getData.text != "NoHayResultados") {

				RunViewUserResult (getData.text);

			} else {

				UpdateDataScript.updateData.RunPopup (nameText.text + " no tiene resultados de ningun partido.", 4);
			}

		} else {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}
	}*/

	private IEnumerator ValideDateResult(){

		DateClassList fechaList;

		string url = UrlsAdmin.self._ftpUrl + "PollaTricolor/Php/GetFechasPartidos.php?" + "action=GetFechaPartido" + "&idDate=" + (int)idFechasPartidos.ultimoPartido;

		//WWW getData = new WWW (urlPhpUltimoPartido + "indata=getNextDate" + "&idDate=" + 2);
		WWW getData = new WWW (url);
		yield return getData;

		if (string.IsNullOrEmpty (getData.error)) {

			if (getData.text != "PredictingUncreated") {

				fechaList = JsonUtility.FromJson<DateClassList> (getData.text);
				StartCoroutine (TieneReSultados (fechaList));
				//RunViewUserResult (fechaList);
				//setupDate();

			} else {

				UpdateDataScript.updateData.RunPopup (nameText.text + " no tiene resultados del ultimo partido.", 4);
			}
		} else {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}

		Debug.Log ("GETDATA: " + getData.text);
	}

	private IEnumerator TieneReSultados(DateClassList fecha){

		DateTime newDate = DateTime.Parse (fecha.dataList [0].fecha);

		string fechaEncuentro = "&buscarFecha=" + newDate.Year + "-" + newDate.Month + "-" + newDate.Day;

		string url = UrlsAdmin.self._ftpUrl + "PollaTricolor/Php/ViewUserResults.php?action=GetDateOldResult" + "&userID=" + userID + fechaEncuentro;
		Debug.Log ("LA URL DE VER RESULTADOS ES: " + url);
		//WWW getData = new WWW (urlPhp + "?indata=GetDateOldResult" + "&userID=" + userID);
		WWW getData = new WWW (url);

		yield return getData;

		if (string.IsNullOrEmpty (getData.error)) {

			if (getData.text == "Si") {

				RunViewUserResult (fecha);

			} else {

				UpdateDataScript.updateData.RunPopup (nameText.text + " no tiene resultados de ningun partido.", 4);
			}

		} else {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}
	}

	private void RunViewUserResult(DateClassList fecha){

		objResultView.updateUserInf.indexPosition = indexPosition;

		objResultView.updateUserInf.userID = userID;

		objResultView.updateUserInf.posRankingText.text = posRankingText.text;

		objResultView.updateUserInf.userPhoto.sprite = userPhoto.sprite;

		objResultView.updateUserInf.nameText.text = nameText.text;

		objResultView.updateUserInf.pointsText.text = pointsText.text;

		objResultView.updateUserInf.cup.gameObject.SetActive (false);
		objResultView.updateUserInf.posRankingText.gameObject.SetActive (true);
		objResultView.updateUserInf.posRankingText.text = indexPosition.ToString ();

		objResultView.RunViewUserRankingResult (userID, fecha);
	}
}

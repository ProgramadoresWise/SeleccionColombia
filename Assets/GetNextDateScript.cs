using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

[System.Serializable]
public class DateClass
{
	public string fecha;
	public string hora;
	public string equipo;
}

[System.Serializable]
public class DateClassList {

	public List<DateClass> dataList;
}

public class GetNextDateScript : MonoBehaviour {

	public ScrollPrediccionScript predictionPanel;

	public Text days;
	public Text hours;
	public Text minutes;

	[SerializeField]
	public DateClassList fechaList;

	private bool isLoadNextDate = false;

	private string urlDatoHoraApertura = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/GetDate.php?";

	public IEnumerator ValidateStatusNextData (string action){

		WWW getData = new WWW (urlDatoHoraApertura + "indata=getNextDate" + "&idDate=" + 3);
		yield return getData;

		if (string.IsNullOrEmpty (getData.error)) {

			if (getData.text != "PredictingUncreated") {

				fechaList = JsonUtility.FromJson<DateClassList> (getData.text);
				StartCoroutine (ShowFecha (action));
			} else {
				
				Debug.Log("¡No hay datos!");
			}
		} else {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}

	//	Debug.Log ("GETDATA: " + getData.text);
	}

	private IEnumerator ShowFecha(string action){

		DateTime nextDate = DateTime.Parse (fechaList.dataList [0].fecha + " " + fechaList.dataList [0].hora);

		nextDate = nextDate.AddHours(5-(DateTime.UtcNow - DateTime.Now).TotalHours);

		DateTime currentDate = DateTime.Now;

		TimeSpan diferencia = nextDate - currentDate;

		if (diferencia.Days >= 0 && diferencia.Hours >= 0 && diferencia.Minutes >= 0) {

			days.text = diferencia.Days.ToString ();
			hours.text = diferencia.Hours.ToString ();
			minutes.text = diferencia.Minutes.ToString ();
		} else {
		
			days.text = "00";
			hours.text = "00";
			minutes.text = "00";
		}

		if ((diferencia.Days + diferencia.Hours + diferencia.Minutes) <= 0) {

			predictionPanel._predictionIsEnable = false;
		
		} else {
		
			predictionPanel._predictionIsEnable = true;
		}

		if(action == "_SendDataDB"){

			predictionPanel.SendDataDB ();
		}

//		Debug.Log ("Futuro " + nextDate.ToString());
//		Debug.Log ("Actual " + currentDate.ToString ());
//		Debug.Log ("Dif " + diferencia.ToString ());

		yield return null;
	}

	void OnEnable() {

		if (isLoadNextDate == false) {

			//ShowUpdatePopup ();

			StartCoroutine(ValidateStatusNextData ("none"));
		}
	}

	public void EndUpdateData(string state){

		isLoadNextDate = true;
		//HideUpdatePopup ();
	}
}

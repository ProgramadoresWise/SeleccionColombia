using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using System.Globalization;
using MaterialUI;


public class tituloInfo : MonoBehaviour 
{
	public Text apodoUsuario;
	public Text puntosUsuario;
	public Text rankingUsuario;
	public Image userPhoto;

	private string urlPhpPronostico = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/UserPronostico.php?";

	[SerializeField]
	private ContentRankingListScript userRanking;



	public void GetRankingDataUser(){

		//DataApp.main.EnableLoading();

		UpdateDataScript.updateData.RunUpdatePanel ();

		StartCoroutine(UpdateUserRanking()); 
	}

	IEnumerator UpdateUserRanking(){
		
		WWW updFlag = new WWW(DataApp.main.host + DataApp.main.urlActualizarInfo + (int)idActualizacion.userRanking);
		yield return updFlag;

		if( string.IsNullOrEmpty(updFlag.error)){

			StartCoroutine (GetUserRanking(int.Parse(updFlag.text)));
		}else{

			//DataApp.main.popUpInformative(true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}

	}

	public IEnumerator GetUserRanking(int NumAct){

		if( DataApp.main.GetMyInfoInt("UserHeaderRanking") == NumAct){

			string jsonGuardado = DataApp.main.GetMyInfoString("JsonUserHeaderRanking");
			userRanking = JsonUtility.FromJson<ContentRankingListScript> (jsonGuardado);
			yield return userRanking;
			StartCoroutine (ShowUserRanking ());

		}else{

			string urlPhp = DataApp.main.host + "PollaTricolor/Php/GetUsersRankings.php?" + "action=GetRankingUser&buscarUserID=" + DataApp.main.GetMyID();
			Debug.Log("La Url es: " + urlPhp);

			WWW getData = new WWW(urlPhp);

			yield return getData;

			if (string.IsNullOrEmpty (getData.error)) {

				if (getData.text != "PredictingUncreated") {

					DataApp.main.SetMyInfo("JsonUserHeaderRanking", getData.text, 3);
					DataApp.main.SetMyInfo("UserHeaderRanking", NumAct.ToString(), 1);

					userRanking = JsonUtility.FromJson<ContentRankingListScript> (getData.text);
					StartCoroutine (ShowUserRanking ());
				} else {

				}
			} else {

				UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
			}

			Debug.Log (getData.text);
		}
	}

	private IEnumerator ShowUserRanking(){

		ContentRanking obj = userRanking.dataList[0];

		userPhoto.sprite = ImgLoadManager.main.UsersImg(userPhoto, DataApp.main.GetMyID().ToString(), false);
		apodoUsuario.text = User.main.GetMyName();//obj.nameText;
		puntosUsuario.text = obj.points.ToString ();
		rankingUsuario.text =  obj.userID.ToString ();

		Debug.Log (obj.userID.ToString ());

		yield return null;

		Debug.Log (obj.nameText + " - "  + obj.points.ToString () + " - " + obj.userID.ToString ());

		UpdateDataScript.updateData.StopUpdatePanel ();
		//DataApp.main.DisableLoading();
	}

}

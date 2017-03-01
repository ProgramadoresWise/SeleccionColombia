using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;


public class PopUpRegitro : MonoBehaviour 
{
	

	int userID; 

	public GameObject popUpRegistro;

    public Text apodoUsuario;
	public Text puntosUsuario;
	public Text rankingUsuario;

	public string rtaTitulo;
	private string[] arregloRTA;

	private string temp;


	string getUrlTipoUser = "http://2waysports.com/2waysports/Ecuador/Barcelona/Registro/getTipoUsuario.php";
	string getUrlInfoUser = "http://2waysports.com/2waysports/Ecuador/Barcelona/Registro/getInfoTitle.php";


	public void mostrarPopupRegistro ()
	{
		if (DataApp.main.IsRegistered () == true) 
		{
			popUpRegistro.SetActive (false);
			userID = DataApp.main.GetMyID ();

			StartCoroutine ("tipoUsuario");
		} 
		else 
		{
			popUpRegistro.SetActive (true);
			this.GetComponent<NavigationTabPanelsScript> ().DisableTabButtons ();
		}
	}


	private IEnumerator tipoUsuario() 
	{
		WWW tipodeusuario = new WWW (getUrlTipoUser+ "?IDuser=" +userID);
		yield return tipodeusuario;
		temp = tipodeusuario.text;

		StartCoroutine ("infoTitle");
	}


	private IEnumerator infoTitle()
	{
		WWW titulo = new WWW (getUrlInfoUser + "?IDuser=" + userID+ "&tipoUser=" + int.Parse(temp));
		yield return titulo;

		rtaTitulo = titulo.text;

		arregloRTA = rtaTitulo.Split (new string[] {","},System.StringSplitOptions.None );

		apodoUsuario.text = arregloRTA [0];

		if (arregloRTA.Length >= 2) 
		{
			puntosUsuario.text = arregloRTA [1];
			rankingUsuario.text = arregloRTA [2];
		} 
		else 
		{
			puntosUsuario.text = "0";
			rankingUsuario.text = " ";
		}

	}


}

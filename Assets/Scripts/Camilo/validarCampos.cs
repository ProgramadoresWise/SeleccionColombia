using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Globalization;
using System.Text.RegularExpressions;


public class validarCampos : MonoBehaviour 
{
	public InputField iFieldDia;
	public InputField iFieldMes;
	public InputField iFieldAnio;

	public GameObject poUpWarning;
	public Text textoWarning;

	private string textoDia;
	private string textoMes;
	private string textoAnio;

	private int anioValidado=0;

	private bool restric1=false;
	private bool restric2=false;

	public GameObject ImgMayorEdad;
	public GameObject ImgHome;

	public GameObject eventManager;

	public void validarAcceso ()
	{
		textoDia = iFieldDia.text;
		textoMes = iFieldMes.text;
		textoAnio = iFieldAnio.text;

		if (String.IsNullOrEmpty (textoDia) || String.IsNullOrEmpty (textoMes) || String.IsNullOrEmpty (textoAnio)) {
			poUpWarning.SetActive (true);
			textoWarning.text = "Ninguna casilla puede quedar vacia";
			restric1 = false;
		}
		else 
		{
			restric1 = true;
			anioValidado = int.Parse (textoAnio);
			if (anioValidado > 1998 || anioValidado == 0) 
			{
				poUpWarning.SetActive(true);
				textoWarning.text = "Eres menor de edad por tanto no puedes participar";
				iFieldAnio.text = " ";
				restric2 = false;
			}
			else 
			{
				restric2 = true;
				poUpWarning.SetActive (false);
				textoWarning.text = " ";
			}
		}


		if (restric1 == true && restric2 == true) 
		{
			ImgMayorEdad.SetActive (false);

			//eventManager.GetComponent<tituloInfo> ().estaregistrado ();

			ImgHome.SetActive (true);
		}



	}


}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

public class formatText : MonoBehaviour
{
	public InputField iField;
	private string texto;

	private bool valido = false;
	private char[] c_arr; 

	public GameObject poUpWarning;
	public Text textoWarning;

	private Regex regex;
	private Match matchin;


	public void cambiarMayusculas ()
	{
		texto = iField.text.ToUpper();
		iField.text = texto;
	}

	public void IsValidEmail()
	{
		texto = iField.text;

		regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
//		matchin = regex.Match(texto);

		if (String.IsNullOrEmpty (texto)) 
		{
			poUpWarning.SetActive(true);
			textoWarning.text = "Ninguna casilla puede quedar en cero o vacía"; 
		}
		else if (Regex.Match(texto, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.ECMAScript).Success)
		{
			poUpWarning.SetActive(false);
			textoWarning.text = " ";
		}
		else
		{
			poUpWarning.SetActive(true);
			textoWarning.text = "El correo no es correcto"; 
			iField.text = " ";
		}

	}


	private string DomainMapper(System.Text.RegularExpressions.Match match)
	{
		// IdnMapping class with default property values.
		IdnMapping idn = new IdnMapping();

		string domainName = match.Groups[2].Value;
		try {
			domainName = idn.GetAscii(domainName);
		}
		catch (ArgumentException)
		{
			
		}
		return match.Groups[1].Value + domainName;
	}


	public void dia()
	{
		texto = iField.text;

		if (String.IsNullOrEmpty (texto)) 
		{
			poUpWarning.SetActive (true);
			textoWarning.text = "Ninguna casilla puede quedar en cero o vacía";
		} 
		else if (int.Parse(texto) >= 32 || int.Parse(texto) <= 0) 
		{
			poUpWarning.SetActive (true);
			textoWarning.text = "Solo pueden ser numeros menores a 31";
			iField.text = " ";
		} 
		else 
		{
			poUpWarning.SetActive (false);
			textoWarning.text = " ";
		}
	}


	public void mes()
	{
		texto = iField.text;

		if (String.IsNullOrEmpty (texto)) 
		{
			poUpWarning.SetActive (true);
			textoWarning.text = "Ninguna casilla puede quedar en cero o vacía";
		}
		else if (int.Parse(texto) >= 13 || int.Parse(texto) <= 0) 
		{
			poUpWarning.SetActive (true);
			textoWarning.text = "Solo pueden ser numeros menores a 12";
			iField.text = " ";
		}
		else 
		{
			poUpWarning.SetActive (false);
			textoWarning.text = " ";
		}
	}

	public void anio()
	{
		texto = iField.text;
		c_arr = texto.ToCharArray ();

		if (String.IsNullOrEmpty (texto)) 
		{
			poUpWarning.SetActive(true);
			textoWarning.text = "Ninguna casilla puede quedar en cero o vacía";
		}
		else if (c_arr.Length != 4) 
		{
			poUpWarning.SetActive(true);
			textoWarning.text = "Solo pueden ser 4 digitos";
			iField.text = " ";
		}
		else 
		{
			poUpWarning.SetActive (false);
			textoWarning.text = " ";
		}

		/*if ((int)strAnio <= 2000) 
		{
			print ("llamar al popUp y decir que solo pueden ser numeros menores a 12");
		}*/
	}



}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComplementLayout : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}



	public void viewPass( ){
		if( this.GetComponent<InputField>().contentType == InputField.ContentType.Password)
			this.GetComponent<InputField>().contentType =  InputField.ContentType.Alphanumeric;
		else
			this.GetComponent<InputField>().contentType =  InputField.ContentType.Password;
	}
}

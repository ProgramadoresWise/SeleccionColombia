using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ValidateIfTheButtonIsEnabledScript : MonoBehaviour {

	public List<InputField> objInputFields;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		ValidateIfTheButtonIsEnabled ();
	}

	private void ValidateIfTheButtonIsEnabled (){

		bool enableBtn = true;

		foreach(InputField obj in objInputFields){

			if(obj.text == ""){

				enableBtn = false;
			}
		}

		if (enableBtn) {

			EnableButton ();

		} else {

			DisableButton ();
		}
	}

	private void EnableButton (){

		Button btn = gameObject.GetComponent<Button> ();
		btn.interactable = true;

		Color32 color = btn.GetComponentInChildren<Text> ().color;
		color.a = 255;

		btn.GetComponentInChildren<Text> ().color = color;
	}

	private void DisableButton (){
	
		Button btn = gameObject.GetComponent<Button> ();
		btn.interactable = false;

		Color32 color = btn.GetComponentInChildren<Text> ().color;
		color.a = 130;

		btn.GetComponentInChildren<Text> ().color = color;
	}
}

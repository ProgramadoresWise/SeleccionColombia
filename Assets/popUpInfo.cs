using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class popUpInfo : MonoBehaviour {

	public Text titlePopup;
	public Text msgPopUp;


	void OnEnable( ){

		DataApp.main.IsActivePopUpInfo = true;
	}

	void OnDisable( ){
		
		DataApp.main.IsActivePopUpInfo = false;
	}

}

using UnityEngine;
using System.Collections;
using MaterialUI;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class AgeDropdown : MonoBehaviour {


	public int initAge;
	int finalAge;
	void Start( ){
		completeDropdown();
	}



	void completeDropdown () {
		finalAge = DateTime.Now.Year;
		for(int i = finalAge; i >= initAge; i--){
			this.GetComponent<MaterialDropdown>().AddData( new OptionData( i.ToString(),null, () => {
//				Debug.Log("Agregado " + i);
			}));
		}
	}
}

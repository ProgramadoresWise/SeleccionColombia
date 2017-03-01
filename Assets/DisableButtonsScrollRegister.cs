using UnityEngine;
using System.Collections;
using MaterialUI;

public class DisableButtonsScrollRegister : MonoBehaviour {


	public TabView tabRegister;
	public CanvasGroup rabioButtons;

	bool disable;
	// Update is called once per frame


	public void activeButtons( ){
		if( tabRegister.currentPage == 5){
			rabioButtons.gameObject.SetActive(false);
		}else{
			rabioButtons.gameObject.SetActive(true);
		}
	}



}

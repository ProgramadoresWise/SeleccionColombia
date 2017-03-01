using UnityEngine;
using System.Collections;

public class LoadPanelTimeWaitScrip : MonoBehaviour {

	public GameObject valideEdadPanel;

	private string action;

	public void EnterTheApp(string _action, int timeWait){

		action = _action;

		if (gameObject.activeSelf) {
			
			StartCoroutine (TimeWaitToContinue (timeWait));
		}
	}

	private IEnumerator TimeWaitToContinue(int timeWait){
	
		yield return new WaitForSeconds (timeWait);

		if (action == "IsRegistered" && valideEdadPanel.activeSelf) {

			valideEdadPanel.SetActive (false);
		}

		if (gameObject.activeSelf) {

			gameObject.SetActive (false);
		}
	}
}

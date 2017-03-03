using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateDataScript : MonoBehaviour {

	public static UpdateDataScript updateData;

	public GameObject imgUpdate;
	public GameObject popupMessage;
	public GameObject popupConfirmation;
	public Button acceptBtn;

	private GameObject objConfirmationPopup;
	private string actionDelete;

	public float rotationSpeed = 0.1f;
	public float timeWaitAfterLoad;

	public GameObject eventManager;

	private int indexTabContinue;

	private string updateState = "none";
	public string _updateState {get{ return updateState; } set{ updateState = value; }}

	// Use this for initialization
	void Start () {
	
		//acceptBtn.GetComponent<Button> ().onClick.AddListener (ClosePopup);

	//	InvokeRepeating("RotateImg",0,rotationSpeed);
	}
	
	// Update is called once per frame
	void Update () {

	}




	public IEnumerator CheckInternet  ( System.Action<bool> hasInternet ){
		bool result = false;
		string chcInternet = "http://2WAYSPORTS.COM/2waysports/Ecuador/Barcelona/ConexionInternet/isConection.php?conection=validarConexion";
		WWW getData = new WWW( chcInternet );
		yield return getData;
		Debug.Log( "fucd: " +getData.text );
		if ( getData.text == "ConexionEstablecida") {
			Debug.Log( getData.text );
			result = true;
		}else{	
			// mostrart pop up 
		}
		hasInternet( result );
		Debug.Log("EL INTERNET ES : " + result);
	}

	public void RunUpdatePanel(){

		gameObject.SetActive (true);

		popupMessage.SetActive (false);
		popupConfirmation.SetActive (false);
		
		imgUpdate.SetActive (true);

		//CancelInvoke("RotateImg");

		//InvokeRepeating("RotateImg",0,rotationSpeed);
	}

	public void StopUpdatePanel(){
		//StartCoroutine (StopUpdate ());
		//ApiChat.main.StartCoroutine (StopUpdate ());
		StopUpdate ();
	}

	private void StopUpdate(){
	
		//yield return new WaitForSeconds(timeWaitAfterLoad);

		//CancelInvoke("RotateImg");

		imgUpdate.SetActive (false);
		gameObject.SetActive (false);
		popupConfirmation.SetActive (false);
	}

//	private void RotateImg () {
//
//		popupMessage.SetActive (false);
//		popupConfirmation.SetActive (false);
//		
//		imgUpdate.SetActive (true);
//		imgUpdate.transform.Rotate (0,0,-40);
//	}

	public void RunPopup(string text, int indexTab){

		//CancelInvoke("RotateImg");
		imgUpdate.SetActive (false);
		popupConfirmation.SetActive (false);

		updateState = "isOpen";
		indexTabContinue = indexTab;

		//gameObject.SetActive (true);
		NavigatorManager.main.panelsPrincipales[13]._enablePopUpInfoPanel (1);
		popupMessage.SetActive (true);
		popupMessage.transform.FindChild ("Text").gameObject.GetComponent<Text> ().text = text;
	}

	public void ClosePopup(){

		updateState = "isClose";
		popupMessage.SetActive (false);
		gameObject.SetActive (false);
		popupConfirmation.SetActive (false);

		Debug.Log ("LLAMO A CERRAR POPUP");

		GameObject.Find ("EventManager").GetComponent<NavigationTabPanelsScript> ().SelectedButtonPanel(indexTabContinue);
	}

	public void RunPopupConfirmation(string text, int indexTab, string action, GameObject obj){

		//CancelInvoke("RotateImg");
		imgUpdate.SetActive (false);
		popupMessage.SetActive (false);
		indexTabContinue = indexTab;

		actionDelete = action;
		objConfirmationPopup = obj;

		gameObject.SetActive (true);
		popupConfirmation.SetActive (true);
		popupConfirmation.transform.FindChild ("Text").gameObject.GetComponent<Text> ().text = text;
	}

	public void AcceptConfirmation(){
		
		popupMessage.SetActive (false);
		popupConfirmation.SetActive (false);
		gameObject.SetActive (false);

		if(actionDelete == "deleteUser"){

			//gameObject.SetActive (false);
			//objConfirmationPopup.GetComponent<ScrollRankingGrupoScript> ().RemoveUserToRanking ();
			
		} else if (actionDelete == "deleteGroup"){

			//gameObject.SetActive (false);
			//objConfirmationPopup.GetComponent<ScrollTusRankingScript> ().RemoveGroupRanking ();

		} else if (actionDelete == "recoveryPass"){

			RunUpdatePanel ();
//			eventManager.GetComponent<RegisterAndLogin> ().sendPasstoEmail ();
		}
	}

	public void CancelConfirmation(){

		popupMessage.SetActive (false);
		gameObject.SetActive (false);
		popupConfirmation.SetActive (false);

		GameObject.Find ("EventManager").GetComponent<NavigationTabPanelsScript> ().SelectedButtonPanel(indexTabContinue);
	}
}

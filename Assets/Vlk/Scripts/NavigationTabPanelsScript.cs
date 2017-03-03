using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NavigationTabPanelsScript : MonoBehaviour {

	public GameObject tabButtonsPanel;
	public List<Button> objButtons;
	public List<GameObject> objPanels;

	public int idCurrentEnablePanel;
	public int _idCurrentEnablePanel {get{ return idCurrentEnablePanel; } set{ idCurrentEnablePanel = value; }}

	public int idOldEnablePanel;
	public int _idOldEnablePanel {get{ return idOldEnablePanel; } set{ idOldEnablePanel = value; }}

	[SerializeField]
	private GameObject updateDataPanel;
	public GameObject _updateDataPanel {get{ return updateDataPanel; } set{ updateDataPanel = value; }}

	private float speed = 8f;

	public GameObject profileManager;
	public GameObject RegistrationManager;

	public GameObject chatGo; // Eduardo
	// Use this for initialization

	public GameObject BlockPanel;

	public string goAfterActivation = "none";

	public ScrollPrediccionScript TapPrediccionPanel;

	[SerializeField]
	private GameObject viewUserResultPanel;

	//Variables para Caso especial en Android boton Back:
	public GameObject instructionPanel;
	public Button instructionBackBtn;

	void Start () {
		
		UpdateDataScript.updateData = updateDataPanel.GetComponent<UpdateDataScript> ();
		//EditProfile.myUser =  profileManager.GetComponent<EditProfile>();
//		ProfileUser.MyUser =  profileManager.GetComponent<ProfileUser>();
		//RegistrationUserNClub.MyResgUser = RegistrationManager.GetComponent<RegistrationUserNClub>();
		idCurrentEnablePanel = 0;
		idOldEnablePanel = 0;
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetKeyDown (KeyCode.Escape) && !UpdateDataScript.updateData.isActiveAndEnabled) {

			if (!viewUserResultPanel.activeSelf) {

				if (instructionPanel.activeSelf) {

					if (objPanels [0].GetComponent<RunInstructionsFromPredictionScript> ()._openInstruction) {

						SelectedButtonPanel (1);

					} else {
						instructionBackBtn.onClick.Invoke ();
					}

				} else if (idCurrentEnablePanel != 0 || idOldEnablePanel != 0) {

					idOldEnablePanel = 0;
					SelectedBackButton (0);

				} else {

				}

			} else {
			
				//viewUserResultPanel.SetActive (false);
			}
		}
		*/
	}

	public void DisableTabButtons (){

		tabButtonsPanel.GetComponent<CanvasGroup> ().blocksRaycasts = false;

		/*
		foreach(Button btn in objButtons){

			btn.interactable = false;
		}
		*/
	}

	public void EnableTabButtons (){

		tabButtonsPanel.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		/*
		foreach(Button btn in objButtons){

			btn.interactable = true;
		}
		*/
	}

	public int GetidCurrentEnablePanel(){
	
		return idCurrentEnablePanel;
	}

//	public void SelectedCurrentButtonPanel(int idButton){
//
//		SelectedButtonPanel(int idCurrentEnablePanel);
//	}

	public void SelectedButtonPanel(int idButton){

		//goAfterActivation = idButton.ToString ();

		//Debug.Log ("Go After Activation Value: " + goAfterActivation);

		if (idCurrentEnablePanel != idButton) {

			goAfterActivation = idButton.ToString ();

			if (IsEnableTap (idButton)) {

				idOldEnablePanel = idCurrentEnablePanel;

				int idCurrent = idCurrentEnablePanel;

				idCurrentEnablePanel = idButton;

				disableCurrentTabButton (idCurrent);

				EnableNextTabButton (idButton);

				StartCoroutine (DisableCurrentPanel (idCurrent));

				StartCoroutine (EnableNextPanel (idButton));
				
			} else {
			
				//BlockPanel.SetActive (true);

				//goAfterActivation = idButton.ToString ();	
				//EditProfile.myUser.InitValidation ();
			}
		}
	}

	public void SelectedBackButton(int idButton){

		int idCurrent = idCurrentEnablePanel;

		idCurrentEnablePanel = idButton;

		disableCurrentTabButton (idCurrent);

		EnableNextTabButton (idButton);

		StartCoroutine (DisableCurrentPanel (idCurrent));

		StartCoroutine (EnableNextPanel (idButton));

	}

	public void RunAfterActivationLevelClub(){
	
		if (goAfterActivation == "3") {

			SelectedButtonPanel (3);

		} else if (goAfterActivation == "4") {

			SelectedButtonPanel (4);

		} else if (goAfterActivation == "1") {
		
			TapPrediccionPanel.RunPredictionsHinchaNivelClub ();
		}

//		this.GetComponent<UpdateDataScript> ().RunUpdatePanel ();
//		this.GetComponent<UpdateDataScript> ().StopUpdatePanel ();
	}

	private bool IsEnableTap(int tapIndex){

		if (tapIndex == 0 || tapIndex == 1 || tapIndex == 2 || tapIndex == 4) {

			return true;

		} else {
				return true;
		}
	}

	IEnumerator DisableCurrentPanel (int indexPanel){

		GameObject panelCurrent = objPanels [indexPanel];
		//panelCurrent.SetActive (false);

//		CanvasGroup canvasGroup = panelCurrent.GetComponent<CanvasGroup> ();
//
//		while(canvasGroup.alpha > 0f){
//			canvasGroup.alpha -= Time.deltaTime * speed;
//			yield return null;
//		}

		panelCurrent.SetActive (false);
		//chatGo.SetActive (false);//Eduardo

	//	Debug.Log ("+++++" + panelCurrent.name + " -> Panel Disable +++++");

		yield return null;
	}

	IEnumerator EnableNextPanel (int indexPanel){

		GameObject panelNext = objPanels [indexPanel];

//		CanvasGroup canvasGroup = panelNext.GetComponent<CanvasGroup> ();
//		canvasGroup.alpha = 0f;
//		panelNext.SetActive (true);
//
//		while(canvasGroup.alpha < 1f){
//			canvasGroup.alpha += Time.deltaTime * speed;
//			yield return null;
//		}

		panelNext.SetActive (true);

		yield return null;

//		Debug.Log ("+++++" + panelNext.name + " -> Panel Enable +++++");
	}

	void disableCurrentTabButton (int indexButton){
	
		Button buttonCurrent = objButtons [indexButton];
		buttonCurrent.GetComponent<Image>().color = new Color32(34,34,34,255);

		Image img = buttonCurrent.transform.Find("Image").GetComponent<Image>();
		img.color = new Color32(78, 78, 78, 255);

		Text text = buttonCurrent.transform.Find("Text").GetComponent<Text>();
		text.color = new Color32(78, 78, 78, 255);

//		Button buttonCurrent = objButtons [indexButton];
//		buttonCurrent.GetComponent<Image>().color = new Color(1,1,1,0);
//		Image img = buttonCurrent.transform.Find("Image").GetComponent<Image>();
//		img.color = new Color32(122, 108, 19, 255);
	}

	void EnableNextTabButton (int indexButton){

		Button buttonNext = objButtons [indexButton];
		buttonNext.GetComponent<Image>().color = new Color32(240,188,16,255);

		Transform img = buttonNext.transform.Find("Image");
		img.GetComponent<Image>().color = new Color32(34,34,34,255);

		Text text = buttonNext.transform.Find("Text").GetComponent<Text>();
		text.color = new Color32(34,34,34,255);

//		Button buttonNext = objButtons [indexButton];
//		buttonNext.GetComponent<Image>().color = new Color(0,0,0,255);
//		Transform img = buttonNext.transform.Find("Image");
//		img.GetComponent<Image>().color = new Color32(238,194,27,255);
	}

	public void ShowUpdateDataPanel(){
		updateDataPanel.SetActive (true);
		updateDataPanel.GetComponent<UpdateDataScript> ().RunUpdatePanel ();
	}

	public void HideUpdateDataPanel(){
		updateDataPanel.GetComponent<UpdateDataScript> ().StopUpdatePanel ();
	}

	public void ShowPopup(string text, int indexTabContinue){

		//gameObject.GetComponent<FadePanelsScript> ().FadeInPanel (updateDataPanel,1);
		updateDataPanel.SetActive (true);
		updateDataPanel.GetComponent<UpdateDataScript> ().RunPopup (text, indexTabContinue);
	}

	public void HideUpdaPopup(){

		updateDataPanel.GetComponent<UpdateDataScript> ().StopUpdatePanel ();
	}

	public void ShowPopupConfirmation(string text, int indexTab, string action, GameObject obj){

		updateDataPanel.SetActive (true);
		updateDataPanel.GetComponent<UpdateDataScript> ().RunPopupConfirmation (text, indexTab, action, obj);
	}
}

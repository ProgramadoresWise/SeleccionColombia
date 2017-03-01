using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonParticipaScript : MonoBehaviour {

	public GameObject currentPanel;
	public GameObject nextPanel;
	public bool disableCurrentPanel;
	public bool openTabPanel = false;
	public int indexTabPanelToOpen;

	private float speed = 8f;

	// Use this for initialization
	void Start () {

		this.GetComponent<Button> ().onClick.AddListener (() => OpenOtherPanel(currentPanel, nextPanel, disableCurrentPanel));
	}

	// Update is called once per frame
	void Update () {

	}

	private void OpenOtherPanel(GameObject currentP, GameObject nextP, bool disableCurrentP){

		GameObject eventManager = GameObject.Find ("EventManager");
		eventManager.GetComponent<NavigationTabPanelsScript> ().SelectedButtonPanel (indexTabPanelToOpen);

//		if (!RegisterAndLogin.IsLog) {

//			Debug.Log (">> " + currentP.name + ", " + nextP.name + ", " + disableCurrentP + " <<");
//			//DisableCurrentPanel (currentP, disableCurrentP);
//			//EnableNextPanel (nextP);
//			StartCoroutine (DisableCurrentPanel (currentP, disableCurrentP));
//			StartCoroutine (EnableNextPanel (nextP));
//		} else {
//
//			GameObject eventManager = GameObject.Find ("EventManager");
//			eventManager.GetComponent<NavigationTabPanelsScript> ().SelectedButtonPanel (indexTabPanelToOpen);
//		}
	}

	IEnumerator DisableCurrentPanel (GameObject currentP, bool disableCurrentP){

		if (disableCurrentP) {

			//currentP.SetActive (false);

			CanvasGroup canvasGroup = currentP.GetComponent<CanvasGroup> ();
			//canvasGroup.alpha = 0f;

			while (canvasGroup.alpha > 0f) {
				canvasGroup.alpha -= Time.deltaTime * speed;
				yield return null;
			}

			currentP.SetActive (false);

		} else { //Inactivamos lo botones del "TabButtonPanel" ya que es un Popup el que se abre

			GameObject eventManager = GameObject.Find ("EventManager");
			eventManager.GetComponent<NavigationTabPanelsScript> ().DisableTabButtons ();

		}
		//yield return null;

		Debug.Log ("+++++" + currentP.name + " -> Disable " + disableCurrentP + "+++++");
	}

	IEnumerator EnableNextPanel (GameObject nextP){

		//nextP.SetActive (true);

		CanvasGroup canvasGroup = nextP.GetComponent<CanvasGroup> ();
		canvasGroup.alpha = 0f;
		nextP.SetActive (true);

		while(canvasGroup.alpha < 1f){
			canvasGroup.alpha += Time.deltaTime * speed;
			yield return null;
		}

		Debug.Log ("+++++" + nextP.name + " -> Panel Enable +++++");

	}
}

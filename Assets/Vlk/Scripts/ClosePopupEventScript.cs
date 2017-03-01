using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClosePopupEventScript : MonoBehaviour {

	//int a = 3;
	//int b = 5;

	private EventTrigger trigger;

	private float speed = 8f;

	void Start () {

		trigger = gameObject.AddComponent <EventTrigger> ();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		//entry.callback.AddListener((eventData) => Test(a,b));
		entry.callback.AddListener((eventData) => ClosePopup());
		trigger.triggers.Add(entry);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ClosePopup(){
	
		if (gameObject.activeSelf) {
		
			StartCoroutine(DisablePopupPanel ());
		}
	}

	IEnumerator DisablePopupPanel (){

		CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup> ();

		while(canvasGroup.alpha > 0f){
			canvasGroup.alpha -= Time.deltaTime * speed;
			yield return null;
		}

		gameObject.SetActive (false);

		//Enable TabButtons:
//		if(RegisterAndLogin.IsLog){
		
			GameObject eventManager = GameObject.Find ("EventManager");
			eventManager.GetComponent<NavigationTabPanelsScript> ().EnableTabButtons ();
//		}

		Debug.Log ("+++++" + gameObject.name + " -> Disable +++++");
	}

	/*
	private void Test(int currentP, int nextP){

		Debug.Log (">> " + currentP + ", " + nextP + " <<");
	}
	*/
}

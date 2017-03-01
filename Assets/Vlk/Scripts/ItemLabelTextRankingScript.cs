using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemLabelTextRankingScript : MonoBehaviour {

	void Awake(){
	

	}

	// Use this for initialization
	void Start () {
	
		if(gameObject.GetComponent<Toggle>().isOn == true){

			Text itemLabelText = gameObject.transform.FindChild ("Item Label").GetComponent<Text> ();
			itemLabelText.color = new Color32 (0, 0, 0, 255);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

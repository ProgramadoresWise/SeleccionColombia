using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Loading : MonoBehaviour {
	
	public List<string> msgs;
	public Text msgLoadingText;


	void Start( ){
		if( msgs.Count > 0)
			msgLoadingText.text = msgs[Random.Range(0,4)];
	}



	void OnDisable( ){
		if( msgs.Count > 0)
			msgLoadingText.text = msgs[Random.Range(0,4)];
	}

}

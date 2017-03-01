using UnityEngine;
using System.Collections;
using System.IO;

public class RefreshNews : MonoBehaviour {


	public NewsManager newContent;
	public bool update;


	void Update( ){
		if( Input.GetMouseButtonUp(0) && update && !NavigatorManager.main.susccesfullEventInback ){
			update = false;
			StartCoroutine(newContent.createNew());
		}
	}

	void OnTriggerEnter2D ( Collider2D col){
		if(col.transform.tag == "RefreshNews" && !update ){
			Debug.Log("TOCANDO");
			update = true;
		}
	}
//
//	void OnTriggerStay2D ( Collider2D col){
//		if( Input.GetMouseButton(0) && update ){
//			Debug.Log("LLAMANDO");
//			StartCoroutine(newContent.createNew());
//			update = false;
//		}
//	}

	void OnTriggerExit2D ( Collider2D col){
		if(col.transform.tag == "RefreshNews"  ){
			Debug.Log("NO TOCANDO");
			update = false;
		}
	}

}

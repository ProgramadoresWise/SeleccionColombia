using UnityEngine;
using System.Collections;
using UnityEngine.UI;




public class scrollComponent : MonoBehaviour {

	public GameObject[] sonDesactive;
	public Image[] imgDesactive;
	public component cm;
	float height;


	public bool IsDownload;
	public bool callImg;

	void Start( ){
		this.GetComponent<Image>().enabled = false;
		foreach( GameObject gm in sonDesactive ){
			gm.SetActive(false);
		}	
	}


//
//	#region STAY
//	void OnTriggerStay2D (Collider2D col){
//
//		if( col.gameObject.tag == "UI" ){
//			this.GetComponent<Image>().enabled = true;
//			foreach( GameObject gm in sonDesactive ){
//				gm.SetActive(true); }
//		}
//	}
//
//	#endregion


	#region TRIGGER
	void OnTriggerEnter2D (Collider2D col){

		if( col.gameObject.tag == "UI" ){
			this.GetComponent<Image>().enabled = true;
			foreach( GameObject gm in sonDesactive ){
				gm.SetActive(true); }
		}
		resizeComponents(true);
	}

	#endregion

	#region EXIT
	void OnTriggerExit2D (Collider2D col){
		if( col.gameObject.tag == "UI" ){
			this.GetComponent<Image>().enabled = false;
			foreach( GameObject gm in sonDesactive ){
				gm.SetActive(false); }	
		}
		resizeComponents(false);
	}

	#endregion










	void resizeComponents( bool act){
		switch( act ){
		case true:  /// ACTIVADO
			if(cm == component.ViewNew){
				height = this.GetComponent<RectTransform>().sizeDelta.y;
				this.GetComponent<LayoutElement>().enabled =false;
			}
			break;

		case false:  // DESACTIVADO
			if(cm == component.ViewNew){
				height = this.GetComponent<RectTransform>().sizeDelta.y;
				this.GetComponent<LayoutElement>().enabled =  true;
				this.GetComponent<LayoutElement>().preferredHeight = height;
			}
			break;
		}
	}

	public enum component{
		ViewNew,
		convocados
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;




public class testraycast : MonoBehaviour {

	public GameObject[] sonDesactive;
	public Image[] imgDesactive;
	public component cm;
	float height;


	void Start( ){
		this.GetComponent<Image>().enabled = false;
		foreach( GameObject gm in sonDesactive ){
			gm.SetActive(false);
		}	
	}

	void OnTriggerStay2D (Collider2D col){

		if( col.gameObject.tag == "UI" ){
			this.GetComponent<Image>().enabled = true;
			foreach( GameObject gm in sonDesactive ){
				gm.SetActive(true); }
		}
	}

	void OnTriggerEnter2D (Collider2D col){

		if( col.gameObject.tag == "UI" ){
			this.GetComponent<Image>().enabled = true;
			foreach( GameObject gm in sonDesactive ){
				gm.SetActive(true); }
		}
		resizeComponents(true);
	}

	void OnTriggerExit2D (Collider2D col){
		if( col.gameObject.tag == "UI" ){
			this.GetComponent<Image>().enabled = false;
			foreach( GameObject gm in sonDesactive ){
				gm.SetActive(false); }	
		}
		resizeComponents(false);
	}



	void resizeComponents( bool act){
		switch( act ){
		case true:  /// ACTIVADO
			if(cm == component.ViewNew){
				height = this.GetComponent<RectTransform>().sizeDelta.y;
				this.GetComponent<LayoutElement>().enabled =false;
//				this.GetComponent<LayoutElement>().preferredHeight = height;
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

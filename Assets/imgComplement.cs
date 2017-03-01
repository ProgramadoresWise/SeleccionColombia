using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class imgComplement : MonoBehaviour {


	public ScrollRect scroll;
	public bool inTouch;
	public float inertia;



	public void TouchActiveScroll( ){

		if(scroll.velocity.y == 0 ){
			this.gameObject.SetActive(true);
			inTouch = false;
			return;
		}
			

		if( scroll.velocity.y >= inertia*-1 &&  scroll.velocity.y <= inertia){
			this.gameObject.SetActive(true);
			inTouch = false;
		}else{
			this.gameObject.SetActive(false);
			inTouch = true;
		}

	}

	void OnTriggerStay2D ( Collider2D col){

		if(col.transform.tag == "scrollComponent" && !inTouch && !col.GetComponent<scrollComponent>().IsDownload && !col.GetComponent<scrollComponent>().callImg && ( scroll.velocity.y >= inertia*-1 &&  scroll.velocity.y <= inertia)){
			if(col.GetComponent<scrollComponent>().cm == scrollComponent.component.convocados){
				col.GetComponent<JugadoresConvocados>().loadImagePlayerConvocados();
			}

			if(col.GetComponent<scrollComponent>().cm == scrollComponent.component.ViewNew){
				StartCoroutine (  col.GetComponent<viewNew> ().LoadImageInit ( NewsManager.main.update )); 
			}

			col.GetComponent<scrollComponent>().callImg = true;
			col.GetComponent<BoxCollider2D>().enabled = false;
//			col.GetComponent<scrollComponent>().IsDownload = true;
			col.GetComponent<scrollComponent>().enabled = false;
		}
	}






}

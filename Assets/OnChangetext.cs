using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MaterialUI;

public class OnChangetext : MonoBehaviour {


	public Text textDropdowun;
	public RectTransform tittleDropDown;
	public float TopPlus;
	public float CantPlus;


	public bool isInteractable;

	void Update( ){
		if(isInteractable){
			if( this.GetComponent<MaterialDropdown>().currentlySelected == -1){
				textDropdowun.color = new Color(0,0,0,0 );
				TopPlus=0;
				isInteractable = false;
			}else {
				textDropdowun.color = new Color(0,0,0,255);
			}
		}


		if(tittleDropDown.offsetMax.y < TopPlus )
			tittleDropDown.offsetMax = new Vector2(0,tittleDropDown.offsetMax.y+1f*Time.fixedTime);
		else if(tittleDropDown.offsetMax.y == 0  )
			tittleDropDown.offsetMax = new Vector2(0,tittleDropDown.offsetMax.y-1f*Time.fixedTime);
	}


	public void ChangeSize( ){
		isInteractable = true;
		TopPlus = CantPlus;
	}

}

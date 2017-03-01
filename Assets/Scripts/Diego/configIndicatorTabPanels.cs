using UnityEngine;
using System.Collections;
using System;
using MaterialUI;

public class configIndicatorTabPanels : MonoBehaviour {

	public float _posCant;
	public int cantTbs;
	float offsetx, offsety;
	float newPos;
	public RectTransform content;
	public RectTransform indicator;
	public TabView _tapview;
	 int indSwipe;

	void Start( ){
		offsetx = indicator.offsetMin.x;
	}

	public void _translate ( float f){
			newPos = f;
			Debug.Log("pos ini: " + newPos);
	}


	void Update () {

		offsetx = indicator.offsetMin.x;
		offsety = indicator.offsetMax.x;

		if(indSwipe != _tapview.currentPage){
			TapIndexSwipe(cantTbs);
		}
			if(newPos > offsetx ){
				indicator.offsetMin = new Vector2 (indicator.offsetMin.x +_posCant , indicator.offsetMin.y );
				indicator.offsetMax = new Vector2 (indicator.offsetMin.x, indicator.offsetMax.y );

			}else if ( newPos < offsetx){
				indicator.offsetMin = new Vector2 (indicator.offsetMin.x -_posCant, indicator.offsetMin.y );
				indicator.offsetMax = new Vector2 (indicator.offsetMin.x, indicator.offsetMax.y );
			}


//		if( Input.GetMouseButton(0)){
//			indicator.offsetMin = new Vector2 ( Input.mousePosition.x*-1 , indicator.anchoredPosition.y  );
//			indicator.offsetMax = new Vector2 (indicator.offsetMin.x, indicator.offsetMax.y );
//		}

	}



	void TapIndexSwipe(int countTabs ){
//		Debug.Log( _tapview.currentPage );
		indSwipe = _tapview.currentPage;
		if(countTabs == 4) 
			newPos = (indSwipe == 0 )?  0: (indSwipe == 1)? 180 : (indSwipe == 2)? 360: 540;
		else if(countTabs == 3 )
			newPos = (indSwipe == 0 )?  0: (indSwipe == 1)? 240 : 480;
		else if(countTabs == 2 )
			newPos = (indSwipe == 0 )?	0: 360;
	}

}

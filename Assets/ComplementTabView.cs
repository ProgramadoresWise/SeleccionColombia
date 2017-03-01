using UnityEngine;
using System.Collections;
using MaterialUI;

public class ComplementTabView : MonoBehaviour {


	TabView thisTab;
	[SerializeField]
	int MaxIndex;

	// Use this for initialization
	void Awake ( ) {
		thisTab = this.GetComponent<TabView>();
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetMouseButtonUp(0) && thisTab.gameObject.activeSelf && thisTab.currentPage >= MaxIndex){
			thisTab.SetPage(0,true);
		}
			
	}



	public void changeCantScroll( int cnt ){
		MaxIndex = cnt;
	}
}

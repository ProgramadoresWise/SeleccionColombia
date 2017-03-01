using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DropdownRankingData : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		this.GetComponent<Dropdown> ().ClearOptions ();
		this.GetComponent<Dropdown> ().options.Add (new Dropdown.OptionData ("<b>RANKING GENERAL</b> \nPRONOSTICO"));
		this.GetComponent<Dropdown> ().options.Add (new Dropdown.OptionData ("Chu Chu"));
		this.GetComponent<Dropdown> ().value = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

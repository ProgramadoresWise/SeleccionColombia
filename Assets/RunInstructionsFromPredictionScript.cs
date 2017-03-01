using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RunInstructionsFromPredictionScript : MonoBehaviour {

	public Button goInstructionBtn;

	[SerializeField]
	private bool openInstruction;
	public bool _openInstruction {get{ return openInstruction; } set{ openInstruction = value; }}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable() {

		if(openInstruction){
		
			goInstructionBtn.onClick.Invoke ();
		}
	}

	void OnDisable () {

		if(openInstruction){

			openInstruction = false;
		}
	}
}

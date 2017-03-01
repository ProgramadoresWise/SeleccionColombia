using UnityEngine;
using System.Collections;

public class GoalsPanelScript : MonoBehaviour {

	[SerializeField]
	private ScrollPrediccionScript scrollPrediction;

	[SerializeField]
	private GameObject goalsTutorial;

	void OnEnable() {

		if(DataApp.main.GetMyInfoString("ShowGoalsTutorial") != "no" && scrollPrediction.panelsData.Count <= 0){

			goalsTutorial.SetActive (true);
		}

		DataApp.main.SetMyInfo("ShowGoalsTutorial", "no", 3);
	}
}

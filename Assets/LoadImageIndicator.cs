using UnityEngine;
using System.Collections;

public class LoadImageIndicator : MonoBehaviour {

	public GameObject loadIndicator;


	public void LoadInd( bool act){
		loadIndicator.SetActive(act);
	}
}

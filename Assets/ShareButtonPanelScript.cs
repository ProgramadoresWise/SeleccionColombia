using UnityEngine;
using System.Collections;

public class ShareButtonPanelScript : MonoBehaviour {

	public NativeShare shareObj;

	private string sharePanel;

	public void ShowShareSocialMedia(string panel){

		sharePanel = panel;
		gameObject.SetActive (true);
	}

	public void ShareSocialMedia(string shareType){

		gameObject.SetActive (false);

		if(sharePanel == "sharePronostico"){

			shareObj.SharePrediction ("", shareType);

		} else if (sharePanel == "shareRanking"){

			//shareObj.ShareScreenshotWithText ("", shareType);
			shareObj.ShareRanking ("", shareType);
		}
	}
}

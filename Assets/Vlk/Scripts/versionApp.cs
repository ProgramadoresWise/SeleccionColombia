using UnityEngine;
using System.Collections;

public class versionApp : MonoBehaviour {


	public int numberVersion;
	public int myVersion;
	public GameObject UpdateVersion;

	// Use this for initialization
	void Awake () {
		//StartCoroutine (DownloadAppVersion ());
	}

	// Update is called once per frame
	IEnumerator DownloadAppVersion ( ){
		WWW r = new WWW ("http://2WAYSPORTS.COM/2waysports/Ecuador/Barcelona/Admin/versionAppPro.php?opc=revision");
		yield return r;
		if( string.IsNullOrEmpty ( r.error )){
			if( r.text == "0" ){
				WWW w = new WWW ("http://2WAYSPORTS.COM/2waysports/Ecuador/Barcelona/Admin/versionAppPro.php?opc=versionApp");
				yield return w;
				if( string.IsNullOrEmpty ( w.error )){
					if( !string.IsNullOrEmpty( w.text)){
						numberVersion = int.Parse ( w.text );
						if(numberVersion != myVersion){
							UpdateVersion.SetActive (true);
						}
					}
				}
			}
		}
	}



}

using UnityEngine;
using System.Collections;

using System.IO;

using UnityEngine.UI;

public class buttonInteractive : MonoBehaviour
{
	[Header ("Share")]
	public Camera cameraRender;
	public Texture2D texture;

	public string text;

	[Header("Open App")]
	public string idAndroid;
	public string idIOS;


	public void OpenApp(){
		Debug.Log ("envio abrir");
		if (!Application.isEditor)
		{
			#if UNITY_ANDROID
//			mobileManager.openAppAndroid (idAndroid);
			#endif

			#if UNITY_IOS
			//mobileManager.Instancer.openAppInStore (idIOS);
			#endif
		}
		else
		{
			Debug.Log ("AppstoreTestScene:: Cannot view app in Editor.");
		}
	}


}
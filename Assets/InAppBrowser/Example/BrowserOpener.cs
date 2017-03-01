using UnityEngine;
using System.Collections;

public class BrowserOpener : MonoBehaviour 
{
	public string pageToOpen = "http://www.google.com";

	public void OnButtonClicked()
	{
		StartCoroutine("GetURL");
	}

	public IEnumerator GetURL()
	{
		string url = string.Empty;

		url = "http://fcf.2waysports.com/2waysports/Colombia/actualizaciones/php/getURL1.php";
		
		WWW www = new WWW(url);
		yield return www;
		pageToOpen = www.text;
		InAppBrowser.OpenURL(pageToOpen);
	}
}

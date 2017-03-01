using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class LoadImageScript : MonoBehaviour {


	public string urlImage = "http://2WAYSPORTS.COM/2waysports/Ecuador/Barcelona/Pronostico/Patrocinadores/PadrinoOro2.png";

	public float rotationSpeed = 0.1f;

	public float timeWaitAfterLoad;

	public GameObject currentPanel;

	//public GameObject panelToOpen;

	private string initImgName;

	private bool error = false;

	void Awake() {
		
		initImgName = gameObject.GetComponent<Image> ().sprite.name;
	}

	IEnumerator Start(){
	/*	
		InvokeRepeating("RotateImg",0,rotationSpeed);

		StartCoroutine (LoadImage());

		yield return null;
	*/
		yield return null;
	}
	
	// Update is called once per frame
	void Update () {

		if(error && GameObject.Find ("EventManager").GetComponent<NavigationTabPanelsScript> ()._updateDataPanel.GetComponent<UpdateDataScript>()._updateState == "isClose"){

			//InvokeRepeating("RotateImg",0,rotationSpeed);
			error = false;
			StartCoroutine (LoadImage());
		}

		//Debug.Log ("-------" + gameObject.GetComponent<Image>().sprite.name + "-------");
		//Debug.Log ("-------" + initImgName + "-------");
	}

	IEnumerator LoadImage(){
	
		WWW img = new WWW(ToAntiCache(urlImage));
		yield return img;

		transform.rotation = Quaternion.identity;

		if (string.IsNullOrEmpty (img.error)) {
			CancelInvoke("RotateImg");
			Rect size = new Rect (0, 0, img.texture.width, img.texture.height);
			//uiImg.sprite = Sprite.Create (img.texture, size, Vector2.zero);
			gameObject.GetComponent<Image> ().sprite = Sprite.Create (img.texture, size, Vector2.zero);
		} else {
		
			error = true;
			Debug.Log ("Load Image Failed");
			GameObject eventManager = GameObject.Find ("EventManager");
			//eventManager.GetComponent<NavigationTabPanelsScript> ().ShowPopup ("Load Image Failed", eventManager.GetComponent<NavigationTabPanelsScript> ()._idCurrentEnablePanel);
		}

	}

	public  string ToAntiCache( string url)
	{
		string r = "";
		r += UnityEngine.Random.Range(1000000,9000000).ToString();
		r += UnityEngine.Random.Range(1000000,9000000).ToString();
		string result="";
		if(url.Substring(url.Length -4,4   ) == ".php" || url.Substring(url.Length -4,4   ) == ".png"|| url.Substring(url.Length -4,4   ) == ".jpg"){
			result = url + "?key=" + r;
		}else{
			result = url + "&key=" + r;
		}
		return result;
	}

	void RotateImg () {

		transform.Rotate (0,0,-40); //Rota 40 grados por segundo sobre el eje z
	}


	void OnEnable() {

		if (initImgName == gameObject.GetComponent<Image> ().sprite.name) {
		
			InvokeRepeating("RotateImg",0,rotationSpeed);

			StartCoroutine (LoadImage());
		}

		//print("script was enabled");
	}

	void OnDisable() {

		CancelInvoke("RotateImg");

		//print("script was disable");
	}
}

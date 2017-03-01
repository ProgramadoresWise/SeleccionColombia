using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class LoadInstructionsScript : MonoBehaviour {


	public string urlImage = "http://2WAYSPORTS.COM/2waysports/Ecuador/Barcelona/Pronostico/Patrocinadores/PadrinoOro2.png";

	public float rotationSpeed = 0.1f;

	public float timeWaitAfterLoad;

	public GameObject currentPanel;

	//public GameObject panelToOpen;

	private string initImgName;

	private bool error = false;

	private bool instructionIsLoad = false;

	void Awake() {
		
		//initImgName = gameObject.GetComponent<Image> ().sprite.name;
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

		string url = ToAntiCache(urlImage);
	
		Debug.Log("Las Instrucciones Son: " + url);

		Texture2D tex;
		tex = new Texture2D(4, 4, TextureFormat.DXT1, false);

		WWW img = new WWW("http://192.169.155.181/~fcf2waysports/2waysports/Colombia/PollaTricolor/InstruccionesPronostico.jpg");
		yield return img;

		//Debug.Log("IMAGEN ISNTRUCCIONES: " + img.ToString());

		transform.rotation = Quaternion.identity;

		if (string.IsNullOrEmpty (img.error)) {
			CancelInvoke("RotateImg");

			img.LoadImageIntoTexture(tex);

			Rect size = new Rect (0, 0, tex.width, tex.height);
			gameObject.GetComponent<Image> ().sprite = Sprite.Create (tex, size, Vector2.zero);

//			Rect size = new Rect (0, 0, img.texture.width, img.texture.height);
//			gameObject.GetComponent<Image> ().sprite = Sprite.Create (img.texture, size, Vector2.zero);
		} else {
		
			error = true;
			Debug.Log ("Load Image Failed");
			GameObject eventManager = GameObject.Find ("EventManager");
			//eventManager.GetComponent<NavigationTabPanelsScript> ().ShowPopup ("Load Image Failed", eventManager.GetComponent<NavigationTabPanelsScript> ()._idCurrentEnablePanel);
		}

		instructionIsLoad = true;

		UpdateDataScript.updateData.StopUpdatePanel ();

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

		//if (initImgName == gameObject.GetComponent<Image> ().sprite.name) {
		if (!instructionIsLoad) {

			UpdateDataScript.updateData.RunUpdatePanel ();
		
			//InvokeRepeating("RotateImg",0,rotationSpeed);

			StartCoroutine (LoadImage());
		}

		//print("script was enabled");
	}

	void OnDisable() {

		CancelInvoke("RotateImg");

		//print("script was disable");
	}
}

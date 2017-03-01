using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadImgRankingScript : MonoBehaviour {

	private string urlImage = "http://2WAYSPORTS.COM/2waysports/Ecuador/Barcelona/Pronostico/Patrocinadores/";

	private bool imageIsLoad = false;

	private bool error = false;

	private string userPhotoName;

	private bool photoNameIsReady = false;

	private int flag;

	IEnumerator LoadImage(int f){

		Debug.Log (DataApp.main.host + "Users/PhotosRankings/" + userPhotoName);

		//WWW img = new WWW(urlImage + userPhotoName);
		string url = DataApp.main.host + "Users/PhotosRankings/" + userPhotoName;
		WWW img = new WWW(ToAntiCache(url));
		yield return img;

		if (string.IsNullOrEmpty (img.error)) {

			if(f == flag){

				Rect size = new Rect (0, 0, img.texture.width, img.texture.height);
				gameObject.GetComponent<Image> ().sprite = Sprite.Create (img.texture, size, Vector2.zero);
			}

		} else {

			error = true;
			/*
			Debug.Log ("Load Image Failed");
			GameObject eventManager = GameObject.Find ("EventManager");
			eventManager.GetComponent<NavigationTabPanelsScript> ().ShowPopup ("Load Image Failed", eventManager.GetComponent<NavigationTabPanelsScript> ()._idCurrentEnablePanel);
			*/
		}

		//imageIsLoad = (!error) ? true : false;
		imageIsLoad = true;

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

	public void loadImgRanking(string namePhoto){

		gameObject.GetComponent<Image> ().sprite = Resources.Load <Sprite> ("Various/RankingDefaultPhoto");
	
		userPhotoName = namePhoto;
		photoNameIsReady = true;	

		flag++;
		StartCoroutine (LoadImage(flag));
	}


	void OnEnable() {

		flag = 0;

		if (imageIsLoad == false && photoNameIsReady == true) {

			StartCoroutine (LoadImage(flag));
		}
	}
}

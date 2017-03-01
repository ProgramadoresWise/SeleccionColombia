using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BigViewNew : MonoBehaviour {

	public viewNew shareViewNew;
	public Sprite deafultNew;
	public RectTransform Content;
	public Text tituloN, tituloback;
	public Text descN, descback;
	public Image imgN;
	public GameObject scrollnewView, bigNewbackg, buttonsPanelShare;
	[Space(20)]


	public Text titleNewShare;
	public Image imgNewShare;

//	public SocialLog shareInstance;
	float sizeImgh, sizeImgw;





	public IEnumerator BigNews( viewNew nNew ) {
//		InternetManager.main.loading.SetActive(true);
		// 1 es Video, 0 es noticia
		Content.anchoredPosition = new Vector2( Content.anchoredPosition.x, 0 );
		DataApp.main.EnableLoading();

		tituloN.text = " ";
		tituloback.text = " ";
		descN.text = " ";
		descback.text = " ";

		yield return new WaitForSecondsRealtime(.5f);

		imgN.sprite = ImgLoadManager.main.NewsImg(imgN,nNew.idNew.ToString(),false);

		sizeImgh = nNew.sizeheight;
		sizeImgw= nNew.sizewidth;

			tituloN.text = nNew.tituloNew.text;
			tituloback.text = nNew.tituloNew.text;
			descN.text = nNew.descNew.text;
			descback.text = nNew.descNew.text;
			scrollnewView.SetActive (true);
			imgN.sprite = nNew.imageNew.sprite;
			imgN.GetComponent<LayoutElement> ().preferredHeight = nNew.imageNew.GetComponent<LayoutElement> ().preferredHeight + 100;
			this.gameObject.SetActive (true);
			imgNewShare.sprite = imgN.sprite;
			shareViewNew.imageNew.GetComponent<LayoutElement> ().preferredHeight = nNew.imageNew.GetComponent<LayoutElement> ().preferredHeight;

		DataApp.main.DisableLoading();
	}

	public void setNewImageShare(){
		StartCoroutine ( _setNewImageShare ( tituloN.text ));
	}




	IEnumerator _setNewImageShare( string titleNew ){
		shareViewNew.tituloNew.text = titleNew;
//		InternetManager.main.loading.SetActive (true);
		buttonsPanelShare.SetActive (false);
		yield return new WaitForEndOfFrame ();
//		IEnumerator waitEnum = shareInstance.ShareNews ("news", sizeImgw, sizeImgh);
//		yield return shareInstance.StartCoroutine (waitEnum);
	}


	public  string ToAntiCache( string url){
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
}

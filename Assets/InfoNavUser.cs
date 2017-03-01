using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoNavUser : MonoBehaviour {

	public Image userImgProfile;
	public Image userImg;
	public Text nameUser;
	// Use this for initialization
	void Start (){
//		userImg.sprite = DataApp.main.userImgSprite( userImg, DataApp.main.GetMyID().ToString(),"PhotoUser","Users/Photos",false );
		userImg.sprite = ImgLoadManager.main.UsersImg( userImg , DataApp.main.GetMyID().ToString(), false);
		userImgProfile.sprite = userImg.sprite;
		nameUser.text = User.main.GetMyName() + " " + User.main.GetMyLastName();
//		userImg.sprite = userImgProfile.sprite;
	}

	public void loadData(){
		nameUser.text = User.main.GetMyName() + " " + User.main.GetMyLastName();
		userImgProfile.sprite = userImg.sprite;
//		userImg.sprite = DataApp.main.userImgSprite( userImg, DataApp.main.GetMyID().ToString(),"PhotoUser","Users/Photos",false );
	}


}

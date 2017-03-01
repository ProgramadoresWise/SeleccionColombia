using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class carnet : MonoBehaviour {

	public Text nameUser, fechaInit, fechaVent, numHincha;
	public Image photuser;



	public void loadCarnet(Sprite photo, string name, string _fechaInit, string _fechavent, int _numHincha ){
		nameUser.text = name;
		fechaInit.text = "DESDE "+_fechaInit;
		fechaVent.text = "FV. "+_fechavent;
		numHincha.text = _numHincha.ToString("0000000");
		photuser.sprite = photo;

		PlayerPrefs.SetString("codigoBarrasNum",_numHincha.ToString());
	}









}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ElementoVotacion : MonoBehaviour 
{
	public string idElemento;
	public Image imgElemento;
	public Text txtNombreElemento;
	public Text txtVotos;
	public Toggle toggleVotacion;
	public int _votos;

	GetTuEliges main;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GenerarContenidoElemento(string id,Sprite img, string nomElem, string votos) {
		idElemento = id;
		imgElemento.sprite = img;
		imgElemento.sprite = ImgLoadManager.main.TuElijesImg(imgElemento,id,main.update);
		txtNombreElemento.text = nomElem;
		txtVotos.text = "<color=#00ff00>"+votos+"</color> <size=8>votos</size>";
	}

	public void SetMain(GetTuEliges mainRef)
	{
		main = mainRef;
	}


	public void SendVoto()
	{
		if(main)
		{
			main.StartCoroutine(main.SendVoto( this, idElemento, toggleVotacion.isOn));
		}
		else
		{
			Debug.LogError("Main no asignado aun en "+gameObject.name);
		}
	}
}

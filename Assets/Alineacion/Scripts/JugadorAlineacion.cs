using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JugadorAlineacion : MonoBehaviour 
{
	[Header("Informacion Jugador")]
	public string linea;
	public string posicion;
	public Sprite foto;

	// Use this for initialization
	void Start () {
		this.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
		this.gameObject.AddComponent<LayoutElement>();
		this.GetComponent<LayoutElement>().preferredWidth = 0;
		this.GetComponent<LayoutElement>().preferredHeight = 0;
		this.GetComponent<LayoutElement>().flexibleHeight = 1;
		this.GetComponent<LayoutElement>().flexibleWidth = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

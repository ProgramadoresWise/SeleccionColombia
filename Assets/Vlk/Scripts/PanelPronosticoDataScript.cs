using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelPronosticoDataScript : MonoBehaviour {

	public Text goalTitle;

	public int indexPanel;
	public int _indexPanel {get{ return indexPanel; } set{ indexPanel = value; }}

	private int idGoleador;
	public int _idGoleador {get{ return idGoleador; } set{ idGoleador = value; }}

	public Text goleadorText;

	public Text goalTypeText;

	public Text minGoalText;

	private int idPaseGoal;
	public int _idPaseGoal {get{ return idPaseGoal; } set{ idPaseGoal = value; }}

	public Text paseGoalText;

	private GameObject inputFieldObj;
	public GameObject _inputFieldObj {get{ return inputFieldObj; } set{ inputFieldObj = value; }}

	//**** - ******
	public InputField minutes;

	public Dropdown tipoGol;
	public Dropdown asistencia;
	public GameObject arrowAsistencia;
	public Dropdown goleador;
	public GameObject arrowGoleador;

	// Use this for initialization
	void Start () {
	
		//gameObject.GetComponent<Button> ().onClick.AddListener(delegate {DeletePanelData();});
		gameObject.transform.FindChild("DeleteButton").GetComponent<Button>().onClick.AddListener(DeletePanelData);
	}
	/*
	void Update(){
	
		goleadorText.text = "ppppp";
	}
	*/
	private void DeletePanelData() {

		inputFieldObj.GetComponent<ScrollPrediccionScript> ().DeletePanelData (indexPanel);
	}

	public void ValidarTipoDeGol(){

		Debug.Log ("Se selecciona: " + tipoGol.captionText.text);

		if (tipoGol.captionText.text == "PELOTA QUIETA") {

			asistencia.captionText.text = "Sin Pase Gol";

			asistencia.GetComponent<Dropdown> ().enabled = false;

			arrowAsistencia.SetActive (false);

			if (goleador.captionText.text == "Sin Goleador") {

				goleador.value = -1;

				goleador.captionText.text = "Goleador";

				goleador.GetComponent<Dropdown> ().enabled = true;

				arrowGoleador.SetActive (true);
			}

		} else if (tipoGol.captionText.text == "AUTO GOL") {

			asistencia.captionText.text = "Sin Pase Gol";

			asistencia.GetComponent<Dropdown> ().enabled = false;

			arrowAsistencia.SetActive (false);

			goleador.captionText.text = "Sin Goleador";

			goleador.GetComponent<Dropdown> ().enabled = false;

			arrowGoleador.SetActive (false);

		} else if (goleador.captionText.text == "Sin Goleador") {

			asistencia.value = -1;

			asistencia.captionText.text = "Pase Gol";

			asistencia.GetComponent<Dropdown> ().enabled = true;

			arrowAsistencia.SetActive (true);

			goleador.value = -1;

			goleador.captionText.text = "Goleador";

			goleador.GetComponent<Dropdown> ().enabled = true;

			arrowGoleador.SetActive (true);

		} else if (asistencia.captionText.text == "Sin Pase Gol") {

			asistencia.value = -1;

			asistencia.captionText.text = "Pase Gol";

			asistencia.GetComponent<Dropdown> ().enabled = true;

			arrowAsistencia.SetActive (true);
		}
	}

//	public void ValidarTipoDeGol(){
//
//		Debug.Log ("Se selecciona: " + tipoGol.captionText.text);
//
//		if (tipoGol.captionText.text == "PELOTA QUIETA") {
//
//			asistencia.captionText.text = "Sin Pase Gol";
//
//			asistencia.GetComponent<Dropdown> ().enabled = false;
//
//			arrowAsistencia.SetActive (false);
//
//		} else {
//		
//			asistencia.captionText.text = "";
//
//			asistencia.value = -1;
//
//			asistencia.GetComponent<Dropdown> ().enabled = true;
//
//			arrowAsistencia.SetActive (true);
//		}
//	}

	void OnDisable() {

		goleador.GetComponent<Dropdown> ().Hide ();
		tipoGol.GetComponent<Dropdown> ().Hide ();
		asistencia.GetComponent<Dropdown> ().Hide ();
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayersDropDown : MonoBehaviour {

	public GameObject panelPronosticoData;

	public List<string> goalsTipe;

	private FootballPlayerListScript footballPlayers;

	// Use this for initialization
	void Start () {

		SetDropDownOptions ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetDropDownOptions(){

		footballPlayers = GameObject.Find ("TabPrediccionPanel").GetComponent<GetFootballPlayersDataScript> ().footballPlayersdata;
	
		gameObject.GetComponent<Dropdown> ().ClearOptions ();

		if (gameObject.name == "GoleadorDropdown" || gameObject.name == "PaseGolDropdown") {

			gameObject.GetComponent<Dropdown> ().onValueChanged.AddListener (delegate {
				SeDesplego ();
			});

			//FootballPlayerListScript footballPlayers = GameObject.Find ("TabPrediccionPanel").GetComponent<GetFootballPlayersDataScript> ().footballPlayersdata;

			foreach (FootballPlayer player in footballPlayers.dataList) {

				string[] aNom;

				aNom =  player.nameData.Split (' ');

				string namePlayer = aNom [0] [0] + ". " + aNom [1];

				namePlayer = namePlayer.ToUpper ();
//				string namePlayer = player.idData + ". " + player.nameData.ToUpper ();

				gameObject.GetComponent<Dropdown> ().options.Add (new Dropdown.OptionData (namePlayer, player._imgPhoto));
			}

			if (gameObject.name == "GoleadorDropdown") {

				gameObject.GetComponent<Dropdown> ().captionText.text = "Goleador";

			} else if (gameObject.name == "PaseGolDropdown") {

				gameObject.GetComponent<Dropdown> ().captionText.text = "Pase Gol";

			}

		} else {

			foreach (string tipe in goalsTipe) {

				gameObject.GetComponent<Dropdown> ().options.Add (new Dropdown.OptionData (tipe));
			}

			gameObject.GetComponent<Dropdown> ().captionText.text = "Tipo de Gol";
		}
	}

	private void SeDesplego(){

		Debug.Log ("Index: " + gameObject.GetComponent<Dropdown>().value);


		if (gameObject.name == "GoleadorDropdown") {

			FootballPlayer player = footballPlayers.dataList [gameObject.GetComponent<Dropdown>().value];

			panelPronosticoData.GetComponent<PanelPronosticoDataScript> ()._idGoleador = player.idData;

			Debug.Log ("IdPlayer: " + panelPronosticoData.GetComponent<PanelPronosticoDataScript> ()._idGoleador);

		} else if(gameObject.name == "PaseGolDropdown"){

			FootballPlayer player = footballPlayers.dataList [gameObject.GetComponent<Dropdown>().value];

			panelPronosticoData.GetComponent<PanelPronosticoDataScript> ()._idPaseGoal = player.idData;

		}

		Debug.Log ("SE DESPLEGO");
	}
}

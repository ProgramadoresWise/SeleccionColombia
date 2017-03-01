using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FootballShowPlayerItem : MonoBehaviour {

	[SerializeField]
	private Image playerPhoto;
	public Image _playerPhoto {get{ return playerPhoto; } set{ playerPhoto = value; }}

	[SerializeField]
	private Text playerName;
	public Text _playerName {get{ return playerName; } set{ playerName = value; }}

	[SerializeField]
	private int playerId;
	public int _playerId {get{ return playerId; } set{ playerId = value; }}

	[SerializeField]
	private string position;
	public string _position {get{ return position; } set{ position = value; }}

	[SerializeField]
	private bool isSelected = false;
	public bool _isSelected {get{ return isSelected; } set{ isSelected = value; }}

	public void GetNombre(string n){
		
		string[] aNom;

		aNom = n.Split (' ');

		playerName.text = aNom [0] [0] + ". " + aNom [1];
	}
}

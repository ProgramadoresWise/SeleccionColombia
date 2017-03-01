using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlignmentResultData : MonoBehaviour {

	[SerializeField]
	private Text leftPlayerName;
	public Text _leftPlayerName {get{ return leftPlayerName; } set{ leftPlayerName = value; }}

	[SerializeField]
	private Text points;
	public Text _points {get{ return points; } set{ points = value; }}

	[SerializeField]
	private Text rightPlayerName;
	public Text _rightPlayerName {get{ return rightPlayerName; } set{ rightPlayerName = value; }}
}

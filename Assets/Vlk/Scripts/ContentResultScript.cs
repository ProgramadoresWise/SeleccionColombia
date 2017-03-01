using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ContentResultScript : MonoBehaviour {

	[SerializeField]
	private Text leftTitle;
	public Text _leftTitle {get{ return leftTitle; } set{ leftTitle = value; }}

	[SerializeField]
	private Text rightTitle;
	public Text _rightTitle {get{ return rightTitle; } set{ rightTitle = value; }}

//	[SerializeField]
//	private Image ball;
//	public Image _ball {get{ return ball; } set{ ball = value; }}

	[SerializeField]
	private Text points;
	public Text _points {get{ return points; } set{ points = value; }}
}

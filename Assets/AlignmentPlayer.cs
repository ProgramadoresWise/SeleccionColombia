using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlignmentPlayer : MonoBehaviour {

	[SerializeField]
	private Image img; 
	public Image _img {get{ return img; } set{ img = value; }}
}

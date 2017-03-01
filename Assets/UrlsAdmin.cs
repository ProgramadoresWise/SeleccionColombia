using UnityEngine;
using System.Collections;

public class UrlsAdmin : MonoBehaviour {

	public static UrlsAdmin self;

	[SerializeField]
	private string ftpUrl = "";
	public string _ftpUrl {get{ return ftpUrl; } set{ ftpUrl = value; }}

	// Use this for initialization
	void Awake () {
	
		self = this;
	}
}

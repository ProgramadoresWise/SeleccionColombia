using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ScrollCrontoller : MonoBehaviour {

	public RectTransform panel;
	public Button[] bttn;
	public RectTransform center;

	private float [] distance;
	private bool dragging = false;
	private int bttnDistance;
	private int minButtonNum;


	void Start( ){
		int bttnLeng = bttn.Length;
		distance = new float[bttnLeng];

		bttnDistance = (int)Math.Abs(bttn[1].GetComponent<RectTransform>().anchoredPosition.x - bttn[0].GetComponent<RectTransform>().anchoredPosition.x);
	}

}

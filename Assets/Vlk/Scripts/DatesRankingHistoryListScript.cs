using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class HistoryDate {

	public int indexPosition;
	public string date;
	public string name;

	[SerializeField]
	private Sprite imgInf;
	public Sprite _imgInf {get{ return imgInf; } set{ imgInf = value; }}
}

[System.Serializable]
public class DatesRankingHistoryListScript {

	public List<HistoryDate> dataList;
}

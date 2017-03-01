using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ContentRanking {

	public int userID;

	public string nameText;

	public int points;
}

[System.Serializable]
public class ContentRankingListScript {

	public List<ContentRanking> dataList;
}

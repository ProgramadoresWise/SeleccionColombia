using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FootballPlayer
{
	public int idData;

	public string nameData;

	public string position;

	public Sprite imgPhoto;
	public Sprite _imgPhoto {get{ return imgPhoto; } set{ imgPhoto = value; }}

	//Constructor:
	public FootballPlayer(int id, string name){

		idData = id;
		nameData = name;
	}

}

[System.Serializable]
public class FootballPlayerListScript {
	//Vars:
	public List<FootballPlayer> dataList;

	//Constructor:
	public FootballPlayerListScript() {
		
		dataList = new List<FootballPlayer> {

			new FootballPlayer(25, "VICTOR MENDOZA"), 
			new FootballPlayer(20, "LUIS CHECA"),
			new FootballPlayer(27, "JEISON DOMINGUEZ"),
			new FootballPlayer(21, "ANDERSON O."),
			new FootballPlayer(31, "PEDRO V."),
			new FootballPlayer(1, "MAXIMO B."),
			new FootballPlayer(22, "DAMIAN LANZA"),
			new FootballPlayer(12, "AYRTON MORALES"),
			new FootballPlayer(28, "ROOSEVELT OYOLA"),
			new FootballPlayer(2, "MARIO PINEIDA"),
			new FootballPlayer(3, "XAVIER ARREAGA"),
			new FootballPlayer(5, "GABRIEL M."),
			new FootballPlayer(19, "DARIO AIMAR"),
			new FootballPlayer(18, "MATIAS OYOLA"),
			new FootballPlayer(30, "WASHINGTON V."),
			new FootballPlayer(24, "WILLIAN ERREYES"),
			new FootballPlayer(6, "OSWALDO M."),
			new FootballPlayer(8, "RICHARD C."),
			new FootballPlayer(10, "DAMIAN DIAZ"),
			new FootballPlayer(14, "SEGUNDO C."),
			new FootballPlayer(16, "ERICK CASTILLO"),
			new FootballPlayer(7, "CHRISTIAN S."),
			new FootballPlayer(17, "MARCOS CAICEDO"),
			new FootballPlayer(13, "ELY ESTERILLA"),
			new FootballPlayer(11, "TITO VALENCIA"),
			new FootballPlayer(15, "HERNAN LINO"),
			new FootballPlayer(44, "JONATHAN A."),
			new FootballPlayer(23, "CHRISTIAN ALEMAN")
		};

		//SortFootballPlayerList ();
	}

	//Funtions:
	public void SortFootballPlayerList(){

		dataList.Sort (delegate(FootballPlayer a, FootballPlayer b) {

			return((a.position).CompareTo (b.position));
		});

//		dataList.Sort (delegate(FootballPlayer a, FootballPlayer b) {
//
//			return((a.idData).CompareTo (b.idData));
//		});

		/*foreach (FootballPlayer player in dataList) {

			Debug.Log (player.idData + ", ");
		}*/
		
//		FootballPlayer auxPlayer;
//		int auxIndex;
//
//		for (int i = 0; i < dataList.Count; i++) {
//
//			auxIndex = i;
//
//			for(int j = i; j < dataList.Count; j++){
//
//				if(dataList[j].idData < dataList[auxIndex].idData){
//					auxIndex = j;
//				}
//			}
//
//			if (auxIndex != i) {
//			
//				auxPlayer = dataList [i];
//				dataList [i] = dataList [auxIndex];
//				dataList [auxIndex] = auxPlayer;
//			}
//		}
//
//		foreach (FootballPlayer player in dataList) {
//
//			Debug.Log (player.idData + ", ");
//		}
	}
}

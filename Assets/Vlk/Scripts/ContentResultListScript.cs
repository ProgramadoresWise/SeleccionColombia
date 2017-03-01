using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ContentResult {

	//public int UserID;

	public string autor_Gol;
	public string autor_Gol_Result;
	public int autor_Gol_Points;

	public string tipo_Gol;
	public string tipo_Gol_Result;
	public int tipo_Gol_Points;

	public int min_Gol;
	public int min_Gol_Result;
	public int min_Gol_Points;

	public string autor_PaseGol;
	public string autor_PaseGol_Result;
	public int autor_PaseGol_Points;

	public string jugador_Cancha;
	public string jugador_Cancha_Result;
	public int jugador_Cancha_Points;

	public int goles_Barcelona;
	public int goles_Barcelona_Result;
	public int goles_Barcelona_Points;

	public int goles_Oponente;
	public int goles_Oponente_Result;
	public int goles_Oponente_Points;

	public int prediction_Points;

	public int esLocal;

	public int IsFirst;

	public int ptnAlig;

	public int acertoTodo;

	public int multBonus;

	public string fechaPronostico;
}

[System.Serializable]
public class ContentResultListScript {

	public List<ContentResult> dataList;

	//Constructor:
	public ContentResultListScript(){
	
		dataList = new List<ContentResult> ();
	}
		
	public void SortMinNegativos(){

		dataList.Sort (delegate(ContentResult a, ContentResult b) {

			return((a.min_Gol_Result).CompareTo (b.min_Gol_Result));
		});

//		foreach (FootballPlayer player in dataList) {
//
//			Debug.Log (player.idData + ", ");
//		}

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

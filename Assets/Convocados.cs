using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MaterialUI;
using UnityEngine.UI;
using System.Linq;

public class Convocados : MonoBehaviour {


	public static Convocados main;

	[SerializeField]
	private DataRowList playerSummoned;

	public TabView tabPartidos;
	public GameObject playerPrefab;

	[Header("CONVOCADOS ELIMINATORIA")]
	public GameObject ContentSummonedComplete;
	[SerializeField]
	private List<JugadoresConvocados> players;

	[Space(10)]

	[Header("CONVOCADOS POR PARTIDO")]
	public GameObject ContentSummonedMatch;
	[SerializeField]
	private List<JugadoresConvocados> playersMatch;
	int currentTb = -1;

	public bool update;
	bool reload;

	[SerializeField]
	int stateIndxPlayer;

	public void LoadSummoned( bool bck ) {

		NavigatorManager.main.susccesfullEventInback = bck;


		if(!reload && players.Count == 0)
			DataApp.main.EnableLoading();
		
		StartCoroutine(UpdateConvocados());

//		OrderElimatoria( );
		OrderByMatchs( );
	}






	IEnumerator UpdateConvocados( ){
		WWW updConvocados = new WWW(DataApp.main.host + DataApp.main.urlActualizarInfo + (int)idActualizacion.convocados);
		yield return updConvocados;
		if( string.IsNullOrEmpty( updConvocados.error ) ){
			StartCoroutine( getDatasConvocados (int.Parse(updConvocados.text) ));
		}else{
			DataApp.main.popUpInformative(true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
		}

	}


	public IEnumerator getDatasConvocados( int NumAct ){
		if( DataApp.main.GetMyInfoInt("Convocados") !=  NumAct){
			reload = false;
			update = true;
			ToastManager.Show("Actalizando Jugadores",2f,null);
			createPlayersCompletes() ;
		}else{
			if( !update ){
				 createPlayersCompletes();
			}else{
				update = false;
			}

		}

		DataApp.main.SetMyInfo("Convocados",NumAct.ToString(),1);

		yield return new WaitForEndOfFrame();
	}


	void createPlayersCompletes(  ){
		update = false;

		if(playersMatch.Count >= 3 ){
			DataApp.main.DisableLoading();
		}


		int indx = 0;
//		for( int i = index; i <  SeleccionC.club.PlayersInfo.Count ; i++){
		foreach(Players pl in SeleccionC.club.PlayersInfo){
//			
			if(playersMatch.Count == 5 ){
				DataApp.main.DisableLoading();
			}

			if( stateIndxPlayer <  SeleccionC.club.PlayersInfo.Count && indx >= stateIndxPlayer){
				GameObject player = Instantiate(playerPrefab);
				player.AddComponent<JugadoresConvocados>();
				player.transform.SetParent(ContentSummonedComplete.transform,false);
				player.SetActive(true);
				player.name = pl._id.ToString();
				player.GetComponent<JugadoresConvocados>().idJugador = pl._id;
				string [] matchs = pl._cantidadPartidos.Split('-');
				player.GetComponent<JugadoresConvocados>().orderEliminatoria = pl._ordenEliminatoria;
				player.GetComponent<JugadoresConvocados>().orderMatch = pl._ordenPartido;
				if( !string.IsNullOrEmpty( pl._cantidadPartidos ) ){
					foreach ( string m in matchs){
						player.GetComponent<JugadoresConvocados>().partidos.Add(int.Parse(m));
					}
				}
				players.Add(player.GetComponent<JugadoresConvocados>());

				GameObject playerM = Instantiate(playerPrefab);
				playerM.AddComponent<JugadoresConvocados>();
				playerM.transform.SetParent(ContentSummonedMatch.transform,false);
				playerM.SetActive(true);
				playerM.name =  pl._id.ToString();
			
				playerM.GetComponent<JugadoresConvocados>().idJugador =  pl._id;
				string [] matchsS = pl._cantidadPartidos.Split('-');
				playerM.GetComponent<JugadoresConvocados>().orderEliminatoria = pl._ordenEliminatoria;
				playerM.GetComponent<JugadoresConvocados>().orderMatch = pl._ordenPartido;
				if( !string.IsNullOrEmpty( pl._cantidadPartidos ) ){
					foreach ( string m in matchsS){
						playerM.GetComponent<JugadoresConvocados>().partidos.Add(int.Parse(m));
					}
				}
				playersMatch.Add(playerM.GetComponent<JugadoresConvocados>());

				stateIndxPlayer++;
			}
			indx++;
		}


//		OrderElimatoria( );


		NavigatorManager.main.susccesfullEventInback = false;
		reload =  true;

		DataApp.main.DisableLoading();
	}



	public void OrderByMatchs(  ){
		callbackOrderMatch() ;
	}


	void callbackOrderMatch( ){
//		if( currentTb != tabPartidos.currentPage ){
			DisablePlayersByMatchs();
			foreach( JugadoresConvocados gm in playersMatch ){
				foreach ( int mat in gm.partidos ){
					if( mat == (tabPartidos.currentPage+1)){
						gm.gameObject.SetActive(true);
					}
				} 
			}
			if(playersMatch.Count>0){
				playersMatch = playersMatch.OrderBy( x => x.GetComponent<JugadoresConvocados>().orderMatch).ToList();
			}

					foreach(JugadoresConvocados obj in playersMatch){
						obj.gameObject.transform.SetParent(obj.gameObject.transform.parent.parent);
					}

					foreach(JugadoresConvocados obj in playersMatch){
						obj.gameObject.transform.SetParent(ContentSummonedMatch.transform);
					}
			currentTb = tabPartidos.currentPage;
//		}

	}

	void OrderElimatoria( ){
		if(players.Count>0){
			players = players.OrderBy( x => x.GetComponent<JugadoresConvocados>().orderEliminatoria).ToList();
		}

		foreach(JugadoresConvocados obj in players){
			obj.gameObject.transform.SetParent(obj.gameObject.transform.parent.parent);
		}

		foreach(JugadoresConvocados obj in players){
			obj.gameObject.transform.SetParent(ContentSummonedComplete.transform);
		}
	}

	void DisablePlayersByMatchs( ){
		foreach(JugadoresConvocados obj in playersMatch){
			obj.gameObject.SetActive(false);
		}
	}


	void DestroyPlayersUpdate( ){
		foreach(JugadoresConvocados obj in playersMatch){
			Destroy(obj.gameObject);
		}

		foreach(JugadoresConvocados obj in players){
			Destroy(obj.gameObject);
		}
	}

}
//
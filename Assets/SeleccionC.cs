using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using MaterialUI;

public class SeleccionC : MonoBehaviour {

	public static SeleccionC club;
	[SerializeField]
	private DataRowList PlayersDataList;

	//	[SerializeField]
	public List<Players> PlayersInfo;
	bool updatePlayers;


	public void loadClubInfo( ){
		LoadPlayers( );
	}

	#region CREACION DE JUGADORESS
	void LoadPlayers( ) {
		StartCoroutine (UpdatePlayers());

	}

	public IEnumerator UpdatePlayers( ){
		WWW updNew = new WWW(DataApp.main.host + DataApp.main.urlActualizarInfo + (int)idActualizacion.jugadores);
		yield return updNew;
		if( string.IsNullOrEmpty( updNew.error ) ){
			StartCoroutine (getDatasPlayers( int.Parse(updNew.text), DataApp.main.host + "Club/Jugadores/jugadores.php"));
		}else{
			DataApp.main.popUpInformative(true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
			StartCoroutine(getDatasPlayers(DataApp.main.GetMyInfoInt("Players"), DataApp.main.host + "Club/Jugadores/jugadores.php"));
		}
	}


	public IEnumerator getDatasPlayers( int NumAct, string url ){
		
		//		DataApp.main.EnableLoading();

		if( DataApp.main.GetMyInfoInt("Players") !=  NumAct || PerfilJugador.main.isvote ){

			yield return StartCoroutine( GetJsonDataScript.getJson.GetPhpData( url));
			DataApp.main.SetMyInfo("JsonPlayers", GetJsonDataScript.getJson.GetDataConsult(), 3);
			if( DataApp.main.IsRegistered() && !PerfilJugador.main.isvote)
				ToastManager.Show("Actualizando Información",5f,null);
			else if (DataApp.main.IsRegistered() && PerfilJugador.main.isvote )
				ToastManager.Show("Tu voto ha sido registrado.",5f,null);
		}else{

			string jsonGuardado = DataApp.main.GetMyInfoString("JsonPlayers");
			yield return StartCoroutine(GetJsonDataScript.getJson.GetLocalData(jsonGuardado));

		}


		yield return StartCoroutine( GetJsonDataScript.getJson.GetPhpData( url));
		if( GetJsonDataScript.getJson._state == "Successful" ){
			if( PlayersInfo.Count > 0){
				PlayersInfo.Clear();
			}
			DataApp.main.SetMyInfo("Players",NumAct.ToString(),1);
			PlayersDataList  = GetJsonDataScript.getJson.GetData( PlayersDataList , "id", "nombre", "aplausosUltimo", "totalAplausos", "twitter",  "facebook" ,"instagram" ,"snapchat", "posicion", "ordenEliminatoria", "ordenPartido", "cantidadPartidos");
			CreateInfoPlayers();
		}else if( GetJsonDataScript.getJson._state == "Warning_01" ){
			print("w1");
		}else if( GetJsonDataScript.getJson._state == "Warning_02" ){
			print("w2");
		}else if (GetJsonDataScript.getJson._state == "Warning_03"){
			Debug.Log("Mensaje: Datos con error.");
			DataApp.main.SetMyInfo("JsonPlayers", "", 3);
			DataApp.main.SetMyInfo("Players", "0", 1);
			StartCoroutine(getDatasPlayers(NumAct, url));
		}
		GetJsonDataScript.getJson._state = "";

	}

	void CreateInfoPlayers( ){
		foreach(DataRow obj in PlayersDataList.dataList){
			Players newPlayer = new Players();
			newPlayer._id = int.Parse(obj.GetValueToKey("id"));
			newPlayer._name = obj.GetValueToKey("nombre");
			newPlayer._aplausosUltimoPartidol = int.Parse(obj.GetValueToKey("aplausosUltimo"));
			newPlayer._totalAplausos = int.Parse(obj.GetValueToKey("totalAplausos"));
			newPlayer._tiwtterLink = obj.GetValueToKey("twitter");
			newPlayer._facebookLink = obj.GetValueToKey("facebook");
			newPlayer._instagramLink = obj.GetValueToKey("instagram");
			newPlayer._snapchatLink = obj.GetValueToKey("snapchat");
			newPlayer._pos = obj.GetValueToKey("posicion");
			newPlayer._ordenEliminatoria = int.Parse(obj.GetValueToKey("ordenEliminatoria"));
			newPlayer._ordenPartido = int.Parse(obj.GetValueToKey("ordenPartido"));
			newPlayer._cantidadPartidos = obj.GetValueToKey("cantidadPartidos");
			PlayersInfo.Add(newPlayer);
		}
		//				foreach(Players obj in PlayersInfo){
		//					Debug.Log(obj._name);
		//				}
		NavigatorManager.main.susccesfullEventInback = false;

	}



	#endregion


}







			
					





using UnityEngine;
using System.Collections;

public class GetFootballPlayersDataScript : MonoBehaviour {

	public FootballPlayerListScript footballPlayersdata;

	private string urlPhpFootballPlayersData = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Admin/GetDatosJugador.php?ind0=footballPlayersData";
	private string urlFootballPlayersPhoto = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/FootballPlayers/";

	//private string footballPlayers = "{\"dataList\":[{\"idData\":\"25\",\"nameData\":\"VICTOR MENDOZA\"},{\"idData\":\"20\",\"nameData\":\"LUIS CHECA\"},{\"idData\":\"27\",\"nameData\":\"JEISON DOMINGUEZ\"},{\"idData\":\"21\",\"nameData\":\"ANDERSON O.\"},{\"idData\":\"31\",\"nameData\":\"PEDRO V.\"},{\"idData\":\"1\",\"nameData\":\"MAXIMO B.\"},{\"idData\":\"22\",\"nameData\":\"DAMIAN LANZA\"},{\"idData\":\"12\",\"nameData\":\"AYRTON MORALES\"},{\"idData\":\"28\",\"nameData\":\"ROOSEVELT OYOLA\"},{\"idData\":\"2\",\"nameData\":\"MARIO PINEIDA\"},{\"idData\":\"3\",\"nameData\":\"XAVIER ARREAGA\"},{\"idData\":\"5\",\"nameData\":\"GABRIEL M.\"},{\"idData\":\"19\",\"nameData\":\"DARIO AIMAR\"},{\"idData\":\"18\",\"nameData\":\"MATIAS OYOLA\"},{\"idData\":\"30\",\"nameData\":\"WASHINGTON V.\"},{\"idData\":\"24\",\"nameData\":\"WILLIAN ERREYES\"},{\"idData\":\"6\",\"nameData\":\"OSWALDO M.\"},{\"idData\":\"8\",\"nameData\":\"RICHARD C.\"},{\"idData\":\"10\",\"nameData\":\"DAMIAN DIAZ\"},{\"idData\":\"14\",\"nameData\":\"SEGUNDO C.\"},{\"idData\":\"16\",\"nameData\":\"ERICK CASTILLO\"},{\"idData\":\"7\",\"nameData\":\"CHRISTIAN S.\"},{\"idData\":\"17\",\"nameData\":\"MARCOS CAICEDO\"},{\"idData\":\"13\",\"nameData\":\"ELY ESTERILLA\"},{\"idData\":\"11\",\"nameData\":\"TITO VALENCIA\"},{\"idData\":\"15\",\"nameData\":\"HERNAN LINO\"},{\"idData\":\"44\",\"nameData\":\"JONATHAN A.\"},{\"idData\":\"23\",\"nameData\":\"CHRISTIAN ALEMAN\"}]}";

	void Start () {

		//StartCoroutine (GetFootballPlayersData ());
		/*footballPlayersdata = new FootballPlayerListScript();

		foreach (FootballPlayer player in footballPlayersdata.dataList)
		{

			player._imgPhoto = Resources.Load <Sprite> ("FootballPlayersPhoto/" + player.idData);
		}*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RunGetFootballPlayersData (){

		//StartCoroutine (GetFootballPlayersData ());
		StartCoroutine(GetFootballPlayersPhoto());

	}

	public IEnumerator GetFootballPlayersData(){
		
		UpdateDataScript.updateData.RunUpdatePanel ();

		WWW hs_get = new WWW (urlPhpFootballPlayersData);
		yield return hs_get;
		if (!string.IsNullOrEmpty(hs_get.text)) 
		{
			Debug.Log(hs_get.text);
			AssignFootballPlayersData(hs_get.text);

		}else{
			//EnablePopUp("Fallo en la Conexión.\nIntentelo de nuevo");
			Debug.Log("Error Json");
		}
		//InstanceLoading.main.DisableLoading();
	} 

	private void AssignFootballPlayersData(string getinfo){

		Debug.Log (getinfo);

		footballPlayersdata = JsonUtility.FromJson<FootballPlayerListScript>(getinfo);

		//footballPlayersdata.SortFootballPlayerList ();

		if(footballPlayersdata != null){

			footballPlayersdata.SortFootballPlayerList ();
		}

		StartCoroutine(GetFootballPlayersPhoto ());
	}

	private IEnumerator GetFootballPlayersPhoto(){

		UpdateDataScript.updateData.RunUpdatePanel ();
//		int temporalCounter = 1;
//
//		foreach (FootballPlayer player in footballPlayersdata.dataList) {
//			
//			Debug.Log ("Counter Load Image: " + temporalCounter + " " + urlFootballPlayersPhoto + player.idData + ".jpg");
////			player._imgPhoto = ImgLoadManager.main.PlayerImg( null , player.idData.ToString(), false);
//			yield return StartCoroutine( ImgLoadManager.main.PlayerImg_dAlineacionPolla( player.idData.ToString(), resultsp =>{
//				player._imgPhoto = resultsp;
//			},false));
////			WWW photo = new WWW(urlFootballPlayersPhoto + player.idData + ".jpg");
////			yield return photo;
////
////			if (string.IsNullOrEmpty (photo.error)) {
////				
////				Rect size = new Rect (0, 0, photo.texture.width, photo.texture.height);
////				player._imgPhoto = Sprite.Create (photo.texture, size, Vector2.zero);
////			} else {
////
////				player._imgPhoto = Resources.Load <Sprite> ("FootballPlayersPhoto/DefaultSilhouette");
////				Debug.Log ("Counter Load Image: " + temporalCounter + " " + "Load Image Failed Set DefaultSilhouette" + player.idData + "-" + player.nameData);
////			}
//
//			temporalCounter++;
//		}



		Debug.Log("Y AJA!!");

		foreach ( Players pl in SeleccionC.club.PlayersInfo ){
			if(  pl._ordenPartido != 0 ){
				FootballPlayer newPl = new FootballPlayer(pl._id, pl._name);
				newPl.position = pl._pos;
				yield return StartCoroutine( ImgLoadManager.main.PlayerImg_dAlineacionPolla(newPl.imgPhoto  , newPl.idData.ToString(), resultsp =>{
					newPl._imgPhoto = resultsp;
				},false)); 
				footballPlayersdata.dataList.Add(newPl);
			}
		}

		if(footballPlayersdata != null){
		
			footballPlayersdata.SortFootballPlayerList ();
		}

		this.GetComponent<ScrollPrediccionScript> ().ValidatePredictionStatus ();

		Debug.Log ("Full load images");

		yield return null;
	}
}

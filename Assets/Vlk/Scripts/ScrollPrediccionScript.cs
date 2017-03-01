using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using System.Globalization;

public enum idFechasPartidos {

	proximoPartido = 1,
	ultimoPartido = 2
}

public class ScrollPrediccionScript : MonoBehaviour {

	private string[] titleGols = new [] {"PRIMER","SEGUNDO","TERCER","CUARTO","QUITO","SEXTO","SÉPTIMO","OCTAVO","NOVENO","DÉCIMO","ONCEAVO","DOCEAVO","TRECEAVO","CATORCEAVO","QUINCEAVO","DIECISEISAVO","DIECISIETEAVO","DIECIOCHOAVO","DIECINUEVEAVO","VEINTEAVO"};

	public GameObject prefabPanelData;
	public GameObject prefabShowData;
	public GameObject ScrollContent;
	public InputField inputBarcelonaScore;

	public InputField goalsBarcelona;
	public InputField goalsOponente;
	public Dropdown jugadorDeLaCancha;

	public GameObject predecirBtn;
	public GameObject compartirBtn;

	public GameObject tutoBtnAligment;
	public GameObject instructionBtnAligment;

	public GameObject tutoBtnPrediction;
	public GameObject instructionBtnPrediction;

	[Header("Escudos:")]

	[SerializeField]
	private Image EscudoEquipo;
	[SerializeField]
	private Image EscudoOponente;

	[SerializeField]
	private Image EscudoEquipoGoals;
	[SerializeField]
	private Image EscudoOponenteGoals;

	[Space(20)]

	private bool dataIsValidated = false;
	public bool _dataIsValidated {get{ return dataIsValidated; } set{ dataIsValidated = value; }}

	public Text textState;
	public Text textStateAling;

	public Text fechaPronosticoText;

	[SerializeField]
	private int minuteLimit;

	[SerializeField]
	private AlignmentScript alignmentPanel;

	public UnityEvent MakePrediction;
	public UnityEvent PredictionHincha;
	public UnityEvent PredictionHinchaNivelClub;

	[SerializeField]
	private bool predictionIsEnable;
	public bool _predictionIsEnable {get{ return predictionIsEnable; } set{ predictionIsEnable = value; }}

	private int SizeScrollContent = 338;

	//public Text tituloPantalla;

	//public Text fechaPrediccion;

//	[SerializeField]
//	private PredictionsListScript predictionsList;
//	public PredictionsListScript _predictionsList {get{ return predictionsList; } set{ predictionsList = value; }}

	[SerializeField]
	private List<GameObject> alignmentList;
	public List<GameObject> _alignmentList {get{ return alignmentList; } set{ alignmentList = value; }}

	[SerializeField]
	private List<GameObject> selecedAlingnPlayer;
	public List<GameObject> _selecedAlingnPlayer {get{ return selecedAlingnPlayer; } set{ selecedAlingnPlayer = value; }}

	[SerializeField]
	private DataRowList predictionsList;
	public DataRowList _predictionsList {get{ return predictionsList; } set{ predictionsList = value; }}

	[SerializeField]
	private DataRowList playerAlignmentList;
	public DataRowList _playerAlignmentList {get{ return playerAlignmentList; } set{ playerAlignmentList = value; }}

	[SerializeField]
	private List<int> alineacionIdList;

	public List<GameObject> panelsData = new List<GameObject>();

	//private List<PrediccionDataScript> panelsDatas = new List<PrediccionDataScript> ();
	private int currentGoals = 0;

	//private string urlPhpSetPronostico = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/SetUserPronostico.php?indata=createPronostico&userID=123&autorGol=Pepe&tipoGol=pipo&minGol=50&autorPaseGol=carlos&jugadorCancha=Papo&golesBarcelona=2&golesOponente=3";

	//private string urlPhpPronostico = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/UserPronostico.php?";
	//private string urlPhpPronostico = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/UserPronosticoTest.php?";
	//private string urlPhpUltimoPartido = "http://www.2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/GetDate.php?";

	private string predictionState = "none";

	[SerializeField]
	private DateClassList fechaList;
	public DateClassList _fechaList {get{ return fechaList; } set{ fechaList = value; }}

	[SerializeField]
	private List<string> alignmentShareList = new List<string> ();
	public List<string> _alignmentShareList {get{ return alignmentShareList; } set{ alignmentShareList = value; }}

	private DateTime date;

	public Text fechaTextAlineacion;

	public Text fechaText;

	private bool teamIsLocal = false;

	public GetNextDateScript tiempoRestate;

	// Use this for initialization
	void Start () {
		
//		inputBarcelonaScore.onEndEdit.AddListener(delegate {UpdateSpaceData();});
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region PANEL CREATOR

	private void UpdateSpaceData(){
		
		int goals = int.Parse(inputBarcelonaScore.text);

		//Debug.Log ("Input: " + int.Parse(inputBarcelonaScore.text));

		if (goals > 20) {
		
			goals = 20;

		} else if (goals < 0) {
		
			goals = 0;
		}

		inputBarcelonaScore.text = goals.ToString();

		if (goals > currentGoals) {

			CreateDataPanels (goals);

		} else if (goals < currentGoals) {
		
			UpdateDataPanels (goals);
		}
	}
		
	private void CreateDataPanels(int CantGoals){
	
		int goals = CantGoals;
		Debug.Log ("¡Input Modificado! " + goals);

		if (goals > 1) {

			ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, goals * SizeScrollContent);
		} else {

			ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, (goals * SizeScrollContent) + SizeScrollContent);
		}

		for (int i = currentGoals; i < goals; i++) {

			GameObject panelData = Instantiate (prefabPanelData);
			panelData.transform.SetParent (ScrollContent.transform);
			panelData.GetComponent<PanelPronosticoDataScript> ()._indexPanel = i;
			panelData.GetComponent<PanelPronosticoDataScript> ().goalTitle.text = titleGols[i] + " GOL";
			panelData.GetComponent<PanelPronosticoDataScript> ()._inputFieldObj = gameObject;
			panelData.GetComponent<PanelPronosticoDataScript> ().minutes.GetComponent<InputLimitNumberScript>().maxLimit = minuteLimit;
			panelsData.Add (panelData);
			//panelDataA.GetComponent<RectTransform>().anchoredPosition = new Vector3 (0, 0, 0);
			panelData.GetComponent<RectTransform>().localPosition = new Vector3 (402, i * -SizeScrollContent, 0);
			panelData.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);

			//TEMPORAL:
			panelData.GetComponent<PanelPronosticoDataScript> ().goleadorText.text = i.ToString ();

//			if (i % 2 != 0) {
//
//				panelData.GetComponent<Image>().color = new Color32(0,0,0,255);
//			} else {
//
//				panelData.GetComponent<Image>().color = new Color32(28,28,28,255);
//			}

			currentGoals++;
		}  

		Debug.Log (currentGoals + "<-----");
	}

	private void UpdateDataPanels(int CantGoals){

		int goals = CantGoals;
		Debug.Log ("¡Input Modificado! " + goals);

		if (goals > 1) {

			ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, goals * SizeScrollContent);
		} else {

			ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 2 * SizeScrollContent);
		}

		int j = goals;
		for (int i = currentGoals-1; i >= j; i--) {

			GameObject objPanel = panelsData [i];
			panelsData.RemoveAt (i);
			Destroy (objPanel);

			currentGoals--;
		}

		Debug.Log (currentGoals + ">-----");
	}

	public void DeletePanelData (int indexPanel) {

		GameObject objPanel = panelsData [indexPanel];
		panelsData.RemoveAt (indexPanel);
		Destroy (objPanel);

		currentGoals--;

		inputBarcelonaScore.text = currentGoals.ToString();

		Debug.Log (currentGoals + ">-----");

		OrganizePanelsDataList ();
	}

	private void OrganizePanelsDataList () {
		
		if (currentGoals > 1) {

			ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, currentGoals * SizeScrollContent);
		} else {

			ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, (currentGoals * SizeScrollContent) + SizeScrollContent);
		}

		for (int i = 0; i <= panelsData.Count-1; i++) {

			GameObject panelData = panelsData[i];
			panelData.GetComponent<PanelPronosticoDataScript> ()._indexPanel = i;
			panelData.GetComponent<RectTransform>().localPosition = new Vector3 (402, i * -SizeScrollContent, 0);
			//panelData.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);

//			if (i % 2 != 0) {
//
//				panelData.GetComponent<Image>().color = new Color32(0,0,0,255);
//			} else {
//
//				panelData.GetComponent<Image>().color = new Color32(28,28,28,255);
//			}
		}
	}

           	#endregion

	public void validateTimeAndSendPronostic(){

		UpdateDataScript.updateData.RunUpdatePanel ();
	
		 StartCoroutine(tiempoRestate.ValidateStatusNextData("_SendDataDB"));
	}

	public void SendDataDB(){

		//UpdateDataScript.updateData.StopUpdatePanel ();

		if(predictionIsEnable){

			StartCoroutine (RunMakePrediction());
		
		} else {

			UpdateDataScript.updateData.RunPopup ("El tiempo para predecir a terminado.", 1);
		}
	}

	private IEnumerator RunMakePrediction(){
	
		bool isEmptyData = false;

		if (goalsBarcelona.text == ""){//|| goalsBarcelona.text == "") {

			Debug.Log ("goalsBarcelona");

			isEmptyData = true;
			
		} else if (goalsOponente.text == ""){ //|| goalsOponente.text == "") {

			//Debug.Log ("goalsOponente");

			isEmptyData = true;

		} else {

			foreach(GameObject obj in panelsData){

				if (obj.GetComponent<PanelPronosticoDataScript> ().goleadorText.text == "") {
					
					//Debug.Log ("goleadorText");

					isEmptyData = true;

					break;

				} else if (obj.GetComponent<PanelPronosticoDataScript> ().goalTypeText.text == "") {

					//Debug.Log ("goalTypeText");

					isEmptyData = true;

					break;

				} else if (obj.GetComponent<PanelPronosticoDataScript> ().minGoalText.text == "") {

					//Debug.Log ("minGoalText");

					isEmptyData = true;

					break;

				} else if (obj.GetComponent<PanelPronosticoDataScript> ().paseGoalText.text == "") {

					//Debug.Log ("paseGoalText");

					isEmptyData = true;

					break;
				}
			}
		}

		yield return isEmptyData;

		if (isEmptyData) {

			ShowMessagePopup ("Datos de la predicción incompletos", GameObject.Find ("EventManager").GetComponent<NavigationTabPanelsScript> ()._idCurrentEnablePanel);

		} else {

//			if (RegisterAndLogin.isActive == 1) 
//			{
//
//				//predictionState = "EndMakePredictions";
//				predictionState = "MakePredictionsHinchaNivelClub";
//
//				//GameObject eventManager = GameObject.Find ("EventManager");
//				//eventManager.GetComponent<NavigationTabPanelsScript> ().ShowUpdateDataPanel();
//
//				StartCoroutine (SendPronostico());
//			}
//			else {
//
//				MakePrediction.Invoke ();
//				UpdateDataScript.updateData.StopUpdatePanel ();
//			}

			StartCoroutine (SendPronostico());
		}
	}

//	public void SendDataDB(){
//
//		if (RegisterAndLogin.isActive == 1) {
//
//			//predictionState = "EndMakePredictions";
//			predictionState = "MakePredictionsHinchaNivelClub";
//
//			GameObject eventManager = GameObject.Find ("EventManager");
//			eventManager.GetComponent<NavigationTabPanelsScript> ().ShowUpdateDataPanel();
//
//			StartCoroutine (SendPronostico());
//		} else {
//
//			MakePrediction.Invoke ();
//		}
//	}

	public void PredicirComoHinchaNivelClub(){
		predictionState = "MakePredictionsHinchaNivelClub";
		//EditProfile.myUser.InitValidation ();
		//BlockPanel.GetComponent<ActiveLevelClubScript> ().RunLevelClubActivation (idButton.ToString ());
	}

	public void PredicirComoHincha(){
		predictionState = "MakePredictionsHincha";

		GameObject eventManager = GameObject.Find ("EventManager");
		eventManager.GetComponent<NavigationTabPanelsScript> ().ShowUpdateDataPanel();

		StartCoroutine (SendPronostico());
	}

	public void RunPredictionsHinchaNivelClub(){
	
		predictionState = "MakePredictionsHinchaNivelClub";

		GameObject eventManager = GameObject.Find ("EventManager");
		eventManager.GetComponent<NavigationTabPanelsScript> ().ShowUpdateDataPanel();

		StartCoroutine (SendPronostico());
	}

	private IEnumerator SendPronostico(){

		predictionState = "SubirPrediccion";

		StartCoroutine(RunSendGoalsPronostico ());

		yield return null;
	}

	private IEnumerator RunSendGoalsPronostico(){

		Debug.Log ("ENTRO AQUI RunSendGoalsPronostico");
	
		//Validar si soy el primero en enviar el pronostico:

		string fechaEncuentro = "&fechaEncuentro=" + date.Year + "-" + date.Month + "-" + date.Day;

		int soyPrimero = 0;

		//WWW soyPrimeroEnEnviar = new WWW (urlPhpPronostico + "indata=SoyElPrimeroEnEnviar" + fechaEncuentro);
		Debug.Log ("La url es: " + UrlsAdmin.self._ftpUrl + "PollaTricolor/Php/SendAndReceivePrediction.php" + "?action=SoyElPrimeroEnEnviar" + fechaEncuentro);
		WWW soyPrimeroEnEnviar = new WWW (UrlsAdmin.self._ftpUrl + "PollaTricolor/Php/SendAndReceivePrediction.php" + "?action=SoyElPrimeroEnEnviar" + fechaEncuentro);

		yield return soyPrimeroEnEnviar;

		if (string.IsNullOrEmpty (soyPrimeroEnEnviar.error)) {

			soyPrimero = int.Parse(soyPrimeroEnEnviar.text);
			Debug.Log ("EL ESTADO ES: " + soyPrimeroEnEnviar.text);

		} else {
			
			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}


		WWW hs_get;
		bool error = false;

		//FootballPlayerListScript footballPlayers = GameObject.Find ("TabPrediccionPanel").GetComponent<GetFootballPlayersDataScript> ().footballPlayersdata;
		//FootballPlayer player = footballPlayers.dataList [jugadorDeLaCancha.value];

		//string fechaEncuentro = "&fechaEncuentro=" + "2016-10-14";
		string userID = "&userID=" + DataApp.main.GetMyID (); //654321;
		string idAutorGol;
		string autorGol;
		string tipoGol;
		string minGol;
		string idAutorPaseGol;
		string autorPaseGol;
		string idjugadorCancha = "&idJugadorCancha=" + 0;//player.idData;
		string jugadorCancha = "&jugadorCancha=" + "";//jugadorDeLaCancha.options[jugadorDeLaCancha.value].text;

		string golesBarcelona;
		string golesOponente;

		DateTime localTime = DateTime.Now;

		string fechaPronosticado = "&fechaPronosticado=" + localTime.Year + "-" + localTime.Month + "-" + localTime.Day + " " + localTime.Hour + ":" + localTime.Minute + ":" + localTime.Second;

		Debug.Log ("EL VALOR ES: " + teamIsLocal);

		int valor;

		if (teamIsLocal) {

			valor = 1;
		
			golesBarcelona = "&golesBarcelona=" + goalsBarcelona.text;
			golesOponente = "&golesOponente=" + goalsOponente.text;

		} else {

			valor = 0;
		
			golesBarcelona = "&golesBarcelona=" + goalsOponente.text;
			golesOponente = "&golesOponente=" + goalsBarcelona.text;
		}

		string urlPronostico;

		if (panelsData.Count == 0) {
			//Debug.Log ("ENTRO AQUI RunSendGoalsPronostico");

			Debug.Log ("0000000000000000");
		
			idAutorGol = "&idAutorGol=" + 0;
			autorGol = "&autorGol=" + "none";
			tipoGol = "&tipoGol=" + "none";
			minGol = "&minGol=" + -2;
			idAutorPaseGol = "&idAutorPaseGol=" + 0;
			autorPaseGol = "&autorPaseGol=" + "none";

			//urlPronostico = urlPhpPronostico + "indata=createPronostico" + fechaEncuentro + userID + idAutorGol + autorGol + tipoGol + minGol + idAutorPaseGol + autorPaseGol + idjugadorCancha + jugadorCancha + golesBarcelona + golesOponente + "&esLocal=" + valor + "&soyPrimero=" + soyPrimero + fechaPronosticado + alineacionToUrl;
			urlPronostico = UrlsAdmin.self._ftpUrl + "PollaTricolor/Php/SendAndReceivePrediction.php" + "?action=SendGoalsPrediction" + fechaEncuentro + userID + idAutorGol + autorGol + tipoGol + minGol + idAutorPaseGol + autorPaseGol + idjugadorCancha + jugadorCancha + golesBarcelona + golesOponente + "&esLocal=" + valor + "&soyPrimero=" + soyPrimero + fechaPronosticado;
			Debug.Log ("lA url 0 es:" + urlPronostico.Replace(" ", "%20"));
			hs_get = new WWW (urlPronostico.Replace(" ", "%20"));

			yield return hs_get;

			if (string.IsNullOrEmpty (hs_get.error)) {

				Debug.Log ("Sii!!");

			} else {

				error = true;
				UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
				//break;
				Debug.Log ("Noo!!");
			}

		} else {

			Debug.Log ("1111111111111");
		
			foreach(GameObject panel in panelsData){

				idAutorGol = "&idAutorGol=" + panel.GetComponent<PanelPronosticoDataScript> ()._idGoleador;
				autorGol = "&autorGol=" + panel.GetComponent<PanelPronosticoDataScript> ().goleadorText.text;
				tipoGol = "&tipoGol=" + panel.GetComponent<PanelPronosticoDataScript> ().goalTypeText.text;
				minGol = "&minGol=" + panel.GetComponent<PanelPronosticoDataScript> ().minGoalText.text;
				idAutorPaseGol = "&idAutorPaseGol=" + panel.GetComponent<PanelPronosticoDataScript> ()._idPaseGoal;
				autorPaseGol = "&autorPaseGol=" + panel.GetComponent<PanelPronosticoDataScript> ().paseGoalText.text;

				//urlPronostico = urlPhpPronostico + "indata=createPronostico" + fechaEncuentro + userID + idAutorGol + autorGol + tipoGol + minGol + idAutorPaseGol + autorPaseGol + idjugadorCancha + jugadorCancha + golesBarcelona + golesOponente + "&esLocal=" + valor + "&soyPrimero=" + soyPrimero + fechaPronosticado + alineacionToUrl;
				urlPronostico = UrlsAdmin.self._ftpUrl + "PollaTricolor/Php/SendAndReceivePrediction.php" + "?action=SendGoalsPrediction" + fechaEncuentro + userID + idAutorGol + autorGol + tipoGol + minGol + idAutorPaseGol + autorPaseGol + idjugadorCancha + jugadorCancha + golesBarcelona + golesOponente + "&esLocal=" + valor + "&soyPrimero=" + soyPrimero + fechaPronosticado;
				urlPronostico = urlPronostico.Replace (" ", "%20");

				Debug.Log ("lA url Goles Pronostico es:" + urlPronostico);
				hs_get = new WWW (urlPronostico);

				yield return hs_get;

				if (string.IsNullOrEmpty (hs_get.error)) {

					Debug.Log ("Sii!!");

				} else {

					error = true;
					UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
					break;
				}

			}
		}

		if (!error) {

			//StartCoroutine(ClearAndShowPrediction ()); //CONTINUAR AQUIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			StartCoroutine(RunSendAlignmentPronostico());
		}
	}

	private IEnumerator RunSendAlignmentPronostico(){
	
		Debug.Log ("ENTRO AQUI SendPronostico");

		alineacionIdList = new List<int>();

		alignmentList.ForEach (delegate (GameObject obj){

			alineacionIdList.Add(obj.GetComponent<FootballPlayerItem> ()._playerId);
		});

		string userID = "&userID=" + DataApp.main.GetMyID ();

		string fechaEncuentro = "&fechaEncuentro=" + date.Year + "-" + date.Month + "-" + date.Day;

		string alineacionToUrl = "&alinId1=" + alineacionIdList [0] + 
			"&alinId2=" + alineacionIdList [1] + 
			"&alinId3=" + alineacionIdList [2] + 
			"&alinId4=" + alineacionIdList [3] + 
			"&alinId5=" + alineacionIdList [4] + 
			"&alinId6=" + alineacionIdList [5] + 
			"&alinId7=" + alineacionIdList [6] + 
			"&alinId8=" + alineacionIdList [7] + 
			"&alinId9=" + alineacionIdList [8] + 
			"&alinId10=" + alineacionIdList [9] + 
			"&alinId11=" + alineacionIdList [10];

		string url = UrlsAdmin.self._ftpUrl + "PollaTricolor/Php/SendAndReceivePrediction.php" + "?action=SendAlignmentPrediction" + fechaEncuentro + userID + alineacionToUrl;
		url = url.Replace (" ", "%20");
		Debug.Log ("lA url 1 es:" + url);

		bool error = false;

		WWW hs_get = new WWW (url);

		yield return hs_get;

		if (string.IsNullOrEmpty (hs_get.error)) {

			Debug.Log ("Sii!!");

		} else {

			error = true;
			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}

		if (!error) {

			StartCoroutine(ClearAndShowPrediction ());
		}
	}

	public void ContinueAfterUploadingPrediction(){
	
		if(predictionState == "HayPrediccion"){

			EndUpdateData("none");
			
		} else if(predictionState == "SubirPrediccion"){

			EndUpdateData("MakePredictionsHinchaNivelClub");
		}
	}

	private IEnumerator ClearAndShowPrediction (){

		string fechaEncuentro = "&buscarFecha=" + date.Year + "-" + date.Month + "-" + date.Day;

		Debug.Log ("FECHAAAAAA: " + date.Year + "-" + date.Month + "-" + date.Day);

//		string uu = "http://2waysports.com/2waysports/Ecuador/Barcelona/Pronostico/Php/UserPronosticoSColombia.php";
//
//		string url = uu + "?indata=GetPredictions" + fechaEncuentro + "&buscarUserID=" + DataApp.main.GetMyID ();

		string url = UrlsAdmin.self._ftpUrl + "PollaTricolor/Php/SendAndReceivePrediction.php" + "?action=ReceivePredictionGols" + fechaEncuentro + "&buscarUserID=" + DataApp.main.GetMyID ();

		//Debug.Log ("LA URL ES: " + url);

		yield return StartCoroutine (GetJsonDataScript.getJson.GetPhpData (url));

		if (GetJsonDataScript.getJson._state == "Successful") {

			predictionsList = GetJsonDataScript.getJson.GetData (predictionsList, "autor_Gol", "tipo_Gol", "min_Gol", "autor_PaseGol", "jugador_Cancha", "goles_Barcelona", "goles_Oponente", "esLocal", "fechaPronostico");

			StartCoroutine(DestroyCurrentPanelsAndShowPrediction ());

		} else if (GetJsonDataScript.getJson._state == "Warning_01") {

			UpdateDataScript.updateData.RunPopup ("Error al enviar la predicción \nIntentelo de nuevo.", 1);

		} else if (GetJsonDataScript.getJson._state == "Warning_02") {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}

		yield return null;
	}

	private IEnumerator DestroyCurrentPanelsAndShowPrediction(){

		foreach (GameObject objPanel in panelsData) {
		
			Destroy (objPanel);
		}

		panelsData.Clear ();

		yield return panelsData;

		Debug.Log ("Destroi -> " + panelsData.Count);

		StartCoroutine (ShowPredictions (true));
	}

	private IEnumerator ValidateStatusPrediction (){

		string url = UrlsAdmin.self._ftpUrl + "PollaTricolor/Php/GetFechasPartidos.php?" + "action=GetFechaPartido" + "&idDate=" + (int)idFechasPartidos.proximoPartido;

		WWW getData = new WWW (url);

		yield return getData;

		if (string.IsNullOrEmpty (getData.error)) {

			if (getData.text != "PredictingUncreated") {

				fechaList = JsonUtility.FromJson<DateClassList> (getData.text);
				//StartCoroutine (GetPredictions ());
				setupDate();

			} else {

				//this.GetComponent<GetFootballPlayersDataScript> ().RunGetFootballPlayersData ();
				Debug.Log("¡No hay datos!");
			}
		} else {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}

		Debug.Log ("GETDATA: " + getData.text);
	}

	private void setupDate(){

		DateTime newdate = DateTime.Parse (fechaList.dataList [0].fecha);

		if (date != newdate) {

			date = newdate;
			//DateTime nextDate = DateTime.Parse (fechaList.dataList [0].fecha + " " + fechaList.dataList [0].hora);
			String isLocal = fechaList.dataList [0].equipo;

			if (isLocal [isLocal.Length - 1] == '.') {

				goalsBarcelona.onEndEdit.AddListener (delegate {
					UpdateSpaceData ();
				});
				inputBarcelonaScore = goalsBarcelona;
				teamIsLocal = true;
				print ("ES LOCAL");
			} else {

				goalsOponente.onEndEdit.AddListener (delegate {
					UpdateSpaceData ();
				});
				inputBarcelonaScore = goalsOponente;
				teamIsLocal = false;
				print (" NO ES LOCAL");
			}

			CultureInfo ci = new CultureInfo ("es-ES");
			string mes = date.ToString ("MMMM", ci);
			string dia = date.ToString ("dddd", ci);
			string diaNum = date.ToString ("dd", ci).ToUpper ();

			fechaTextAlineacion.text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (dia) + " " + diaNum + " de " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase (mes);
			fechaText.text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (dia) + " " + diaNum + " de " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase (mes);

			if (dataIsValidated) {

				Debug.Log ("Entro Aqui: " + dataIsValidated);

				StartCoroutine (ClearAndEnablePrediction ("NuevaFecha"));
			
			} else {
			
				Debug.Log ("Entro Aqui: " + dataIsValidated);

				//StartCoroutine (GetPredictions ());
				StartCoroutine (GetMinuteLimit());
			}
		
		} else {
		
			EndUpdateData ("none");
		}
	}

	private IEnumerator ClearAndEnablePrediction(string action){

		goalsBarcelona.text = "";
		goalsBarcelona.interactable = true;
		goalsOponente.text = "";
		goalsOponente.interactable = true;
		inputBarcelonaScore.text = "";
		currentGoals = 0;

		predictionsList.dataList.Clear();

		foreach (GameObject objPanel in panelsData) {

			Destroy (objPanel);
		}

		panelsData.Clear ();

		yield return panelsData;

		predecirBtn.SetActive (false);
		compartirBtn.SetActive (false);

		tutoBtnAligment.SetActive (false);
		instructionBtnAligment.SetActive (false);

		tutoBtnPrediction.SetActive (false);
		instructionBtnPrediction.SetActive (false);

		if (action == "NuevaFecha") {

			StartCoroutine (GetPredictions ());

		} else if (action == "Limpiar") {

			predecirBtn.SetActive (true);
			compartirBtn.SetActive (false);

			tutoBtnAligment.SetActive (false);
			instructionBtnAligment.SetActive (true);

			tutoBtnPrediction.SetActive (false);
			instructionBtnPrediction.SetActive (true);

			HideUpdatePopup ();
		}
	}

	public void ValidatePredictionStatus(){
	
		StartCoroutine (GetPredictions ());
	}

	private IEnumerator GetPredictions (){
		/*
		WWW updFlag = new WWW(DataApp.main.host + DataApp.main.urlActualizarInfo + 8);
		yield return updFlag;

		if( string.IsNullOrEmpty(updFlag.error)){

			StartCoroutine (DownloadPredictions(int.Parse(updFlag.text)));
		}else{
			DataApp.main.popUpInformative(true,"Fallo en la conexíon.", "Revisa tu conexión a internet.");
//			UpdateDataScript.updateData.RunPopup ("Fallo en la conexíon.", "Revisa tu conexión a internet.", 0);
		}*/

		StartCoroutine (DownloadPredictions(-2));

		yield return null;
	}

	private IEnumerator DownloadPredictions (int NumAct){

		bool downloadPrediction = true;

		if( DataApp.main.GetMyInfoInt("UserPrediction") == NumAct){

			string jsonGuardado = DataApp.main.GetMyInfoString("JsonUserPrediction");
			yield return StartCoroutine (GetJsonDataScript.getJson.GetLocalData (jsonGuardado));

			if (GetJsonDataScript.getJson._state == "Successful") {

				downloadPrediction = false;
			}
		}

		if(downloadPrediction){

			string fechaEncuentro = "&buscarFecha=" + date.Year + "-" + date.Month + "-" + date.Day;
			//string fechaEncuentro = "&buscarFecha=" + 2017 + "-" + 3 + "-" + 1;

			Debug.Log ("FECHAAAAAA: " + date.Year + "-" + date.Month + "-" + date.Day);

			string url = UrlsAdmin.self._ftpUrl + "PollaTricolor/Php/SendAndReceivePrediction.php" + "?action=ReceivePredictionGols" + fechaEncuentro + "&buscarUserID=" + DataApp.main.GetMyID ();

			Debug.Log ("LA URL ES: " + url);

			yield return StartCoroutine (GetJsonDataScript.getJson.GetPhpData (url));

			if (GetJsonDataScript.getJson._state == "Successful") {
			
				DataApp.main.SetMyInfo("JsonUserPrediction", GetJsonDataScript.getJson.GetDataConsult(), 3);
				//DataApp.main.SetMyInfo("UserPrediction", NumAct.ToString(), 1);
			}
		}

		if (GetJsonDataScript.getJson._state == "Successful") {

			predictionsList = GetJsonDataScript.getJson.GetData (predictionsList, "autor_Gol", "tipo_Gol", "min_Gol", "autor_PaseGol", "jugador_Cancha", "goles_Barcelona", "goles_Oponente", "esLocal", "fechaPronostico");

			predictionState = "HayPrediccion";

			Debug.Log("SI HAY PRONOSTICO");

			StartCoroutine (ShowPredictions (downloadPrediction));

		} else if (GetJsonDataScript.getJson._state == "Warning_01") {

			Debug.Log("NO HAY PRONOSTICO");

			textState.text = "REGISTRA TU PREDICCIÓN";
			textStateAling.text = "ELIGE LA ALINEACÍON";

			predecirBtn.SetActive (true);
			compartirBtn.SetActive (false);

			tutoBtnAligment.SetActive (true);
			instructionBtnAligment.SetActive (false);

			tutoBtnPrediction.SetActive (true);
			instructionBtnPrediction.SetActive (false);

			//StartCoroutine (GetMinuteLimit());
			//EndUpdateData (predictionState);
			EndUpdateData ("EndLoadFootballPlayers");

		} else if (GetJsonDataScript.getJson._state == "Warning_02") {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}

		Debug.Log ("Json Lista Ranking: " + JsonUtility.ToJson(predictionsList));
	}

	private IEnumerator GetMinuteLimit(){
	
		string url = UrlsAdmin.self._ftpUrl + "PollaTricolor/Php/AppData.php?indata=getMinuteLimit";
		WWW getData = new WWW (url);
		yield return getData;

		if (string.IsNullOrEmpty (getData.error)) {

			if (getData.text != "PredictingUncreated") {

				minuteLimit = int.Parse (getData.text);
				
			} else {

				minuteLimit = 120;				
			}

			predictionState = "NoHayPrediction";

			Debug.Log ("Prediction State es: " + predictionState);

			//EndUpdateData (predictionState);
			this.GetComponent<GetFootballPlayersDataScript> ().RunGetFootballPlayersData ();

		} else {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}
	}

	private IEnumerator ShowPredictions(bool downloadPrediction){

		int goals = predictionsList.dataList.Count;

		if (goals > 1) {

			ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, goals * SizeScrollContent);
		} else {

			ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, (goals * SizeScrollContent) + SizeScrollContent);
		}


		int index = 0;

		if(predictionsList.dataList[0].GetValueToKey("autor_Gol") != "none"){

			foreach(DataRow obj in predictionsList.dataList){

				GameObject panelData = Instantiate (prefabShowData);
				//panelData.GetComponent<CanvasGroup> ().alpha = 0f;
				panelData.transform.SetParent (ScrollContent.transform);
				panelsData.Add (panelData);
				panelData.GetComponent<RectTransform>().localPosition = new Vector3 (402, index * -SizeScrollContent, 0);
				panelData.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);
				yield return new WaitForEndOfFrame (); //WaitForSeconds(0.1f);
				yield return new WaitForSeconds(0.5f);
				panelData.GetComponent<ShowPronosticoScript> ()._indexPanel = index;
				panelData.GetComponent<ShowPronosticoScript> ().goalTitle.text = titleGols[index] + " GOL";
				panelData.GetComponent<ShowPronosticoScript> ().goleadorText.text = obj.GetValueToKey("autor_Gol");
				panelData.GetComponent<ShowPronosticoScript> ().goalTypeText.text = obj.GetValueToKey("tipo_Gol");
				panelData.GetComponent<ShowPronosticoScript> ().minutes.text = obj.GetValueToKey ("min_Gol");
				panelData.GetComponent<ShowPronosticoScript> ().paseGoalText.text = obj.GetValueToKey("autor_PaseGol");
				Debug.Log("1: "+ obj.GetValueToKey("autor_Gol") +"2: "+ obj.GetValueToKey("tipo_Gol") +"3: "+ obj.GetValueToKey("min_Gol") +"4: "+ obj.GetValueToKey("autor_PaseGol")  );
				//panelData.GetComponent<CanvasGroup> ().alpha = 1f;

//				if (index % 2 == 0) {
//
//					panelData.GetComponent<Image>().color = new Color32(0,0,0,255);
//
//				} else {
//
//					panelData.GetComponent<Image>().color = new Color32(28,28,28,255);
//				}

				index++;
			}
		}

		string oponente = fechaList.dataList [0].equipo.Replace (".", "");

		Debug.Log ("OPONENTE ES: " + oponente);

		if (int.Parse(predictionsList.dataList [0].GetValueToKey("esLocal")) == 1) {

			EscudoEquipo.sprite = Resources.Load<Sprite>("Equipos/"+"colombia"); //ImgLoadManager.main.teamImg (EscudoEquipo, "COLOMBIA", false);
			EscudoOponente.sprite = Resources.Load<Sprite>("Equipos/"+oponente.ToLower()); //ImgLoadManager.main.teamImg (EscudoOponente, oponente.ToUpper(), false);

			EscudoEquipoGoals.sprite = Resources.Load<Sprite>("Equipos/"+"colombia"); //ImgLoadManager.main.teamImg (EscudoEquipoGoals, "COLOMBIA", false);
			EscudoOponenteGoals.sprite = Resources.Load<Sprite>("Equipos/"+oponente.ToLower()); //ImgLoadManager.main.teamImg (EscudoOponenteGoals, oponente.ToUpper(), false);

			goalsBarcelona.text = predictionsList.dataList [0].GetValueToKey ("goles_Barcelona");
			goalsBarcelona.interactable = false;
			goalsOponente.text = predictionsList.dataList [0].GetValueToKey ("goles_Oponente");
			goalsOponente.interactable = false;

			Debug.Log ("ES LOCALLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL");

		} else {

			EscudoEquipo.sprite = Resources.Load<Sprite>("Equipos/"+oponente.ToLower()); //ImgLoadManager.main.teamImg (EscudoEquipo, oponente.ToUpper(), false);
			EscudoOponente.sprite = Resources.Load<Sprite>("Equipos/"+"colombia"); //ImgLoadManager.main.teamImg (EscudoOponente, "COLOMBIA", false);

			EscudoEquipoGoals.sprite = Resources.Load<Sprite>("Equipos/"+oponente.ToLower()); //ImgLoadManager.main.teamImg (EscudoEquipoGoals, oponente.ToUpper(), false);
			EscudoOponenteGoals.sprite = Resources.Load<Sprite>("Equipos/"+"colombia"); //ImgLoadManager.main.teamImg (EscudoOponenteGoals, "COLOMBIA", false);

			goalsBarcelona.text = predictionsList.dataList [0].GetValueToKey ("goles_Oponente");
			goalsBarcelona.interactable = false;
			goalsOponente.text = predictionsList.dataList [0].GetValueToKey ("goles_Barcelona");
			goalsOponente.interactable = false;

			Debug.Log ("NO ES LOCALLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL");
		}

		SetFechaCuandoSePronostico ();

		//		jugadorDeLaCancha.transform.FindChild("Label").GetComponent<Text>().text = predictionsList.dataList[0].jugador_Cancha;
		//		jugadorDeLaCancha.transform.FindChild ("Arrow").gameObject.SetActive(false);
		//		jugadorDeLaCancha.enabled = false;

		/////////////////////////////////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA qui!
		textState.text = "TU PREDICCIÓN FUE REGISTRADA";
		textStateAling.text = "ALINEACÍON REGISTRADA";

		predecirBtn.SetActive (false);
		compartirBtn.SetActive (true);

		tutoBtnAligment.SetActive (false);
		instructionBtnAligment.SetActive (true);

		tutoBtnPrediction.SetActive (false);
		instructionBtnPrediction.SetActive (true);

		//EndUpdateData (predictionState);

		StartCoroutine (GetAlignmentPlayers(downloadPrediction));

	}

	public void ShowShieldTeams (){

		string oponente = fechaList.dataList [0].equipo.Replace (".", "");

		Debug.Log ("OPONENTE ES: " + oponente);

		bool esLocal = fechaList.dataList [0].equipo.Contains(".");

		if (esLocal) {

			EscudoEquipo.sprite = Resources.Load<Sprite>("Equipos/"+"colombia"); //ImgLoadManager.main.teamImg (EscudoEquipo, "COLOMBIA", false);
			EscudoOponente.sprite = Resources.Load<Sprite>("Equipos/"+oponente.ToLower()); //ImgLoadManager.main.teamImg (EscudoOponente, oponente.ToUpper(), false);

			EscudoEquipoGoals.sprite = Resources.Load<Sprite>("Equipos/"+"colombia"); //ImgLoadManager.main.teamImg (EscudoEquipoGoals, "COLOMBIA", false);
			EscudoOponenteGoals.sprite = Resources.Load<Sprite>("Equipos/"+oponente.ToLower()); //ImgLoadManager.main.teamImg (EscudoOponenteGoals, oponente.ToUpper(), false);

			Debug.Log ("ES LOCALLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL");

		} else {

			EscudoEquipo.sprite = Resources.Load<Sprite>("Equipos/"+oponente.ToLower()); //ImgLoadManager.main.teamImg (EscudoEquipo, oponente.ToUpper(), false);
			EscudoOponente.sprite = Resources.Load<Sprite>("Equipos/"+"colombia"); //ImgLoadManager.main.teamImg (EscudoOponente, "COLOMBIA", false);

			EscudoEquipoGoals.sprite = Resources.Load<Sprite>("Equipos/"+oponente.ToLower()); //ImgLoadManager.main.teamImg (EscudoEquipoGoals, oponente.ToUpper(), false);
			EscudoOponenteGoals.sprite = Resources.Load<Sprite>("Equipos/"+"colombia"); //ImgLoadManager.main.teamImg (EscudoOponenteGoals, "COLOMBIA", false);

			Debug.Log ("NO ES LOCALLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL");
		}

		UpdateDataScript.updateData.StopUpdatePanel ();
	}

	private void SetFechaCuandoSePronostico (){

		DateTime nextDate = DateTime.Parse (predictionsList.dataList [0].GetValueToKey("fechaPronostico"));

		CultureInfo ci = new CultureInfo ("es-ES");
		string mes = nextDate.ToString ("MMMM", ci);
		string dia = nextDate.ToString ("dddd", ci);
		string diaNum = nextDate.ToString ("dd", ci).ToUpper ();

		mes = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (mes);

		dia = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (dia);

		fechaPronosticoText.text = "<b>Fecha y hora de tu Pronóstico: </b>" + dia.ToLower() + " " + diaNum + " de " + mes.ToLower() + " a las " + nextDate.Hour + "h, " + nextDate.Minute + "m, " + nextDate.Second + "s";
	}

	private IEnumerator GetAlignmentPlayers(bool downloadPrediction) {

		if(!downloadPrediction){

			string jsonGuardado = DataApp.main.GetMyInfoString("JsonUserPredictionAlignment");
			yield return StartCoroutine (GetJsonDataScript.getJson.GetLocalData (jsonGuardado));

			if (GetJsonDataScript.getJson._state == "Successful") {

				downloadPrediction = true;
			}
		}

		if(downloadPrediction){

			string fechaEncuentro = "&buscarFecha=" + date.Year + "-" + date.Month + "-" + date.Day;

			string url = UrlsAdmin.self._ftpUrl + "PollaTricolor/Php/SendAndReceivePrediction.php" + "?action=ReceivePredictionAlignment" + fechaEncuentro + "&buscarUserID=" + DataApp.main.GetMyID ();

			Debug.Log ("LA URL ES: " + url);

			yield return StartCoroutine (GetJsonDataScript.getJson.GetPhpData (url));

			if (GetJsonDataScript.getJson._state == "Successful") {

				DataApp.main.SetMyInfo("JsonUserPredictionAlignment", GetJsonDataScript.getJson.GetDataConsult(), 3);
			}
		}

		if (GetJsonDataScript.getJson._state == "Successful") {

			playerAlignmentList = GetJsonDataScript.getJson.GetData (playerAlignmentList, "Player1", "Player2", "Player3", "Player4", "Player5", "Player6", "Player7", "Player8", "Player9", "Player10", "Player11");

			//StartCoroutine (ShowAlignment ());
			alignmentPanel.ShowAlignment ();

			//this.GetComponent<GetFootballPlayersDataScript> ().RunGetFootballPlayersData ();

			//EndUpdateData (predictionState);

		} else if (GetJsonDataScript.getJson._state == "Warning_01") {

			Debug.Log ("No se descargaron datos de alineacion.");

		} else if (GetJsonDataScript.getJson._state == "Warning_02") {

			UpdateDataScript.updateData.RunPopup ("Fallo en la conexión.\nIntentelo de nuevo.", 0);
		}
	}

	private IEnumerator ShowAlignment(){

		int goals = predictionsList.dataList.Count;

		if (goals > 1) {

			ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, goals * SizeScrollContent);
		} else {

			ScrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, (goals * SizeScrollContent) + SizeScrollContent);
		}

		int index = 0;

		if(predictionsList.dataList[0].GetValueToKey("autor_Gol") != "none"){

			foreach(DataRow obj in predictionsList.dataList){

				GameObject panelData = Instantiate (prefabShowData);
				panelData.transform.SetParent (ScrollContent.transform);
				panelsData.Add (panelData);
				panelData.GetComponent<RectTransform>().localPosition = new Vector3 (402, index * -SizeScrollContent, 0);
				panelData.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);
				yield return new WaitForEndOfFrame (); //WaitForSeconds(0.1f);
				panelData.GetComponent<ShowPronosticoScript> ()._indexPanel = index;
				panelData.GetComponent<ShowPronosticoScript> ().goalTitle.text = titleGols[index] + " GOL";
				panelData.GetComponent<ShowPronosticoScript> ().goleadorText.text = obj.GetValueToKey("autor_Gol");
				panelData.GetComponent<ShowPronosticoScript> ().goalTypeText.text = obj.GetValueToKey("tipo_Gol");
				panelData.GetComponent<ShowPronosticoScript> ().minutes.text = obj.GetValueToKey ("min_Gol");
				panelData.GetComponent<ShowPronosticoScript> ().paseGoalText.text = obj.GetValueToKey("autor_PaseGol");

				index++;
			}
		}

		if (int.Parse(predictionsList.dataList [0].GetValueToKey("esLocal")) == 1) {

			goalsBarcelona.text = predictionsList.dataList [0].GetValueToKey ("goles_Barcelona");
			goalsBarcelona.interactable = false;
			goalsOponente.text = predictionsList.dataList [0].GetValueToKey ("goles_Oponente");
			goalsOponente.interactable = false;

		} else {

			goalsBarcelona.text = predictionsList.dataList [0].GetValueToKey ("goles_Oponente");
			goalsBarcelona.interactable = false;
			goalsOponente.text = predictionsList.dataList [0].GetValueToKey ("goles_Barcelona");
			goalsOponente.interactable = false;
		}

		EndUpdateData (predictionState);
	}

	public void EndUpdateData(string state){

		if (state == "EndLoadFootballPlayers") {

			alignmentPanel.SetFootBallPlayersAlignmentScroll ();

		} else if (state == "EndMakePredictions") {

			ShowMessagePopup ("¡Pronostico registrado con éxito!", 1);

		} else if (state == "MakePredictionsHincha") {

			HideUpdatePopup ();

			PredictionHincha.Invoke ();

		} else if (state == "MakePredictionsHinchaNivelClub") {

			HideUpdatePopup ();

			PredictionHinchaNivelClub.Invoke ();

		} else if (state == "NoHayPrediction") {

			//this.GetComponent<GetFootballPlayersDataScript> ().RunGetFootballPlayersData ();

		} else {
		
			HideUpdatePopup ();
		}

		predictionState = "none";
	
		//dataIsValidated = true;
	}

	void OnEnable() {

		//if (dataIsValidated == false) {

			predictionState = "valideData";

			ShowUpdatePopup ();

			StartCoroutine(ValidateStatusPrediction ());
		//}
	}

	void OnDisable() {

		CancelInvoke("RotateImg");
	}

	private void ShowUpdatePopup(){
	
		GameObject eventManager = GameObject.Find ("EventManager");
		eventManager.GetComponent<NavigationTabPanelsScript> ().ShowUpdateDataPanel();
	}

	private void HideUpdatePopup(){

		GameObject eventManager = GameObject.Find ("EventManager");
		eventManager.GetComponent<NavigationTabPanelsScript> ().HideUpdateDataPanel();
	}

	private void ShowMessagePopup(string message, int indexTabContinue){

		GameObject eventManager = GameObject.Find ("EventManager");
		eventManager.GetComponent<NavigationTabPanelsScript> ().ShowPopup (message, indexTabContinue);
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using MaterialUI;

public class PerfilJugador : MonoBehaviour {

	public static PerfilJugador main;

	public Convocados ManagerConvocados;
	int idPlayer;
	[HideInInspector]
	public Sprite defaultImg;
//	[HideInInspector]
	public Sprite defaultImgStats;
//	[HideInInspector]
	public Sprite defaultImgFinalMachs;
	[HideInInspector]
	public Image InfoPlayerImg;
	[HideInInspector]
	public Image StatsPlayerImg;
	[HideInInspector]
	public Image ultimoPartidoImg;
	[HideInInspector]
	public Text  cantAplausosTotales;
	[HideInInspector]
	public Text cantAplausosPartido;

	[Header("Componentes de las Noticias")]
	public GameObject contentNew;
	public GameObject contentPlayer;
	public GameObject contentListPlayer;
	public GameObject prefabNew;
	public GameObject blackLine;
	public List<GameObject> listNews;
	public Button BackNewsBtn;

	[Header("Redes Sociales")]
	public Button fbbtn;
	public Button twbtn;
	public Button insbtn;
	[Space(10)]
	public BigViewNew bigView;

	public Toggle votePlayerAplausos;

	public bool isvote;
	bool reload;
	public string fb, tw, inst, snap;

	List<viewNew> newsPlayer = new List<viewNew>();



	bool canResize;


	void Awake( ){
		if( !PlayerPrefs.HasKey("myVotesPlayer") ){
			StartCoroutine(dpwnloadVotes());
		}

	}


//	void Update ( ){
//		if (contentListPlayer.GetComponent<RectTransform> ().anchoredPosition.y > 500)
//			BackNewsBtn.gameObject.SetActive (true);
//		else
//			BackNewsBtn.gameObject.SetActive (false);
//	}

	public void GetPlayer(int id, int aplUltimo, int aplTotal, string _fb, string _tw, string _inst, string _snap ){


		DataApp.main.EnableLoading();
		NavigatorManager.main.panelsPrincipales[NavigatorManager.main.actualPanel]._enablePopUpInfoPanel(0);
		InfoPlayerImg.sprite = defaultImg;
		StatsPlayerImg.sprite = defaultImgStats;
		ultimoPartidoImg.sprite = defaultImgFinalMachs;
		idPlayer = id;
//		InfoPlayerImg.sprite = ImgLoadManager.main.PlayerConvocadosImg(InfoPlayerImg, id.ToString() , ManagerConvocados.update );
		// StatsPlayerImg.sprite = ImgLoadManager.main.PlayerStats(StatsPlayerImg, id+"s", ManagerConvocados.update );
//		ultimoPartidoImg.sprite = ImgLoadManager.main.PlayerFinalMatch(ultimoPartidoImg, id.ToString() , ManagerConvocados.update );
		cantAplausosTotales.text = aplTotal.ToString();
		cantAplausosPartido.text = aplUltimo.ToString();


		//REDES 
		fb = _fb;
		tw = _tw;
		inst = _inst;
		snap = _snap;

		if( string.IsNullOrEmpty(_fb) ){
			fbbtn.gameObject.SetActive(false);
		}
		if( string.IsNullOrEmpty(_tw) ){
			twbtn.gameObject.SetActive(false);
		}
		if( string.IsNullOrEmpty(_inst) ){
			insbtn.gameObject.SetActive(false);
		}

		StartCoroutine(_getPlayer(id));

	}


	public IEnumerator _getPlayer (int id ){

		yield return new WaitForSeconds(2);

		yield return StartCoroutine( ImgLoadManager.main.PlayerConvocadosImg_d(InfoPlayerImg, id.ToString(), sp =>{
			InfoPlayerImg.sprite = sp;
		}, ManagerConvocados.update ));

		yield return StartCoroutine( ImgLoadManager.main.PlayerFinalMatch_d(ultimoPartidoImg, id.ToString(), sp =>{
			ultimoPartidoImg.sprite = sp;
		}, ManagerConvocados.update ));
//		InfoPlayerImg.sprite = ImgLoadManager.main.PlayerConvocadosImg(InfoPlayerImg, id.ToString() , ManagerConvocados.update );
//		ultimoPartidoImg.sprite = ImgLoadManager.main.PlayerFinalMatch(ultimoPartidoImg, id.ToString() , ManagerConvocados.update );
		yield return StartCoroutine( callWaitGet( ) );

	}



	public void ResetPosContent( ){
		StartCoroutine (enumResetPosContent ());

	}


	IEnumerator callWaitGet( ){
		setTooglesVotation();
		loadNewsPlayers();
		yield return new WaitForSecondsRealtime(1f);	
		DataApp.main.DisableLoading();
	}


	IEnumerator enumResetPosContent(){
		
		float restPosY = contentListPlayer.GetComponent<RectTransform> ().anchoredPosition.y;
		while (restPosY > 0 ){
			if (restPosY <= 20f) {
				contentListPlayer.transform.parent.GetComponent<ScrollRect>().inertia = false;
				restPosY = 0;
				break;   }
			restPosY-=contentListPlayer.GetComponent<RectTransform> ().anchoredPosition.y/4;
			yield return new WaitForSeconds (.00001f);
			contentListPlayer.GetComponent<RectTransform> ().anchoredPosition = new Vector2(contentListPlayer.GetComponent<RectTransform> ().anchoredPosition.x, restPosY ) ;
			BackNewsBtn.gameObject.SetActive (false);

		}
		yield return new WaitForSeconds (.5f);
		contentListPlayer.transform.parent.GetComponent<ScrollRect>().inertia = true;
		contentPlayer.GetComponent<RectTransform> ().anchoredPosition = new Vector2(contentNew.GetComponent<RectTransform> ().anchoredPosition.x, 0);
	}

	public void OpenSocialRed ( string socialRed ){
		
		switch ( socialRed ){
			case  "FB":
				Application.OpenURL(fb);
				break;
			case  "TW":
				Application.OpenURL(tw);
				break;
			case  "INS":
				Application.OpenURL(inst);
				break;
		}
	}


	public void loadNewsPlayers( ){
		
		ClearListPrefabs();
		CreateNewsPlayer();

	}



	void CreateNewsPlayer( ){
		DataApp.main.EnableLoading();
		int index = 0;
		blackLine.GetComponent<LayoutElement>().preferredHeight =0;
		foreach( DataRow n in NewsManager.main.DatanewsList.dataList){
			if( int.Parse(n.GetValueToKey("noticiaJugador")) == idPlayer){
				index++;
				GameObject nNew = Instantiate( prefabNew ); 
				nNew.transform.SetParent(contentNew.transform, false);
				listNews.Add( nNew );
				nNew.gameObject.SetActive(true);
				nNew.name = n.GetValueToKey("idnoticia");;
				nNew.name = n.GetValueToKey("idnoticia");
				nNew.GetComponent<viewNew>().idNew = int.Parse(n.GetValueToKey("idnoticia"));
				nNew.GetComponent<viewNew> ().tituloNew.text = n.GetValueToKey("titulonoticia");
				nNew.GetComponent<viewNew> ().descNew.text = n.GetValueToKey("descnoticia");
				nNew.GetComponent<viewNew> ().dateNew.text = "                        "+ n.GetValueToKey("dateNoticia");
				nNew.GetComponent<viewNew> ().linkVideo = n.GetValueToKey("linknoticia");
				nNew.GetComponent<viewNew> ().isvideo =  int.Parse(n.GetValueToKey("isvideo"));
				nNew.GetComponent<viewNew> ().selectFunction.text = NewsManager.main.selFunction (int.Parse(n.GetValueToKey("isvideo")));
				nNew.GetComponent<viewNew> ().videoButton.SetActive( NewsManager.main.actBtnVideo (int.Parse(n.GetValueToKey("isvideo"))));
				nNew.GetComponent<viewNew> ().noticiaJugador = int.Parse(n.GetValueToKey("noticiaJugador"));
				nNew.GetComponent<viewNew> ().nImportante = n.GetValueToKey("noticiaImportante");
				//StartCoroutine (  nNew.GetComponent<viewNew> ().LoadImageInit ( false ));
			}
		}


		blackLine.transform.SetParent(blackLine.transform.parent.parent);
		blackLine.transform.SetParent(contentNew.transform.parent);
		blackLine.GetComponent<LayoutElement>().preferredHeight  += ((index*100)+100);

//		DataApp.main.DisableLoading();

	 }





	void ClearListPrefabs ( ){
		foreach(GameObject g in listNews ){
			Destroy (g);
		}
		newsPlayer.Clear();
	}



	public void vote( bool selvote){
		isvote = true;
		if(!reload){
			if( User.main.GetMyEmailVerif( ) == 1 ){
				DataApp.main.EnableLoading();
				StartCoroutine(voteEnum(selvote));
			}else if( selvote == true ){
				votePlayerAplausos.isOn = true;
				if(votePlayerAplausos.isOn){
					SnackbarManager.Show("Debes verificar tu cuenta desde tu E-mail",3f, "Re-Enviar Correo", () => {
						DataApp.main.EnableLoading();
						SnackbarManager.OnSnackbarCompleted();
						RegisterLoginManager.main.SendEmailto( User.main.GetMyEmail() , User.main.GetMyToken(), true );
					});
				}

			}else{
				votePlayerAplausos.isOn = true;
			}
		}else{
			
		}
	}

	IEnumerator voteEnum( bool selVote ){

		if(selVote){
			WWW vot = new WWW(DataApp.main.host + "Aplausos/php/getAplausos.php?indata=remVoto&idj="+idPlayer);
			yield return vot;
			if(string.IsNullOrEmpty(vot.error) && vot.text == "true"){
				string myvotes = PlayerPrefs.GetString("myVotesPlayer");
				myvotes = myvotes.Replace(idPlayer.ToString()+"-","");
				PlayerPrefs.SetString("myVotesPlayer",myvotes);
			}
		}else{
			WWW vot = new WWW(DataApp.main.host + "Aplausos/php/getAplausos.php?indata=addVoto&idj="+idPlayer);
			yield return vot;
			if(string.IsNullOrEmpty(vot.error)  && vot.text == "true"){
				if( !PlayerPrefs.GetString("myVotesPlayer").Contains(idPlayer.ToString())){
					string myvotes = PlayerPrefs.GetString("myVotesPlayer");
					myvotes += idPlayer+"-";
					PlayerPrefs.SetString("myVotesPlayer",myvotes);
				}
			}
		}

		StartCoroutine(uploadVotes());

		yield return StartCoroutine (SeleccionC.club.UpdatePlayers());
		yield return new WaitForSeconds(2);

		cantAplausosPartido.text = SeleccionC.club.PlayersInfo[idPlayer-1]._aplausosUltimoPartidol.ToString();
		cantAplausosTotales.text = SeleccionC.club.PlayersInfo[idPlayer-1]._totalAplausos.ToString();
		DataApp.main.DisableLoading();


	}


	IEnumerator dpwnloadVotes( ){
		WWW myvotes = new WWW( DataApp.main.host + "Aplausos/php/getAplausos.php?indata=votosJugadores&id=" + DataApp.main.GetMyID());
		yield return myvotes;
		if( string.IsNullOrEmpty( myvotes.error ) && !string.IsNullOrEmpty( myvotes.text )){
			PlayerPrefs.SetString("myVotesPlayer",myvotes.text);
		} 
	}



	IEnumerator uploadVotes( ){
		WWW myvotes = new WWW( DataApp.main.host + "Aplausos/php/getAplausos.php?indata=votosJugadoresUpdate&id=" + DataApp.main.GetMyID() + "&cantvotos=" +  PlayerPrefs.GetString("myVotesPlayer"));
		yield return myvotes;
		if( string.IsNullOrEmpty( myvotes.error ) && !string.IsNullOrEmpty( myvotes.text )){
			PlayerPrefs.SetString("myVotesPlayer",myvotes.text);
		} 
	}


	void setTooglesVotation(  ){
		reload =true;
		string[] nn = PlayerPrefs.GetString("myVotesPlayer").Split('-');
		votePlayerAplausos.isOn = true;
		foreach( string n in nn ){
				if(idPlayer.ToString() == n){
					votePlayerAplausos.isOn = false;
				}
		}
		reload =false;
	}



//	void OnEnable( ){
//		contentListPlayer.GetComponent<RectTransform> ().anchoredPosition = new Vector2(contentListPlayer.GetComponent<RectTransform> ().anchoredPosition.x, 0);
//	}
}

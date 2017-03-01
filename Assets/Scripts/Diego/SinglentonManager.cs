using UnityEngine;
using System.Collections;

public class SinglentonManager : MonoBehaviour {

	public NavigatorManager _NavigatorManager;
	public DataApp DataAppManager;
	public MediaPlayerManager videoManager;
	public ImgLoadManager loadImgManager;
	public NewsManager newsManager;
	public SeleccionC club;
	public RegisterLoginManager registerManager;
	public PerfilJugador perfilJugador;
	public StartPronostico pronostico;
	public HomeSplash homeMain;
	public Convocados convocadosManager;
//	public User userManager;
	// Use this for initialization
	void Awake () {
		NavigatorManager.main = _NavigatorManager;
		DataApp.main = DataAppManager;
		MediaPlayerManager.main = videoManager;
		ImgLoadManager.main = loadImgManager;
		NewsManager.main =  newsManager;
		SeleccionC.club = club;
		PerfilJugador.main =  perfilJugador;
		RegisterLoginManager.main = registerManager;
		StartPronostico.main = pronostico;
		HomeSplash.main =  homeMain;
		Convocados.main = convocadosManager;
	}

}

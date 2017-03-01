using UnityEngine;
using System.Collections;
using System.Security.AccessControl;
using UnityEngine.Events;

public class StartPronostico : MonoBehaviour {

	public static StartPronostico main;


	public bool ActivatedPolla;
	[SerializeField]
	private GameObject eventManager;
	[SerializeField]
	private CanvasGroup panelPrincipal,tittlePrincipal;



	public UnityEvent AccessPollaTricolor;

	void Start () {
		eventManager.GetComponent<tituloInfo>().GetRankingDataUser();
	}


	public void habilityPanelGame( bool act){
		if( User.main.IsAdult()){
			if( act ){
				panelPrincipal.alpha = 0;
				panelPrincipal.blocksRaycasts = false;
				tittlePrincipal.alpha = 0;
				tittlePrincipal.blocksRaycasts = false;
				AccessPollaTricolor.Invoke( );
			}else{
				panelPrincipal.alpha = 1;
				panelPrincipal.blocksRaycasts = true;
				tittlePrincipal.alpha = 1;
				tittlePrincipal.blocksRaycasts = true;
			}
			ActivatedPolla = act;
		}else{
			DataApp.main.popUpInformative(true, "Acceso no permitido", "Debes ser mayor de edad para poder jugar Polla Tricolor");
		}
			
	}
}

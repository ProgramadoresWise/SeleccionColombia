using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Threading;


[RequireComponent (typeof (CanvasGroup))]
public class Panel : MonoBehaviour {



	public string titlePanel;
	public int index;
	public List<GameObject> subPanelsButtons;
	public List<GameObject> subPanels;
	public List<GameObject> popUpsInfoPanels;
	public UnityEvent OnClickEnable;

	void Awake( ){
		OnClickEnable.AddListener(OnEnableFunction);
	}

	void OnEnableFunction( ){   // AGREGAR CUALQUIER FUNCION QUE QUIERA APENAS SE CLIQUEEE EL  BOTON DEL PANEL
//		Debug.Log(": " +  titlePanel);
	}

	public IEnumerator WaitOnclickEnable( ){
		yield return new WaitForSeconds(.2f);
		OnClickEnable.Invoke( );
	}

	public void SavePanelNavigation( ){

		if (!NavigatorManager.main.enablePopUpInfoPanel) {
			NavigatorManager.main.isTapPanel = false;
			NavigatorManager.main.saveIndex ( index, 0, titlePanel );
		} else {
			NavigatorManager.main.popUpInfoPanelToDesactived.SetActive (false);
			NavigatorManager.main.saveIndex ( index, 0, titlePanel);
		}
	}


	public void _enablePopUpInfoPanel(int indx ){
		popUpsInfoPanels [indx].SetActive(true);
		NavigatorManager.main.popUpInfoPanelToDesactived =  popUpsInfoPanels [indx];
		NavigatorManager.main.enablePopUpInfoPanel = true;
	}

	public void _enablePopUpInfoSubPanel(int indx ){
		popUpsInfoPanels [indx].SetActive(true);
		NavigatorManager.main.popUpInfoSubPanelToDesactived =  popUpsInfoPanels [indx];
		NavigatorManager.main.enableSubPopUpInfoPanel = true;
	}


}

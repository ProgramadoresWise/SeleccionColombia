using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using MaterialUI;
using UnityEngine.UI;
using System.Security.Policy;

public class ButtonsubPanel : MonoBehaviour {

	public bool IgnoredComponentPanel;

	[Space(20)]

	public Image btn_bg;
	public Color dis, enabl, textEna, textDis;
	public TabView P_TabView;

	public Text text_bg;

	public bool addedButtonsDefault;

	public Panel contentPanel;
	public int subIndex;

	public string titleSubpanel;

	public UnityEvent OnClickEnable;





	void Awake( ){	

		OnClickEnable.AddListener(OnEnableFunction);
		ConfugSubButton();

	}


	void OnEnableFunction( ){
		//Debug.Log("--SP: " + titleSubpanel);
	}

	public IEnumerator WaitOnclickEnable( ){
		contentPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
		yield return new WaitForSeconds(.2f);
		contentPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
		DataApp.main.DisableLoading();
		OnClickEnable.Invoke( );
	}

	public void SavePanelNavigation( ){
		NavigatorManager.main.saveIndex (contentPanel.index, subIndex, titleSubpanel);
		P_TabView.SetPage(subIndex,true);
		NavigatorManager.main.isTapPanel = true;
//		NavigatorManager.main.saveIndex (contentPanel.index, subIndex, titleSubpanel);
	
		setImageBtn();

	}

	public void setImageBtn(  ){
		foreach( GameObject p in contentPanel.subPanelsButtons){
			p.GetComponent<ButtonsubPanel>().btn_bg.color = dis;
			p.GetComponent<ButtonsubPanel>().text_bg.color = textDis;
		}
		contentPanel.subPanelsButtons[subIndex].GetComponent<ButtonsubPanel>().btn_bg.color = enabl;
		contentPanel.subPanelsButtons[subIndex].GetComponent<ButtonsubPanel>().text_bg.color = textEna;
	}




	void ConfugSubButton() {
		if (!IgnoredComponentPanel) {
			if (this.name != "ItemPrefab" ) {
				if( addedButtonsDefault){
					this.GetComponent<TabItem> ().id = subIndex;
					contentPanel.subPanelsButtons.Add (this.gameObject);
					subIndex = contentPanel.subPanelsButtons.Count-1;
				}
				contentPanel.subPanelsButtons[NavigatorManager.main.actualSubPanel].GetComponent<ButtonsubPanel>().btn_bg.color = enabl;
				contentPanel.subPanelsButtons[NavigatorManager.main.actualSubPanel].GetComponent<ButtonsubPanel>().text_bg.color = textEna;
			}
		}
	}
}

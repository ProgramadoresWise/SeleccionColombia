using System;
using System.Collections;
using System.Collections.Generic;
using MaterialUI;
using UnityEngine;
using UnityEngine.UI;

public class NavigatorManager : MonoBehaviour
{

    public static NavigatorManager main;

    float TiempoEspera;
    [HideInInspector]
    public Text TituloPanel;
    [HideInInspector]
    public bool isTapPanel;
    //	[HideInInspector]
    public bool enablePopUpInfoPanel, enableSubPopUpInfoPanel;
    //	[HideInInspector]
    public GameObject popUpInfoPanelToDesactived;
    public GameObject popUpInfoSubPanelToDesactived;
    public GoogleAnalyticsV4 GAV4;

    public int actualPanel, actualSubPanel;

    public List<Panel> panelsPrincipales;
    public List<Vector2> historialIndexPanels;
    [SerializeField]
    List<string> titlePanels = new List<string> (1);

    private string getUrlPos = "Analytics/PostSelectMenu.php";
    private int userID = 0;
    private int itemID = 0;

    bool closeApp;

    void Start ()
    {
        //		si si lo estoy seteo la page Panel en actual panel en  0  y actualsubpanel en 0 
        //		if( DataApp.main.IsRegistered() ){
        //			Debug.Log("--------------------- USUARIO EN PROCESO --------------  ID: " + DataApp.main.GetMyID());
        //			actualPanel = 0;
        //			actualSubPanel =0;
        //		}else{
        //			actualPanel = 15;
        //			actualSubPanel =0;
        //		}
        //
        //		historialIndexPanels.Add ( new Vector2( actualPanel,actualSubPanel ));
        //		titlePanels.Add( TituloPanel.text );
        //		openNewPanel();

		GAV4.StartSession();
    }

    void Update ()
    {
        if (Input.GetKeyDown (KeyCode.Escape) && !DataApp.main.loadingPrincipal.activeSelf)
        {

            if (MediaPlayerManager.main._panelVideos.gameObject.activeSelf)
                MediaPlayerManager.main.Exit ();
            // ir desactivando paneles en cascada de prioridad
            if (this.GetComponent<MaterialNavDrawer> ().panelLayer.activeSelf)
            {
                this.GetComponent<MaterialNavDrawer> ().panelLayer.SetActive (false);
                gameObject.GetComponent<MaterialNavDrawer> ().Close ();
                return;
            }

			if (StartPronostico.main.ActivatedPolla  && !enablePopUpInfoPanel)
            {
                StartPronostico.main.habilityPanelGame (false);
            }

            if (DataApp.main.IsActivePopUpInfo)
            {
                DataApp.main.popUpInformative (false, "", "");
                return;
            }

            if (!enablePopUpInfoPanel && !enableSubPopUpInfoPanel && historialIndexPanels.Count > 0)
                DesactivePanel ();
            else if (enablePopUpInfoPanel)
                disablePopUpInfo ();
        }
    }

    public void disablePopUpInfo ()
    {
        DataApp.main.DisableLoading ();
        if (enableSubPopUpInfoPanel)
        {
            enableSubPopUpInfoPanel = false;
            popUpInfoSubPanelToDesactived.SetActive (false);
            return;
        }
        popUpInfoPanelToDesactived.SetActive (false);
        enablePopUpInfoPanel = false;
    }

    void DesactivePanel ()
    {
        StartCoroutine (DisposeDesctive ());
    }

    IEnumerator DisposeDesctive ()
    {
        yield return new WaitForEndOfFrame ();
        if (historialIndexPanels.Count > 0)
        {
            if (historialIndexPanels.Count == 1)
            {
                if (!closeApp)
                {
                    closeApp = true;
                    ToastManager.Show ("Oprime Back de nuevo para salir", 5, null);
                    yield return new WaitForSecondsRealtime (5);
                    closeApp = false;
                }
                else
                {
                    Application.Quit ();
                }
                yield break;
            }

            for (int i = 0; i < panelsPrincipales.Count; i++)
            {
                panelsPrincipales[i].gameObject.SetActive (false);
            }

            if (historialIndexPanels.Count > 1)
                historialIndexPanels.RemoveAt (historialIndexPanels.Count - 1);
				GAV4.LogScreen(titlePanels[titlePanels.Count-1]);

            actualPanel = (int) historialIndexPanels[historialIndexPanels.Count - 1].x;
            actualSubPanel = (int) historialIndexPanels[historialIndexPanels.Count - 1].y;

            if (titlePanels.Count > 1)
                titlePanels.RemoveAt (titlePanels.Count - 1);
            TituloPanel.text = titlePanels[titlePanels.Count - 1];

            if ( /* isTapPanel &&*/ panelsPrincipales[actualPanel].subPanelsButtons.Count > 0 && actualPanel != 0)
            {
                int x = actualPanel;
                int y = actualSubPanel;
                Debug.Log (x + " - " + y);
                if (panelsPrincipales[x].subPanelsButtons[y].gameObject.activeSelf)
                {
                    panelsPrincipales[x].subPanelsButtons[y].gameObject.GetComponent<TabItem> ().tabView.SetPage (y, true);
                    panelsPrincipales[(int) historialIndexPanels[historialIndexPanels.Count - 1].x].subPanelsButtons[(int) historialIndexPanels[historialIndexPanels.Count - 1].y].GetComponent<ButtonsubPanel> ().setImageBtn ();
                }
            }
            openNewPanel ();
        }
    }

    public void ChangeActiveScrollCalendar (bool act)
    {
        calendarData.changeCalendarActive = act;
    }

    public void ChangeCanvas (bool _out, int panelSelect)
    {
        panelsPrincipales[panelSelect].gameObject.SetActive (true);
        StartCoroutine (_TimeLoadCanvas (_out, panelSelect));
    }

    IEnumerator _TimeLoadCanvas (bool _out, int panelSelect)
    {
        CanvasGroup cnvs = panelsPrincipales[panelSelect].GetComponent<CanvasGroup> ();
        if (_out)
        {
            for (float i = 0f; i < 1f; i += 0.1f)
            {
                cnvs.alpha += 0.1f;
                yield return new WaitForSeconds (TiempoEspera);
            }
            cnvs.alpha = 1f;
            cnvs.blocksRaycasts = true;
        }
        else
        {
            for (float i = 0f; i < 1f; i += 0.1f)
            {
                cnvs.alpha -= 0.1f;
                yield return new WaitForSeconds (TiempoEspera);
            }
            cnvs.alpha = 0f;
            cnvs.blocksRaycasts = false;
        }
    }

    void openNewPanel ()
    {
        panelsPrincipales[(int) historialIndexPanels[historialIndexPanels.Count - 1].x].gameObject.SetActive (true);
//        panelsPrincipales[(int) historialIndexPanels[historialIndexPanels.Count - 1].x].GetComponent<CanvasGroup> ().alpha = 1;
//        panelsPrincipales[(int) historialIndexPanels[historialIndexPanels.Count - 1].x].GetComponent<CanvasGroup> ().blocksRaycasts = true;

        if (panelsPrincipales[(int) historialIndexPanels[historialIndexPanels.Count - 1].x].subPanels.Count > 0)
        {
            panelsPrincipales[(int) historialIndexPanels[historialIndexPanels.Count - 1].x].subPanels[(int) historialIndexPanels[historialIndexPanels.Count - 1].y].gameObject.SetActive (true);
            //			if(panelsPrincipales[(int)historialIndexPanels[historialIndexPanels.Count-1].x].subPanels.Count > 0)
            //			panelsPrincipales[(int)historialIndexPanels[historialIndexPanels.Count-1].x].subPanelsButtons[(int)historialIndexPanels[historialIndexPanels.Count-1].y].GetComponent<ButtonsubPanel>().OnClickEnable.Invoke();
            StartCoroutine (panelsPrincipales[(int) historialIndexPanels[historialIndexPanels.Count - 1].x].subPanelsButtons[(int) historialIndexPanels[historialIndexPanels.Count - 1].y].GetComponent<ButtonsubPanel> ().WaitOnclickEnable ());
        }
        //		panelsPrincipales[(int)historialIndexPanels[historialIndexPanels.Count-1].x].OnClickEnable.Invoke();
        StartCoroutine (panelsPrincipales[(int) historialIndexPanels[historialIndexPanels.Count - 1].x].WaitOnclickEnable ());
        TituloPanel.text = titlePanels[titlePanels.Count - 1];
    }

    public void saveIndex (int newpanel, int newsubPane, string tittle)
    {
        //		if( newsubPane >=0 && newsubPane >= 0){
        if (newsubPane != actualSubPanel || newpanel != actualPanel || historialIndexPanels.Count == 0)
        {
            titlePanels.Add (tittle);
            //analitys(tittle);
            actualSubPanel = newsubPane;
            actualPanel = newpanel;
            historialIndexPanels.Add (new Vector2 (actualPanel, actualSubPanel));
            ClearList ();
            openNewPanel ();
        }
        //		}
		//
		GAV4.LogScreen (tittle);
    }

    public void HistorialClear ()
    {
        historialIndexPanels.Clear ();
        titlePanels.Clear ();
    }

    void ClearList ()
    {
        if (!susccesfullEventInback)
        {
            for (int i = 0; i < panelsPrincipales.Count; i++)
            {
                panelsPrincipales[i].gameObject.SetActive (false);
                panelsPrincipales[i].GetComponent<CanvasGroup> ().alpha = 1;
                panelsPrincipales[i].GetComponent<CanvasGroup> ().blocksRaycasts = true;
            }
        }
        else
        {
            for (int i = 0; i < panelsPrincipales.Count; i++)
            {
                panelsPrincipales[i].GetComponent<CanvasGroup> ().alpha = 0;
                panelsPrincipales[i].GetComponent<CanvasGroup> ().blocksRaycasts = false;
            }
        }
    }

    public bool susccesfullEventInback;
    public IEnumerator runEventInBackGround (int panel, int buttonSubPanel)
    {
        susccesfullEventInback = true;
        panelsPrincipales[panel].gameObject.SetActive (true);
//        panelsPrincipales[panel].GetComponent<CanvasGroup> ().alpha = 0;
//        panelsPrincipales[panel].GetComponent<CanvasGroup> ().blocksRaycasts = false;
        yield return StartCoroutine (panelsPrincipales[panel].subPanelsButtons[buttonSubPanel].GetComponent<ButtonsubPanel> ().WaitOnclickEnable ());
        yield return new WaitUntil (() => !susccesfullEventInback);

        yield return new WaitForSecondsRealtime (.1f);
        panelsPrincipales[panel].gameObject.SetActive (false);
//        panelsPrincipales[panel].GetComponent<CanvasGroup> ().blocksRaycasts = true;
//        panelsPrincipales[panel].GetComponent<CanvasGroup> ().alpha = 1;
    }

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MaterialUI;
using LitJson;

public class calendarData : MonoBehaviour{
    [SerializeField]
    private DataRowList calendarList;

    [SerializeField]
    calendarDataList calendarListSlide = new calendarDataList();

    private List<dataMatchCalendar> infoSlide = new List<dataMatchCalendar>();
 
    private WWW getInfo;
    private string actualizar;
    private int actualizarNum;
    private int actualizarInfo;
    public List <GameObject> matchList;
	public List <GameObject> ParentMatch;
    public GameObject calendarSlide;
    public GameObject ContentScrollList;

    public BigViewCalendar bigView;
    private string jsonGuardado;
    public bool canCreate;

	public TabView tabP;
	public ComplementTabView c_tabP;
	bool reload;
	bool loadHome;

	public static bool changeCalendarActive;

    public void LoadCalendar( ) {
		if(!reload)
			DataApp.main.EnableLoading();

        StartCoroutine(DataApp.main.CheckInternet(internet => {
			if (internet){
				loadHome = false;
                StartCoroutine(validarActualizacion( ));
			}else{
				DataApp.main.popUpInformative(true, "Fallo en la conexión.", "Revisa tu conexión a internet.");}
        }));
    }


	public void LoadCalendarHome( ) {
		if(!reload)
			DataApp.main.EnableLoading();
		
		StartCoroutine(DataApp.main.CheckInternet(internet => {
			if (internet){
				loadHome = true;
				StartCoroutine(validarActualizacion( ));
			}else{
				DataApp.main.popUpInformative(true, "Fallo en la conexión.", "Revisa tu conexión a internet.");}
		}));
	}





    void ClearListPrefabs()
    {
        foreach (GameObject g in matchList)
        {
            Destroy(g);
        }


//        infoSlide.Clear();
//        calendarList.dataList.Clear();
    }

    private IEnumerator validarActualizacion() {
		
		getInfo = new WWW(DataApp.main.host + DataApp.main.urlActualizarInfo + (int)idActualizacion.calendario);
        yield return getInfo;
        actualizar = getInfo.text;
        actualizarNum = int.Parse(actualizar);
		if( string.IsNullOrEmpty( getInfo.error ) ){
			StartCoroutine(cargarInfo());
		}else{
			DataApp.main.popUpInformative(true, "Fallo en la conexión.", "Revisa tu conexión a internet.");
		}
        

    }



    private IEnumerator cargarInfo(){
        string url = DataApp.main.host + "Calendario/calendarData.php";

		calendarList.dataList.Clear ();
        if (actualizarNum != DataApp.main.GetMyInfoInt("MyCalendar"))  {
            yield return StartCoroutine(GetJsonDataScript.getJson.GetPhpData(url));
            DataApp.main.SetMyInfo("JsonCalendar", GetJsonDataScript.getJson.GetDataConsult(), 3);
            DataApp.main.SetMyInfo("MyCalendar", actualizar, 1);
			if( DataApp.main.IsRegistered())
				ToastManager.Show("Calendario actualizado", 5f , null);
            actualizarInfo = 1;
        }else{
			if(!loadHome)
				DataApp.main.EnableLoading();
			if(!reload ){
				jsonGuardado = DataApp.main.GetMyInfoString("JsonCalendar");
				yield return StartCoroutine(GetJsonDataScript.getJson.GetLocalData(jsonGuardado));
				actualizarInfo = 0;
			}else{
				DataApp.main.DisableLoading();
				yield break;
			}
           
        }


                
        if (GetJsonDataScript.getJson._state == "Successful")
        {
            calendarList = GetJsonDataScript.getJson.GetData(calendarList, "pos", "name", "iniciales", "marcador", "tipo", "fecha","hora","title","description", "local");
			if(!loadHome)
				StartCoroutine(guardarDatosSolicitados());
			else
				StartCoroutine(calendarHorizontalScroll());
			reload = true;
        }
        else if (GetJsonDataScript.getJson._state == "Warning_01")
        {
            Debug.Log("Mensaje: No se descargaron datos.");
			reload =false;
        }
        else if (GetJsonDataScript.getJson._state == "Warning_02")
        {
            Debug.Log("Mensaje: Fallo en la conexión. Intentelo de nuevo.");
			reload =false;
        }
        else if (GetJsonDataScript.getJson._state == "Warning_03")
        {
            Debug.Log("Mensaje: Datos con error.");

            DataApp.main.SetMyInfo("JsonCalendar", "", 3);

            DataApp.main.SetMyInfo("MyCalendar", "0", 1);
			reload =false;
            StartCoroutine(cargarInfo());
        }

		GetJsonDataScript.getJson._state = "";

    }

	public  IEnumerator guardarDatosSolicitados() {
			DataApp.main.EnableLoading();

		matchList.Clear();
		ClearListPrefabs ();
		TabPage [] temp = new TabPage[0];
        foreach (DataRow obj in calendarList.dataList)  {
            infoSlide.Add(new dataMatchCalendar(int.Parse(obj.GetValueToKey("pos")), obj.GetValueToKey("name"), obj.GetValueToKey("iniciales"), obj.GetValueToKey("marcador"), obj.GetValueToKey("tipo"), obj.GetValueToKey("fecha"), obj.GetValueToKey("hora"), obj.GetValueToKey("title"), obj.GetValueToKey("description"), int.Parse(obj.GetValueToKey("local")), actualizarInfo));          
        }

        canCreate = true;
		int indx=0;
        foreach (dataMatchCalendar tarjeta in infoSlide) {
			
            yield return new WaitUntil(() => canCreate);
			indx ++;
            GameObject nMatch = Instantiate(calendarSlide);
            nMatch.transform.SetParent(ContentScrollList.transform, false);

            canCreate = !canCreate;

            matchList.Add(nMatch); 
            nMatch.gameObject.SetActive(true);
            nMatch.GetComponent<viewCalendar>().posId = tarjeta.pos;
            nMatch.GetComponent<viewCalendar>().nameTeam = tarjeta.nombreEquipo;
            nMatch.GetComponent<viewCalendar>().inicTeam = tarjeta.inicialesEquipo;
            nMatch.GetComponent<viewCalendar>().score.text = tarjeta.marcador;
            nMatch.GetComponent<viewCalendar>().type.text = tarjeta.tipo;
            nMatch.GetComponent<viewCalendar>().dateMatch.text = tarjeta.fecha;
            nMatch.GetComponent<viewCalendar>().timeMatch.text = tarjeta.hora;

            nMatch.GetComponent<viewCalendar>().title.text = tarjeta.titulo;
            nMatch.GetComponent<viewCalendar>().description.text = tarjeta.descripcion;

            nMatch.GetComponent<viewCalendar>().localVisitante = tarjeta.local;

            nMatch.GetComponent<viewCalendar>().actualizar = actualizarInfo;
            StartCoroutine(nMatch.GetComponent<viewCalendar>().LoadImageInit());
			if(indx == 1){
				nMatch.GetComponent<viewCalendar>().titleFinalmatchs.text = "Próximo Partido";
			}
        }

		DataApp.main.DisableLoading();  
		
    }





	public  IEnumerator calendarHorizontalScroll() {

		matchList.Clear();
		ClearListPrefabs ();
		foreach (DataRow obj in calendarList.dataList)  {
			infoSlide.Add(new dataMatchCalendar(int.Parse(obj.GetValueToKey("pos")), obj.GetValueToKey("name"), obj.GetValueToKey("iniciales"), obj.GetValueToKey("marcador"), obj.GetValueToKey("tipo"), obj.GetValueToKey("fecha"), obj.GetValueToKey("hora"), obj.GetValueToKey("title"), obj.GetValueToKey("description"), int.Parse(obj.GetValueToKey("local")), actualizarInfo));          
		}
		canCreate = true;
		int indx=0;
		foreach (dataMatchCalendar tarjeta in infoSlide) {
			yield return new WaitUntil(() => canCreate);
			GameObject nMatch = Instantiate(calendarSlide);
			nMatch.transform.SetParent(ParentMatch[indx].transform, false);
			indx ++;
			canCreate = !canCreate;

			matchList.Add(nMatch); 
			nMatch.gameObject.SetActive(true);
			nMatch.GetComponent<viewCalendar>().posId = tarjeta.pos;
			nMatch.GetComponent<viewCalendar>().nameTeam = tarjeta.nombreEquipo;
			nMatch.GetComponent<viewCalendar>().inicTeam = tarjeta.inicialesEquipo;
			nMatch.GetComponent<viewCalendar>().score.text = tarjeta.marcador;
			nMatch.GetComponent<viewCalendar>().type.text = tarjeta.tipo;
			nMatch.GetComponent<viewCalendar>().dateMatch.text = tarjeta.fecha;
			nMatch.GetComponent<viewCalendar>().timeMatch.text = tarjeta.hora;

			nMatch.GetComponent<viewCalendar>().title.text = tarjeta.titulo;
			nMatch.GetComponent<viewCalendar>().description.text = tarjeta.descripcion;

			nMatch.GetComponent<viewCalendar>().localVisitante = tarjeta.local;

			nMatch.GetComponent<viewCalendar>().actualizar = actualizarInfo;
			StartCoroutine(nMatch.GetComponent<viewCalendar>().LoadImageInit());
			if(indx == 1){
				nMatch.GetComponent<viewCalendar>().titleFinalmatchs.text = "Próximo Partido";
			}
		}

		c_tabP.changeCantScroll(infoSlide.Count);
		DataApp.main.DisableLoading(); 
		NavigatorManager.main.susccesfullEventInback = false;
	}

}

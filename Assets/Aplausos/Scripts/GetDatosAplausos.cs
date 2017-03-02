using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Net;
using System.IO;

public class GetDatosAplausos : MonoBehaviour 
{
	[HeaderAttribute("Pruebas")]
	public Sprite p_Foto;
	public Sprite p_Bandera1;
	public Sprite p_Bandera2;
	public string p_Equipo1;
	public string p_Equipo2;

	[HeaderAttribute("Tab Partido")]
	public Image imgLocal;
	public Image imgVisitante;
	public Text txtLocal;
	public Text txtVisitante;

	public List<PrefabElementoJugador> listPlayersElements;
	public List<GameObject> listPlayersElements_;

	public RectTransform [] contents;
	// Use this for initialization

	public void OnEventClick (bool act) {
		DataApp.main.EnableLoading();
		if(act)
			StartCoroutine(getDatabaseAplausos());
	}

	#region Rellenar Contenidos

	public void RellenarContenidos(){

		clearContents();
		//Rellenar lista de Partido
		int countPartido = 0;
		foreach(DataRow obj in datosList.dataList)
		{
			countPartido++;
			int id = int.Parse(obj.GetValueToKey("idJugador"));
			string nom = obj.GetValueToKey("nombreJugador".ToString());
			int apl = int.Parse(obj.GetValueToKey("aplausosUltimoPartido").ToString());
			float por = retCalculoPorcentaje(apl, TotalAplPartido);
				CrearPrefabJugador( parentScrollPartido, id,countPartido, nom, apl, por, p_Foto);
			
		}

		//Rellenar lista de Acumulado  
		int countHistorial = 0;
		foreach(DataRow obj in datosListAcumulado.dataList){
			countHistorial++;
			int id = int.Parse(obj.GetValueToKey("idJugador"));
			string nom = obj.GetValueToKey("nombreJugador".ToString());
			int apl = int.Parse(obj.GetValueToKey("aplausosHistorial").ToString());
			float por = retCalculoPorcentaje(apl, TotalAplHistorial);
				CrearPrefabJugador( parentScrollAcumulado,id, countHistorial, nom, apl, por, p_Foto);
			
				
		}

		//Rellenar la pestaña de partido
		StartCoroutine(GetFinalMatch());
	

		if( !update ){
			update = true;
		}
	}


	IEnumerator GetFinalMatch( ){
		WWW fn = new WWW( DataApp.main.host + "Fechas/fechas.php?indata=ultimoEquipo");
		yield return fn;
		if( string.IsNullOrEmpty(fn.error)){
			if( fn.text.Contains(".")){
				p_Equipo2 = fn.text.Replace(".","");
				p_Equipo1 = "COLOMBIA";
			}else{
				p_Equipo1 = fn.text;
				p_Equipo2 = "COLOMBIA";
			}
		}


		p_Bandera1 = Resources.Load<Sprite>("Equipos/"+p_Equipo1);



		p_Bandera2 = Resources.Load<Sprite>("Equipos/"+p_Equipo2);
	

		RellenarPartido(p_Bandera1, p_Bandera2, p_Equipo1, p_Equipo2);
	}


	public void RellenarPartido(Sprite banderaLocal, Sprite banderaVisita, string equipoLocal, string equipoVisita)
	{
		imgLocal.sprite = banderaLocal;
		imgVisitante.sprite = banderaVisita;
		txtLocal.text = equipoLocal;
		txtVisitante.text = equipoVisita;
		DataApp.main.DisableLoading();

	}

	#endregion

	#region Jugador

	[HeaderAttribute("Jugador")]
	public GameObject PrefabJugador;
	public Transform parentScrollPartido;
	public Transform parentScrollAcumulado;
	List<GameObject> listaJugadores = new List<GameObject>();
	public string urlFotos;

	public void CrearPrefabJugador(Transform t, int id, int pos, string nombre, int aplausos, float porcentaje, Sprite foto){
		GameObject go = Instantiate(PrefabJugador) as GameObject;
		PrefabElementoJugador p = go.GetComponent<PrefabElementoJugador>();
		go.GetComponent<scrollComponent>().enabled = false;
		go.GetComponent<BoxCollider2D>().enabled = false;
		go.name = id.ToString();
		p.ModificarJugador( id , pos,getApellido(nombre),aplausos,porcentaje,foto,update,"Aplausos");
		p.imgFoto.sprite = ImgLoadManager.main.PlayerImg(p.imgFoto, p.idPlayer.ToString(),false);
//		StartCoroutine( GetFotoJugador(getApellido(nombre).ToLower(), p.imgFoto));
		listaJugadores.Add(go);

		EventTrigger tr = p.imgFoto.GetComponent<EventTrigger>();
		EventTrigger.Entry ee = new EventTrigger.Entry();
		ee.eventID = EventTriggerType.PointerClick;
		ee.callback.AddListener((eventData) => {IrAPerfilDeJugador(id);});
		tr.triggers.Add(ee);
		go.transform.SetParent(t, false);

		listPlayersElements.Add(p);
		listPlayersElements_.Add(go);
	}


	public void UpdateJugador ( PrefabElementoJugador p, int id, int pos, string nombre, int aplausos, float porcentaje, Sprite foto){
//		if(p.idPlayer == id){
			p.ModificarJugador( id , pos,getApellido(nombre),aplausos,porcentaje,foto,update,"Aplausos");
			p.imgFoto.sprite = ImgLoadManager.main.PlayerImg(p.imgFoto, p.idPlayer.ToString(),false);
			EventTrigger tr = p.imgFoto.GetComponent<EventTrigger>();
			EventTrigger.Entry ee = new EventTrigger.Entry();
			ee.eventID = EventTriggerType.PointerClick;
			ee.callback.AddListener((eventData) => {IrAPerfilDeJugador(id);});
			tr.triggers.Add(ee);
//		}	
	}


	public void IrAPerfilDeJugador(int idplayer){
		foreach ( Players pj in SeleccionC.club.PlayersInfo ){
			if( pj._id == idplayer){
				PerfilJugador.main.GetPlayer(pj._id,pj._aplausosUltimoPartidol,pj._totalAplausos,pj._facebookLink,pj._tiwtterLink,pj._instagramLink,pj._snapchatLink);
			}
		}
	}

	public void ResetContents( ){
		clearContents();
		foreach ( RectTransform rt in contents){
			rt.anchoredPosition = new Vector2(rt.anchoredPosition.x ,0 );
		}
	}

	public void clearContents( ){
		foreach ( GameObject gm in listPlayersElements_){
			Destroy(gm);
		}
		listPlayersElements_.Clear();

	}


	public IEnumerator GetFotoJugador(string nombre, Image foto)
	{
		Texture2D tex;
		tex = new Texture2D(4, 4, TextureFormat.RGBA32, false);
		WWW www = new WWW(urlFotos+"/"+nombre+".png");
		yield return www;
		if(www.error == null)
		{		
			www.LoadImageIntoTexture(tex);
			Sprite spt;
			spt = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height), new Vector2(0.5f,0.5f));
			foto.sprite = spt;
		}
		else if(www.error.Contains("404"))
		{
			Debug.LogError("La imagen "+nombre+".png"+ " no existe.");
		}
		//
	}

	#endregion

	#region Calculo

	[HeaderAttribute("Calculo")]
	public List<int> aplsPartido;
	public List<int> aplsHistorial;
	int TotalAplPartido;
	int TotalAplHistorial;

	public float retCalculoPorcentaje(int actual, int total)
	{
		float r;
		r = (float)(actual*100)/total;
		return r;
	}

	public void SumaTotal()
	{
		int tempTotalP = 0;
		int tempTotalH = 0;

		foreach(int n in aplsPartido)
		{
			tempTotalP += n;
		}
		foreach(int n in aplsHistorial)
		{
			tempTotalH += n;
		}

		TotalAplPartido = tempTotalP;
		TotalAplHistorial = tempTotalH;
	}

	public void FillListas()
	{	
		
		foreach (DataRow obj in datosList.dataList)
		{
			aplsPartido.Add(int.Parse((obj.GetValueToKey("aplausosUltimoPartido"))));
		}
		//
		foreach (DataRow obj in datosListAcumulado.dataList)
		{
			aplsHistorial.Add(int.Parse((obj.GetValueToKey("aplausosHistorial"))));
		}
	}

	#endregion

	#region Corrutinas

	[Header("Base de Datos")]
	[SerializeField]
	DataRowList datosList;
	[SerializeField]
	DataRowList datosListAcumulado;

	public bool update;

	public IEnumerator getDatabaseAplausos(){
		
		string url = string.Empty;

		url = DataApp.main.host + "Aplausos/php/getAplausos.php?indata=getAplausos";
	
		yield return StartCoroutine (GetJsonDataScript.getJson.GetPhpData (url));

		string result = GetJsonDataScript.getJson.GetDataConsult();
	
		if( DataApp.main.GetMyInfoString("JsonAplausos") == result &&  listPlayersElements_.Count > 0){
			Debug.Log("SON PINCHES IGAULES CAPULLO");
			DataApp.main.DisableLoading();
			yield break;
		}else{
			DataApp.main.SetMyInfo("JsonAplausos", GetJsonDataScript.getJson.GetDataConsult(), 3);
		}

		if (GetJsonDataScript.getJson._state == "Successful")
		{
			datosList = GetJsonDataScript.getJson.GetData (datosList, "idJugador", "nombreJugador", "aplausosUltimoPartido");
			//Que hacer???
			StartCoroutine(getDatabaseAplausosAcumulado());
		} 
		else if (GetJsonDataScript.getJson._state == "Warning_01") 
		{
			Debug.Log("Mensaje: No se descargaron datos.");
			DataApp.main.popUpInformative(true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
		} 
		else if (GetJsonDataScript.getJson._state == "Warning_02") 
		{
			Debug.Log("Mensaje: Fallo en la conexión. Intentelo de nuevo.");
			DataApp.main.popUpInformative(true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
		}		
	}

	public IEnumerator getDatabaseAplausosAcumulado()
	{
		string url = string.Empty;

		url = DataApp.main.host + "Aplausos/php/getAplausos.php?indata=getAplausosAcu";
		
		yield return StartCoroutine (GetJsonDataScript.getJson.GetPhpData (url));

		string result = GetJsonDataScript.getJson.GetDataConsult();

		if( DataApp.main.GetMyInfoString("JsonAplausosAcu") == result &&  listPlayersElements_.Count > 0 ){
			Debug.Log("SON PINCHES IGAULES CAPULLO");
			DataApp.main.DisableLoading();
			yield break;
		}else{
			DataApp.main.SetMyInfo("JsonAplausosAcu", GetJsonDataScript.getJson.GetDataConsult(), 3);
		}

		if (GetJsonDataScript.getJson._state == "Successful")
		{	

			ResetContents();
			listPlayersElements.Clear();
			listPlayersElements_.Clear();
			aplsPartido.Clear();
			aplsHistorial.Clear();
			
			datosListAcumulado = GetJsonDataScript.getJson.GetData (datosListAcumulado, "idJugador", "nombreJugador", "aplausosHistorial");
			//Que hacer???

			FillListas();
			SumaTotal();
			RellenarContenidos();

		} 
		else if (GetJsonDataScript.getJson._state == "Warning_01") 
		{
			Debug.Log("Mensaje: No se descargaron datos.");
			DataApp.main.popUpInformative(true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
		} 
		else if (GetJsonDataScript.getJson._state == "Warning_02") 
		{
			Debug.Log("Mensaje: Fallo en la conexión. Intentelo de nuevo.");
			DataApp.main.popUpInformative(true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
		}		
	}

	#endregion

	#region Utilidades
	
	string getApellido(string n)
	{
		string[] nn = n.Split(' ');

		return nn[1];
	}

	#endregion
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using MaterialUI;
using UnityEngine.Experimental.Director;
using System.Linq;
using System.Linq.Expressions;

public class GetTuEliges : MonoBehaviour 
{
	[HeaderAttribute("Pruebas")]
	public Sprite p_SpriteElemento;

	[Header("Elementos")]
	public GameObject prefabElementoVotacion;
	public GameObject prefabElementoResultados;
	public GameObject prefabTotalVotos;
	public Transform transformListaVotacion;
	public Transform transformListaResultados;
	public List<ElementoVotacion> listElementosVotacion = new List<ElementoVotacion>();
	public List<PrefabElementoJugador> listElementosResultados = new List<PrefabElementoJugador>();
	string pregunta;
	[SpaceAttribute(10)]
	public Text txtPregunta;

	[HeaderAttribute("Calculos")]
	List<int> listVotos = new List<int>();
	int totalVotos;

	[HeaderAttribute("Reloj")]
	public Text txtDias;
	public Text txtHoras;
	public Text txtMinutos;
	DateTime fechaFinal;
	TimeSpan restante;
	DateTime hoy;
	//
	float counterMiraReloj = 0;
	public float delayMiraReloj = 15;
	float tempDelayMiraReloj = 0;
	bool miraReloj;
	GameObject panelTotalVotes;

	bool reload;
	public bool update;

	[Header("Base de Datos")]
	public string urlAcceso;
	public string funcion;
	public string funcionAddVoto;
	public string funcionRemVoto;
	[SerializeField]
	DataRowList datosList;

	void Awake( ){
		if( !PlayerPrefs.HasKey("myVotesTuelijes") ){
			StartCoroutine(dpwnloadVotes());
		}
	}


	public void LoadYourOpinion( ) {
		if(!reload){
			DataApp.main.EnableLoading();
			StartCoroutine (UpdateYpurOpinion());
		}
	}

	IEnumerator UpdateYpurOpinion(  ){
		WWW updNew = new WWW(DataApp.main.host + DataApp.main.urlActualizarInfo + (int)idActualizacion.tuElijes );
		yield return updNew;
		if( string.IsNullOrEmpty( updNew.error ) ){
			
			StartCoroutine(getDatabaseTuEliges( int.Parse(updNew.text)));
			tempDelayMiraReloj = delayMiraReloj;
		}else{
			DataApp.main.popUpInformative(true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
		}

	}

	// Update is called once per frame
	void Update ( ){
		if(miraReloj) {
			tempDelayMiraReloj -= Time.deltaTime;
			if(tempDelayMiraReloj <= 0) {
				DiffFechaLimite();
				tempDelayMiraReloj = delayMiraReloj;
			}
		}
	}

	#region Funciones
	
	public void CrearElementosVotacion()
	{

		foreach ( ElementoVotacion el in listElementosVotacion){
			Destroy(el.gameObject);
		}

		listVotos.Clear();
		listElementosVotacion.Clear();

		foreach (DataRow d in datosList.dataList)
		{
			if(int.Parse(d.GetValueToKey("idElemento")) < 100)
			{
				GameObject go = Instantiate(prefabElementoVotacion) as GameObject;
				go.transform.SetParent(transformListaVotacion,false);
				//
				ElementoVotacion e = go.GetComponent<ElementoVotacion>();

				e.SetMain(this);
				e.GenerarContenidoElemento(d.GetValueToKey("idElemento"),p_SpriteElemento, d.GetValueToKey("nombreElemento"), d.GetValueToKey("votos"));
				e._votos =  int.Parse(d.GetValueToKey("votos"));
				listElementosVotacion.Add(e);
				listVotos.Add(int.Parse(d.GetValueToKey("votos")));
			}
			else if(int.Parse(d.GetValueToKey("idElemento")) == 100)
			{
				fechaFinal = DateTime.Parse(d.GetValueToKey("fecha"));
			}
			else if(int.Parse(d.GetValueToKey("idElemento")) == 101)
			{
				pregunta = d.GetValueToKey("nombreElemento").ToString();
			}
		}
	
		setTooglesVotation();

		SumarResultados();
	}

	public void CrearElementosResultados()
	{

		foreach ( PrefabElementoJugador el in listElementosResultados){
			Destroy(el.gameObject);
		}

		Destroy(panelTotalVotes);
		listElementosResultados.Clear();
		int countResultados = 0;
		int total = 0;
		foreach (DataRow d in datosList.dataList)
		{
			if(int.Parse(d.GetValueToKey("idElemento")) < 100)
			{
				
				GameObject go = Instantiate(prefabElementoResultados) as GameObject;
				go.transform.SetParent(transformListaResultados,false);
				//
				PrefabElementoJugador e = go.GetComponent<PrefabElementoJugador>();

				string nom = d.GetValueToKey("nombreElemento".ToString());
				int vot = int.Parse(d.GetValueToKey("votos").ToString());
				float por = retCalculoPorcentaje(vot, totalVotos);
				string nomFoto = d.GetValueToKey("nombreFotoFTP".ToString());
				e.ModificarJugador(int.Parse(d.GetValueToKey("idElemento")), countResultados, getApellido(nom, false), vot, por, p_SpriteElemento,update,"TuElijes");
				total += vot;
				listElementosResultados.Add(e);
			}
		}

		listElementosResultados.Sort( (PrefabElementoJugador A, PrefabElementoJugador B ) => B.votes.CompareTo(A.votes));
		foreach( PrefabElementoJugador gm in listElementosResultados){
			gm.gameObject.transform.SetParent(gm.transform.parent.parent);
		}

		foreach( PrefabElementoJugador gm in listElementosResultados){
			countResultados++;
			gm.gameObject.transform.SetParent(transformListaResultados);
			gm.txtPosicion.text = countResultados.ToString();
		}

		GameObject g = Instantiate(prefabTotalVotos) as GameObject;
		panelTotalVotes = g;
		g.transform.SetParent(transformListaResultados,false);
		g.transform.GetChild(0).GetComponent<Text>().text = total.ToString();

		DiffFechaLimite();

		update = false;
	}

	public void SumarResultados()
	{
		totalVotos = 0;

		foreach(int i in listVotos)
		{
			totalVotos += i;
		}
	}

	#endregion

	#region Base de Datos

	public IEnumerator getDatabaseTuEliges(int numAct) {
		
		if( DataApp.main.GetMyInfoInt("TuElijes") !=  numAct){
			update = true;
			DataApp.main.SetMyInfo("TuElijes",numAct.ToString(),1);
			if( DataApp.main.IsRegistered())
				ToastManager.Show("Tu Elijes ha sido actualizada.",2f,null);
		}


		string url = string.Empty;
		url = urlAcceso+"?indata="+funcion;

			DataApp.main.EnableLoading();
			yield return StartCoroutine (GetJsonDataScript.getJson.GetPhpData (url));
			DataApp.main.SetMyInfo("JsonyourOpinion", GetJsonDataScript.getJson.GetDataConsult(), 3);
//			ToastManager.Show("Tu opinión Actualizada",5f,null);
			DataApp.main.DisableLoading();


		if (GetJsonDataScript.getJson._state == "Successful")
		{
			datosList = GetJsonDataScript.getJson.GetData (datosList, "idElemento", "nombreElemento", "votos", "fecha");
			reload = true;
			CrearElementosVotacion();
			CrearElementosResultados();
			SetPregunta();
		} 
		else if (GetJsonDataScript.getJson._state == "Warning_01") 
		{
			Debug.Log("Mensaje: No se descargaron datos.");
		} 
		else if (GetJsonDataScript.getJson._state == "Warning_02") 
		{
			Debug.Log("Mensaje: Fallo en la conexión. Intentelo de nuevo.");
		}
		else if (GetJsonDataScript.getJson._state == "Warning_03"){
			Debug.Log("Mensaje: Datos con error.");
			StartCoroutine(getDatabaseTuEliges(numAct));
		}
		GetJsonDataScript.getJson._state = "";
		DataApp.main.DisableLoading();	
	}

	public IEnumerator SendVoto(ElementoVotacion element, string id, bool isOn){
		if( !reload ){
			if( User.main.GetMyEmailVerif( ) == 1 ){
				if(isOn){
					string url = string.Empty;
					url = urlAcceso+"?indata="+funcionAddVoto+"&var="+id;
					WWW netCall = new WWW(url);
					yield return netCall;
					if(netCall.text == "true"){
						Debug.Log("Se añadio exitosamente el voto al elemento "+id);
					}
				}else{
					string url = string.Empty;
					url = urlAcceso+"?indata="+funcionRemVoto+"&var="+id;
					WWW netCall = new WWW(url);
					yield return netCall;
					if(netCall.text == "true"){
						Debug.Log("Se removio exitosamente el voto del elemento "+id);
					}	
				}
				saveVotes(int.Parse(id),isOn);


			}else if( isOn == true ){
				element.toggleVotacion.isOn = false;
				if(!element.toggleVotacion.isOn){
					SnackbarManager.Show("Debes verificar tu cuenta desde tu E-mail",10f, "Re-Enviar Correo", () => {
						DataApp.main.EnableLoading();
						SnackbarManager.OnSnackbarCompleted();
						RegisterLoginManager.main.SendEmailto( User.main.GetMyEmail() , User.main.GetMyToken(), true );
						//				SnackbarManager.OnSnackbarCompleted();
					});
				}

			}
		}
	}

	#endregion

	#region Calculos

	public float retCalculoPorcentaje(int actual, int total)
	{
		float r;
		r = (float)(actual*100)/total;
		return r;
	}

	public void DiffFechaLimite()
	{
		hoy = DateTime.Now;
		restante = fechaFinal.Subtract(hoy);

		txtDias.text = ( restante.Days > 0 )?restante.Days.ToString(): "00";
		txtHoras.text = ( restante.Hours>0 )?restante.Hours.ToString(): "00";
		txtMinutos.text = (restante.Minutes>0 )? restante.Minutes.ToString():"00";

		miraReloj = true;
	}

	#endregion

	#region Utilidades
	
	string getApellido(string n, bool conNombre)
	{
		string[] nn = n.Split(' ');

		if(conNombre)
		{
			return (nn[0][0]+". "+nn[1]);
		}
		else
		{
			return (nn[1]);
		}
	}

	void SetPregunta()
	{
		txtPregunta.text = pregunta;
	}

	#endregion


	void saveVotes( int v , bool plus){
		if( plus ){
			if( !PlayerPrefs.GetString("myVotesTuelijes").Contains(v.ToString())){
				string myvotes = PlayerPrefs.GetString("myVotesTuelijes");
				myvotes += v+"-";
				PlayerPrefs.SetString("myVotesTuelijes",myvotes);
			}
		}else{
			string myvotes = PlayerPrefs.GetString("myVotesTuelijes");
			myvotes = myvotes.Replace(v.ToString()+"-","");
			PlayerPrefs.SetString("myVotesTuelijes",myvotes);
		}
		ToastManager.Show("Voto registrado con éxito",5f,null);
		StartCoroutine(uploadVotes());
	}


	void setTooglesVotation(  ){
		reload = true;
		string[] nn = PlayerPrefs.GetString("myVotesTuelijes").Split('-');
			foreach( string n in nn ){
			for(int i=0; i< listElementosVotacion.Count ;i++){
				if(listElementosVotacion[i].idElemento == n){
						listElementosVotacion[i].toggleVotacion.isOn = true;
					}
				}
			}
		reload = false;
		if(listElementosVotacion.Count>0){
			listElementosVotacion = listElementosVotacion.OrderBy( x => x.GetComponent<ElementoVotacion>()._votos).ToList();
		}
	}

	IEnumerator dpwnloadVotes( ){
			WWW myvotes = new WWW( DataApp.main.host + "TuEliges/php/getTuEliges.php?indata=votosTuElijes&id=" + DataApp.main.GetMyID());
			yield return myvotes;
			if( string.IsNullOrEmpty( myvotes.error ) && !string.IsNullOrEmpty( myvotes.text )){
				PlayerPrefs.SetString("myVotesTuelijes",myvotes.text);
			} 
		}



	IEnumerator uploadVotes( ){
		WWW myvotes = new WWW( DataApp.main.host + "TuEliges/php/getTuEliges.php?indata=votosTuElijesUpdate&id=" + DataApp.main.GetMyID() + "&cantvotos=" +  PlayerPrefs.GetString("myVotesTuelijes"));
		yield return myvotes;
		if( string.IsNullOrEmpty( myvotes.error ) && !string.IsNullOrEmpty( myvotes.text )){
			PlayerPrefs.SetString("myVotesTuelijes",myvotes.text);
		} 
		LoadYourOpinion();

	}

}

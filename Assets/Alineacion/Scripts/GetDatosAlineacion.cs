using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MaterialUI;
using UnityEngine.EventSystems;

public class GetDatosAlineacion : MonoBehaviour 
{
    [Header ("Pruebas")]
    public Modo modo;
    public Sprite p_nJugadorFoto;

    [Header ("Jugadores")]
    public List<Jugador> jugadoresTitulares;
    public List<Jugador> jugadoresSuplentes;
    public List<JugadorAlineacion> jugadoresAlineacion;

    [Header ("Objetos")]
    public Text txtDirectorTecnico;
    public Image imgDirectorTecnico;
    public List<Text> txtNombresTitulares;
    public List<Text> txtNombresSuplentes;
    public List<Image> imgFotosTitulares;
    public List<Image> imgFotosSuplentes;

    [HeaderAttribute ("Datos del Partido")]
    public Text txtFecha;
    public Text txtHora;
    public Text txtPartido;
    public Text txtRival;
	public Text txtLocal;
    public Image imgRival;
	public Image imgLocal;

    [Header ("Alineacion")]
    public PosicionAlineacion lineasAlineacion;

    [Header ("Base de Datos")]
    public string dominio;
    public string acceso;
    public string funcionAlineacion;
    public string funcionEventos;
    public string funcionDatosPartido;

    [SerializeField]
    private DataRowList jugadoresList;
    [SerializeField]
    private DataRowList eventosList;
    [SerializeField]
    private DataRowList datosList;

    [Header ("Actualizar")]
    public Transform rectContenido;
    public GameObject loadingScreen;

    // Use this for initialization

    bool reload, update, refresh;

    void Update ()
    {
        if (Input.GetMouseButtonUp (0) && rectContenido.gameObject.GetComponent<RectTransform> ().anchoredPosition.y < -200)
        {
            GetActualizacion ();
            //			if( jugadoresList.dataList.Count <= 0)
            //				loadAlineacion();
        }
    }

    public void loadAlineacion ()
    {

        if (!reload)
            DataApp.main.EnableLoading ();

        StartCoroutine (DataApp.main.CheckInternet (internet =>
        {
            if (internet)
            {
                StartCoroutine (UpdateTemplate ());
            }
            else
            {
                DataApp.main.popUpInformative (true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
            }
        }));
    }

    IEnumerator UpdateTemplate ()
    {
        WWW updNew = new WWW (DataApp.main.host + "Alineacion/ActualizarInfo.php");
        yield return updNew;
        if (string.IsNullOrEmpty (updNew.error))
        {
            OnEventClick (int.Parse (updNew.text));
        }
        else
        {
            DataApp.main.popUpInformative (true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
        }
    }

    public void OnEventClick (int num)
    {
        //		VerDatosSolicitados();
        StartCoroutine (getDatabaseJugadores (num));
        if (!reload)
        {
            jugadoresTitulares = new List<Jugador> ();
            jugadoresSuplentes = new List<Jugador> ();
            jugadoresAlineacion = new List<JugadorAlineacion> ();

            //Se limpian las lineas de posicion de la alineacion
            foreach (Transform t in lineasAlineacion.arquero)
            {
                Destroy (t.gameObject);
            }
            foreach (Transform t in lineasAlineacion.defensa)
            {
                Destroy (t.gameObject);
            }
            foreach (Transform t in lineasAlineacion.medio)
            {
                Destroy (t.gameObject);
            }
            foreach (Transform t in lineasAlineacion.delantera)
            {
                Destroy (t.gameObject);
            }
        }
    }

    //	void OnEnable()
    //	{
    //			
    //	}

    #region Funciones de llamado de datos

    public void GetActualizacion ()
    {
        refresh = true;
        StartCoroutine (getDatabaseJugadores (DataApp.main.GetMyInfoInt ("Template")));
        //		ToastManager.Show("Actualizando minuto a minuto",2f,null);

        //		SnackbarManager.Show("Correo enviado,\nDeseas verificar tu correo electronico?",10f, "Verificar", () => {
        //								ToastManager.Show("Aqui Abro el navegador",15f,null);
        //								Application.OpenURL("www.gmail.com");
        //							});
    }

    public void GetEventos ()
    {
        StartCoroutine (getDatabaseEventos ());
        ToastManager.Show ("Actualizado", 2f, null);
    }

    public void GetDatos ()
    {
        StartCoroutine (getDatabasePartido ());
    }

    #endregion

    #region Funciones de organizacion de jugadores

    public void SetAlineacion ()
    {
        //Se limpian las lineas de posicion de la alineacion
        foreach (Transform t in lineasAlineacion.arquero)
        {
            Destroy (t.gameObject);
        }
        foreach (Transform t in lineasAlineacion.defensa)
        {
            Destroy (t.gameObject);
        }
        foreach (Transform t in lineasAlineacion.medio)
        {
            Destroy (t.gameObject);
        }
        foreach (Transform t in lineasAlineacion.delantera)
        {
            Destroy (t.gameObject);
        }

        //Se agregan los jugadores por linea
        foreach (JugadorAlineacion j in jugadoresAlineacion)
        {
            string linea;
            linea = GetLineaAlineacion (j.posicion);

            switch (linea)
            {
                case "A":
                    j.transform.SetParent (lineasAlineacion.arquero, false);
                    break;
                case "B":
                    j.transform.SetParent (lineasAlineacion.defensa, false);
                    break;
                case "C":
                    j.transform.SetParent (lineasAlineacion.medio, false);
                    break;
                case "D":
                    j.transform.SetParent (lineasAlineacion.delantera, false);
                    break;
            }
        }

        //Organizar los jugadores dependiendo su posicion	
        foreach (JugadorAlineacion j in jugadoresAlineacion)
        {
            int pos;
            pos = GetPosicionAlineacion (j.posicion);
            j.transform.SetSiblingIndex (pos - 1);
        }
    }

    public void SetTitular ()
    {
        foreach (Jugador j in jugadoresTitulares)
        {
            Jugador jg = txtNombresTitulares[jugadoresTitulares.IndexOf (j)].transform.parent.GetComponent<Jugador> ();
            //
            txtNombresTitulares[jugadoresTitulares.IndexOf (j)].text = GetNombreCorto (j.nombre);
            imgFotosTitulares[jugadoresTitulares.IndexOf (j)].sprite = j.foto;
            j.transform.SetParent(txtNombresTitulares[jugadoresTitulares.IndexOf (j)].transform.parent);

            jg.nombre = j.nombre;
            jg.numero = j.numero;
            jg.fotoJugador.sprite = ImgLoadManager.main.PlayerImg (jg.fotoJugador, jg.numero, false);
        }
    }

    public void SetSuplencia ()
    {
        foreach (Jugador j in jugadoresSuplentes)
        {
            Jugador jg = txtNombresSuplentes[jugadoresSuplentes.IndexOf (j)].transform.parent.GetComponent<Jugador> ();

            if (!jg.gameObject.activeInHierarchy)
            {
                jg.gameObject.SetActive (true);
            }

            txtNombresSuplentes[jugadoresSuplentes.IndexOf (j)].text = GetNombreCorto (j.nombre);
            imgFotosSuplentes[jugadoresSuplentes.IndexOf (j)].sprite = j.foto;
            j.transform.SetParent(txtNombresSuplentes[jugadoresSuplentes.IndexOf (j)].transform.parent, false);

            jg.nombre = j.nombre;
            jg.numero = j.numero;
            //			jg.foto = j.foto;
            jg.fotoJugador.sprite = ImgLoadManager.main.PlayerImg (jg.fotoJugador, jg.numero, false);
        }
    }

    public void SetEventos ()
    {
        foreach (Jugador j in jugadoresTitulares)
        {
            j.transform.parent.GetComponent<Jugador> ().eventos.Clear ();
            j.transform.parent.GetComponent<Jugador> ().objetoEventos.SetActive (false);
            foreach (Transform gm in j.transform.parent.GetComponent<Jugador> ().objetoEventos.transform)
            {
                Destroy (gm.gameObject);
            }
        }

        if (jugadoresList.dataList.Count > 0)
        {
            foreach (DataRow obj in eventosList.dataList)
            {
                foreach (Jugador j in jugadoresTitulares)
                {
                    if (obj.GetValueToKey ("idJugador").ToString () == j.numero)
                    {
                        j.transform.parent.GetComponent<Jugador> ().CrearEvento (obj.GetValueToKey ("tipoEvento").ToString (), obj.GetValueToKey ("minEvento").ToString ());
                    }
                }
            }
        }
        GetDatos ();
    }

    public void SetDatos ()
    {
        foreach (DataRow obj in datosList.dataList)
        {
            if (obj.GetValueToKey ("nombreFecha").ToString ().Contains ("proxima") || obj.GetValueToKey ("nombreFecha").ToString ().Contains ("actual"))
            {
				bool colombiaIsLocal =  obj.GetValueToKey ("equipo").Contains(".");
				string rival = obj.GetValueToKey ("equipo").Replace(".","");
				if(colombiaIsLocal ){
					txtLocal.text = "COLOMBIA";
					txtRival.text = obj.GetValueToKey ("equipo").ToString ();
					imgRival.sprite = Resources.Load<Sprite>("Equipos/"+ rival.ToLower());
					imgLocal.sprite = Resources.Load<Sprite>("Equipos/colombia");
				}else{
					txtRival.text = "COLOMBIA";
					txtLocal.text = obj.GetValueToKey ("equipo").ToString ();
					imgLocal.sprite = Resources.Load<Sprite>("Equipos/"+ rival.ToLower());
					imgRival.sprite = Resources.Load<Sprite>("Equipos/colombia");
				}
                txtFecha.text = obj.GetValueToKey ("fechaApertura").ToString ();
                txtHora.text = obj.GetValueToKey ("horaApertura").ToString ();
                
			
            }
        }
        DataApp.main.DisableLoading ();
        //		loadingScreen.SetActive(false);
    }

    #endregion

    #region Funciones Individuales

    public void AddJugadorTitular (string nom, string num, Sprite foto)
    {
        GameObject nJugador = new GameObject ();
        Jugador compJugador;

        nJugador.AddComponent<Jugador> ();
        nJugador.AddComponent<Image> ();
        compJugador = nJugador.GetComponent<Jugador> ();

        compJugador.nombre = nom;
        compJugador.numero = num;
        compJugador.foto = foto;
        //		compJugador.foto =  ImgLoadManager.main.PlayerImg(compJugador.fotoJugador ,compJugador.numero,update);
        jugadoresTitulares.Add (compJugador);
    }

    public void AddJugadorSuplente (string nom, string num, Sprite foto)
    {
        GameObject nJugador = new GameObject ();
        Jugador compJugador;

        nJugador.AddComponent<Jugador> ();
        nJugador.AddComponent<Image> ();
        compJugador = nJugador.GetComponent<Jugador> ();

        compJugador.nombre = nom;
        compJugador.numero = num;
        compJugador.foto = foto;
        //		compJugador.foto = ImgLoadManager.main.PlayerImg(compJugador.fotoJugador ,compJugador.numero,update);
        jugadoresSuplentes.Add (compJugador);
    }

    public void AddJugadorAlineacion (string id, string pos, Sprite foto)
    {
        GameObject nJugador = new GameObject ();
        JugadorAlineacion compJugador;
        Image imgJugador;
        EventTrigger eventT; 

        nJugador.name = "Jugador" + pos;

        nJugador.AddComponent<JugadorAlineacion> ();
        nJugador.AddComponent<Image> ();
        nJugador.AddComponent<EventTrigger>();
        compJugador = nJugador.GetComponent<JugadorAlineacion> ();
        imgJugador = nJugador.GetComponent<Image> ();
        eventT = nJugador.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        //
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventdata) => {GetComponent<EventosClick>().IrAPerfilJugador(id);});
        eventT.triggers.Add(entry);
        //
        compJugador.posicion = pos;
        compJugador.foto = foto;
        imgJugador.sprite = ImgLoadManager.main.PlayerImg (imgJugador, id, update);
        imgJugador.type = Image.Type.Simple;
        imgJugador.preserveAspect = true;

        jugadoresAlineacion.Add (compJugador);
    }

    #endregion

    #region Utilidades

    string GetNombreCorto (string n)
    {
        string nNom;
        string[] aNom;
        aNom = n.Split (' ');

        nNom = aNom[0] [0] + ". " + aNom[1];

        return nNom;
    }

    string GetLineaAlineacion (string pos)
    {
        string li;
        char[] a = pos.ToCharArray ();

        li = a[0].ToString ();

        return li;
    }

    int GetPosicionAlineacion (string pos)
    {
        int po;
        char[] a = pos.ToCharArray ();

        po = int.Parse (a[1].ToString ());

        return po;
    }

    #endregion

    #region Clases

    [System.Serializable]
    public class PosicionAlineacion
    {
        public Transform arquero;
        public Transform defensa;
        public Transform medio;
        public Transform delantera;
    }

    private void VerDatosSolicitados ()
    {

        //		if(!update){
        foreach (DataRow obj in jugadoresList.dataList)
        {
            if (GetLineaAlineacion (obj.GetValueToKey ("posJugadorAlineacion").ToString ()) != "S")
            {
                AddJugadorTitular (obj.GetValueToKey ("nombreJugador").ToString (), obj.GetValueToKey ("idJugador").ToString (), p_nJugadorFoto);
                AddJugadorAlineacion (obj.GetValueToKey ("idJugador"), obj.GetValueToKey ("posJugadorAlineacion").ToString (), p_nJugadorFoto);
            }
            else
            {
                AddJugadorSuplente (obj.GetValueToKey ("nombreJugador").ToString (), obj.GetValueToKey ("idJugador").ToString (), p_nJugadorFoto);
            }
        }
        //

        //		}

        SetTitular ();
        SetAlineacion ();
        SetSuplencia ();
        GetEventos ();
        //		
    }

    #endregion

    #region Corrutinas

    public IEnumerator getDatabaseJugadores (int NumAct)
    {
        string url = string.Empty;

        if (modo == Modo.Pruebas)
        {
            url = dominio + "/~fcf2waysports/2waysports/Colombia/" + acceso + "/php/getJugadores.php?indata=" + funcionAlineacion;
        }

        if (DataApp.main.GetMyInfoInt ("Template") != NumAct)
        {
            DataApp.main.EnableLoading ();
            yield return StartCoroutine (GetJsonDataScript.getJson.GetPhpData (url));
            DataApp.main.SetMyInfo ("JsonTemplate", GetJsonDataScript.getJson.GetDataConsult (), 3);
            if (DataApp.main.IsRegistered ())
                ToastManager.Show ("Alineación Actualizada", 2f, null);
            update = true;
//            DataApp.main.DisableLoading ();
        }
        else
        {
            if (!reload)
            {
                DataApp.main.EnableLoading ();
                string jsonGuardado = DataApp.main.GetMyInfoString ("JsonTemplate");
                yield return StartCoroutine (GetJsonDataScript.getJson.GetLocalData (jsonGuardado));
                update = false;
            }
            else
            {
                if (refresh)
                    GetEventos ();

                refresh = false;

                yield break;
            }
        }

        if (!reload)
        {
            foreach (Text txt in txtNombresSuplentes)
            {
                txt.transform.parent.gameObject.SetActive (false);
            }

            jugadoresTitulares.Clear ();
            jugadoresSuplentes.Clear ();
            jugadoresAlineacion.Clear ();
        }

        if (GetJsonDataScript.getJson._state == "Successful")
        {
            jugadoresList = GetJsonDataScript.getJson.GetData (jugadoresList, "idJugador", "nombreJugador", "posJugadorAlineacion");
            DataApp.main.SetMyInfo ("Template", NumAct.ToString (), 1);
            VerDatosSolicitados ();
            reload = true;
        }
        else if (GetJsonDataScript.getJson._state == "Warning_01")
        {
            Debug.Log ("Mensaje: No se descargaron datos.");
            reload = false;
        }
        else if (GetJsonDataScript.getJson._state == "Warning_02")
        {
            Debug.Log ("Mensaje: Fallo en la conexión. Intentelo de nuevo.");
            reload = false;
        }
        else if (GetJsonDataScript.getJson._state == "Warning_03")
        {
            Debug.Log ("Mensaje: Datos con error.");
            DataApp.main.SetMyInfo ("JsonTemplate", "", 3);
            DataApp.main.SetMyInfo ("Template", "0", 1);
            reload = false;
            StartCoroutine (getDatabaseJugadores (NumAct));
        }
        GetJsonDataScript.getJson._state = "";
    }

    public IEnumerator getDatabaseEventos ()
    {
        //		if(!loadingScreen.activeInHierarchy)
        //		{
        //			loadingScreen.SetActive(true);
        //		}

        string url = string.Empty;

        if (modo == Modo.Pruebas)
        {
            url = dominio + "/~fcf2waysports/2waysports/Colombia/" + acceso + "/php/getJugadores.php?indata=" + funcionEventos;
        }
        else
        {

        }

        yield return StartCoroutine (GetJsonDataScript.getJson.GetPhpData (url));

        if (GetJsonDataScript.getJson._state == "Successful")
        {
            eventosList = GetJsonDataScript.getJson.GetData (eventosList, "idJugador", "tipoEvento", "minEvento");
            SetEventos ();
        }
        else if (GetJsonDataScript.getJson._state == "Warning_01")
        {
            Debug.Log ("Mensaje: No se descargaron datos.");
            DataApp.main.DisableLoading ();
            //			loadingScreen.SetActive(false);
        }
        else if (GetJsonDataScript.getJson._state == "Warning_02")
        {
            Debug.Log ("Mensaje: Fallo en la conexión. Intentelo de nuevo.");
            DataApp.main.DisableLoading ();
            //			loadingScreen.SetActive(false);
        }
    }

    public IEnumerator getDatabasePartido ()
    {

        string url = string.Empty;

        if (modo == Modo.Pruebas)
        {
            url = dominio + "/~fcf2waysports/2waysports/Colombia/" + acceso + "/php/getJugadores.php?indata=" + funcionDatosPartido;
        }
        else
        {

        }

        yield return StartCoroutine (GetJsonDataScript.getJson.GetPhpData (url));

        if (GetJsonDataScript.getJson._state == "Successful")
        {
            datosList = GetJsonDataScript.getJson.GetData (datosList, "nombreFecha", "fechaApertura", "horaApertura", "equipo");
            SetDatos ();
        }
        else if (GetJsonDataScript.getJson._state == "Warning_01")
        {
			DataApp.main.DisableLoading ();
            DataApp.main.popUpInformative (true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
        }
        else if (GetJsonDataScript.getJson._state == "Warning_02")
        {
            DataApp.main.DisableLoading ();
            DataApp.main.popUpInformative (true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
        }
    }

    #endregion

    #region Enums

    public enum Modo
    {
        Produccion,
        Pruebas
    }

    #endregion
}
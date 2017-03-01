using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[System.Serializable]
public class Jugador : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{

    [Header ("Informacion del Jugador")]
    public string nombre;
    public string numero;
    public Sprite foto;

    [Header ("Detalles")]
    public List<Detalle> eventos;
    public GameObject prefabEvento;
    public Sprite[] imgsTipoDeEventos;

    [Header ("Elementos del prefab")]
    public GameObject objetoEventos;
    public Text nombreJugador;
    public Image fotoJugador;

    [HeaderAttribute ("Click")]
    public EventosClick evClick;

    // Use this for initialization
    void Start ()
    {
        //Inicializa las variables necesarias
        eventos = new List<Detalle> ();
        eventos.Clear ();

        //Si no existen eventos del jugador, se desactiva la casilla de eventos.
        if (eventos.Count <= 0)
        {
            if (objetoEventos)
            {
                objetoEventos.SetActive (false);
            }
        }
    }

    #region Funciones de informacion de jugador

    public void CrearEvento (string t, string m)
    {

        //Destruye todos los eventos existentes en el contenedor.
        if (objetoEventos)
        {
            objetoEventos.SetActive (true);
        }

        //Crea el nuevo evento.
        if (eventos.Count < 3)
        {
            eventos.Add (new Detalle ());
        }
        else
        {
            Debug.LogWarning ("Maximo de eventos alcanzado");
        }

        eventos[eventos.Count - 1].tipo = t;
        eventos[eventos.Count - 1].minuto = m;

        //Instancia los eventos en el contenedor.
        if (objetoEventos)
        {
            foreach (Transform tr in objetoEventos.transform)
            {
                Destroy (tr.gameObject);
            }
            //
            foreach (Detalle e in eventos)
            {
                GameObject go = Instantiate (prefabEvento, objetoEventos.transform) as GameObject;
                int type = int.Parse (e.tipo) - 1;

                go.GetComponentInChildren<Image> ().sprite = imgsTipoDeEventos[type];

                go.GetComponentInChildren<Text> ().text = e.minuto + "'";

                go.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
            }
        }
    }

    #endregion

    #region Clases Serialiadas

    [System.Serializable]
    public class Detalle
    {
        public string tipo;
        public string minuto;
    }

    #endregion

    #region Eventos de Click

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    public void OnPointerClick (PointerEventData eventData)
    {

        if (evClick)
        {

            evClick.IrAPerfilJugador (this);
        }
    }

	public void OnPointerDown( PointerEventData eventData )
     {
     }
 
     public void OnPointerUp( PointerEventData eventData )
     {
     }

    #endregion
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class dataMatchCalendar {
	public int pos;
	public string nombreEquipo;
    public string inicialesEquipo;
    public string marcador;
	public string tipo;
    public string fecha;
    public string hora;
    public string titulo;
    public string descripcion;
    public int local ;
    public int actualizar;

    public dataMatchCalendar(int posicion, string nombreE, string inicialesE, string marcadorP, string tipoP, string fechaP, string horaP, string tituloP, string descripcionP, int localE, int actualizarInfo)
    {
        pos = posicion;
        nombreEquipo = nombreE;
        inicialesEquipo = inicialesE;
        marcador = marcadorP;
        tipo = tipoP;
        fecha = fechaP;
        hora = horaP;
        titulo = tituloP;
        descripcion = descripcionP;
        local = localE;
        actualizar = actualizarInfo;
    }
}


[System.Serializable]
public class calendarDataList
{
	public List<dataMatchCalendar> dataList;
	public calendarDataList ()
    {
		dataList = new List<dataMatchCalendar> ();
	}
}
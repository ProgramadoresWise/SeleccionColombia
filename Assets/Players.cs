using UnityEngine;
using System.Collections;

public class Players : MonoBehaviour {

	[SerializeField]
	private int id;
	public int _id {get{ return id; } set{ id = value; }}

	[SerializeField]
	private string name = "";
	public string _name {get{ return name; } set{ name = value; }}

	[SerializeField]
	private int aplausosUltimoPartido;
	public int _aplausosUltimoPartidol {get{ return aplausosUltimoPartido; } set{ aplausosUltimoPartido = value; }}

	[SerializeField]
	private int totalAplausos;
	public int _totalAplausos {get{ return totalAplausos; } set{ totalAplausos = value; }}

	[SerializeField]
	private string instagramLink;
	public string _instagramLink {get{ return instagramLink; } set{ instagramLink = value; }}

	[SerializeField]
	private string tiwtterLink;
	public string _tiwtterLink {get{ return tiwtterLink; } set{ tiwtterLink = value; }}

	[SerializeField]
	private string facebookLink;
	public string _facebookLink {get{ return facebookLink; } set{ facebookLink = value; }}

	[SerializeField]
	private string snapchatLink;
	public string _snapchatLink {get{ return snapchatLink; } set{ snapchatLink = value; }}

	[SerializeField]
	private string ultimopartido;
	public string _ultimopartido {get{ return ultimopartido; } set{ ultimopartido = value; }}

	[SerializeField]
	private string fechaultimopartido;
	public string _fechaultimopartido {get{ return fechaultimopartido; } set{ fechaultimopartido = value; }}

	[SerializeField]
	private string pos;
	public string _pos {get{ return pos; } set{ pos = value; }}

	[SerializeField]
	private int ordenEliminatoria;
	public int _ordenEliminatoria {get{ return ordenEliminatoria; } set{ ordenEliminatoria = value; }}

	[SerializeField]
	private int ordenPartido;
	public int _ordenPartido {get{ return ordenPartido; } set{ ordenPartido = value; }}


	[SerializeField]
	private string cantidadPartidos;
	public string _cantidadPartidos {get{ return cantidadPartidos; } set{ cantidadPartidos = value; }}

}

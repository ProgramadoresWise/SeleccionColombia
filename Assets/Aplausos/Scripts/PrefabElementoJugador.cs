using UnityEngine;
using UnityEngine.UI;
using System;

public class PrefabElementoJugador : MonoBehaviour {
	
	public Image imgFoto;
	public Text txtPosicion;
	public Text txtNombre;
	public Text txtAplausos;
	public Text txtPorcentaje;

	public Slider barraProgreso;

	public int idPlayer;
	public int votes;

	public void ModificarJugador(int id, int pos, string nombre, int aplausos, float porcentaje, Sprite foto, bool act, string panel) {

		idPlayer = id;
		votes = aplausos;
		txtPosicion.text = pos.ToString();
		txtNombre.text = nombre;
		imgFoto.sprite = foto;
		if(panel == "TuElijes")
			imgFoto.sprite = ImgLoadManager.main.TuElijesImg(imgFoto,idPlayer.ToString(),false);
		else if(panel == "Aplausos")
			imgFoto.sprite = ImgLoadManager.main.PlayerImg(imgFoto,idPlayer.ToString(),false);
		txtAplausos.text = "<color=#3FB24DFF>"+aplausos.ToString()+"</color> votos";
		txtPorcentaje.text = Math.Round(porcentaje, 1).ToString()+"%";
		CalcularBarra(porcentaje);
	}

	void CalcularBarra(float porcentaje)
	{
		barraProgreso.value = porcentaje;
	}
}

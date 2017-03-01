using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;




public class JugadoresConvocados : MonoBehaviour {
	
	public int idJugador;
	public int orderEliminatoria;
	public int orderMatch;
	public List<int> partidos = new List<int>(10);

	void Start(){
		this.GetComponent<Button>().onClick.AddListener( () => {
			IrAPerfilDeJugador( idJugador ); });
	}


	public void IrAPerfilDeJugador(int id ){
		foreach ( Players pj in SeleccionC.club.PlayersInfo ){
			if( pj._id == id){
//				Debug.Log(" JUGADOR: " + pj._name );
				PerfilJugador.main.GetPlayer(pj._id,pj._aplausosUltimoPartidol,pj._totalAplausos,pj._facebookLink,pj._tiwtterLink,pj._instagramLink,pj._snapchatLink);
			}
		}
	}


	public void loadImagePlayerConvocados ( ){
		this.GetComponent<Image>().sprite = ImgLoadManager.main.PlayerConvocadosImg( this.GetComponent<Image>(),  idJugador.ToString( ), Convocados.main.update );
	} 


}

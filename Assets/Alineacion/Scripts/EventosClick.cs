using System.Collections;
using UnityEngine;


public class EventosClick : MonoBehaviour
{
    public void IrAPerfilJugador (string j)
    {
        foreach (Players pj in SeleccionC.club.PlayersInfo)
        {
            if (pj._id.ToString () == j)
            {
                PerfilJugador.main.GetPlayer (pj._id, pj._aplausosUltimoPartidol, pj._totalAplausos, pj._facebookLink, pj._tiwtterLink, pj._instagramLink, pj._snapchatLink);
            }
        }
        //		Debug.LogWarning("Va a perfil del jugador " + j.nombre);
    }

    public void IrAPerfilJugador (Jugador j)
    {
        foreach (Players pj in SeleccionC.club.PlayersInfo)
        {
            if (pj._id.ToString () == j.numero)
            {
                PerfilJugador.main.GetPlayer (pj._id, pj._aplausosUltimoPartidol, pj._totalAplausos, pj._facebookLink, pj._tiwtterLink, pj._instagramLink, pj._snapchatLink);
            }
        }
        //		Debug.LogWarning("Va a perfil del jugador " + j.nombre);
    }
}
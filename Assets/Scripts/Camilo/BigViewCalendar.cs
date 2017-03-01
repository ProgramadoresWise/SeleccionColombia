using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BigViewCalendar : MonoBehaviour
{
    public string nombreEquipo;
    public Text nombreEquipo1;
    public Text nombreEquipo2;
    public Text marcador;
    public Text tipo;
    public Text fecha;
    public Text hora;

    public Text titulo;
    public Text description;
    public Text description1;

    public Image imgN;

    public RawImage imgBandera1;
    public RawImage imgBandera2;

    public int localVisitante;

    [Space(20)]

        
    float sizeImgh, sizeImgw;



    public IEnumerator BigCalendar(viewCalendar nMatch)
    {
        nombreEquipo = nMatch.nameTeam;
        marcador.text = nMatch.score.text;
        tipo.text = nMatch.type.text;
        fecha.text = nMatch.dateMatch.text;
        hora.text = nMatch.timeMatch.text;



        sizeImgh = nMatch.sizeheight;
        sizeImgw = nMatch.sizewidth;

        imgN.GetComponent<Image>().color = Color.white;

        titulo.text = nMatch.title.text;
        description.text = nMatch.description.text;
        description1.text = nMatch.description.text;

        imgN.sprite = nMatch.imageCalendar.sprite;

        imgBandera1.texture = nMatch.imageBandera1.texture;
        imgBandera2.texture = nMatch.imageBandera2.texture;

        localVisitante = nMatch.localVisitante;

        if(localVisitante == 1)
        {
            imgBandera2.enabled = true;
            imgBandera1.enabled = false;

            nombreEquipo2.text = nombreEquipo;
            nombreEquipo1.text = "COLOMBIA";
        }
        else
        {
            imgBandera2.enabled = false;
            imgBandera1.enabled = true;

            nombreEquipo1.text = nombreEquipo;
            nombreEquipo2.text = "COLOMBIA";
        }

       
        this.gameObject.SetActive(true);
   
       
		imgN.GetComponent<LayoutElement> ().preferredHeight = nMatch.RecalculateSize ((int)imgN.GetComponent<LayoutElement> ().preferredHeight  , "h");
		imgN.GetComponent<LayoutElement> ().preferredHeight = nMatch.RecalculateHeigth(imgN.mainTexture.height) + 120;

		yield return new WaitForSeconds(.5f);
		DataApp.main.DisableLoading();
    }

  

    public string ToAntiCache( string url)
    {
		string r = "";
		r += UnityEngine.Random.Range(1000000,9000000).ToString();
		r += UnityEngine.Random.Range(1000000,9000000).ToString();
		string result="";
		if(url.Substring(url.Length -4,4   ) == ".php" || url.Substring(url.Length -4,4   ) == ".png"|| url.Substring(url.Length -4,4   ) == ".jpg"){
			result = url + "?key=" + r;
		}else{
			result = url + "&key=" + r;
		}
		return result;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PositionsTable : MonoBehaviour
{
	//variables para la comunicacion PHP
	//private string getUrlShields = "TablaPos/tabla.jpg";
	/*    private string getUrlName= "TablaPos/MostrarTablaIniciales.php";
    private string getUrlPJ= "TablaPos/MostrarTablaPJ.php";
    private string getUrlPG= "TablaPos/MostrarTablaG.php";
    private string getUrlPE= "TablaPos/MostrarTablaE.php";
    private string getUrlPP= "TablaPos/MostrarTablaP.php";
    private string getUrlGF= "TablaPos/MostrarTablaGF.php";
    private string getUrlGC= "TablaPos/MostrarTablaGC.php";
    private string getUrlDG= "TablaPos/MostrarTablaDIF.php";
    private string getUrlPTO= "TablaPos/MostrarTablaPTS.php";  */
    //private string getActualizarInfo= "actualizaciones/php/actualizarInfo.php?bandera=";



    //varible de recibir los datos
    public RawImage imagenEscudos;
/*     public Text resultName;
     public Text resultPJ;
     public Text resultPG;
     public Text resultPE;
     public Text resultPP;
     public Text resultGF;
     public Text resultGC;
     public Text resultDG;
     public Text resultPTO;  */

    private string actualizar;
    private int actualizarNum;

    private WWW getShield;
    private WWW getInfo;
/*    private WWW getName;
    private WWW getPJ;
    private WWW getPG;
    private WWW getPE;
    private WWW getPP;
    private WWW getGF;
    private WWW getGC;
    private WWW getPTO;
    private WWW getDG;*/


	bool reload;

    public void LoadTable()
    {
		if( !reload ){
			DataApp.main.EnableLoading();
			imagenEscudos.enabled = false;
	/*		resultPTO.text = "";
			resultDG.text = "";
			resultGC.text = "";
			resultGF.text = "";
			resultPP.text = "";
			resultPE.text = "";
			resultPG.text = "";
			resultName.text = "";
			resultPJ.text = ""; */
		}

        StartCoroutine( DataApp.main.CheckInternet( internet =>{
			if( internet)
				StartCoroutine(showTable());
			else
				DataApp.main.popUpInformative(true, "Fallo en la conexión.", "Revisa tu conexión a Internet.");
		}));
    }

    private IEnumerator showTable()  {

		getInfo = new WWW(DataApp.main.host + DataApp.main.urlActualizarInfo + (int)idActualizacion.tablas);
        yield return getInfo;
		if( string.IsNullOrEmpty( getInfo.error ) ){
			actualizar = getInfo.text;
			actualizarNum = int.Parse(actualizar);

		}else{
			DataApp.main.popUpInformative(true, "Fallo en la conexión.", "Revisa tu conexión a Internet.");
		}
        

        if (actualizarNum != DataApp.main.GetMyInfoInt("MyTablaPosNum")) {
			DataApp.main.EnableLoading();
            print("ACTUALIZANDO TABLAS");
			imagenEscudos.texture  = ImgLoadManager.main.ShieldTableTexture(imagenEscudos,"banderas",true);
            imagenEscudos.enabled = true;

          /*  getName = new WWW(DataApp.main.host + getUrlName);
            yield return getName;
            resultName.text = getName.text;

            getPJ = new WWW(DataApp.main.host + getUrlPJ);
            yield return getPJ;
            resultPJ.text = getPJ.text;

            getPG = new WWW(DataApp.main.host + getUrlPG);
            yield return getPG;
            resultPG.text = getPG.text;

            getPE = new WWW(DataApp.main.host + getUrlPE);
            yield return getPE;
            resultPE.text = getPE.text;

            getPP = new WWW(DataApp.main.host + getUrlPP);
            yield return getPP;
            resultPP.text = getPP.text;

            getGF = new WWW(DataApp.main.host + getUrlGF);
            yield return getGF;
            resultGF.text = getGF.text;

            getGC = new WWW(DataApp.main.host + getUrlGC);
            yield return getGC;
            resultGC.text = getGC.text;

            getDG = new WWW(DataApp.main.host + getUrlDG);
            yield return getDG;
            resultDG.text = getDG.text;

            getPTO = new WWW(DataApp.main.host + getUrlPTO);
            yield return getPTO;
            resultPTO.text = getPTO.text;*/

            DataApp.main.SetMyInfo("MyTablaPosNum", actualizar, 1);
          /*  DataApp.main.SetMyInfo("MyTablaName", resultName.text, 3);
            DataApp.main.SetMyInfo("MyTablaPJ", resultPJ.text, 3);
            DataApp.main.SetMyInfo("MyTablaPG", resultPG.text, 3);
            DataApp.main.SetMyInfo("MyTablaPE", resultPE.text, 3);
            DataApp.main.SetMyInfo("MyTablaPP", resultPP.text, 3);
            DataApp.main.SetMyInfo("MyTablaGF", resultGF.text, 3);
            DataApp.main.SetMyInfo("MyTablaGC", resultGC.text, 3);
            DataApp.main.SetMyInfo("MyTablaDG", resultDG.text, 3);
            DataApp.main.SetMyInfo("MyTablaPTO", resultPTO.text, 3);*/
        }
        else
        {
			
				imagenEscudos.texture  = ImgLoadManager.main.ShieldTableTexture(imagenEscudos,"banderas",false);
				imagenEscudos.enabled = true;
			
			/*	resultName.text = DataApp.main.GetMyInfoString ("MyTablaName");
				resultPJ.text = DataApp.main.GetMyInfoString("MyTablaPJ");
				resultPG.text = DataApp.main.GetMyInfoString("MyTablaPG");
				resultPE.text = DataApp.main.GetMyInfoString("MyTablaPE");
				resultPP.text = DataApp.main.GetMyInfoString("MyTablaPP");
				resultGF.text = DataApp.main.GetMyInfoString("MyTablaGF");
				resultGC.text = DataApp.main.GetMyInfoString("MyTablaGC");
				resultDG.text = DataApp.main.GetMyInfoString("MyTablaDG");
				resultPTO.text = DataApp.main.GetMyInfoString("MyTablaPTO");*/
			reload = true;
        }
		DataApp.main.DisableLoading();
    }



    public string ToAntiCache(string url)
    {
        string r = "";
        r += UnityEngine.Random.Range(1000000, 9000000).ToString();
        r += UnityEngine.Random.Range(1000000, 9000000).ToString();
        string result = "";
        if (url.Substring(url.Length - 4, 4) == ".php" || url.Substring(url.Length - 4, 4) == ".png" || url.Substring(url.Length - 4, 4) == ".jpg")
        {
            result = url + "?key=" + r;
        }
        else
        {
            result = url + "&key=" + r;
        }
        return result;
    }
	   

}

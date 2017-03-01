using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatrocinadoresManager : MonoBehaviour
{

    public GameObject prefabPatrocinador;
    public GameObject Content;

    private List<GameObject> patrocinadoreslist = new List<GameObject> ();

    [SerializeField]
    private DataRowList userDataList;

    bool reload;
    bool update;

    public void LoadPatrocinadores ()
    {
        if (!reload)
            DataApp.main.EnableLoading ();

        StartCoroutine (DataApp.main.CheckInternet (internet =>
        {
            if (internet)
            {
                StartCoroutine (UpdatePatrocinadores ());
            }
            else
            {
                DataApp.main.popUpInformative (true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
            }
        }));

    }

    IEnumerator UpdatePatrocinadores ()
    {
        WWW updPatrocinador = new WWW (DataApp.main.host + DataApp.main.urlActualizarInfo + (int) idActualizacion.patrocinadores);
        yield return updPatrocinador;
        if (string.IsNullOrEmpty (updPatrocinador.error))
        {
            StartCoroutine (getDatasPatrocinadores (int.Parse (updPatrocinador.text), DataApp.main.host + "Patrocinadores/patrocinadores.php"));
        }
        else
        {
            DataApp.main.popUpInformative (true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
        }

    }

    public IEnumerator getDatasPatrocinadores (int NumAct, string url)
    {

        if (DataApp.main.GetMyInfoInt ("Patrocinadores") != NumAct)
        {
            DataApp.main.EnableLoading ();
            yield return StartCoroutine (GetJsonDataScript.getJson.GetPhpData (url));
            DataApp.main.SetMyInfo ("JsonPatrocinadores", GetJsonDataScript.getJson.GetDataConsult (), 3);
            update = true;
        }
        else
        {
            update = false;
            if (!reload)
            {
                DataApp.main.EnableLoading ();
                string jsonGuardado = DataApp.main.GetMyInfoString ("JsonPatrocinadores");
                yield return StartCoroutine (GetJsonDataScript.getJson.GetLocalData (jsonGuardado));
            }
            else
            {
                Debug.Log ("ya entre descargue no joda");
                DataApp.main.DisableLoading ();
                yield break;
            }
        }

        if (GetJsonDataScript.getJson._state == "Successful")
        {
            userDataList = GetJsonDataScript.getJson.GetData (userDataList, "idPatrocinador", "nombre", "url");
            ReloadImage ();
            DataApp.main.SetMyInfo ("Patrocinadores", NumAct.ToString (), 1);
            reload = true;
        }
        else if (GetJsonDataScript.getJson._state == "Warning_01")
        {
            print ("w1");
            reload = false;
        }
        else if (GetJsonDataScript.getJson._state == "Warning_02")
        {
            print ("w2");
            reload = false;
        }
        else if (GetJsonDataScript.getJson._state == "Warning_03")
        {
            Debug.Log ("Mensaje: Datos con error.");
            DataApp.main.SetMyInfo ("JsonPatrocinadores", "", 3);
            DataApp.main.SetMyInfo ("Patrocinadores", "0", 1);
            reload = false;
            StartCoroutine (getDatasPatrocinadores (NumAct, url));
        }
        GetJsonDataScript.getJson._state = "";

    }

    public void ReloadImage ()
    {
        foreach (DataRow obj in userDataList.dataList)
        {
            GameObject p = Instantiate (prefabPatrocinador, Content.transform, false) as GameObject;
            patrocinadoreslist.Add (p);
            p.SetActive (true);
            p.transform.SetParent (Content.transform);
            p.GetComponent<patrocinador> ().url = obj.GetValueToKey ("url");
            p.GetComponent<patrocinador> ().nombrePatrocinador = obj.GetValueToKey ("nombre");
            string id = obj.GetValueToKey ("idPatrocinador");
            p.GetComponent<patrocinador> ().imgPatrocinador.sprite = ImgLoadManager.main.sponsorImg (p.GetComponent<patrocinador> ().imgPatrocinador, id, update);
        }
        DataApp.main.DisableLoading ();
    }

    public void ClearDataList ()
    {
        foreach (GameObject gm in patrocinadoreslist)
        {
            Destroy (gm);
        }
    }

}
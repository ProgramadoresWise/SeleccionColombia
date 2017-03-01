using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//STATES:
//Successful: La solicitud fue correcta.
//Warning_01: El Php no retorno datos.
//Warning_02: Fallo en la conexión.
//Warning_03: No hay datos locales o estan guardados con una estructura no compatible
//Warning_00: Otro tipo de error

//NOTE:
//Esta clase se debe usar con su singlenton

public class GetJsonDataScript : MonoBehaviour {

	public static GetJsonDataScript getJson;

	[SerializeField]
	private List<DataRowList> data;
	//public DataRowList _data {get{ return data; } set{ data = value; }}

	[SerializeField]
	private string state = "";
	public string _state {get{ return state; } set{ state = value; }}

	[SerializeField]
	private Dictionary<string, List<string>> Keys;
	public Dictionary<string, List<string>> _Keys {get{ return Keys; } set{ Keys = value; }}

	[SerializeField]
	private string dataConsultaString = "";

	void Awake () {

		getJson = this;

		Keys = new Dictionary<string, List<string>> ();
	}

	public DataRowList GetData(DataRowList obj, params string[] _keys){

		List<string> keyList = new List<string> (_keys);

		string keyToKeys;

		if (Keys.ContainsKey(obj._fatherKey)) {

			keyToKeys = obj._fatherKey;

			Keys [keyToKeys] = keyList;

		} else {

			keyToKeys = Keys.Count.ToString ();

			Keys.Add (keyToKeys, keyList);
		} 

		data [0]._fatherKey = keyToKeys;

		data [0].dataList.ForEach (delegate(DataRow o) {

			o._fatherKey = keyToKeys;
		});
	
		return data[0];
	}

	public string GetDataConsult(){
	
		if (!string.IsNullOrEmpty (dataConsultaString)) {

			return dataConsultaString;

		} else {
		
			return "Datos_No_Descargados";
		}
	}

	public IEnumerator GetPhpData(string url){

		data.Clear ();

		state = "";

		dataConsultaString = "";

		yield return StartCoroutine(ConsultPhpData<DataRowList>( dataList  => {
			
			if(dataList != null) {

				state = "Successful";

				data.Add(dataList);

			} else {

				if(state == ""){

					state = "Warning_00";
				}
			}

		} , url));
	}


	public IEnumerator ConsultPhpData<T>(System.Action<T> dataCallback, string url) {
		_state = "";
		WWW consult = new WWW(url);
		yield return consult;
		T result = default (T);

		string textResult;

		if (string.IsNullOrEmpty(consult.error)) {

			if (!string.IsNullOrEmpty (consult.text)) {

				dataConsultaString = consult.text;

				textResult = consult.text.Replace ("],[", "]},{\"values\":[");

				textResult = textResult.Replace ("[[", "{\"dataList\":[{\"values\":[");

				textResult = textResult.Replace ("]]", "]}]}");
				result = JsonUtility.FromJson< T > (textResult);

			} else {
				
				state = "Warning_01";
			}

		} else {
			
			state = "Warning_02";
		}

		dataCallback(result);
	}

	public IEnumerator GetLocalData(string stringData){

		data.Clear ();

		state = "";

		dataConsultaString = "";

		yield return StartCoroutine(ConsultLocalData<DataRowList>( dataList  => {

			if(dataList != null) {

				state = "Successful";

				dataConsultaString = stringData;

				data.Add(dataList);

			} else {

				if(state == ""){

					state = "Warning_00";
				}
			}

		} , stringData));
	}


	public IEnumerator ConsultLocalData<T>(System.Action<T> dataCallback, string stringData) {

		T result = default (T);

		if (!string.IsNullOrEmpty (stringData)) {

			string textResult;

			textResult = stringData.Replace ("],[", "]},{\"values\":[");

			textResult = textResult.Replace ("[[", "{\"dataList\":[{\"values\":[");

			textResult = textResult.Replace ("]]", "]}]}");

			if (textResult.Contains("]},{\"values\":[") && textResult.Contains("{\"dataList\":[{\"values\":[") && textResult.Contains("]}]}")) {

				//Debug.Log ("Todo BIEN");

				result = JsonUtility.FromJson< T > (textResult);

			} else {

				//Debug.Log ("ERROROROROROROROR");
				state = "Warning_03";
			}

		} else {
		
			state = "Warning_03";
		}

		yield return null;

		dataCallback(result);
	}
}

[System.Serializable]
public class DataRow {

	private string fatherKey = "";
	public string _fatherKey {get{ return fatherKey; } set{ fatherKey = value; }}

	public List<string> values;

	public string GetValueToKey(string _key){

		if (GetJsonDataScript.getJson._Keys [fatherKey].Contains (_key)) {

			return values [GetJsonDataScript.getJson._Keys [fatherKey].IndexOf (_key)];

		} else {
		
			return "key_no_encontrada";
		}
	}
}

[System.Serializable]
public class DataRowList {

	private string fatherKey = "";
	public string _fatherKey {get{ return fatherKey; } set{ fatherKey = value; }}

	public List<DataRow> dataList;

	//Constructor:
	public DataRowList(){

		dataList = new List<DataRow> ();
	}

	public string GetSingleRowValueToKey(string _key){

		return dataList [0].GetValueToKey (_key);
	}

	public void SortBy(string _key){

		int index = GetJsonDataScript.getJson._Keys [fatherKey].IndexOf (_key);

		dataList.Sort (delegate(DataRow a, DataRow b) {

			return((a.values[index]).CompareTo (b.values[index]));
		});
	}
}
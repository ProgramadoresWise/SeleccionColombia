using UnityEngine;
using System.Collections;

public class ContentGolScript : MonoBehaviour {

	public ContentResultScript numGolData;

	public ContentResultScript anotadorData;
	public ContentResultScript minData;
	public ContentResultScript tipoGolData;
	public ContentResultScript asistenciaData;

	public void SetDataGolPanel(string idData, string dataPrediction, string dataResult, int dataPoints){

		dataPrediction = (dataPrediction != "none" && dataPrediction != "-2") ? dataPrediction : "";
		dataResult = (dataResult != "none" && dataResult != "-2") ? dataResult : "";

		dataPrediction = (dataPrediction == "REMATE PIERNA IZQUIERDA") ? "REMATE P. IZQ." : dataPrediction;
		dataPrediction = (dataPrediction == "REMATE PIERNA DERECHA") ? "REMATE P. DER." : dataPrediction;
		dataResult = (dataResult == "REMATE PIERNA IZQUIERDA") ? "REMATE P. IZQ." : dataResult;
		dataResult = (dataResult == "REMATE PIERNA DERECHA") ? "REMATE P. DER." : dataResult;

		ContentResultScript aux = new ContentResultScript ();

		if (idData == "setAnotador") {

			anotadorData._leftTitle.text = dataPrediction;
			anotadorData._rightTitle.text = dataResult;
			aux = anotadorData;
			
		} else if (idData == "setMinuto") {

			minData._leftTitle.text = (dataPrediction != "") ? "Min " + dataPrediction : "";
			minData._rightTitle.text =  (dataResult != "") ? "Min " + dataResult : "";
			aux = minData;
		
		} else if (idData == "setTipoGol") {

			tipoGolData._leftTitle.text = dataPrediction;
			tipoGolData._rightTitle.text = dataResult;
			aux = tipoGolData;

		} else if (idData == "setAsistencia") {

			asistenciaData._leftTitle.text = dataPrediction;
			asistenciaData._rightTitle.text = dataResult;
			aux = asistenciaData;

		} else if (idData == "setNumGol") {

			numGolData._leftTitle.text = dataPrediction;
			numGolData._rightTitle.text = dataResult;
		}

		if (dataPoints > 1) {

			//aux._ball.gameObject.SetActive (true);

			aux._points.text = "+" + dataPoints;
		}
	}
}

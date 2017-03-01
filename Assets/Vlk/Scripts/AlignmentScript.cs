using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AlignmentScript : MonoBehaviour {

	[SerializeField]
	private ScrollPrediccionScript predictionscroll;

	[SerializeField]
	private GameObject scrollContent;

	[SerializeField]
	private GameObject prefabItemContent;

	[SerializeField]
	private GameObject prefabShowItemContent;

	[SerializeField]
	private GameObject prefabPlayerInField;

	[SerializeField]
	private GameObject dragHandlerPanel;

	[SerializeField]
	private int hieghtScrollContent;

	[SerializeField]
	private Button nextBtn;

	[SerializeField]
	private Transform ArqueroPos;
	[SerializeField]
	private Transform defensaPos;
	[SerializeField]
	private Transform volantePos;
	[SerializeField]
	private Transform delanteraPos;

	[SerializeField]
	private GameObject alignmentTutorial;

	[SerializeField]
	private List<GameObject> playerInFieldList = new List<GameObject>();

	[SerializeField]
	private List<GameObject> itemsList = new List<GameObject>();

//	[SerializeField]
//	public List<int> alineacion;

	private FootballPlayerListScript footballPlayers;

	void Update(){
	
		if (predictionscroll._alignmentList.Count >= 11 && !nextBtn.interactable) {

			nextBtn.interactable = true;

		} else if (predictionscroll._alignmentList.Count < 11 && nextBtn.interactable) {
		
			nextBtn.interactable = false;
		}
	}

	public void SetFootBallPlayersAlignmentScroll (){

		StartCoroutine (SetDataFootBallPlayers());
	}

	private IEnumerator SetDataFootBallPlayers(){
	
		footballPlayers = GameObject.Find ("TabPrediccionPanel").GetComponent<GetFootballPlayersDataScript> ().footballPlayersdata;

		scrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector3 (0, 0, 0);

		scrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 0);

		int index = 0;

		foreach (FootballPlayer player in footballPlayers.dataList) {

			scrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, scrollContent.GetComponent<RectTransform> ().sizeDelta.y + hieghtScrollContent);

			GameObject item = Instantiate (prefabItemContent);
			item.transform.SetParent (scrollContent.transform);
			itemsList.Add (item);
			item.GetComponent<RectTransform>().anchoredPosition = new Vector3 (0, index * -hieghtScrollContent, 0);
			item.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);
			yield return new WaitForEndOfFrame ();
			//item.GetComponent<FootballPlayerItem> ()._playerPhoto.sprite = player.imgPhoto;
			item.GetComponent<FootballPlayerItem> ()._playerPhoto.GetComponent<DragHandler>()._playerPhoto.sprite = player.imgPhoto;
			item.GetComponent<FootballPlayerItem> ()._playerPhoto.GetComponent<DragHandler>()._dragHandlerPanel = dragHandlerPanel;
			item.GetComponent<FootballPlayerItem> ()._playerPhoto.GetComponent<DragHandler>()._predictionscroll = predictionscroll;
			item.GetComponent<FootballPlayerItem> ()._backPlayerPhoto.sprite = player.imgPhoto;
			//item.GetComponent<FootballPlayerItem> ()._playerName.text = player.nameData;
			item.GetComponent<FootballPlayerItem> ().GetNombre(player.nameData);
			item.GetComponent<FootballPlayerItem> ()._playerId = player.idData;
			item.GetComponent<FootballPlayerItem> ()._position = player.position;
			item.transform.localScale = Vector3.one;

			index++;
		}

		if(DataApp.main.GetMyInfoString("ShowAligTutorial") != "no"){

			alignmentTutorial.SetActive (true);
		}

		DataApp.main.SetMyInfo("ShowAligTutorial", "no", 3);

		predictionscroll.ShowShieldTeams();

		//UpdateDataScript.updateData.StopUpdatePanel ();
	}

	public void ShowAlignment(){

		gameObject.SetActive (true);

		StartCoroutine (ClearAndShowAlignment());
	}

	private IEnumerator ClearAndShowAlignment(){

		foreach (GameObject objPanel in itemsList) {

			Destroy (objPanel);
		}

		itemsList.Clear ();

		yield return itemsList;

		foreach (GameObject objPanel in predictionscroll._selecedAlingnPlayer) {

			Destroy (objPanel.GetComponent<DragHandler>()._objDestine);

			Destroy (objPanel);
		}

		predictionscroll._selecedAlingnPlayer.Clear ();

		yield return predictionscroll._selecedAlingnPlayer;

		foreach (GameObject objPanel in predictionscroll._alignmentList) {

			Destroy (objPanel);
		}

		predictionscroll._alignmentList.Clear ();

		yield return predictionscroll._alignmentList;

		StartCoroutine (SetAlignmentUserScroll ());
	}

	private IEnumerator SetAlignmentUserScroll(){

		Dictionary<string, Sprite> fotos = new Dictionary<string, Sprite> ();
		Dictionary<string, string> posicion = new Dictionary<string, string> ();
		Dictionary<string, string> names = new Dictionary<string, string> ();

		footballPlayers = GameObject.Find ("TabPrediccionPanel").GetComponent<GetFootballPlayersDataScript> ().footballPlayersdata;

		scrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector3 (0, 0, 0);

		scrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 0);

		int index = 0;

		foreach (FootballPlayer player in footballPlayers.dataList) {

			scrollContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, scrollContent.GetComponent<RectTransform> ().sizeDelta.y + hieghtScrollContent);

			GameObject item = Instantiate (prefabShowItemContent);
			item.transform.SetParent (scrollContent.transform);
			itemsList.Add (item);
			item.GetComponent<RectTransform>().anchoredPosition = new Vector3 (0, index * -hieghtScrollContent, 0);
			item.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);
			yield return new WaitForEndOfFrame ();
			//item.GetComponent<FootballPlayerItem> ()._playerPhoto.sprite = player.imgPhoto;
			item.GetComponent<FootballShowPlayerItem> ()._playerPhoto.sprite = player.imgPhoto;
			item.GetComponent<FootballShowPlayerItem> ().GetNombre(player.nameData);
			item.GetComponent<FootballShowPlayerItem> ()._playerId = player.idData;
			item.GetComponent<FootballShowPlayerItem> ()._position = player.position;
			//item.transform.localScale = Vector3.one;

			if(predictionscroll._playerAlignmentList.dataList[0].values.Contains(player.idData.ToString())){
			
				//item.GetComponent<Image> ().enabled = true;
				//item.GetComponent<FootballShowPlayerItem> ()._playerName.color = new Color32 (34, 34, 34, 255);
				item.GetComponent<FootballShowPlayerItem> ()._isSelected = true;
				item.GetComponent<FootballShowPlayerItem> ()._playerPhoto.color = new Color32 (81, 81, 81, 255);
				fotos.Add (player.idData.ToString(), item.GetComponent<FootballShowPlayerItem> ()._playerPhoto.sprite);
				posicion.Add (player.idData.ToString(), item.GetComponent<FootballShowPlayerItem> ()._position);
				names.Add (player.idData.ToString(), item.GetComponent<FootballShowPlayerItem> ()._playerName.text);
			}

			index++;
		}

		StartCoroutine (SetPlayersInField(fotos, posicion, names));
	}

	private IEnumerator SetPlayersInField(Dictionary<string, Sprite> fotos, Dictionary<string, string> posicion, Dictionary<string, string> names){

		foreach(string value in predictionscroll._playerAlignmentList.dataList[0].values){

			string playerPos = posicion[value];

			GameObject item = Instantiate (prefabPlayerInField);

			if (playerPos == "ARQUERO") {

				item.transform.SetParent (ArqueroPos);

			} else if (playerPos == "DEFENSA") {

				item.transform.SetParent (defensaPos);

			} else if (playerPos == "VOLANTE") {

				item.transform.SetParent (volantePos);

			} else if (playerPos == "DELANTERO") {

				item.transform.SetParent (delanteraPos);
			}

			//playerInFieldList.Add (item);
			predictionscroll._alignmentList.Add (item);
			item.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);
			yield return new WaitForEndOfFrame ();
			item.transform.FindChild("Image").GetComponent<Image>().sprite = fotos[value];

			//predictionscroll._alignmentShareList.Add (value + ". " + names[value]);
			predictionscroll._alignmentShareList.Add (names[value]);
		}

		//gameObject.SetActive (false);

		//UpdateDataScript.updateData.StopUpdatePanel ();
		predictionscroll.ContinueAfterUploadingPrediction();
	}

//	public void ImprimirAlineacion(){
//
//		//List<int> alineacion = new List<int>();
//
//		predictionscroll._alignmentList.ForEach (delegate (GameObject obj){
//
//			alineacion.Add(obj.GetComponent<FootballPlayerItem> ()._playerId);
//			Debug.Log ("La Alineacion Es: " + JsonUtility.ToJson(alineacion));
//		});
//
//		Debug.Log ("La Alineacion Es: " + JsonUtility.ToJson(alineacion));
//	}
}

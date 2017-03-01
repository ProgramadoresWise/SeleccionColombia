using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class DropHandler : MonoBehaviour, IDropHandler {

	[SerializeField]
	private ScrollPrediccionScript predictionscroll;

	[SerializeField]
	private GameObject prefabAlignmentPlayer;

	[SerializeField]
	private Transform ArqueroPos;
	[SerializeField]
	private Transform defensaPos;
	[SerializeField]
	private Transform volantePos;
	[SerializeField]
	private Transform delanteraPos;

//	[SerializeField]
//	private string state = "";

//	[SerializeField]
//	private GameObject player {
//
//		get { 
//		
//			if(transform.childCount > 0){
//
//				return transform.GetChild (0).gameObject;
//			}
//
//			return null;
//		}
//	}


	#region IDropHandler implementation

	public void OnDrop (PointerEventData eventData)
	{
//		state = "";

//		GameObject obj = (GameObject)DragHandler.itemBegingDragged.transform.gameObject;
//
//		string position = obj.GetComponent<DragHandler> ()._fatherObj.GetComponent<FootballPlayerItem> ()._position;
//
//		Debug.Log ("La posicion: " + obj.name);

		if (predictionscroll._alignmentList.Count < 11) {

			GameObject obj = (GameObject)DragHandler.itemBegingDragged.transform.gameObject;

			if (!predictionscroll._alignmentList.Contains (obj.GetComponent<DragHandler> ()._fatherObj)) {
			
				string position = obj.GetComponent<DragHandler> ()._fatherObj.GetComponent<FootballPlayerItem> ()._position;

				bool selectedPlayer = true;

				if (position == "ARQUERO") {

					if (ArqueroPos.transform.childCount > 1) {

						selectedPlayer = false;

						//					state = "Arquero_Warnning";
					}

				}

				if (selectedPlayer) {
					
					obj.GetComponent<DragHandler> ()._isOverFootballField = true;

					StartCoroutine (ConsultPhpData (alignmentPlayer => {

						if (alignmentPlayer != null) {

							//obj.GetComponent<DragHandler>().RunMovement(dataList.transform);
							obj.GetComponent<DragHandler> ().SelectThisPlayer (alignmentPlayer);

						}

					}, obj, position));

				} else {

					UpdateDataScript.updateData.RunPopup ("Ya hay un arquero asignado.", 1);
				}
			}

		} else {
		
			UpdateDataScript.updateData.RunPopup ("Alineacion completa.", 1);
		}
	}

	#endregion

	public IEnumerator ConsultPhpData(System.Action<GameObject> dataCallback, GameObject ob, string pos) {

		GameObject item = Instantiate (prefabAlignmentPlayer);

		if (pos == "ARQUERO") {

			item.transform.SetParent (ArqueroPos);

		} else if (pos == "DEFENSA") {

			item.transform.SetParent (defensaPos);

		} else if (pos == "VOLANTE") {

			item.transform.SetParent (volantePos);

		} else if (pos == "DELANTERO") {

			item.transform.SetParent (delanteraPos);
		}

		item.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
		yield return new WaitForEndOfFrame ();
		//item.GetComponent<AlignmentPlayer> ()._img.sprite = ob.GetComponent<DragHandler>()._fatherObj.GetComponent<FootballPlayerItem>()._playerPhoto.sprite;

		dataCallback (item);
	}
}

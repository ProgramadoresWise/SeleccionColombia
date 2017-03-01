using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{

	[SerializeField]
	private Image playerPhoto;
	public Image _playerPhoto {get{ return playerPhoto; } set{ playerPhoto = value; }}

	[SerializeField]
	private GameObject fatherObj;
	public GameObject _fatherObj {get{ return fatherObj; } set{ fatherObj = value; }}

	[SerializeField]
	private GameObject dragHandlerPanel;
	public GameObject _dragHandlerPanel {get{ return dragHandlerPanel; } set{ dragHandlerPanel = value; }}

	[SerializeField]
	private ScrollPrediccionScript predictionscroll;
	public ScrollPrediccionScript _predictionscroll {get{ return predictionscroll; } set{ predictionscroll = value; }}

	public static GameObject itemBegingDragged;
	Vector3 startPosition;

//	Transform startParent;

	//MOVEMENT:

	[SerializeField]
	private bool isMovement = false;

	[SerializeField]
	private GameObject objDestine;
	public GameObject _objDestine {get{ return objDestine; } set{ objDestine = value; }}

	public float speed = 5.0f;
	public float reachDist = 1.0f;
	public int currentPoint = 0;

	private bool isOverFootballField = false;
	public bool _isOverFootballField {get{ return isOverFootballField; } set{ isOverFootballField = value; }}
	
	#region IBeginDragHandler implementation

	public void OnBeginDrag (PointerEventData eventData)
	{

		UpdateAlignmentList ();

		if (isMovement == true) {
		
			isMovement = false;
			isOverFootballField = false;

			Destroy (objDestine);

		} else {
		
			startPosition = transform.localPosition;
		}

		itemBegingDragged = gameObject;

		GetComponent<CanvasGroup> ().blocksRaycasts = false;

		transform.SetParent (dragHandlerPanel.transform);
	}

	#endregion

	#region IDragHandler implementation
	public void OnDrag (PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}
	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{

		if(!isOverFootballField){

			transform.SetParent (fatherObj.transform);

			itemBegingDragged = null;

			transform.localPosition = startPosition;

			GetComponent<CanvasGroup> ().blocksRaycasts = true;

			EnableRayAlignmentList ();
		}
	}

	#endregion

	public void SelectThisPlayer(GameObject des){

		predictionscroll._alignmentList.Add (fatherObj);
		predictionscroll._selecedAlingnPlayer.Add (gameObject);

		objDestine = des;

		isMovement = true;

		GetComponent<CanvasGroup> ().blocksRaycasts = true;

		EnableRayAlignmentList ();
	}

	// Update is called once per frame
	void Update () {

		if(isMovement){

			Vector3 dir = objDestine.transform.position - transform.position;
			transform.position += dir * Time.deltaTime * speed;

			/*if(dir.magnitude <= reachDist){

				isMovement = false;

				objDestine.gameObject.GetComponent<Image>().color = new Color32 (255, 255, 255, 255);

				transform.SetParent (fatherObj.transform);
	
				itemBegingDragged = null;
	
				transform.localPosition = startPosition;
			}*/
		}
	}

	private void UpdateAlignmentList (){

		if (predictionscroll._selecedAlingnPlayer.Contains (gameObject)) {

			Debug.Log ("Si lo contiene!!!!!!!!!");

			predictionscroll._selecedAlingnPlayer.Remove (gameObject);
		}

		if (predictionscroll._alignmentList.Contains (fatherObj)) {

			Debug.Log ("Si lo contiene!!!!!!!!!");
		
			predictionscroll._alignmentList.Remove (fatherObj);
		}

		predictionscroll._alignmentList.ForEach (delegate (GameObject obj){

			obj.GetComponent<FootballPlayerItem> ()._playerPhoto.GetComponent<DragHandler>().GetComponent<CanvasGroup> ().blocksRaycasts = false;
		});
	}

	private void EnableRayAlignmentList (){

		predictionscroll._alignmentList.ForEach (delegate (GameObject obj){

			obj.GetComponent<FootballPlayerItem> ()._playerPhoto.GetComponent<DragHandler>().GetComponent<CanvasGroup> ().blocksRaycasts = true;
		});
	}
}

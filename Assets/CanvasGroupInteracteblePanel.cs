using UnityEngine;
using System.Collections;



public class CanvasGroupInteracteblePanel : MonoBehaviour {

	CanvasGroup canvas;
	public bool touchAct;


	private Vector2 startPos;

	public float minSwipeDistY;
	public float minSwipeDistX;


	void Awake( ){
		canvas  = gameObject.AddComponent(typeof(CanvasGroup)) as CanvasGroup;
	}

	void Update  ( ){
		#if UNITY_ANDROID || UNITY_IOS
		// SwipeChangePos( );
		#endif
	} 


	public void SwipeChangePos(){
		if (Input.touchCount > 0) {
			Touch touch = Input.touches[0];

			switch (touch.phase) {

			case TouchPhase.Began:
				startPos = touch.position;
				break;

			case TouchPhase.Moved:
				float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
				if (swipeDistVertical > minSwipeDistY){
					float swipeValue = Mathf.Sign(touch.position.y - startPos.y);
						if (swipeValue > 0)//up swipe
								canvas.blocksRaycasts = true;
						else if (swipeValue < 0)//down swipe
								canvas.blocksRaycasts = true;
				}

				float swipeDistHorizontal = (new Vector3(touch.position.x,0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;
				if (swipeDistHorizontal > minSwipeDistX) {
					float swipeValue = Mathf.Sign(touch.position.x - startPos.x);
						if (swipeValue > 0)//right swipe
							canvas.blocksRaycasts = false;
						else if (swipeValue < 0)//left swipe
							canvas.blocksRaycasts = false;
					}
				break;
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScaleAndMove : MonoBehaviour {

	[SerializeField]
	private List<Transform> originsObj;

	[SerializeField]
	private Vector3 normalScale = new Vector3(1, 1, 1);

	[Range(0, 100)]
	[SerializeField]
	private int movSpeed;

	[Range(0, 100)]
	[SerializeField]
	private int scaleSpeed;

	[SerializeField]
	private float reachDist = 1.0f;

	private bool isMovement = false;

	private bool isScale = false;

	private bool isVisible = true;

	[SerializeField]
	private int currentIndexOrigin = 0;

	void Start(){

		transform.localScale = new Vector3(2, 2, 1);

		isMovement = false;

		isScale = true;

		currentIndexOrigin = 0;
	}

	void Update () {

		if(isScale){

			transform.localScale -= new Vector3(0.1f, 0.1f, 0f) * Time.deltaTime * scaleSpeed;

			//if(transform.localScale.magnitude <= reachDist && isVisible){
			if(transform.localScale.x <= 1 && isVisible){

				isScale = false;
				isVisible = false;

				transform.localScale = new Vector3(1, 1, 1);

				isMovement = true;

				StartCoroutine (EndScale());
			}
		}

		if(isMovement){

			transform.position -= new Vector3(0f, 1f) * Time.deltaTime * movSpeed;
		}
	}

	private IEnumerator EndScale(){

		do{

			this.GetComponent<CanvasGroup>().alpha -= 0.1f;

			yield return new WaitForSeconds(0.1f);

		}while(this.GetComponent<CanvasGroup>().alpha > 0);

		isMovement = false;

		this.GetComponent<CanvasGroup> ().alpha = 0f;

		StartCoroutine (StartMovement());
	}

	private IEnumerator StartMovement(){

		currentIndexOrigin = (currentIndexOrigin != 0) ? 0 : 1;

		transform.localScale = new Vector3(2, 2, 1);
		transform.position = new Vector3(originsObj[currentIndexOrigin].position.x, originsObj[currentIndexOrigin].position.y, originsObj[currentIndexOrigin].position.z);

		isScale = true;

		do{

			this.GetComponent<CanvasGroup>().alpha += 0.3f;

			yield return new WaitForSeconds(0.1f);

		}while(this.GetComponent<CanvasGroup>().alpha < 1);

		this.GetComponent<CanvasGroup> ().alpha = 1f;

		isVisible = true;
	}

	void OnEnable() {

		transform.localScale = new Vector3(2, 2, 1);

		currentIndexOrigin = 0;

		transform.position = new Vector3(originsObj[currentIndexOrigin].position.x, originsObj[currentIndexOrigin].position.y, originsObj[currentIndexOrigin].position.z);

		isMovement = false;
		isVisible = false;
		isScale = true;

		this.GetComponent<CanvasGroup> ().alpha = 0f;

		StartCoroutine (StartMovement());
	}
}

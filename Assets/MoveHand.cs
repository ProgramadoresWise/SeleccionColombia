using UnityEngine;
using System.Collections;

public class MoveHand : MonoBehaviour {

	[SerializeField]
	private GameObject originObj;

	[SerializeField]
	private GameObject destineObj;

	[Range(0, 10)]
	[SerializeField]
	private int movSpeed;

	[SerializeField]
	private float reachDist = 1.0f;

	private bool isMovement = false;

	private bool isVisible = true;

	void Start(){

		transform.position = new Vector3(originObj.transform.position.x, originObj.transform.position.y, originObj.transform.position.z);
		isMovement = true;
	}

	void Update () {

		if(isMovement){

			Vector3 dir = destineObj.transform.position - transform.position;
			transform.position += dir * Time.deltaTime * movSpeed;

			if(dir.magnitude <= reachDist && isVisible){

				isVisible = false;

				StartCoroutine (EndMovement());
			}
		}
	}

	private IEnumerator EndMovement(){
	
		do{

			this.GetComponent<CanvasGroup>().alpha -= 0.2f;

			yield return new WaitForSeconds(0.1f);
			
		}while(this.GetComponent<CanvasGroup>().alpha > 0);

		this.GetComponent<CanvasGroup> ().alpha = 0f;

		isMovement = false;

		StartCoroutine (StartMovement());
	}

	private IEnumerator StartMovement(){

		transform.position = new Vector3(originObj.transform.position.x, originObj.transform.position.y, originObj.transform.position.z);

		isMovement = true;

		do{

			this.GetComponent<CanvasGroup>().alpha += 0.3f;

			yield return new WaitForSeconds(0.1f);

		}while(this.GetComponent<CanvasGroup>().alpha < 1);

		this.GetComponent<CanvasGroup> ().alpha = 1f;

		isVisible = true;
	}

	void OnEnable() {

		transform.position = new Vector3(originObj.transform.position.x, originObj.transform.position.y, originObj.transform.position.z);
		isMovement = false;
		isVisible = false;

		this.GetComponent<CanvasGroup> ().alpha = 0f;

		StartCoroutine (StartMovement());
	}
}

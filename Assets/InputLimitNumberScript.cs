using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputLimitNumberScript : MonoBehaviour {

	public int maxLimit;

	public void ValideLimitNumber(){

		int number = int.Parse (gameObject.GetComponent<InputField>().text);

		if (number > maxLimit) {

			number = maxLimit;

		} else if (number < 0) {

			number = 0;
		}

		gameObject.GetComponent<InputField>().text = number.ToString();
	}
}

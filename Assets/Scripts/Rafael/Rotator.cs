using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour 
{
	public Transform toRotate;
	public float rotMultiplier;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{	
		toRotate.Rotate(Vector3.forward * (Time.deltaTime* rotMultiplier));
	}
}

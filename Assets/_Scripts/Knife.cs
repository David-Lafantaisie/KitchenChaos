using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour {
	[SerializeField] private float tempMoveSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		TempMoveInput ();
	}

	//this is for test purposes
	void TempMoveInput() {
		//transform.Translate(tempMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime, tempMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime, tempMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
		transform.Translate(tempMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime, 
			tempMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime, 0);
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "SuperIngredient")
			other.gameObject.GetComponent<ChoppableItem> ().Chop (this.transform);
	}
}

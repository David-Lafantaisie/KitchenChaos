using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableItem : MonoBehaviour {

	[SerializeField] private GameObject choppedItem;
	[SerializeField] private int numberOfChops;
	private int chopsRemaining;
    
	// Use this for initialization
	void Start () {
		chopsRemaining = numberOfChops;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//This function is called when the knife touches the object
	public void Chop(Transform other) {
		chopsRemaining--;
		//randomising the chopped item so it spawns near the chop.... this is EXTREMELY rough and will need to be changed
		//also maybe change the root transform for spawning to the knife's transform?
		Vector3 spawn = new Vector3(this.transform.position.x + (Random.Range(-0.2f, 0.2f)),
			this.transform.position.y + (Random.Range(-0.2f, 0.2f)), this.transform.position.z + (Random.Range(-0.2f, 0.2f)));
		GameObject result = Instantiate (choppedItem, spawn, choppedItem.transform.rotation);
		//This line launches the spawned item in a random direction... needs HEAVY refinement though
		result.GetComponent<Rigidbody>().AddForce(new Vector3(
            Random.Range(-30.0f, 30.0f),//X force
			Random.Range(0.0f, 30.0f),//Y force
            Random.Range(-30.0f, 30.0f)//Z force
            ));
		Resize();
		if (chopsRemaining <= 0)
			Kill();
	}

	void Resize() {
		//Shrinking the item a bit every time it gets chopped
		this.transform.localScale = new Vector3(this.transform.localScale.x * 0.8f,
			this.transform.localScale.y * 0.8f, this.transform.localScale.z * 0.8f);
	}

	void Kill() {
		Destroy(this.gameObject);
	}
}

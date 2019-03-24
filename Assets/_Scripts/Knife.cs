using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour {

    private bool inside = false;

	void OnCollisionEnter(Collision other)
    {
		if (other.gameObject.tag == "SuperIngredient" && inside == false)
        {
            inside = true;
            other.gameObject.GetComponent<ChoppableItem>().Chop(this.transform);
			FindObjectOfType<audioManager>().Play("ChoppingSound");
        }
	}

    void OnCollisionExit(Collision other)
    {
        if(other.gameObject.tag == "SuperIngredient" && inside == true)
        {
            inside = false;
        }
    }
}

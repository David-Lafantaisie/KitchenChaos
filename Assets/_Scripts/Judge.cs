using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour {
	[SerializeField] int[] BurgerIngValue; //CHANGE THE SIZE OF THIS IN THE PREFAB IF THE ENUMERATION FOR INGREDIENTS IS RESIZED
	[SerializeField] bool PickyAboutCookTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	float JudgeBurger(BurgerDishScript burger)
	{
		float cumulativeScore = 0;
		float returnScore = 0;
		int numIngredients = 0;
		int cookTimeModifier = 0;
		for (int i = 0; i < burger.getIngListLength (); i++) {
			BurgerIngredientScript testIngredient = burger.ingredientScripts[i];
			cumulativeScore += BurgerIngValue[(int)testIngredient.getIngredientType()];
			if (testIngredient.getIngredientType () != BurgerIngType.BUN) {
				numIngredients++;
			}
			if (testIngredient.getIngredientType () == BurgerIngType.HAMBURGER && PickyAboutCookTime) {
				cookTimeModifier = testIngredient.getCookScore ();
			}
		}
		returnScore = cumulativeScore / (float)numIngredients;
		returnScore = Mathf.Clamp (returnScore + (float)cookTimeModifier, 0.0f, 10.0f);
		return returnScore;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour {
	[SerializeField] int[] BurgerIngValue; //CHANGE THE SIZE OF THIS IN THE PREFAB IF THE ENUMERATION FOR INGREDIENTS IS RESIZED
	[SerializeField] bool PickyAboutCookTime;

	private bool active = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public float JudgeBurger(BurgerDishScript burger)
	{
		float cumulativeScore = 0;
		float returnScore = 0;
		int numIngredients = 0;
		int cookTimeModifier = 0;
		if (burger.getIngListLength () != 0) {
			for (int i = 0; i < burger.getIngListLength (); i++) {
				BurgerIngredientScript testIngredient = burger.ingredientScripts [i];
				cumulativeScore += BurgerIngValue [(int)testIngredient.getIngredientType ()];
				if (testIngredient.getIngredientType () == BurgerIngType.BUN && i != 0 ||
				    testIngredient.getIngredientType () == BurgerIngType.TOPBUN && i != burger.getIngListLength () - 1) {
					cumulativeScore -= 2;
				} else if (testIngredient.getIngredientType () != BurgerIngType.BUN && testIngredient.getIngredientType () != BurgerIngType.TOPBUN) {
					numIngredients++;
					if (testIngredient.getIngredientType () == BurgerIngType.HAMBURGER) {
						if (i != 1) {
							cumulativeScore -= 1;
						}
						if (PickyAboutCookTime) {
							cookTimeModifier = testIngredient.getCookScore ();
						}
					}
				}

			}
			returnScore = cumulativeScore / (float)numIngredients;
			returnScore = Mathf.Clamp (returnScore + (float)cookTimeModifier, 0.0f, 10.0f);
			Debug.Log (this.gameObject + " RANKS THE BURGER " + returnScore + "POINTS.");
			return returnScore;
		} else
			return 0;
	}

	public void SetActive(bool target)
	{
		active = target;
	}

	public bool IsActive()
	{
		return active;
	}
}

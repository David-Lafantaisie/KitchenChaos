//------------------------------------------------------------//
//---------------------- KITCHEN CHAOS -----------------------//
//---------------------- STOVE SCRIPT ------------------------//
//------------------- By Tyler McMillan ----------------------//  ------>Add your name to this list if you contribute
//------------------ for PROMACHOS STUDIOS -------------------//
//------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveScript : MonoBehaviour {

    private bool cooking = false;

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Ingredient")
        {
            col.gameObject.GetComponent<BurgerIngredientScript>().setItemCooking(true);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Ingredient")
        {
            col.gameObject.GetComponent<BurgerIngredientScript>().incrementCookTime();
            col.gameObject.GetComponent<BurgerIngredientScript>().ingredientState();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Ingredient")
        {
            col.gameObject.GetComponent<BurgerIngredientScript>().setItemCooking(false);
        }
    }
}

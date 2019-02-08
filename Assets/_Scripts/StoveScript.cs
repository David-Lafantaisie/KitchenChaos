//------------------------------------------------------------//
//---------------------- KITCHEN CHAOS -----------------------//
//-------------------- INGREDIENT SCRIPT ---------------------//
//------------------- By Tyler McMillan ----------------------//  ------>Add your name to this list if you contribute
//------------------ for PROMACHOS STUDIOS -------------------//
//------------------------------------------------------------//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveScript : MonoBehaviour {

    private bool cooking = false;

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Ingredient")
        {
            col.gameObject.GetComponent<BurgerIngredientScript>().itemCooking = true;
        }
    }
    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Ingredient")
        {
            col.gameObject.GetComponent<BurgerIngredientScript>().itemCooking = false;
        }
    }
}

//------------------------------------------------------------//
//---------------------- KITCHEN CHAOS -----------------------//
//----------------------- DISH SCRIPT ------------------------//
//------------------- By David Lafantaisie -------------------// ------>Add your name to this list if you contribute
//------------------ for PROMACHOS STUDIOS -------------------//
//------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishScript : MonoBehaviour {
    
    private bool started = false;
    public List<GameObject> ingredientsAttached;
    public List<IngredientScript> ingredientScripts;
    private int ingredientListLength;
    private float burgerHeight = 0.0f;
    private float originalHeight = 0.0f;
    private Vector3 originalUp;

	// Use this for initialization
	void Start () {
        initBurgerHeight();
        originalUp = gameObject.transform.TransformDirection(Vector3.up);
	}
	
	// Update is called once per frame
	void Update () {
        updateIngredientPositions();
        if(Vector3.Angle(originalUp, gameObject.transform.TransformDirection(Vector3.up)) > 45.0f && started == true)
        {
            resetBurger();
        }
	}


    //-----------------------------------------------------//
    //---------------------- SETTERS ----------------------//
    //-----------------------------------------------------//

    public void setStarted(bool s)
    {
        started = s;
        if(s == true)
            originalUp = gameObject.transform.TransformDirection(Vector3.up);
    }

    public void addIngredient(GameObject ingredient, IngredientScript ingScript)
    {
        ingredientsAttached.Add(ingredient);
        ingredientScripts.Add(ingScript);
        ingredientListLength++;
    }

    public void updateIngredientPositions()
    {
        if (getStarted() == true)
        {
            for (int i = 0; i < ingredientListLength; i++)
            {
                ingredientsAttached[i].transform.position = 
                    gameObject.transform.position + new Vector3(0.0f, ingredientScripts[i].getHeightInBurger(), 0.0f);
                ingredientsAttached[i].transform.rotation = gameObject.transform.rotation;
            }
        }
    }

    void resetBurger()
    {
        for (int i = 0; i < ingredientListLength; i++)
        {
            ingredientScripts[i].resetIngredient();
        }
        ingredientsAttached.Clear();
        ingredientScripts.Clear();
        ingredientListLength = 0;
        burgerHeight = originalHeight;
        setStarted(false);
    }

    //Sets the height of the burger when you add or take away an ingredient, true if you add, false if you remove
    public void setBurgerHeight(float ingHeight, bool addHeight)
    {
        if (addHeight == true)
            burgerHeight += ingHeight;
        else
            burgerHeight -= ingHeight;
    }


    //-----------------------------------------------------//
    //---------------------- GETTERS ----------------------//
    //-----------------------------------------------------//

    public bool getStarted()
    {
        return started;
    }

    public float getBurgerHeight()
    {
        return burgerHeight;
    }

    public int getIngListLength()
    {
        return ingredientListLength;
    }

    //----------------------------------------------------------//
    //---------------------- INITIALIZERS ----------------------//
    //----------------------------------------------------------//
    
    void initBurgerHeight()
    {
        originalHeight = gameObject.GetComponent<BoxCollider>().bounds.extents.y;
        burgerHeight = originalHeight;
    }
}

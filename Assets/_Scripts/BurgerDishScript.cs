//------------------------------------------------------------//
//---------------------- KITCHEN CHAOS -----------------------//
//----------------------- DISH SCRIPT ------------------------//
//------------------- By David Lafantaisie -------------------// ------>Add your name to this list if you contribute
//------------------ for PROMACHOS STUDIOS -------------------//
//------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerDishScript : MonoBehaviour
{

    private bool started = false;
    public List<GameObject> ingredientsAttached;
    public List<BurgerIngredientScript> ingredientScripts;
    private int ingredientListLength = 0;
    private float burgerHeight = 0.0f;
    private float originalHeight = 0.0f;
    private Vector3 originalUp;

    // Use this for initialization
    void Start()
    {
        initBurgerHeight();
        originalUp = gameObject.transform.TransformDirection(Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Angle(originalUp, gameObject.transform.TransformDirection(Vector3.up)) > 45.0f && started == true)
        {
            resetBurger();
        }
        updateIngredientPositions();
    }


    //-----------------------------------------------------//
    //---------------------- SETTERS ----------------------//
    //-----------------------------------------------------//

    public void setStarted(bool s)
    {
        started = s;
        if (s == true)
            originalUp = gameObject.transform.TransformDirection(Vector3.up);
    }

    public void addIngredient(GameObject ingredient, BurgerIngredientScript ingScript)
    {
        ingredientsAttached.Add(ingredient);
        ingredientScripts.Add(ingScript);
        ingredientListLength++;
    }

    public void removeIngredient(GameObject ingredient, BurgerIngredientScript ingScript)
    {
        int index = ingredientsAttached.IndexOf(ingredient);
        if(index != 0)
        {
            ingScript.setAttached(false);
            int tempLen = ingredientListLength;
            ingredientListLength = index;

            for (int i = index; i < tempLen; i++)
            {
                ingredientScripts[i].resetIngredient();
                ingredientsAttached[i].GetComponent<Rigidbody>().AddForce(new Vector3(
                Random.Range(-100.0f, 100.0f),//X force
                Random.Range(0.0f, 30.0f),//Y force
                Random.Range(-100.0f, 100.0f)//Z force
                ));
            }

            ingredientsAttached.RemoveRange(index, tempLen - index);
            ingredientScripts.RemoveRange(index, tempLen - index);
            burgerHeight = ingredientScripts[index - 1].getHeightInBurger() + ingredientScripts[index - 1].getHeight();
        }
        else
        {
            resetBurger();
        }
    }

    public void updateIngredientPositions()
    {
        if (getStarted() == true)
        {
            for (int i = 0; i < ingredientListLength; i++)
            {
                if(ingredientScripts[i].getAttached() == true)
                {
                    ingredientsAttached[i].transform.position =
                        gameObject.transform.position + new Vector3(0.0f, ingredientScripts[i].getHeightInBurger(), 0.0f);
                    ingredientsAttached[i].transform.rotation = gameObject.transform.rotation;
                }
            }
        }
    }

    void resetBurger()
    {
        setStarted(false);
        for (int i = 0; i < ingredientListLength; i++)
        {
            ingredientScripts[i].resetIngredient();
        }
        ingredientsAttached.Clear();
        ingredientScripts.Clear();
        ingredientListLength = 0;
        burgerHeight = originalHeight;
    }

    //Sets the height of the burger when you add or take away an ingredient, true if you add, false if you remove
    public void setBurgerHeight(float ingHeight, bool addHeight)
    {
        if (addHeight == true)
            burgerHeight += (ingHeight * 2);
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

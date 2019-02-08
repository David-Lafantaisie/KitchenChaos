//------------------------------------------------------------//
//---------------------- KITCHEN CHAOS -----------------------//
//-------------------- INGREDIENT SCRIPT ---------------------//
//------------------- By David Lafantaisie -------------------// ------>Add your name to this list if you contribute
//---------------------- Tyler McMillan ----------------------// 
//------------------ for PROMACHOS STUDIOS -------------------//
//------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerIngredientScript : MonoBehaviour
{

    //-------------------------------------------------------//
    //---------------------- VARIABLES ----------------------//
    //-------------------------------------------------------//

    [SerializeField] Collider collider;
    private bool attached = false;
    private float height;
    private float heightInBurger = 0.0f;
    private float attachDistance = 0.25f;
    private bool inDistance = false;

    //Cooking Vars
    private float totalTimeCooked = 0f;
    [SerializeField] float prefectCookingTime = 0f;
    public bool itemCooking = false;
    public Color foodColor = Color.red;
    public Renderer rend;

    void StartColor()
    {
        if (gameObject.name == "BurgerPatty")
        {
            if (totalTimeCooked == 0.0f)
            {
                foodColor.g = 0f;
                foodColor.r = 0f;
                foodColor.b = 0f;
                foodColor.a = 0f;
            }
            else if (totalTimeCooked <= prefectCookingTime * 0.2f)
            {
                foodColor.g = 1f;
                foodColor.r = 0f;
                foodColor.b = 0f;
                foodColor.a = 0f;
            }
            else if (totalTimeCooked <= prefectCookingTime * 0.4f)
            {
                foodColor.g = 0f;
                foodColor.r = 1f;
                foodColor.b = 0f;
                foodColor.a = 0f;
            }
            else if (totalTimeCooked <= prefectCookingTime * 0.6f)
            {
                foodColor.g = 0f;
                foodColor.r = 0f;
                foodColor.b = 1f;
                foodColor.a = 0f;
            }
            else if (totalTimeCooked <= prefectCookingTime * 0.8f)
            {
                foodColor.g = 0f;
                foodColor.r = 1f;
                foodColor.b = 1f;
                foodColor.a = 0f;
            }
            else if (totalTimeCooked <= prefectCookingTime * 1.0f)
            {
                foodColor.g = 1f;
                foodColor.r = 1f;
                foodColor.b = 0f;
                foodColor.a = 0f;
            }
            else if (totalTimeCooked > prefectCookingTime * 1.0f)
            {
                foodColor.g = 1f;
                foodColor.r = 1f;
                foodColor.b = 1f;
                foodColor.a = 0f;
            }


        }
    }



    private void Start()
    {
        initHeight();
        if (gameObject.name == "BurgerPatty")
        {
            StartColor();
            rend = GetComponent<Renderer>();
            rend.material.color = foodColor;
        }
    }


    public void checkAttach()
    {
        for (int i = 0; i < GameManager.instance.getDishListLength(); i++)
        {
            //Array of scripts inside the game manager instance
            BurgerDishScript dishS = GameManager.instance.dishScripts[i];

            //If the dish has not been started and the ingredient is in the distance of it, we will attach it
            if (attached == false)
            {
                inDistance = Vector3.Distance(gameObject.transform.position, GameManager.instance.dishes[i].transform.position + new Vector3(0.0f, dishS.getBurgerHeight(), 0.0f)) < attachDistance;
                if (inDistance == true)
                {
                    dishS.addIngredient(gameObject, this);



                    dishS.setBurgerHeight(height, true);
                    heightInBurger = dishS.getBurgerHeight() - height;
                    gameObject.transform.position = GameManager.instance.dishes[i].transform.position + new Vector3(0.0f, dishS.getBurgerHeight() - height, 0.0f);



                    gameObject.transform.rotation = GameManager.instance.dishes[i].transform.rotation;
                    gameObject.GetComponent<Rigidbody>().useGravity = false;
                    gameObject.transform.parent = GameManager.instance.dishes[i].transform;
                    setAttached(true);
                    if (dishS.getStarted() == false)
                        dishS.setStarted(true);
                }
            }
        }
    }

    //Best way to check this ?
    void Update()
    {
        if (itemCooking)
        {
            totalTimeCooked += Time.deltaTime;
            ingredientState();
        }
    }
    //-----------------------------------------------------//
    //---------------------- SETTERS ----------------------//
    //-----------------------------------------------------//

    public void setAttached(bool a)
    {
        attached = a;
    }

    public void resetIngredient()
    {
        heightInBurger = 0.0f;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.transform.parent = null;
        setAttached(false);
    }

    public void ingredientState()
    {
        if(prefectCookingTime == 0 && totalTimeCooked > 0)
        {
            Destroy(gameObject);
        }
        else if(prefectCookingTime > 0)
        {
            if (gameObject.name == "BurgerPatty")
            {
                StartColor();
                rend = GetComponent<Renderer>();
                rend.material.color = foodColor;
            }
        }
    }
    


    //-----------------------------------------------------//
    //---------------------- GETTERS ----------------------//
    //-----------------------------------------------------//

    public bool getAttached()
    {
        return attached;
    }

    public float getHeight()
    {
        return height;
    }

    public float getHeightInBurger()
    {
        return heightInBurger;
    }


    //----------------------------------------------------------//
    //---------------------- INITIALIZERS ----------------------//
    //----------------------------------------------------------//

    void initHeight()
    {
        height = collider.bounds.extents.y;
    }
}

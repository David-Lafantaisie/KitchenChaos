//------------------------------------------------------------//
//---------------------- KITCHEN CHAOS -----------------------//
//-------------------- INGREDIENT SCRIPT ---------------------//
//------------------- By David Lafantaisie -------------------// ------>Add your name to this list if you contribute
//--------------------- & Tyler McMillan ---------------------// 
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

    [SerializeField] private Collider collider;
    [SerializeField] private GameObject model;
    private bool attached = false;
    private float height;
    private float heightInBurger = 0.0f;
    private float attachDistance = 0.25f;
    private bool inDistance = false;
    public Quaternion originalRotation;

    //Cooking Vars
    [SerializeField] private float perfectCookingTime = 0.0f;
    private float totalTimeCooked = 0.0f;
    private bool itemCooking = false;
    private Color foodColor = Color.white;
    private Renderer rend;

	[SerializeField] private BurgerIngType type;

    void StartColor()
    {
        if (gameObject.name == "BurgerPatty")
        {
            if (totalTimeCooked == 0.0f)
            {
                foodColor.r = 1f;
                foodColor.g = 1f;
                foodColor.b = 1f;
                foodColor.a = 1f;
            }
            else if (totalTimeCooked <= perfectCookingTime * 0.2f)
            {
                foodColor.r = 0.95f;
                foodColor.g = 0.8f;
                foodColor.b = 0.8f;
                foodColor.a = 1f;
            }
            else if (totalTimeCooked <= perfectCookingTime * 0.4f)
            {
                foodColor.r = 0.9f;
                foodColor.g = 0.7f;
                foodColor.b = 0.6f;
                foodColor.a = 1f;
            }
            else if (totalTimeCooked <= perfectCookingTime * 0.6f)
            {
                foodColor.r = 0.85f;
                foodColor.g = 0.6f;
                foodColor.b = 0.4f;
                foodColor.a = 1f;
            }
            else if (totalTimeCooked <= perfectCookingTime * 0.8f)
            {
                foodColor.r = 0.8f;
                foodColor.g = 0.5f;
                foodColor.b = 0.2f;
                foodColor.a = 1f;
            }
            else if (totalTimeCooked <= perfectCookingTime * 1.0f)
            {
                foodColor.r = 0.75f;
                foodColor.g = 0.4f;
                foodColor.b = 0.1f;
                foodColor.a = 1f;
            }
            else if (totalTimeCooked > perfectCookingTime * 1.0f && totalTimeCooked < perfectCookingTime * 1.25f)
            {
                foodColor.r = 0.70f;
                foodColor.g = 0.3f;
                foodColor.b = 0.0f;
                foodColor.a = 1f;
            }
            else if(totalTimeCooked > perfectCookingTime * 1.25 && totalTimeCooked < perfectCookingTime * 1.5f)
            {
                foodColor.r = 0f;
                foodColor.g = 0f;
                foodColor.b = 0f;
                foodColor.a = 1f;
            }
            else if(totalTimeCooked >= perfectCookingTime * 1.5f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        initHeight();
        if (gameObject.name == "BurgerPatty")
        {
            StartColor();
            rend = model.GetComponent<Renderer>();
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

    //-----------------------------------------------------//
    //---------------------- SETTERS ----------------------//
    //-----------------------------------------------------//

    public void setAttached(bool a)
    {
        attached = a;
    }

    public void resetIngredient()
    {
        setAttached(false);
        heightInBurger = 0.0f;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.transform.parent = null;
    }

    public void ingredientState()
    {
        if(perfectCookingTime == 0 && totalTimeCooked > 0)
        {
            Destroy(gameObject);
        }
        else if(perfectCookingTime > 0)
        {
            if (gameObject.name == "BurgerPatty")
            {
                StartColor();
                rend = model.GetComponent<Renderer>();
                rend.material.color = foodColor;
            }
        }
    }
    
    public void setItemCooking(bool cooking)
    {
        itemCooking = cooking;
    }

    public void incrementCookTime()
    {
        totalTimeCooked += Time.deltaTime;
    }

    //-----------------------------------------------------//
    //---------------------- GETTERS ----------------------//
    //-----------------------------------------------------//

    public bool getAttached()
    {
        return attached;
    }

    public bool getItemCooking()
    {
        return itemCooking;
    }

    public float getHeight()
    {
        return height;
    }

    public float getHeightInBurger()
    {
        return heightInBurger;
    }

	public BurgerIngType getIngredientType()
	{
		return type;
	}

	public int getCookScore()
	{
		if (totalTimeCooked <= perfectCookingTime * 0.4f) {
			return -2;
		} else if (totalTimeCooked <= perfectCookingTime * 0.6f) {
			return -1;
		} else if (totalTimeCooked <= perfectCookingTime * 0.8f) {
			return 0;
		} else if (totalTimeCooked <= perfectCookingTime * 1.0f) {
			return 1;
		} else if (totalTimeCooked > perfectCookingTime * 1.0f && totalTimeCooked < perfectCookingTime * 1.25f) {
			return 0;
		} else if (totalTimeCooked > perfectCookingTime * 1.25) {
			return -1;
		} else
			return 0;
	}


    //----------------------------------------------------------//
    //---------------------- INITIALIZERS ----------------------//
    //----------------------------------------------------------//

    void initHeight()
    {
        height = collider.bounds.extents.y;
        originalRotation = transform.rotation;
    }
}

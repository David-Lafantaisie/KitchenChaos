//------------------------------------------------------------//
//---------------------- KITCHEN CHAOS -----------------------//
//-------------------- INGREDIENT SCRIPT ---------------------//
//------------------- By David Lafantaisie -------------------// ------>Add your name to this list if you contribute
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

    private void Start()
    {
        initHeight();
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
        heightInBurger = 0.0f;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.transform.parent = null;
        setAttached(false);
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

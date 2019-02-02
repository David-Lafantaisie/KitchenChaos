//------------------------------------------------------------//
//---------------------- KITCHEN CHAOS -----------------------//
//---------------------- GAME MANAGER ------------------------//
//------------------- By David Lafantaisie -------------------// ------>Add your name to this list if you contribute
//------------------ for PROMACHOS STUDIOS -------------------//
//------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //-------------------------------------------------------//
    //---------------------- VARIABLES ----------------------//
    //-------------------------------------------------------//

    //Public
    public enum difficulty { EASY = 1, MEDIUM = 2, HARD = 3 }
    public static GameManager instance = null;
    public GameObject[] dishSpawns;
    public List<GameObject> dishes;
    public List<DishScript> dishScripts;

    //Private
    [SerializeField] private GameObject dish;
    private int stage = 1;
    private int lowStage = 1;
    private int highStage = 3;
    private int dishListLength;
    private difficulty mode = difficulty.EASY;

    //----------------------------------------------------------//
    //---------------------- INITIALIZERS ----------------------//
    //----------------------------------------------------------//

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        initGame();
    }

    void initGame()
    {
        setStage(1);
        setDifficulty(difficulty.HARD);
        dishSpawns = GameObject.FindGameObjectsWithTag("DishSpawn");
        for (int i = 0; i < (int)mode; i++)
        {
            dishes.Add(Instantiate(dish, dishSpawns[i].transform.position, dishSpawns[i].transform.rotation));
            dishScripts.Add(dishes[i].GetComponent<DishScript>());
        }
        dishListLength = (int)mode;
    }

    //-----------------------------------------------------//
    //---------------------- SETTERS ----------------------//
    //-----------------------------------------------------//

    public void setDifficulty(difficulty m)
    {
        mode = m;
    }

    public void setStage(int s)
    {
        if (s <= highStage && s >= lowStage)
            stage = s;
    }

    //-----------------------------------------------------//
    //---------------------- GETTERS ----------------------//
    //-----------------------------------------------------//

    public difficulty getDifficulty()
    {
        return mode;
    }

    public int getStage()
    {
        return stage;
    }

    public int getDishListLength()
    {
        return dishListLength;
    }

}

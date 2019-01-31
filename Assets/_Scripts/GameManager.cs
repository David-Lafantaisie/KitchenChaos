using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //-------------------------------------------------------//
    //---------------------- VARIABLES ----------------------//
    //-------------------------------------------------------//

    //Public
    public enum difficulty { EASY, MEDIUM, HARD }
    public static GameManager instance = null;
    public GameObject[] dishes;
    public List<DishScript> dishScripts;

    //Private
    private int stage = 1;
    private int lowStage = 1;
    private int highStage = 3;
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
        setDifficulty(difficulty.EASY);
        dishes = GameObject.FindGameObjectsWithTag("Dish");
        dishScripts.Clear();
        for(int i = 0; i < dishes.Length; i++)
        {
            dishScripts.Add(dishes[i].GetComponent<DishScript>());
        }
    }

    //-----------------------------------------------------//
    //---------------------- SETTERS ----------------------//
    //-----------------------------------------------------//

    void setDifficulty(difficulty m)
    {
        mode = m;
    }

    void setStage(int s)
    {
        if (s <= highStage && s >= lowStage)
            stage = s;
    }

    //-----------------------------------------------------//
    //---------------------- GETTERS ----------------------//
    //-----------------------------------------------------//

    difficulty getDifficulty()
    {
        return mode;
    }

    int getStage()
    {
        return stage;
    }

}

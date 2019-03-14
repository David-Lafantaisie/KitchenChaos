//------------------------------------------------------------//
//---------------------- KITCHEN CHAOS -----------------------//
//---------------------- GAME MANAGER ------------------------//
//------------------- By David Lafantaisie -------------------// ------>Add your name to this list if you contribute
//------------------ for PROMACHOS STUDIOS -------------------//
//------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //-------------------------------------------------------//
    //---------------------- VARIABLES ----------------------//
    //-------------------------------------------------------//

    //Public
    public enum difficulty { EASY = 1, MEDIUM = 2, HARD = 3 }
    public static GameManager instance = null;
    public GameObject[] dishSpawns;
    public List<GameObject> dishes;
    public List<BurgerDishScript> dishScripts;
	public List<GameObject> judges;
	public List<Judge> judgeScripts;
	public float score;



    //Private
    [SerializeField] private GameObject dish;
    private int stage = 1;
    private int lowStage = 1;
    private int highStage = 3;
    private int dishListLength;
	private int numJudges = 0; //THIS ISNT BEING USED RN, USE FOR IF/WHEN WE INCORPORATE MULTIPLE JUDGES AT ONCE
    private difficulty mode = difficulty.EASY;
	private bool sentForJudges = false;
	private Text scoreUI;

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
		GameObject[] judgesTemp = GameObject.FindGameObjectsWithTag ("Judge");
		int z = Random.Range (0, judgesTemp.Length - 1); //I COULDNT THINK OF A GOOD NAME FOR THIS INT, FEEL FREE TO CHANGE
		judges.Add (judgesTemp[z]); //WHEN WE HAVE MULTIPLE JUDGES, MOVE ALL THIS STUFF INTO THE FOR LOOP BELOW
		judgeScripts.Add(judges[0].GetComponent<Judge>());
		judgeScripts [0].SetActive (true);
		Debug.Log (judges[0] + "SET AS ACTIVE JUDGE");
		numJudges++;
		scoreUI = GameObject.FindWithTag ("ScoreUI").GetComponent<Text>();

        for (int i = 0; i < (int)mode; i++)
        {
            dishes.Add(Instantiate(dish, dishSpawns[i].transform.position, dishSpawns[i].transform.rotation));
            dishScripts.Add(dishes[i].GetComponent<BurgerDishScript>());
        }
        dishListLength = (int)mode;
    }

	void Update() //JUST USING THIS TO TEST THE JUDGE SYSTEM
	{
		if (Input.GetKeyDown (KeyCode.X)) {
			judgeBurgers ();
		}
	}

	public void judgeBurgers()
	{
		if (!sentForJudges) {
			for (int i = 0; i < dishListLength; i++) {
				score += judgeScripts [0].JudgeBurger (dishScripts [i]);
				Debug.Log ("FINAL SCORE = " + score);
			}
            scoreUI.fontSize = 70;
			scoreUI.text = "HIGH SCOrE\n" + score.ToString();
			sentForJudges = true;
		}
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

//REMEMBER: IF YOU ARE APPENDING THE ENUMERATION, ADD TO THE END, NOT THE BEGINNING OR IN BETWEEN
public enum BurgerIngType //add the rest later as more ingredients are added to the game
{
	BUN, //idk if top and bottom bun should be split for this purpose?
	HAMBURGER,
	LETTUCE,
	TOMATO,
	ONION,
	CHEESE,
	TOPBUN
}
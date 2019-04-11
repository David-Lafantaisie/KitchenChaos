using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldUIObject : MonoBehaviour {

    private enum bType{EXIT, OPTIONS, RESTART, LEADERBOARDS};
    [SerializeField] bType buttonType;
    [SerializeField] GameObject uiLight;
    [SerializeField] Text worldText;

	// Use this for initialization
	void Start () {
        uiLight.SetActive(false);
        worldText.enabled = false;
	}

    public void activate()
    {
        switch (buttonType)
        {
            case bType.EXIT:
                uiLight.GetComponent<Light>().color = new Color(98.0f, 255.0f, 125.0f);
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
                break;
            case bType.LEADERBOARDS:
                break;
            case bType.OPTIONS:
                break;
            case bType.RESTART:
                SceneManager.LoadScene("protoLevelAlpha");
                GameManager.instance.restart();
                break;
        }
    }

    public GameObject getUILight()
    {
        return uiLight;
    }

    public Text getWorldText()
    {
        return worldText;
    }
}

// host script by ted bissasa allows host to naviage area and make commentary

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HostScript : MonoBehaviour {


    [SerializeField] GameObject[] waypoints;
    [SerializeField] GameObject hostHead;
    [SerializeField] GameObject player;
    [SerializeField] GameObject hostHeadCube;

    private Material hostMouth;
    private int currentDestanation = -1;
    private int previousDestination = -1;
    private NavMeshAgent agent;
    private bool hasCommented = true;
    private float time = 0;
    private float[] samples =  new float[64];
    float sampleAverage;

	// Use this for initialization
	void Start ()
    {
		FindObjectOfType<audioManager>().Play("Applause+Intro");
        agent = GetComponent<NavMeshAgent>();
        agent.destination = waypoints[0].transform.position;
        InvokeRepeating("MostInterestingPoint", 1.0f, 6.0f);
        hostMouth = hostHeadCube.GetComponent<MeshRenderer>().materials[2];
        SoundManager.instance.playMainTheme(); //starts main theme
    }

    // Update is called once per frame
    void Update ()
    {
        hostHead.transform.LookAt(player.transform.position);
        hostHead.transform.Rotate(0,90,0);
        if (agent.remainingDistance < 0.5f && !hasCommented)
        {
            CommentAboutDestination();
            
        }
        agent.baseOffset = Mathf.Sin(time) + 2;
        time += 0.01f;
        setHostMouthColor();
    }

    private void setHostMouthColor()
    {
        samples = SoundManager.instance.getSamples();
        sampleAverage = 0;
        for(int i = 0; i<64;i++)
        {
            sampleAverage += Mathf.Clamp((samples[i] * (i * i)), 0, 1);
        }
        sampleAverage = sampleAverage / 64;
        hostMouth.color = new Color(sampleAverage, 0.0f, 0.0f, 0.0f);
    }

    private void MostInterestingPoint() // random is working better than the system i came up with 
    {
        hasCommented = false;
        previousDestination = currentDestanation;
        while(currentDestanation == previousDestination)
            currentDestanation = Random.Range(1, waypoints.Length);
        agent.destination = waypoints[currentDestanation].transform.position;
    }

    private void CommentAboutDestination() // i have decided the host will select from 3-5 preset voicelines per destination each with an activation condition
    {
        hasCommented = true;
        switch(currentDestanation)
        {
            case 0:
                Debug.Log("Welcome to the show");
                //SoundManager.instance.playHost1Sound();
                break;
            case 1:
                Debug.Log("And how are the judges dooing today");
                //SoundManager.instance.playHost1Sound();
                break;
            case 2:
                Debug.Log("Hmm food is looking good");
                //SoundManager.instance.playHost1Sound();
                break;
            case 3: // some exmples of conditonals for the host lines
                 //if(Burger burning)
                 Debug.Log("Your burger is burning"); //play the voice clip

                //if(Burger not on stove and not on plate)
                //Debug.Log("you should get cooking you dont have much time ");

                //if(Burger condition good)
                //Debug.Log("smells delicious");
                SoundManager.instance.playHost1Sound();
                break;
        }
    }
}

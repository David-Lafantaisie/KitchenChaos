using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HostScript : MonoBehaviour {


    [SerializeField] GameObject[] waypoints;
    [SerializeField] GameObject hostHead;
    [SerializeField] GameObject player;
    private int currentDestanation = -1;
    private int previousDestination = -1;
    private NavMeshAgent agent;
    private bool hasCommented = true;

	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = waypoints[0].transform.position;
        InvokeRepeating("MostInterestingPoint", 1.0f, 6.0f);
    }

    // Update is called once per frame
    void Update ()
    {
        hostHead.transform.LookAt(player.transform.position);
        if(agent.remainingDistance < 0.5f && !hasCommented)
        {
            CommentAboutDestination();
        }

    }


    private void MostInterestingPoint() // temp solution will eventaully go to highest activity area
    {
        hasCommented = false;
        previousDestination = currentDestanation;
        while(currentDestanation == previousDestination)
            currentDestanation = Random.Range(1, waypoints.Length);
        agent.destination = waypoints[currentDestanation].transform.position;
    }

    private void CommentAboutDestination() // temp solution will eventaully discover relevant information about the area hes in and say that
    {
        hasCommented = true;
        switch(currentDestanation)
        {
            case 0:
                Debug.Log("Im at my spawn point");
                break;
            case 1:
                Debug.Log("Im at the judges table");
                break;
            case 2:
                Debug.Log("Im at the plating area");
                break;
            case 3:
                Debug.Log("Im at the stove");
                break;
        }
    }
}

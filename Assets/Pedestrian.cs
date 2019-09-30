using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian : MonoBehaviour
{
    public Spawner spawner;
    private GameObject goalWaypoint;
    private NavMeshAgent nmAgent;


    // Start is called before the first frame update
    void Start()
    {
        nmAgent = GetComponent<NavMeshAgent>();
        // Choose where to go
        int wpIndex = Random.Range(0, spawner.gameObject.transform.childCount-1);
        goalWaypoint = spawner.gameObject.transform.GetChild(wpIndex).gameObject;
        nmAgent.destination = goalWaypoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

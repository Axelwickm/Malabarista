using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian : MonoBehaviour
{
    public Spawner spawner;
    private GameObject goalWaypoint;
    private UnityEngine.AI.NavMeshAgent nmAgent;


    // Start is called before the first frame update
    void Start()
    {
        nmAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        // Choose where to go
        int wpIndex = Random.Range(0, spawner.gameObject.transform.childCount-1);
        goalWaypoint = spawner.gameObject.transform.GetChild(wpIndex).gameObject;
        nmAgent.destination = goalWaypoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!nmAgent.pathPending)
        {
            if (nmAgent.remainingDistance <= nmAgent.stoppingDistance)
            {
                if (!nmAgent.hasPath || nmAgent.velocity.sqrMagnitude == 0f)
                {
                    spawner.DoneWithLife();
                    Destroy(gameObject);
                }
            }
        }
    }
}

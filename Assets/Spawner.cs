using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public int MaxPedestrians = 10;
    public double spawnRate = 0.05; // Hz
    public GameObject Pedestrian;
    private int currentPedestrians = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.value < spawnRate * Time.deltaTime && currentPedestrians < MaxPedestrians)
        {
            // Spawn a new agent at waypoint
            int wpIndex = Random.Range(0, gameObject.transform.childCount - 1);
            Vector3 position = gameObject.transform.GetChild(wpIndex).transform.position;

            GameObject newPedestrian = Instantiate(Pedestrian, position, Quaternion.identity);
            newPedestrian.GetComponent<Pedestrian>().spawner = this;
            currentPedestrians++;
        }
    }

    // Pedestrian just done with life
    public void DoneWithLife()
    {
        currentPedestrians--;
    }
}

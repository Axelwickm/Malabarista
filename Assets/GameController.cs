using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float points = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Render on screen
    }

    public void AddPoints(float p)
    {
        points += p;
        Debug.Log("Player points: " + points);
    }
}

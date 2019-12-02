using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int historicalPoints = 10;
    public float recordInterval = 3.0f;
    public TextMesh pointsIndicator;

    private float points = 0;
    private Queue<float> pointHistory = new Queue<float>();
    private float lastHistoryTime;



    // Start is called before the first frame update
    void Start()
    {
        pointHistory.Enqueue(points);
        lastHistoryTime = Time.time;
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastHistoryTime+recordInterval < Time.time)
        {
            pointHistory.Enqueue(points);
            lastHistoryTime = Time.time;
        }

        if (historicalPoints < pointHistory.Count)
        {
            pointHistory.Dequeue();
        }


        // TODO: Render on screen
    }


    public float GetPointGain()
    {
        float a = 0;
        foreach (float p in pointHistory)
        {
            a += p;
        }

        a /= pointHistory.Count;
        return a;
    }

    public void AddPoints(float p)
    {
        points += p;
        UpdateText();
        Debug.Log("Player points: " + points);
    }

    private void UpdateText()
    {
        pointsIndicator = "Points: " + Mathf.Round(points);
    }
}

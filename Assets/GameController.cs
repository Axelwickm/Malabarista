using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool logToFile = true;
    public string path = "./playlogs/log.csv";
    private StreamWriter writer;



    public int historicalPoints = 10;
    public float recordInterval = 3.0f;
    public TextMesh pointsIndicator;
    public TextMesh hVecMagIndicator;
    public float hVecMag;

    private float points = 0;
    private float pointsLast = 0;
    private Queue<float> pointHistory = new Queue<float>();
    private float lastHistoryTime;



    // Start is called before the first frame update
    void Start()
    {
        pointHistory.Enqueue(points - pointsLast);
        lastHistoryTime = Time.time;
        UpdateText();
        if (logToFile)
        {
            writer = new StreamWriter(path, false);
            writer.WriteLine("Time; Points; Gain");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lastHistoryTime+recordInterval < Time.time)
        {
            pointHistory.Enqueue(points - pointsLast);
            pointsLast = points;
            lastHistoryTime = Time.time;
            UpdateText();
            if (logToFile)
            {
                writer.WriteLine(lastHistoryTime+"; "+points+"; "+GetPointGain());
            }
        }

        if (historicalPoints < pointHistory.Count)
        {
            pointHistory.Dequeue();
        }

    }

    public void OnApplicationQuit()
    {
        if (logToFile)
        {
            Debug.Log("Close file: "+path   );
            writer.Close();
        }
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
        pointsIndicator.text = "Points: " + Mathf.Round(points) + "\nGain: "+GetPointGain();
        hVecMagIndicator.text = "hVecMag: " + hVecMag;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Limit_Velocity : MonoBehaviour
{
    public int max_Velocity;
    public int max_FallVelocity;
    Rigidbody rg;
    Transform transf;
    Vector3 v;
    Vector3 middlePoint;
    public Transform player;
    public float heightFactor;
    private float height;
    public GameObject respawnPlace;
    private float touchedFloorTime;

    public Vector3 startPoint; 
    public Vector3 endPoint;
    public Vector3 heightVector; 
    public float count;
    public bool followTrajectory;
    // Start is called before the first frame update
    void Start()
    {
        respawnPlace = GameObject.Find("BallRespawn");
        rg = GetComponent<Rigidbody>();
        transf = GetComponent<Transform>();
        Time.timeScale = 1.0f;
        //Time.fixedDeltaTime = 0.7f;
        heightFactor = 1.0f;
        followTrajectory = false;
        count = 2.0f;
        startPoint = new Vector3(0.0f, 0.0f, 0.0f);
        endPoint = new Vector3(10.0f, 10.0f, 10.0f);

    }

    // Update is called once per frame
    void Update()
    {
        
        v = rg.velocity;
        
        
   
        if (count < 1.0f && followTrajectory)
        {
            
            height = heightVector.magnitude * heightFactor;

            middlePoint = startPoint + (endPoint - startPoint) / 2 + Vector3.up * height;
            count += (0.4f - 0.12f * (heightVector.magnitude - 7.0f)/7.0f)* Time.deltaTime;
           
            Debug.Log(startPoint.ToString());
            Vector3 m1 = Vector3.Lerp(startPoint, middlePoint, count);
            Vector3 m2 = Vector3.Lerp(middlePoint, endPoint, count);
            rg.position = Vector3.Lerp(m1, m2, count);
        }

        if(Mathf.Abs(rg.position.x - player.position.x) > 2 && Mathf.Abs(rg.position.z - player.position.z) > 2 || rg.position.y < -2)
        {
            Respawn();
        }

        if (rg.position.y < 1.0)
        {
            touchedFloorTime += Time.deltaTime;
        }
        else
        {
            touchedFloorTime = 0;
        }

        if (3 < touchedFloorTime)
        {
            Respawn();
        }

        /*if (rg.velocity.y > max_Velocity)
        {
            v.y = max_Velocity;
            
        }
        if (rg.velocity.y < -max_FallVelocity)
        {
            v.y = -max_FallVelocity;

        }



        if (Vector3.Distance(new Vector3(player.position.x,0.0f, player.position.z), new Vector3(transf.position.x, 0.0f, transf.position.z)) > maxDistance)
        {
            v.x = 0.0f;
            v.z = 0.0f;
            Debug.Log("In Limit_Velocity");
        }

        

        //if(rg.transform.position.y > 12)
        //{
        //    v.y = -1;
        //}

        rg.velocity = v;

        // if(lastHand.startingHandType == Hand.HandType.Right)
        //{

        //}*/
    }

    public void Respawn()
    {
        rg.position = respawnPlace.transform.position + new Vector3(0.2f * (Random.value - 0.5f), 0, 0.2f * (Random.value - 0.5f));
        rg.velocity = Vector3.zero;
    }

}

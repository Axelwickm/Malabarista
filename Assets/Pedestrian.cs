using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian : MonoBehaviour
{
    public Spawner spawner;
    public const int maxInterestDistance = 14;
    public const float interestChance = 0.15F; // Per second;
    public const float loseInterestChance = 0.04F; // Per second;
    public const float physicsModeDistance = 1.3F;

    public GameObject PlayerHead;

    public List<GameObject> bodies;
    public List<GameObject> hairs;
    public GameObject boringHair;

    private GameObject body;
    private GameObject hair;


    private GameObject goalWaypoint;
    private UnityEngine.AI.NavMeshAgent nmAgent;
    private GameObject gatherPoint;
    private Rigidbody rb;

    private Vector3 lastPosition;

    private enum ModeEnum
    {
        MovingToWaypoint,
        MovingToPlayer,
        WatchingPlayer
    };
    private ModeEnum mode;

    // Start is called before the first frame update
    void Start()
    {
        nmAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        gatherPoint = GameObject.Find("GatherPoint");
        PlayerHead = GameObject.Find("PlayerHeadPlaceholder");
        rb = GetComponent<Rigidbody>();

        Bounds bounds = GetComponent<Collider>().bounds;
        Vector3 feetPosition = bounds.center  - new Vector3(0.0f,  bounds.extents.y, 0.0f);
        int index;

        // Load body parts
        index = (int) Mathf.Floor(Random.Range(0, 199) / 100);
        body = Instantiate(bodies[index], feetPosition, Quaternion.identity);
        body.transform.parent = transform;

        if (Random.value < 1.0/500.0)
        {
            hair = Instantiate(boringHair, feetPosition, Quaternion.Euler(-90, 0, 0));
            hair.transform.parent = transform;
        }
        else
        {
            index = Random.Range(0, (hairs.Count - 1));
            if (hairs[index] != null)
            {
                hair = Instantiate(hairs[index], feetPosition, Quaternion.Euler(-90, 0, 0));
                hair.transform.parent = transform;
            }

        }

        // Choose where to go
        int wpIndex = Random.Range(0, spawner.gameObject.transform.childCount - 1);
        goalWaypoint = spawner.gameObject.transform.GetChild(wpIndex).gameObject;
        nmAgent.destination = goalWaypoint.transform.position;
        mode = ModeEnum.MovingToWaypoint;

        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == ModeEnum.MovingToWaypoint)
        {
            if (!nmAgent.pathPending)
            {
                if (nmAgent.remainingDistance <= nmAgent.stoppingDistance)
                {
                    // Should stop?
                    if (!nmAgent.hasPath || nmAgent.velocity.sqrMagnitude == 0f)
                    {
                        spawner.DoneWithLife();
                        Destroy(gameObject);
                    }
                }
                else
                {
                    // Become interested in the player?
                    Vector3 diff = transform.position - gatherPoint.transform.position;
                    float distanceToGather = diff.magnitude;
                    if (distanceToGather < maxInterestDistance)
                    {
                        Vector3 velocity = (transform.position - lastPosition) / Time.fixedDeltaTime;
                        if (0.0 < Vector3.Dot(diff.normalized, velocity.normalized))
                        {
                            // Should I stop
                            if (Random.value < interestChance * Time.fixedDeltaTime)
                            {
                                nmAgent.destination = gatherPoint.transform.position;
                                mode = ModeEnum.MovingToPlayer;
                            }
                        }
                    }
                }
            }
        }
        else if (mode == ModeEnum.MovingToPlayer)
        {
            if (!nmAgent.pathPending)
            {
                // Close enough to switch to physics mode?
                Vector3 diff = transform.position - gatherPoint.transform.position;
                if (diff.magnitude < physicsModeDistance)
                {
                    mode = ModeEnum.WatchingPlayer;
                    print("In physics mode");
                }
            }
        }
        else if (mode == ModeEnum.WatchingPlayer)
        {
            // Move towards gather point
            Vector3 toGather = transform.position - gatherPoint.transform.position;
            Vector3 toNormGather = toGather.normalized;
            toGather = Vector3.Min(toNormGather, toNormGather * toGather.magnitude * 1.0F);

            rb.AddForce(-toNormGather, ForceMode.Acceleration);

            if (Random.value < loseInterestChance * Time.fixedDeltaTime)
            {
                mode = ModeEnum.MovingToWaypoint;
                nmAgent.destination = goalWaypoint.transform.position;
            }
            
            Vector3 relative2Player = PlayerHead.transform.position - transform.position;
            relative2Player.y = 0;
            Vector3 cross = Vector3.Cross(transform.forward, relative2Player);
            rb.AddTorque(cross*0.03F);
        }

        lastPosition = transform.position;
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (mode != ModeEnum.WatchingPlayer)
        {
            return;
        }

        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.otherCollider.gameObject.name == gameObject.name)
            {
                Vector3 velDiff = contact.otherCollider.gameObject.GetComponent<Rigidbody>().velocity - rb.velocity;
                if (.075F < velDiff.magnitude)
                {
                    rb.AddForce(0.15F * contact.normal, ForceMode.Impulse);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class PeerPresenceManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> peers;

    public float checkRadius = 4.5f;

    // Time delay before the first invocation
    public float startDelay = 0.0f;

    // Time interval between invocations
    public float repeatInterval = 2.0f;

    public AgentParameters agentParam;

    [SerializeField]
    public float peerPresenceLevel;

    // Store a list of game objects currently inside the sphere
    [SerializeField]
    private List<GameObject> objectsInside = new List<GameObject>();

    private void Start()
    {
        agentParam = this.gameObject.GetComponent<AgentParameters>();
        peers = agentParam.peers;
        InvokeRepeating(nameof(CalculateDistances), startDelay, repeatInterval);
    }

    private void OnTriggerEnter(Collider other)
    {
        // When a game object enters the collider, add it to the list
        if (peers.Contains(other.gameObject))
        {
            objectsInside.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When a game object exits the collider, remove it from the list
        if (objectsInside.Contains(other.gameObject))
        {
            objectsInside.Remove(other.gameObject);
        }
    }

    private void CalculateDistances()
    {
        float lowestDistance = 100f;

        if (objectsInside.Count == 0)
        {
            peerPresenceLevel = 3;
        }

        foreach (var obj in objectsInside)
        {
            if (agentParam.peers.Contains(obj.gameObject))
            {
                
                //may need to check if they are on the same y axis. 

                //If there is a peer within 2 meters, stress level is 1 (lowest), 
                //If no peer is within 9 meters, stress level is 3 (highest)
                float distance = Vector3.Distance(this.gameObject.transform.position, obj.gameObject.transform.position);
                if(distance < lowestDistance)
                {
                    lowestDistance = distance;
                }
               
                print("agent " + agentParam.name + " is colliding with " + obj.gameObject.name + " who is " + distance + "m away");
            }

        }

        if (lowestDistance >= 0 && lowestDistance < 2)
        {
            //Low
            peerPresenceLevel = 1;

        }
        else if (lowestDistance >= 2 && lowestDistance <= 9)
        {
            //Med
            peerPresenceLevel = 2;
        }
        else
        {
            peerPresenceLevel = 3;
        }

      
    }

    private void getStressFromPeerPresence()
    {

    }
}

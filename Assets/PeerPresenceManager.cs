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

    //Data collection
    private DataCollection dataCollection;
    public Dictionary<float, float> closePeers2m = new Dictionary<float, float>();
    public Dictionary<float, float> closePeers9m = new Dictionary<float, float>();

    private void Start()
    {
        agentParam = this.gameObject.GetComponent<AgentParameters>();
        peers = agentParam.peers;
        InvokeRepeating(nameof(CalculateDistances), startDelay, repeatInterval);
    }

    private void CalculateDistances()
    {
        float lowestDistance = 100f;

        // Get all colliders within the sphere
        Collider[] objectsInside = Physics.OverlapSphere(transform.position, checkRadius);

        // If no game objects are inside the sphere, set peerPresenceLevel to 3
        if (objectsInside.Length == 0)
        {
            peerPresenceLevel = 3;
            return;
        }

        foreach (var obj in objectsInside)
        {
            if (peers.Contains(obj.gameObject))
            {

                //may need to check if they are on the same y axis. 

                //If there is a peer within 2 meters, stress level is 1 (lowest), 
                //If no peer is within 9 meters, stress level is 3 (highest)
                float distance = Vector3.Distance(this.gameObject.transform.position, obj.gameObject.transform.position);
                if (distance < lowestDistance)
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
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
    private void getStressFromPeerPresence()
    {

    }
}

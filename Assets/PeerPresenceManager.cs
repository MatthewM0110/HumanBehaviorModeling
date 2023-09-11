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
    public Dictionary<float, float> closePeers2m = new Dictionary<float, float>(); // 0 to 2 m
    public Dictionary<float, float> closePeers9m = new Dictionary<float, float>(); // 2 to 9 m
    int interval = 0;
    private void Start()
    {
        agentParam = this.gameObject.GetComponent<AgentParameters>();
        peers = agentParam.peers;
        InvokeRepeating(nameof(CalculateDistances), startDelay, repeatInterval);
    }
    private void CalculateDistances()
    {
        float lowestDistance = 100f;
        int peerCountTotal = 0;
        int peerCount0to2 = 0;  // Counter for peers within 0 to 2 meters
        int peerCount2to9 = 0;  // Counter for peers within 2 to 9 meters
        closePeers2m[interval] = 0;
        closePeers9m[interval] = 0;

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
                peerCountTotal++;  // Increment the total peer counter

                float distance = Vector3.Distance(this.gameObject.transform.position, obj.gameObject.transform.position);
                if (distance < lowestDistance)
                {
                    lowestDistance = distance;
                }

                // Increment the respective counter based on the distance
                if (distance >= 0 && distance < 2)
                {
                    peerCount0to2++;
                    closePeers2m[interval] = peerCount0to2;
                }
                else if (distance >= 2 && distance <= 9)
                {
                    peerCount2to9++;
                    closePeers9m[interval] = peerCount2to9;

                }

                print("agent " + agentParam.name + " is colliding with " + obj.gameObject.name + " who is " + distance + "m away");
            }
        }

        // Print the peer counts
        print("Total number of peers detected: " + peerCountTotal);
        print("Number of peers between 0 to 2 meters: " + peerCount0to2);
        print("Number of peers between 2 to 9 meters: " + peerCount2to9);

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
        interval += 2;
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

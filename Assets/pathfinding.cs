using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class pathfinding : MonoBehaviour
{
    public Transform goal;
    public NavMeshAgent agent;
    [SerializeField]
    public GameObject exit;
    // Start is called before the first frame update
    void Start()
    {
         agent = GetComponent<NavMeshAgent>();
         agent.speed = Random.Range(3, 5);
    }

    // Update is called once per frame
    void Update() {

        agent.destination = exit.transform.position;
        
    }
}

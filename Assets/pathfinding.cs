using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class pathfinding : MonoBehaviour 
    //DEPRECIATED
{
    public Transform goal;
    public NavMeshAgent agent;
    [SerializeField]
    public GameObject exit;
    // Start is called before the first frame update
    void Start()
    {
         agent = this.gameObject.GetComponent<NavMeshAgent>();
         agent.avoidancePriority = Random.Range(0, 99); 
    }

    // Update is called once per frame
    void Update() {

        
        
        
    }
}

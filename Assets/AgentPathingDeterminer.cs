using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentPathingDeterminer : MonoBehaviour
{
    private SimulationManager simulationManager;
    private bool isSpawned = false; //unique for each agent. True if Begin() has been called

    private NavMeshAgent navMeshAgentComponent;
    private AgentParameters agentParameters;

    private Transform destination = null;
    [SerializeField]
    private GameObject[] exits;
    // Start is called before the first frame update
    void Start()
    {
       
        simulationManager = GameObject.Find("SimulationManager").GetComponent<SimulationManager>();

    }

    public void Begin() {
        isSpawned = true;
        navMeshAgentComponent = gameObject.GetComponent<NavMeshAgent>();
        agentParameters = gameObject.GetComponent<AgentParameters>();
        exits = GameObject.FindGameObjectsWithTag("exit");
        if (agentParameters.EmergencyRecognition == AgentParameterGeneration.EmergencyRecognition.Lagging) {
            StartCoroutine(LaggingEvacuation());
        } else {
            setDestination();
        }

    }
    // Update is called once per frame
    void Update()
    {
        if(simulationManager.isAgentsSpawned && !isSpawned ) {
            Begin();
        } else if(simulationManager.simIsRunning) {
           navMeshAgentComponent.destination = destination.transform.position;

        }

    }

    private void setDestination() {

        //Get algo to find "known exits"
        int exitToChoose = Random.Range(0, exits.Length);
        destination = exits[exitToChoose].transform;
    }
    
    IEnumerator LaggingEvacuation() {
        //Print the time of when the function is first called.
       // Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(Random.Range(5,15));
        setDestination();
        //After we have waited 5 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}

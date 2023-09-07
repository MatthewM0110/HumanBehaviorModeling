using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentPathingDeterminer : MonoBehaviour
{

    public enum State
    {
        Calm,
        Herding,
        Panicking
    }
    [SerializeField] AgentParameterGeneration.StressLevel agentStressLevel;
    [SerializeField] private State lastState = State.Calm;
    [SerializeField] private State currentState = State.Calm;
    [SerializeField] private GameObject closestAgent;
    private SimulationManager simulationManager;
    private bool isSpawned = false; //unique for each agent. True if Begin() has been called
    //Navigation 
    private NavMeshAgent navMeshAgentComponent;
    private AgentParameters agentParameters;
    [SerializeField] private Transform destination = null;
    [SerializeField] private Transform closestExit;
    [SerializeField] private GameObject[] exits;
    private GameObject[] closestExits = new GameObject[5];

    public float checkRadius = 100f; // Set this to the desired radius
    private float wanderRadius = 50f;

    private float smallAgentExitThreshold = 0.05f; //as a percentage (5%)
    private float smallAgentExitTimeLimit = 60f; // 60 Seconds
    private float smallAgentExitTimeCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        simulationManager = GameObject.Find("SimulationManager").GetComponent<SimulationManager>();
        agentParameters = gameObject.GetComponent<AgentParameters>();
        exits = GameObject.FindGameObjectsWithTag("exit");


    }

    public void Begin()
    {

        calcExitDistances();
        isSpawned = true;
        navMeshAgentComponent = gameObject.GetComponent<NavMeshAgent>();
        //navMeshAgentComponent.avoidancePriority = Random.Range(0, 99);
        //navMeshAgentComponent.avoidancePriority = 100 - (int)agentParameters.Age ;
        //navMeshAgentComponent.avoidancePriority = (int)agentParameters.Speed;
        navMeshAgentComponent.avoidancePriority = 1;

        //Agent stress initializer
        agentParameters.stressManager = this.gameObject.AddComponent<StressManager>();

        agentParameters.stressManager.DetermineStressLevel();
        agentParameters.stressManager.Begin();
        if (agentParameters.EmergencyRecognition == AgentParameterGeneration.EmergencyRecognition.Lagging)
        {
            print("I have lagging emergency Recognition");
            StartCoroutine(LaggingEvacuation());
        }
        else
        {
            setInitialDestination();
        }

        InvokeRepeating("DecisionState", 0, 10f);

    }
    // Update is called once per frame
    void Update()
    {
        if (simulationManager.isAgentsSpawned && !isSpawned && simulationManager.simIsRunning)
        {
            Begin();
        }


    }

    public void DecisionState()
    {
        agentStressLevel = agentParameters.stressManager.StressLevel;
        
        if (agentStressLevel == AgentParameterGeneration.StressLevel.Low)
        {
            currentState = State.Calm;
            goToExit();
        }
        else if (agentStressLevel == AgentParameterGeneration.StressLevel.Medium)
        {
            currentState = State.Herding;
            FollowClosestAgent();
        }
        else
        {
            currentState = State.Panicking;
            Panicking();
        }

        if (smallAgentExitTimeCount >= smallAgentExitTimeLimit)
        {
            goToExit();
        }
        else if (simulationManager.currentAgents < simulationManager.initialAgentSize * smallAgentExitThreshold)
        {
            smallAgentExitTimeCount += 10;
        }

        //What to do if their stress is high, med, low
        lastState = currentState;
    }
    public void FollowClosestAgent()
    {
        float smallestDistance = float.MaxValue;

        // Get all colliders within the sphere
        Collider[] objectsInside = Physics.OverlapSphere(transform.position, checkRadius);

        foreach (var obj in objectsInside)
        {
            NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
            AgentPathingDeterminer otherAgentPathing = obj.gameObject.GetComponent<AgentPathingDeterminer>();
            if (agent != null && agent != navMeshAgentComponent && otherAgentPathing.currentState != State.Herding && otherAgentPathing.closestAgent != this.gameObject)
            {
                float distance = Vector3.Distance(navMeshAgentComponent.transform.position, agent.transform.position);
                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    closestAgent = agent.gameObject;
                }
            }
        }

        if (closestAgent != null)
        {
            navMeshAgentComponent.destination = closestAgent.transform.position;
        }
    }
    public void Panicking()
    {

        Vector3 randomDirection = Random.insideUnitCircle * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
        navMeshAgentComponent.SetDestination(hit.position);



    }
    private void setInitialDestination()
    {

        int exitsToChoose = 0;

        if (this.agentParameters.SpatialKnowledge == AgentParameterGeneration.SpatialKnowledge.Low)
        {
            exitsToChoose = Random.Range(0, 5);
        }
        else if (this.agentParameters.SpatialKnowledge == AgentParameterGeneration.SpatialKnowledge.Medium)
        {
            exitsToChoose = Random.Range(0, 2);
        }
        else if (this.agentParameters.SpatialKnowledge == AgentParameterGeneration.SpatialKnowledge.High)
        {
            exitsToChoose = 0;
        }

        // Get algo to find "known exits"
        destination = closestExits[exitsToChoose].transform;
        goToExit();
    }


    private void calcExitDistances()
    {
        List<GameObject> allExits = new List<GameObject>(exits);

        // Sort the list of exits based on their distances from the current GameObject
        allExits.Sort((a, b) => Vector3.Distance(this.gameObject.transform.position, a.transform.position)
                                .CompareTo(Vector3.Distance(this.gameObject.transform.position, b.transform.position)));

        // Store the 5 closest exits in the 'closestExits' array
        for (int i = 0; i < 5 && i < allExits.Count; i++)
        {
            closestExits[i] = allExits[i];
        }

        // Print the closest exits and their distances for debugging purposes
        foreach (GameObject exit in closestExits)
        {
            //Debug.Log($"Exit: {exit.name}, Distance: {Vector3.Distance(this.gameObject.transform.position, exit.transform.position)}");
        }
    }

    private void goToExit()
    {

        navMeshAgentComponent.destination = destination.transform.position;

    }


    IEnumerator LaggingEvacuation()
    {
        //Print the time of when the function is first called.
        // Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(Random.Range(10, 25));
        setInitialDestination();
        //After we have waited 5 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentParameters : MonoBehaviour {
   
   
    [Header("Agent Parameters")]

    [SerializeField] private AgentParameterGeneration.Gender gender;
    [SerializeField] private AgentParameterGeneration.MovementPercentChange movementPercentChange;
    [SerializeField] private AgentParameterGeneration.SpatialKnowledge spatialKnowledge;
    [SerializeField] private AgentParameterGeneration.EmergencyRecognition emergencyRecognition;
    [SerializeField] private float speed;
    [SerializeField] private float stress;
    [SerializeField] private float nervousness;
    [SerializeField] private float age;
    [SerializeField] private float timeToEvacuate;


    private NavMeshAgent navMeshAgent;
    private pathfinding pathfinding;
    private Material maleMaterial;
    private Material femaleMaterial;

    public AgentParameterGeneration.Gender Gender { get => gender; set => gender = value; }
    public AgentParameterGeneration.MovementPercentChange MovementPercentChange { get => movementPercentChange; set => movementPercentChange = value; }
    public AgentParameterGeneration.SpatialKnowledge SpatialKnowledge { get => spatialKnowledge; set => spatialKnowledge = value; }
    public AgentParameterGeneration.EmergencyRecognition EmergencyRecognition { get => emergencyRecognition; set => emergencyRecognition = value; }
    public float Speed { get => speed; set => speed = value; }
    public float Stress { get => stress; set => stress = value; }
    public float Nervousness { get => nervousness; set => nervousness = value; }
    public float Age { get => age; set => age = value; }
    public float TimeToEvacuate { get => timeToEvacuate; set => timeToEvacuate = value; }

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        pathfinding = gameObject.GetComponent<pathfinding>();

        pathfinding.exit = GameObject.FindGameObjectWithTag("exit");
        navMeshAgent.speed = speed;

        maleMaterial = Resources.Load<Material>("Male");
        femaleMaterial = Resources.Load<Material>("Female");

        if (gender == AgentParameterGeneration.Gender.Male) {
            GetComponent<Renderer>().material = maleMaterial;
        } else {
            GetComponent<Renderer>().material = femaleMaterial;
        }

  

    }

    // Update is called once per frame
    void Update()
    {
       

    }


}

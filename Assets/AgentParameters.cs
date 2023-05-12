using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentParameters : MonoBehaviour {

    private SimulationManager simulationManager;
    public StressManager stressManager;
    [Header("Peers")]
    [SerializeField] public List<GameObject> peers = new List<GameObject>();
    [Header("Agent Parameters")]
    [SerializeField] private int uniqueID;
    [SerializeField] private AgentParameterGeneration.Gender gender;
    [SerializeField] private AgentParameterGeneration.MovementPercentChange movementPercentChange;
    [SerializeField] private AgentParameterGeneration.SpatialKnowledge spatialKnowledge;
    [SerializeField] private AgentParameterGeneration.EmergencyRecognition emergencyRecognition;
    [SerializeField] private AgentParameterGeneration.Cooperation cooperation;
    [SerializeField] private AgentParameterGeneration.EmergencyTraining emergencyTraining;
    [SerializeField] private float speed;
    [SerializeField] private float mobilityStress;
    
    [SerializeField] private float stressThreshold;
    [SerializeField] private float nervousness;
    [SerializeField] private float age;
    [SerializeField] private float timeToEvacuate;
  


    private NavMeshAgent navMeshAgent;
    private Material maleMaterial;
    private Material femaleMaterial;

    public int UniqueID { get => uniqueID; set => uniqueID = value; }
    public AgentParameterGeneration.Gender Gender { get => gender; set => gender = value; }
    public AgentParameterGeneration.MovementPercentChange MovementPercentChange { get => movementPercentChange; set => movementPercentChange = value; }
    public AgentParameterGeneration.SpatialKnowledge SpatialKnowledge { get => spatialKnowledge; set => spatialKnowledge = value; }
    public AgentParameterGeneration.EmergencyRecognition EmergencyRecognition { get => emergencyRecognition; set => emergencyRecognition = value; }
    public AgentParameterGeneration.Cooperation Cooperation { get => cooperation; set => cooperation = value; }
    public AgentParameterGeneration.EmergencyTraining EmergencyTraining { get => emergencyTraining; set => emergencyTraining = value; }
    public float Speed { get => speed; set => speed = value; }
    public float MobilityStress { get => mobilityStress; set => mobilityStress = value; }
    public float Nervousness { get => nervousness; set => nervousness = value; }
    public float Age { get => age; set => age = value; }
    public float TimeToEvacuate { get => timeToEvacuate; set => timeToEvacuate = value; }
    

    // Start is called before the first frame update
    void Start()
    {
        stressManager = gameObject.GetComponent<StressManager>();
        simulationManager = GameObject.Find("SimulationManager").GetComponent<SimulationManager>(); 
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
       
        navMeshAgent.speed = speed;

        maleMaterial = Resources.Load<Material>("Male");
        femaleMaterial = Resources.Load<Material>("Female");

        TimeToEvacuate = 0;

        if (gender == AgentParameterGeneration.Gender.Male) {
            GetComponent<Renderer>().material = maleMaterial;
        } else {
            GetComponent<Renderer>().material = femaleMaterial;
        }

        stressManager.Stress = mobilityStress;
  

    }

    // Update is called once per frame
    void Update()
    {
        if (simulationManager.simIsRunning) {
            TimeToEvacuate += Time.deltaTime;
        }


    }

    public int getTrainingStressLevel()
    {
        if(this.emergencyTraining == AgentParameterGeneration.EmergencyTraining.Low)
        {
            return 1;
        }
        if (emergencyTraining == AgentParameterGeneration.EmergencyTraining.Medium)
        {
            return 2;
        }
        else
        {
            return 3;
        }

    }
    


}

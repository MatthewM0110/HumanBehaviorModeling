using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentParameters : MonoBehaviour
{
    [Header("Agent Parameters")]
    private AgentParameterGeneration.Gender gender;
    private AgentParameterGeneration.MovementPercentChange movementPercentChange;
    private AgentParameterGeneration.SpatialKnowledge spatialKnowledge;
    private float speed;
    private float stress;
    private float nervousness;
    public AgentParameterGeneration.Gender Gender { get => gender; set => gender = value; }
    public AgentParameterGeneration.MovementPercentChange MovementPercentChange { get => movementPercentChange; set => movementPercentChange = value; }
    public AgentParameterGeneration.SpatialKnowledge SpatialKnowledge { get => spatialKnowledge; set => spatialKnowledge = value; }
    public float Speed { get => speed; set => speed = value; }
    public float Stress { get => stress; set => stress = value; }
    public float Nervousness { get => nervousness; set => nervousness = value; }


    private NavMeshAgent navMeshAgent;
    private pathfinding pathfinding;
    public Material maleMaterial;
    public Material femaleMaterial;

    // Start is called before the first frame update
    void Start()
    {
        pathfinding = gameObject.GetComponent<pathfinding>();
        pathfinding.exit = GameObject.FindGameObjectWithTag("exit");

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

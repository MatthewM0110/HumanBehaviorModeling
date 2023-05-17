using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting;

public class StressManager : MonoBehaviour
{



    [SerializeField] private float stressFromPeerPresence;
    [SerializeField] private float stressFromTraining;
    [SerializeField] private float stressFromCooperation;
    [SerializeField] private float stressFromDisability;
    [SerializeField] private float stressFromPersonality;
    [SerializeField] private float stressFromMovement;

    private int mobilityWeight;
    private int trainingWeight;
    private int cooperationWeight;
    private int movementWeight;
    private int peerPresenceWeight;

    private float agentSpeed;

    private AgentParameters agentParameters;
    private PeerPresenceManager peerPresenceManager;

    [SerializeField] private float currentStress;
    [SerializeField] private float maxStress;
    [SerializeField] private float averageStress;
    [SerializeField] private int stressUpdateCount;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private float distanceMoved;

    private float totalDistanceMoved = 0f;
    private int numUpdates = 0;
    [SerializeField] private float averageDistanceMoved = 0f;

    [SerializeField] private AgentParameterGeneration.StressLevel stressLevel;
    private Vector3 position;
    public float Stress { get => currentStress; set => currentStress = value; }
    public float MaxStress { get => maxStress; set => maxStress = value; }
    public float AverageStress { get => averageStress; set => averageStress = value; }
    public AgentParameterGeneration.StressLevel StressLevel { get => stressLevel; set => stressLevel = value; }

    void Start()
    {
        agentParameters = gameObject.GetComponent<AgentParameters>();
        peerPresenceManager = gameObject.GetComponent<PeerPresenceManager>();
        sphereCollider = gameObject.GetComponent<SphereCollider>();
        agentSpeed = agentParameters.Speed;

    }
    public void Begin()
    {
        SimulationManager sim = FindObjectOfType<SimulationManager>();

        //Get simulation weights
        mobilityWeight = sim.mobilityWeight;
        trainingWeight = sim.trainingWeight;
        cooperationWeight = sim.cooperationWeight;
        movementWeight = sim.movementWeight;
        peerPresenceWeight = sim.peerPresenceWeight;


        maxStress = 0;
        averageStress = 0;
        stressUpdateCount = 0;
        position = new Vector3(0, 0, 0);
        InvokeRepeating("UpdateStress", 0, 1f);
        InvokeRepeating("GetDistanceMoved", 0, 3f);
        numUpdates = 0;
        totalDistanceMoved = 0;
        position = this.gameObject.transform.position;

    }

    void Update()
    {
         
    }
    
    public void DetermineStressLevel()
    {
        if (currentStress <= 0.85)
        {
            StressLevel = AgentParameterGeneration.StressLevel.Low;
        }
        else if (currentStress > 0.85 && currentStress <= 1.5)
        {
            StressLevel = AgentParameterGeneration.StressLevel.Medium;
        }
        else if (currentStress > 1.5)
        {
            StressLevel = AgentParameterGeneration.StressLevel.High;
        }
       
    }

    private void UpdateStress()
    {
        // Add  factors that affect stress here and update the currentStress value
        // Example: currentStress += someStressFactor;
        /*
         *
         *
         *Stress related to personality*associated weight+ Stress level due to peer 
         *presence*associated weight+Stsress due to training * associated weight + Stress due to cooperation
         ** associated weight +Stress related to signage clarity* 
         *associated weight +Stress related to time spent on a block* associated weight.
         */


        //Here we can get the values and weights to quantify stress per agent. This is calculated every second. 
        //Mobility, Peer presence (reduces stress), Training, Cooperation, Movement 

        //Stress level from evacuation movement



        currentStress = calculateCurrentStress();

        if (currentStress > maxStress)
        {
            maxStress = currentStress;
        }



        // Update averageStress
        stressUpdateCount++;
        averageStress = ((averageStress * (stressUpdateCount - 1)) + currentStress) / stressUpdateCount;

        // Update stress level
        DetermineStressLevel();
        stressFromMovement = calculateStressFromMovement();
    }

    private void GetDistanceMoved()
    {

        float distance = Vector3.Distance(position, this.gameObject.transform.position);
        //How far the agent moved since 10 seconds. Recalculated every 10s. in Meters. 
        distanceMoved = distance;
        position = this.gameObject.transform.position;

        // Update the total distance moved and the number of updates
        totalDistanceMoved += distanceMoved;
        numUpdates++;

        // Update the average
    
    }
   
    private float calculateCurrentStress()
    {

        //how do we quantify stress from these factors?
        // float stressFromMobility = agentParameters.MobilityStress;
        stressFromPeerPresence = peerPresenceManager.peerPresenceLevel;
        stressFromTraining = agentParameters.getTrainingStressLevel();
        stressFromCooperation = 1;
        stressFromMovement = calculateStressFromMovement();
        stressFromPersonality = 1;
        //StressFromDisability??? or Mobility
        float calculatedStress =
           //  stressFromMobility * sim.mobilityWeight +
           stressFromPeerPresence * (intToFraction(peerPresenceWeight)) +
           stressFromTraining * (intToFraction(trainingWeight)) +
           stressFromMovement * (intToFraction(movementWeight))                              
        
          ;
        print("Current stress of _" + calculatedStress);

        return calculatedStress;
    }

    private float calculateStressFromMovement()
    {
        
        float ratio = distanceMoved / agentSpeed;
        float stressLevel;
        
        // Check which range the ratio falls into and assign the corresponding stress level
        if (ratio >= 0 && ratio < 0.50)
        {
            stressLevel = 3;
        }
        else if (ratio >= 0.50 && ratio < 0.75)
        {
            stressLevel = 2;
        }
        else
        {
            stressLevel = 1;
        }

        return stressLevel;
    }

    private float intToFraction(int i)
    {
        return (float)i/100;
    }

}

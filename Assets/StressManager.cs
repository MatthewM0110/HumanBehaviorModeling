using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class StressManager : MonoBehaviour
{



    [SerializeField] private float stressFromPeerPresence;
    [SerializeField] private float stressFromTraining;
    [SerializeField] private float stressFromCooperation;
    [SerializeField] private float stressFromMovement;
    [SerializeField] private float stressFromPersonality;


    private AgentParameters agentParameters;
    private PeerPresenceManager peerPresenceManager;

    [SerializeField] private float currentStress;
    [SerializeField] private float maxStress;
    [SerializeField] private float averageStress;
    [SerializeField] private int stressUpdateCount;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private float distanceMoved;
    
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
        maxStress = 0;
        averageStress = 0;
        stressUpdateCount = 0;
        position = new Vector3(0, 0, 0);
        InvokeRepeating("UpdateStress", 0, 1f);
        InvokeRepeating("GetDistanceMoved", 0, 3f);

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
    }

    private void GetDistanceMoved()
    {

        float distance = Vector3.Distance(position, this.gameObject.transform.position);
        //How far the agent moved since 10 seconds. Recalculated every 10s. in Meters. 
        distanceMoved = distance;
        position = this.gameObject.transform.position;

        
    }

    private float calculateCurrentStress()
    {

        //how do we quantify stress from these factors?
        SimulationManager sim = FindObjectOfType<SimulationManager>();
        // float stressFromMobility = agentParameters.MobilityStress;
        stressFromPeerPresence = peerPresenceManager.peerPresenceLevel;
        stressFromTraining = agentParameters.getTrainingStressLevel();
        stressFromCooperation = 1;
        //stressFromMovement =  agentParameters.;
        stressFromPersonality = 1;

        float calculatedStress =
           //  stressFromMobility * sim.mobilityWeight +
           stressFromPeerPresence * ((float)sim.peerPresenceWeight / 100) +
           stressFromTraining * ((float)sim.trainingWeight / 100) //+
                                                           // stressFromCooperation * (sim.cooperationWeight / 100)

          ;
        print("CUrrent stress of _" + calculatedStress);

        return calculatedStress;
    }


}

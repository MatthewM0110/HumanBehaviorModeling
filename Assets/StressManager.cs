using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class StressManager : MonoBehaviour
{
    private AgentParameters agentParameters;

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
        sphereCollider = gameObject.GetComponent<SphereCollider>();
        maxStress = 0;
        averageStress = 0;
        stressUpdateCount = 0;
        position = new Vector3(0, 0, 0);
        InvokeRepeating("UpdateStress", 0, 1f);
        InvokeRepeating("GetDistanceMoved", 0, 3f);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (agentParameters.peers.Contains(collision.gameObject))
        {
            print("Collided with a peer!");
        }
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

        //HOW DO WE QUANTIFY STRESS FROM THESE FACTORS?
        SimulationManager sim = FindObjectOfType<SimulationManager>();
        float stressFromMobility = agentParameters.MobilityStress;
        float stressFromPeerPresence = 1;
        float stressFromTraining = 1;
        float stressFromCooperation = 1;
        float stressFromMovement = 1;

        float currentStress =
            stressFromMobility * sim.mobilityWeight +
            stressFromPeerPresence * sim.peerPresenceWeight +
            stressFromTraining * sim.trainingWeight +
            stressFromCooperation * sim.cooperationWeight +
            stressFromMovement * sim.movementWeight;

        return currentStress;
    }
}

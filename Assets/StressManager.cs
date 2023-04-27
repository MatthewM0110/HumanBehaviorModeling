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

    [SerializeField] private AgentParameterGeneration.StressLevel stressLevel;

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
        InvokeRepeating("UpdateStress", 0, 1f);

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
}

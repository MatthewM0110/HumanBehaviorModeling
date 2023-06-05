using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CooperationManager : MonoBehaviour
{
    public float radius = 2f; // radius of the sphere for detection
    [SerializeField] public int cooperationStress;
   
    [SerializeField] public float averageCooperationLevel;
    private AgentParameters agentParameters;

    private float originalSpeed;
    void Start()
    {
        agentParameters = this.gameObject.GetComponent<AgentParameters>();
        originalSpeed = agentParameters.originalSpeed;
        InvokeRepeating(nameof(CalculateCooperationStress), 0f, 2f);
    }



    public int getCooperationStress() { return cooperationStress; }

    void CalculateCooperationStress()
    {
        float averageCooperation = GetAverageCooperation();
        averageCooperationLevel = averageCooperation;
        // Assign a score based on the average cooperation
        int score;
        if (averageCooperation <= 1.66f)
        {
            score = 3; //3, high stress due to low cooperation
        }
        else if (averageCooperation <= 2.33f)
        {
            score = 2;
        }
        else
        {
            score = 1;
        }

        cooperationStress = score;
        Debug.Log("Cooperation Score: " + score);

        ModifyAgentSpeed();
    }

    private void ModifyAgentSpeed()
    {
       if(cooperationStress == 1)
        {
            agentParameters.Speed = originalSpeed * 0.9f;

        }
        else if(cooperationStress == 2){

            agentParameters.Speed = originalSpeed;

        }else if(cooperationStress == 3)
        {
            agentParameters.Speed = originalSpeed * 1.1f;
        }
    }

    float GetAverageCooperation()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        int totalCooperation = 0;
        int count = 0;

        if (hitColliders.Count() == 0)
        {
            return 2;
        }

        foreach (var hitCollider in hitColliders)
        {
            AgentParameters agent = hitCollider.gameObject.GetComponent<AgentParameters>();

            if (agent != null)
            {
                totalCooperation += (int)agent.Cooperation;
                count++;
            }
        }

        return count > 0 ? (float)totalCooperation / count : 0;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

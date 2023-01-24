using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentParameterGeneration : MonoBehaviour {

    [SerializeField]
    private GameObject spawnPoint;
    private AgentParameterChances agentParameterChances;

    public enum MovementPercentChange {

        None = 0,
        WakingStick = 15,
        Crutch = 10,
        WheelChair = 10

    }
    public enum Gender {
        Male,
        Female
    }

    public enum SpatialKnowledge {
        Low, 
        Medium, 
        High
    }

    public enum EmergencyRecognition {
        Immediate, 
        Lagging
    }
    private void Awake() {

        agentParameterChances = gameObject.GetComponent<AgentParameterChances>();
    }
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void GenerateAgents() {
        int agentPop = 50;
        GameObject agentPrefab = Resources.Load<GameObject>("Agent");

        for (int i = 0; i < agentPop; i++) {
            GameObject agent = Instantiate(agentPrefab, null, spawnPoint.transform);
            agent.transform.name = "Agent" + i;
            AgentParameters agentParam = agent.AddComponent<AgentParameters>();
            agentParam.Gender = calcGender();



        }


    }

    private Gender calcGender() {

        float generateRandomFloat = Random.Range(1f, 99f);
        if(generateRandomFloat < agentParameterChances.malePercentage) {
            return Gender.Male;
        } else {
            return Gender.Female;
        }

    }

}
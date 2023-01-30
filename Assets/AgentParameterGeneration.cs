using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentParameterGeneration : MonoBehaviour {

    [SerializeField]
    private GameObject spawnPoint;
    private AgentParameterChances agentParameterChances;
    private GameObject agentPrefab;

    public enum MovementPercentChange {

        None = 0, //0% change
        WakingStick = 15, // 15% change 
        Crutch = 10, // 10% change  
        WheelChair = 10 // 10% change

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

       agentPrefab = Resources.Load<GameObject>("Agent");
       spawnPoint = GameObject.FindGameObjectWithTag("spawn");

    }

    // Update is called once per frame
    void Update() {

    }

    public void GenerateAgents() {
        int agentPop = 50;


        for (int i = 0; i < agentPop; i++) {

            GameObject agent = Instantiate(agentPrefab, spawnPoint.transform.position, Quaternion.identity);
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
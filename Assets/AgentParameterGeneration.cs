using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor.U2D;
using UnityEngine;

public class AgentParameterGeneration : MonoBehaviour {

    private static System.Random random = new System.Random();


    [SerializeField]
    private GameObject spawnPoint;
    private AgentParameterChances agentParameterChances;
    private AgentInitialSpeedGeneration agentInitialSpeedGeneration;
    private GameObject agentPrefab;
    private SimulationManager simulationManager;
    private int agentPop;
    private int disabledAgentCount;
    public enum MovementPercentChange {

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
       agentInitialSpeedGeneration = gameObject.GetComponent<AgentInitialSpeedGeneration>();
        simulationManager = GameObject.Find("SimulationManager").GetComponent<SimulationManager>(); 
     }

    // Update is called once per frame
    void Update() {

    }

    public void GenerateAgents() {
        agentPop = 10;
        int agentSpawned = 0;
        try {
             agentPop = int.Parse(agentParameterChances.numberOfAgents.text);
        } catch {
             
        }
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("spawn");
        for (int j = 0; j < spawnPoints.Length; j++) {
            for (int i = 0; i < agentPop; i++) {

                GameObject agent = Instantiate(agentPrefab, spawnPoints[j].transform.position, Quaternion.identity);
                agent.transform.name = "Agent" + i;
                AgentParameters agentParam = agent.AddComponent<AgentParameters>();
                Gender gender = calcGender();
                int age = calcAge();
                MovementPercentChange movementPercentChange = calculateDisabilities();
                agentParam.Gender = gender;
                agentParam.Age = age;
                Debug.Log(age + " " + gender);
                agentParam.MovementPercentChange = movementPercentChange;
                agentParam.Speed = calcSpeed(age, gender, movementPercentChange);
                agentParam.EmergencyRecognition = calcEmergencyRecognition();
                agentSpawned++;
            }
        }
        simulationManager.currentAgents = agentSpawned;
        simulationManager.isAgentsSpawned = true;

    }

    private float calcSpeed(int age, Gender gender, MovementPercentChange movementPercentChange) {

        float speed;
        if (gender.Equals(Gender.Male)) {

            speed =  (agentInitialSpeedGeneration.getMaleValueGivenKey(age));

        } else {

             speed =  (agentInitialSpeedGeneration.getFemaleValueGivenKey(age));

        }

        print("mPc:" + (float)movementPercentChange);

        if((float)movementPercentChange != 0) {
            speed = speed * ((float)movementPercentChange/100);
        }
        return speed;
    }

    private Gender calcGender() {

        float generateRandomFloat = UnityEngine.Random.Range(1f, 99f);
        if(generateRandomFloat < agentParameterChances.malePercentage) {
            return Gender.Male;
        } else {
            return Gender.Female;
        }

    }

    private int calcAge() {

        float min = agentParameterChances.minAge;
        float max = agentParameterChances.maxAge;

        return random.Next((int)min, (int)max + 1);
    }

    private MovementPercentChange calculateDisabilities() {
        float percentOfDisabledAgents = agentParameterChances.maxPercentOfDisabledAgents;
        float numberOfAgents = agentPop;
        float numberOfDisabledAgentsToCreate = numberOfAgents * (percentOfDisabledAgents/100f);
        if(disabledAgentCount < numberOfDisabledAgentsToCreate) {

            var values = Enum.GetValues(typeof(MovementPercentChange));
            var random = new System.Random();
            var randomMovementPercentChange = (MovementPercentChange)values.GetValue(random.Next(values.Length));
            return randomMovementPercentChange;

        }
        return 0;
    }
    private EmergencyRecognition calcEmergencyRecognition() {

        //Later will be affected by Environment Knowdlege and Experience
        float generateRandomFloat = UnityEngine.Random.Range(1, 99);

        if(generateRandomFloat < 25) {
            return EmergencyRecognition.Immediate;
        }

        return EmergencyRecognition.Lagging;
    }
}
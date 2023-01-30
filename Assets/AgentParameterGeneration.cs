using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentParameterGeneration : MonoBehaviour {

    private static System.Random random = new System.Random();

    [SerializeField]
    private GameObject spawnPoint;
    private AgentParameterChances agentParameterChances;
    private AgentInitialSpeedGeneration agentInitialSpeedGeneration;
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
        agentInitialSpeedGeneration = gameObject.GetComponent<AgentInitialSpeedGeneration>();
     }

    // Update is called once per frame
    void Update() {

    }

    public void GenerateAgents() {
        int agentPop = 10;
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

                agentParam.Gender = gender;
                agentParam.Age = age;
                Debug.Log(age + " " + gender);
                agentParam.Speed = calcSpeed(age, gender);

            }
        }

    }

    private float calcSpeed(int age, Gender gender) {


        if (gender.Equals(Gender.Male)) {

           return (agentInitialSpeedGeneration.getMaleValueGivenKey(age));

        } else {

            return (agentInitialSpeedGeneration.getFemaleValueGivenKey(age));

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

    private int calcAge() {

        float min = agentParameterChances.minAge;
        float max = agentParameterChances.maxAge;

        return random.Next((int)min, (int)max + 1);
    }


}
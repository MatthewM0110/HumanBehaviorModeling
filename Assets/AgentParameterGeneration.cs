using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor.U2D;
using UnityEngine;

public class AgentParameterGeneration : MonoBehaviour
{

    private static System.Random random = new System.Random();


    [SerializeField]
    private GameObject spawnPoint;
    private AgentParameterChances agentParameterChances;
    private AgentInitialSpeedGeneration agentInitialSpeedGeneration;
    private GameObject agentPrefab;
    private SimulationManager simulationManager;
    private int agentPop;
    [SerializeField]
    private int disabledAgentCount = 0;
    List<GameObject> activeAgents = new List<GameObject>();


    GameObject[] spawnPointsSmall;
    GameObject[] spawnPointsMed;
    GameObject[] spawnPointsLarge;
    GameObject[] spawnPointsXL;

    public enum MovementPercentChange
    {

        WakingStick = 15, // 15% change 
        Crutch = 10, // 10% change  
        WheelChair = 10 // 10% change

    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum SpatialKnowledge
    {
        Low,
        Medium,
        High
    }

    public enum EmergencyRecognition
    {
        Immediate,
        Lagging
    }

    public enum StressLevel // will affect decision making
    {
        Low = 1,
        Medium,
        High
    }
    public enum EmergencyTraining 
    {
        Low = 1,
        Medium, 
        High  
    }
    public enum Cooperation
    {
        Low = 1, //1
        Medium, //2
        High //3
    }

    public enum PeerPresence
    {
        Low = 1,    
        Medium,
        High
    }

    public enum Movement
    {
        Low, 
        Medium, 
        High
    }
 
    private void Awake()
    {
        agentParameterChances = gameObject.GetComponent<AgentParameterChances>();
    }
    // Start is called before the first frame update
    void Start()
    {

        agentPrefab = Resources.Load<GameObject>("Agent");
        spawnPoint = GameObject.FindGameObjectWithTag("spawn");
        agentInitialSpeedGeneration = gameObject.GetComponent<AgentInitialSpeedGeneration>();
        simulationManager = GameObject.Find("SimulationManager").GetComponent<SimulationManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int agentSpawned = 0;
    //Iteratively Generates Agents based off default or user-defined value
    public void GenerateAgents()
    {

        print("Generate");
        agentPop = 10;
        int multiplyAgent = 1;

        try
        {
            multiplyAgent = int.Parse(agentParameterChances.numberOfAgents.text);
        }
        catch
        {

        }
        spawnPointsSmall = GameObject.FindGameObjectsWithTag("spawnSmall");
        spawnPointsMed = GameObject.FindGameObjectsWithTag("spawnMedium");
        spawnPointsLarge = GameObject.FindGameObjectsWithTag("spawnLarge");
        spawnPointsXL = GameObject.FindGameObjectsWithTag("spawnXL");

        GameObject[][] allSpawns = new GameObject[][]
        {
            spawnPointsSmall,
            spawnPointsMed,
            spawnPointsLarge,
            spawnPointsXL
        };

        for (int k = 0; k < allSpawns.Length; k++)
        { //Loops through allSpawns
            //print(allSpawns.Length);
            for (int j = 0; j < allSpawns[k].Length; j++)
            { //Loops through each spawnPoints i
                switch (k)
                {
                    //small
                    case 0:
                        agentPop = UnityEngine.Random.Range(1, 3);
                        agentPop = agentPop * multiplyAgent;
                        break;
                    //Med
                    case 1:
                        agentPop = UnityEngine.Random.Range(8, 15);
                        agentPop = agentPop * multiplyAgent;

                        break;
                    //Large
                    case 2:
                        agentPop = UnityEngine.Random.Range(15, 20);
                        agentPop = agentPop * multiplyAgent;

                        break;
                    //XL
                    case 3:
                        agentPop = UnityEngine.Random.Range(20, 30);
                        agentPop = agentPop * multiplyAgent;

                        break;

                }
                for (int i = 0; i < agentPop; i++)
                {

                    GameObject agent = Instantiate(agentPrefab, allSpawns[k][j].transform.position, Quaternion.identity);
                    createAgent(agent);

                }
            }
        }

        foreach(GameObject agent in activeAgents)
        {
            //int numberOfPeers = UnityEngine.Random.Range(1,4);
            AgentParameterChances agentParameterChances = FindObjectOfType<AgentParameterChances>();
            if(agentParameterChances == null)
            {
                print("AGENT PARAM IS NULL");
            }
            int numberOfPeers = (int)(activeAgents.Count * FindObjectOfType<AgentParameterChances>().percentOfPeers/100f);
          
            print("Agent Pop" + agentPop + " " + activeAgents.Count);
            print("PercentOfPeers" + numberOfPeers);
            for (int i = 0;i < numberOfPeers;i++)
            {
                GameObject randomAgent = activeAgents[UnityEngine.Random.Range(0, (int)activeAgents.Count)];
                AgentParameters agentParam = agent.GetComponent<AgentParameters>();

                if (randomAgent != agent && !agentParam.peers.Contains(randomAgent))
                {
                    agent.GetComponent<AgentParameters>().peers.Add(randomAgent);

                }
            }
            
        }
        
        simulationManager.currentAgents = agentSpawned;
        simulationManager.isAgentsSpawned = true;

    }

    private void createAgent(GameObject agent)
    {
        AgentParameters agentParam = agent.AddComponent<AgentParameters>();
       // StressManager stressManager = agent.AddComponent<StressManager>();
        PeerPresenceManager peerPresenceManager = agent.AddComponent<PeerPresenceManager>();
        Gender gender = calcGender();
        int age = calcAge();
        //Gender gender = Gender.Male;
        //int age = 40;
        MovementPercentChange movementPercentChange = calcDisabilities();
        agentParam.MovementPercentChange = movementPercentChange;
        agentParam.Gender = gender;
        agentParam.Age = age;
        //Debug.Log(age + " " + gender);
        agentParam.Speed = calcSpeed(age, gender, movementPercentChange);
        agentParam.EmergencyRecognition = calcEmergencyRecognition();
        agentParam.SpatialKnowledge = calcSpatialKnowledge();
        agentSpawned++;
        agent.transform.name = "Agent" + agentSpawned;
        agentParam.UniqueID = agentSpawned;
        agent.transform.rotation = new Quaternion(90f, -0.5f, 0.25f, 0);
        agentParam.MobilityStress = calcInitialStress(agentParam.Speed);
        agentParam.EmergencyTraining = calcEmergencyTraining();
        ///stressManager.DetermineStressLevel();
        activeAgents.Add(agent);
       

    }


    private float calcInitialStress(float speed)
    {

        return 1 / speed;

    }
    private float calcSpeed(int age, Gender gender, MovementPercentChange movementPercentChange)
    {

        float speed;
        if (gender.Equals(Gender.Male))
        {

            speed = (agentInitialSpeedGeneration.getMaleValueGivenKey(age));

        }
        else
        {

            speed = (agentInitialSpeedGeneration.getFemaleValueGivenKey(age));

        }

        print("mPc:" + (float)movementPercentChange);

        if ((float)movementPercentChange != 0)
        {
            speed = speed - (speed * ((float)movementPercentChange / 100));
        }
        return speed;
    }

    private Gender calcGender()
    {

        float generateRandomFloat = UnityEngine.Random.Range(1f, 99f);
        if (generateRandomFloat < agentParameterChances.malePercentage)
        {
            return Gender.Male;
        }
        else
        {
            return Gender.Female;
        }

    }

    private int calcAge()
    {

        float min = agentParameterChances.minAge;
        float max = agentParameterChances.maxAge;

        return random.Next((int)min, (int)max + 1);
    }

    private MovementPercentChange calcDisabilities()
    {
        float percentOfDisabledAgents = agentParameterChances.maxPercentOfDisabledAgents;

        float randomValue = UnityEngine.Random.Range(0f, 1f);
        if (randomValue < percentOfDisabledAgents / 100f)
        {
            var values = Enum.GetValues(typeof(MovementPercentChange));
            var random = new System.Random();
            var randomMovementPercentChange = (MovementPercentChange)values.GetValue(random.Next(values.Length));
            disabledAgentCount++;
            print(disabledAgentCount);
            return randomMovementPercentChange;
        }


        return 0;
    }
    private EmergencyRecognition calcEmergencyRecognition()
    {

        //Later will be affected by Environment Knowdlege and Experience
        float generateRandomFloat = UnityEngine.Random.Range(1, 99);

        if (generateRandomFloat < 25)
        {
            return EmergencyRecognition.Immediate;
        }

        return EmergencyRecognition.Lagging;
    }
    private EmergencyTraining calcEmergencyTraining()
    {

        //75% are low, 15% are med, and 10% high
        float generateRandomFloat = UnityEngine.Random.Range(1, 99);

        if (generateRandomFloat < 75)
        {
            return EmergencyTraining.Low;
        }else if(generateRandomFloat < 90)
        {
            return EmergencyTraining.Medium;

        }

         return EmergencyTraining.High;

    }
    private SpatialKnowledge calcSpatialKnowledge()
    {
        //30 % high, 30% medium, 40% low
        // Array values = Enum.GetValues(typeof(SpatialKnowledge));
        // System.Random random = new System.Random();
        // SpatialKnowledge spatialKnowledge = (SpatialKnowledge)values.GetValue(random.Next(values.Length));
        // return spatialKnowledge;
        System.Random random = new System.Random();
        int randomValue = random.Next(100);

        if (randomValue < 40) // 40% chance for Low
        {
            return SpatialKnowledge.Low;
        }
        else if (randomValue < 70) // 30% chance for Medium
        {
            return SpatialKnowledge.Medium;
        }
        else // 30% chance for High
        {
            return SpatialKnowledge.High;
        }
    }

    private Array calculateKnownExits()
    {





        return null;

    }
}
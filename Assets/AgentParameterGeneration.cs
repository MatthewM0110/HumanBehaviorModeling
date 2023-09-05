using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        WalkingStick = 15, // 15% change 
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
        Low = 1, //1  No cooperation, high stress
        Medium, //2
        High //3 high cooperation, low stress
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
        float multiplyAgent = 1;

        float scalingFactor = multiplyAgent;  // Renamed from multiplyAgent for clarity
        float u1, u2;  // Uniformly-distributed random variables
        float randStdNormal;  // Standard normally-distributed variable
        float randNormal;  // Normally-distributed variable with custom mean and stdDev

        try
        {
            //should be a percentage of total peers. Ie 80 will be 80% of normal spawn rates
            multiplyAgent = int.Parse(agentParameterChances.numberOfAgents.text);
            //multiplyAgent = 10;
            multiplyAgent = multiplyAgent / 100;
            print("agent multiplying" + multiplyAgent);
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
                /*
                switch (k)
                {
                    //small
                    case 0:
                        agentPop = UnityEngine.Random.Range(1, 3);
                        agentPop = (int)Math.Round(agentPop * multiplyAgent);

                        break;
                    //Med
                    case 1:
                        agentPop = UnityEngine.Random.Range(10, 15);
                        agentPop = (int)Math.Round(agentPop * multiplyAgent);


                        break;
                    //Large
                    case 2:
                        agentPop = UnityEngine.Random.Range(20, 25);
                        agentPop = (int)Math.Round(agentPop * multiplyAgent);

                        break;
                    //XL
                    case 3:
                        agentPop = UnityEngine.Random.Range(30, 40);
                        agentPop = (int)Math.Round(agentPop * multiplyAgent);

                        break;

                }*/

                switch (k)
                {
                    case 0:  // Small
                        u1 = UnityEngine.Random.Range(0f, 1f);
                        u2 = UnityEngine.Random.Range(0f, 1f);
                        randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);  // Box-Muller transform
                        randNormal = 2 + randStdNormal * 1;  // mean=2, stdDev=1
                        agentPop = (int)Mathf.Round(randNormal * scalingFactor);
                        if (agentPop < 0)
                        {
                            agentPop = 0;  // Ensure population is non-negative
                        }
                        Debug.Log($"Case: Small, Agent Population: {agentPop}");

                        break;

                    case 1:  // Medium
                        u1 = UnityEngine.Random.Range(0f, 1f);
                        u2 = UnityEngine.Random.Range(0f, 1f);
                        randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);  // Box-Muller transform
                        randNormal = 12.5f + randStdNormal * 2.5f;  // mean=12.5, stdDev=2.5
                        agentPop = (int)Mathf.Round(randNormal * scalingFactor);
                        Debug.Log($"Case: Medium, Agent Population: {agentPop}");
                        break;
                       

                    case 2:  // Large
                        u1 = UnityEngine.Random.Range(0f, 1f);
                        u2 = UnityEngine.Random.Range(0f, 1f);
                        randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);  // Box-Muller transform
                        randNormal = 22.5f + randStdNormal * 2.5f;  // mean=22.5, stdDev=2.5
                        agentPop = (int)Mathf.Round(randNormal * scalingFactor);
                        Debug.Log($"Case: Large, Agent Population: {agentPop}");

                        break;

                    case 3:  // Extra Large
                        u1 = UnityEngine.Random.Range(0f, 1f);
                        u2 = UnityEngine.Random.Range(0f, 1f);
                        randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);  // Box-Muller transform
                        randNormal = 35f + randStdNormal * 5f;  // mean=35, stdDev=5
                        agentPop = (int)Mathf.Round(randNormal * scalingFactor);
                        Debug.Log($"Case: XL, Agent Population: {agentPop}");

                        break;
                }

                for (int i = 0; i < agentPop; i++)
                {

                    GameObject agent = Instantiate(agentPrefab, allSpawns[k][j].transform.position, Quaternion.identity);
                    createAgent(agent); 

                }
            }
        }
        //All agents have been created, we could ensure disabilities percentages are accuracte

        int totalAgents = activeAgents.Count; // Assuming activeAgents contains all the agents
        int desiredDisabledAgentCount = (int)Math.Round(totalAgents * (agentParameterChances.maxPercentOfDisabledAgents / 100f));
        int discrepancy = desiredDisabledAgentCount - disabledAgentCount;
        if (discrepancy > 0)
        {
            List<GameObject> nonDisabledAgents = activeAgents.Where(a => a.GetComponent<AgentParameters>().MovementPercentChange == 0).ToList();
            // Randomly choose agents from nonDisabledAgents and set them to be disabled
            for (int i = 0; i < discrepancy; i++)
            {
                int indexToDisable = UnityEngine.Random.Range(0, nonDisabledAgents.Count);
                GameObject agentToDisable = nonDisabledAgents[indexToDisable];
                AgentParameters agentParam = agentToDisable.GetComponent<AgentParameters>();
                agentParam.MovementPercentChange = MovementPercentChange.Crutch; // This could be any disabled status

                // Recalculate speed after changing the disability status
                agentParam.Speed = calcSpeed((int)agentParam.Age, agentParam.Gender, agentParam.MovementPercentChange);

                nonDisabledAgents.RemoveAt(indexToDisable);
                disabledAgentCount++;
            }
        }
        else if (discrepancy < 0)
        {
            List<GameObject> disabledAgents = activeAgents.Where(a => a.GetComponent<AgentParameters>().MovementPercentChange != 0).ToList();
            // Randomly choose agents from disabledAgents and set them to be non-disabled
            for (int i = 0; i < Math.Abs(discrepancy); i++)
            {
                int indexToEnable = UnityEngine.Random.Range(0, disabledAgents.Count);
                GameObject agentToEnable = disabledAgents[indexToEnable];
                AgentParameters agentParam = agentToEnable.GetComponent<AgentParameters>();
                agentParam.MovementPercentChange = 0; // This sets the agent to be non-disabled

                // Recalculate speed after changing the disability status
                agentParam.Speed = calcSpeed((int)agentParam.Age, agentParam.Gender, agentParam.MovementPercentChange);

                disabledAgents.RemoveAt(indexToEnable);
                disabledAgentCount--;
            }
        }
        foreach (GameObject agent in activeAgents)
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
        simulationManager.BeginSimulation();

    }

    private void createAgent(GameObject agent)
    {
        AgentParameters agentParam = agent.AddComponent<AgentParameters>();
       // StressManager stressManager = agent.AddComponent<StressManager>();
        PeerPresenceManager peerPresenceManager = agent.AddComponent<PeerPresenceManager>();
        CooperationManager cooperationManager = agent.AddComponent<CooperationManager>();
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
        agentParam.Cooperation = calcCooperation();
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
    public Cooperation calcCooperation()
    {
        // Generate a random number between 1 and 3
        int randomNum = UnityEngine.Random.Range(1, 4);

        // Convert the random number to a Cooperation value
        Cooperation cooperation = (Cooperation)randomNum;

        return cooperation;
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
        // Set mu to be closer to min to ensure peak is around 20-30
        float mu = min + 5; // Mean (peak of the distribution)

     
        float sigma = (max - min) / 3; // Standard deviation (spread)

        // Create a normally distributed value using Box-Muller transform
        double u1 = 1.0 - random.NextDouble();
        double u2 = 1.0 - random.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        double randNormal = mu + sigma * randStdNormal;

        // Clamp the value between min and max
        int age = (int)Math.Round(Math.Clamp(randNormal, min, max));

        return age;
    }
    

    private MovementPercentChange calcDisabilities()
    {
        //Change to be exact percent of disabled Agents. 
        float percentOfDisabledAgents = agentParameterChances.maxPercentOfDisabledAgents;

        float randomValue = UnityEngine.Random.Range(0f, 1f);
        if (randomValue < percentOfDisabledAgents / 100f)
        {
            var values = Enum.GetValues(typeof(MovementPercentChange));
            var random = new System.Random();
            var randomMovementPercentChange = (MovementPercentChange)values.GetValue(random.Next(values.Length));
            disabledAgentCount++;
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

        if (randomValue < 40) // 40% chance for Loww
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
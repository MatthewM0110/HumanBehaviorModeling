using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

public class SimulationManager : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField]
    private GameObject agentGenerator;
    [SerializeField]
    private GameObject dataCollector;
    [SerializeField]
    public bool simIsRunning;
    [SerializeField]
    public bool isAgentsSpawned = false;
    [SerializeField]
    public int currentAgents;
    [SerializeField]
    public TMP_Text currentAgentsDisplay;
    [SerializeField]
    Slider simulationSpeedSlider;
    [SerializeField]
    TMP_Text simulationSpeedText;
    [SerializeField]
    [Range(1, 15)]
    public float timeScale = 1;

    [SerializeField] private bool agentsSpawned;

    [SerializeField]
    public int testNumber;
    [SerializeField]
    private int trial = 0;

    [Header("Stress Weights")]
    // [SerializeField]
    // [Range(0, 100)] public int mobilityWeight = 20;
    [SerializeField]
    [Range(0, 100)] public int trainingWeight = 20;
    [SerializeField]
    [Range(0, 100)] public int cooperationWeight = 20;
    [SerializeField]
    [Range(0, 100)] public int movementWeight = 20; //ie Time spend without moving
    [SerializeField]
    [Range(0, 100)] public int peerPresenceWeight = 20;

    //Disability weight?

    public string[] targetTags = { "spawnSmall", "spawnMedium", "spawnLarge", "spawnXL" };
    void Start()
    {

        // Array to hold the target tags
        PrintObjectCountsForTags();

    }
    // Function to count and print the number of objects for each specified tag
    public void PrintObjectCountsForTags()
    {
        foreach (string tag in targetTags)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
            int count = objectsWithTag.Length;
            Debug.Log("Number of objects with tag '" + tag + "': " + count);
        }
    }


public void OnValidate()
    {
        NormalizeWeights();
    }
    private void runTest()
    {
        if (trial <= testNumber)
        {
            SpawnAgents();
            
          
            trial++;
            
        }
        
    }
    private void NormalizeWeights()
    {
      //  int totalWeight = mobilityWeight + trainingWeight + cooperationWeight + movementWeight + peerPresenceWeight;
        int totalWeight =  trainingWeight + cooperationWeight + movementWeight + peerPresenceWeight;
        //mobilityWeight = 100 * mobilityWeight / totalWeight;
        trainingWeight = 100 * trainingWeight / totalWeight;
        cooperationWeight = 100 * cooperationWeight / totalWeight;
        movementWeight = 100 * movementWeight / totalWeight;
        peerPresenceWeight = 100 * peerPresenceWeight / totalWeight;

    }
    // Update is called once per frame
    void Update() {

        currentAgentsDisplay.text = currentAgents.ToString();
        if(currentAgents <= 0) {
            simIsRunning = false;
           agentsSpawned = false;
            //dataCollector.GetComponent<DataCollection>().exportData();
            //runTest();
         
        }
        
    }

    public void SpawnAgents() {

            agentGenerator.GetComponent<AgentParameterGeneration>().GenerateAgents(); //Access AgentParamGen script and calls GenAgent funct;'
        
       
    }

    public void BeginSimulation() {
      
            simIsRunning = true;
            dataCollector.GetComponent<DataCollection>().beginDataCollection(); //Access AgentParamGen script and calls GenAgent funct
            CameraController mainCamController = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<CameraController>();
            mainCamController.CameraTarget = GameObject.Find("Agent1").gameObject.transform;

        
    }
    public void setSimulationSpeed(float newSpeed)
    {

        timeScale =  newSpeed;
        Time.timeScale = timeScale;
        simulationSpeedText.text = "Simulation Speed: " + timeScale.ToString() + "x";

    }
}

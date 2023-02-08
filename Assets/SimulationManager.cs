using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using TMPro;


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
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        currentAgentsDisplay.text = currentAgents.ToString();
        if(currentAgents <= 0) {
            simIsRunning = false;
        }
    }

    public void SpawnAgents() {

        agentGenerator.GetComponent<AgentParameterGeneration>().GenerateAgents(); //Access AgentParamGen script and calls GenAgent funct
    }

    public void BeginSimulation() {

        simIsRunning = true;
        dataCollector.GetComponent<DataCollection>().beginDataCollection(); //Access AgentParamGen script and calls GenAgent funct
        CameraController mainCamController = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<CameraController>();
        mainCamController.CameraTarget = GameObject.Find("Agent0").gameObject.transform;
    }

}

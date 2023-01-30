using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class SimulationManager : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField]
    private GameObject agentGenerator;
    [SerializeField]
    private GameObject dataCollector;
    [SerializeField]
    public bool simIsRunning;
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    public void BeginSimulation() {

        simIsRunning = true;
        agentGenerator.GetComponent<AgentParameterGeneration>().GenerateAgents(); //Access AgentParamGen script and calls GenAgent funct
        dataCollector.GetComponent<DataCollection>().beginDataCollection(); //Access AgentParamGen script and calls GenAgent funct
        CameraController mainCamController = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<CameraController>();
        mainCamController.CameraTarget = GameObject.Find("Agent0").gameObject.transform;
    }

}

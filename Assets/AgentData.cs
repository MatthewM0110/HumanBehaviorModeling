using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering;

public class AgentData : MonoBehaviour {

    private AgentParameters currentAgentParameter;
    private SimulationManager simulationManager;
    private DataCollection dataCollection;

    // Start is called before the first frame update
    void Start()
    {
        simulationManager = GameObject.Find("SimulationManager").GetComponent<SimulationManager>();
        dataCollection = GameObject.Find("DataCollector").GetComponent<DataCollection>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other) {

        if(other.tag.Equals("exit")) { //If the agent reaches an exit, collect their data.
            string disability;                   
            currentAgentParameter = this.gameObject.GetComponent<AgentParameters>();
            
            if(currentAgentParameter.MovementPercentChange == 0) {
                disability = "None";
            } else {
                disability = currentAgentParameter.MovementPercentChange.ToString();

            }
            dataCollection.addDataRow(currentAgentParameter.TimeToEvacuate, currentAgentParameter.Age, currentAgentParameter.Gender, disability, currentAgentParameter.SpatialKnowledge, currentAgentParameter.EmergencyRecognition, currentAgentParameter.MobilityStress, 0, 0, currentAgentParameter.stressManager.Stress, currentAgentParameter.peers.Count);
            simulationManager.currentAgents--;
            Destroy(this.gameObject);
        }
    }

    
}

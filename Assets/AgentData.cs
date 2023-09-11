using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering;

public class AgentData : MonoBehaviour {

    private AgentParameters currentAgentParameter;
    private SimulationManager simulationManager;
    private DataCollection dataCollection;
    private Dictionary<float, float> stressData = new Dictionary<float, float>();
    private Dictionary<float, float> averageStressData = new Dictionary<float, float>();

    private float timeInterval = 2f;
    private float interval = 0f; 
    // Start is called before the first frame update
    void Start()
    {
        currentAgentParameter = this.gameObject.GetComponent<AgentParameters>();
        simulationManager = GameObject.Find("SimulationManager").GetComponent<SimulationManager>();
        dataCollection = GameObject.Find("DataCollector").GetComponent<DataCollection>();
        InvokeRepeating("addStressValue", 0f, timeInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void addStressValue()
    {
        stressData[interval] = this.currentAgentParameter.stressManager.Stress; //Should I get average stress or current stress?
        averageStressData[interval] = this.currentAgentParameter.stressManager.AverageStress; //Should I get average stress or current stress?

        interval += timeInterval;
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
            dataCollection.addDataRow(currentAgentParameter.UniqueID, currentAgentParameter.TimeToEvacuate, currentAgentParameter.Age, currentAgentParameter.Gender, disability, currentAgentParameter.SpatialKnowledge, currentAgentParameter.EmergencyRecognition, currentAgentParameter.EmergencyTraining, currentAgentParameter.MobilityStress, currentAgentParameter.stressManager.MaxStress, currentAgentParameter.stressManager.AverageStress, currentAgentParameter.stressManager.Stress, currentAgentParameter.peers.Count);
            dataCollection.addStressDataRow(currentAgentParameter.UniqueID, stressData);
            dataCollection.addAverageStressDataRow(currentAgentParameter.UniqueID, averageStressData);
            dataCollection.addAverageCooperationDataRow(currentAgentParameter.UniqueID, this.gameObject.GetComponent<CooperationManager>().averageCooperationDict);
            dataCollection.addClosePeers2mDataRow(currentAgentParameter.UniqueID, this.gameObject.GetComponent<PeerPresenceManager>().closePeers2m);
            dataCollection.addClosePeers9mDataRow(currentAgentParameter.UniqueID, this.gameObject.GetComponent<PeerPresenceManager>().closePeers9m);

            simulationManager.currentAgents--;

            Destroy(this.gameObject);
        }
    }

    
}

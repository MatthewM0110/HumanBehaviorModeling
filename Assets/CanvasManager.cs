using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private Button spawnAgentButton;
    [SerializeField]
    private Button runSimulationButton;
    [SerializeField]
    public SimulationManager simulationManager;
    // Start is called before the first frame update
    void Start()
    {
        runSimulationButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
       if(simulationManager.isAgentsSpawned) {
            runSimulationButton.interactable = true;
            
        }
    }
}

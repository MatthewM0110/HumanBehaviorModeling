using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;
using System.IO;
using System;



public class DataCollection : MonoBehaviour
{
    [SerializeField]
    public TMP_Text timeElapsed;
    private bool isDataCollecting;
    [SerializeField]
    private SimulationManager simulationManager;
    DataTable masterDataTable = new DataTable();
    DataTable stressDataTable = new DataTable();
    string date;

    string masterDataFilePath;
    string stressDataFilePath; 

    // Start is called before the first frame update
    private float timeT = 0;
    void Start()
    {
        masterDataTable.Columns.Add("UniqueID");
        masterDataTable.Columns.Add("Time to Evacuate");
        masterDataTable.Columns.Add("Age");
        masterDataTable.Columns.Add("Gender");
        masterDataTable.Columns.Add("Disability");
        masterDataTable.Columns.Add("SpatialKnowledge");
        masterDataTable.Columns.Add("Observation of Environment");
        masterDataTable.Columns.Add("Emergency Training");
        masterDataTable.Columns.Add("Initial Stress");
        masterDataTable.Columns.Add("Maximum Stress");
        masterDataTable.Columns.Add("Average Stress");
        masterDataTable.Columns.Add("Stress on Exit");
        masterDataTable.Columns.Add("Number of Peers");

        stressDataTable.Columns.Add("AgentID");


        date = DateTime.Now.ToString("dd-MM-yyyy&HH-mm");

        // Define the folder based on the current date
        string folderPath = Application.dataPath + "/Data/" + date;

        // Check if the directory exists, if not, create it
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        else
        {
        }

        // Now, define your file paths inside this folder
        masterDataFilePath = folderPath + "/MasterData.csv";
        stressDataFilePath = folderPath + "/StressData.csv";

        
    

}

    // Update is called once per frame
    void Update()
    {
        if (isDataCollecting && simulationManager.simIsRunning) {
            timeT += Time.deltaTime;
        }
        timeElapsed.text = (timeT).ToString();
        if (Input.GetKeyDown(KeyCode.D)) {
            ExportDataTableToCSV(masterDataTable, masterDataFilePath);
            ExportDataTableToCSV(stressDataTable, stressDataFilePath);

        }

    }
    public void exportData()
    {
        ExportDataTableToCSV(masterDataTable, masterDataFilePath);
    }
    public void beginDataCollection() {
        isDataCollecting = true;
    }
    public void addDataRow(int agentID, float timeToEvacuate, float age ,AgentParameterGeneration.Gender gender, 
        String disability, AgentParameterGeneration.SpatialKnowledge spatialKnowledge, AgentParameterGeneration.EmergencyRecognition emergencyRecognition, 
        AgentParameterGeneration.EmergencyTraining emergencyTraining,
        float initialStress, float maxStress, float averageStress, float exitStress, int numberOfPeers) {

        masterDataTable.Rows.Add(agentID.ToString(), timeToEvacuate.ToString(), age.ToString(), gender.ToString(), disability.ToString(), spatialKnowledge, emergencyRecognition, emergencyTraining
            , initialStress, maxStress, averageStress, exitStress, numberOfPeers);
    }

    public void addStressDataRow(int agentID, Dictionary<float, float> stressData)
    {
        // Convert the dictionary values (stress scores) into a list
        List<float> stressScores = new List<float>(stressData.Values);

        // Check if the table has enough columns; if not, add them
        while (stressDataTable.Columns.Count < stressScores.Count + 1)  // +1 for the AgentID column
        {
            stressDataTable.Columns.Add("Interval " + stressDataTable.Columns.Count);
        }

        // Create a new data row
        DataRow newRow = stressDataTable.NewRow();

        // Set the agent's ID as the first column's value
        newRow["AgentID"] = agentID.ToString();

        // Fill in the rest of the columns with stress scores
        for (int i = 0; i < stressScores.Count; i++)
        {
            newRow["Interval " + (i + 1)] = stressScores[i].ToString();
        }

        // Add the new row to the data table
        stressDataTable.Rows.Add(newRow);
    }

    private void ExportDataTableToCSV(DataTable dataTable, string filePath) {
        print("Saving data...");
        // Create the CSV file
        StreamWriter sw = new StreamWriter(filePath, false);

        // Write the headers
        int columnCount = dataTable.Columns.Count;
        for (int i = 0; i < columnCount; i++) {
            sw.Write(dataTable.Columns[i]);
            if (i < columnCount - 1) {
                sw.Write(",");
            }
        }
        sw.Write(sw.NewLine);

        // Write the data
        foreach (DataRow dataRow in dataTable.Rows) {
            for (int i = 0; i < columnCount; i++) {
                if (!Convert.IsDBNull(dataRow[i])) {
                    string value = dataRow[i].ToString();
                    if (value.Contains(",")) {
                        value = String.Format("\"{0}\"", value);
                    }
                    sw.Write(value);
                }
                if (i < columnCount - 1) {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
        }

        // Close the file
        sw.Close();
        print("Data Saved!!");
    }

}

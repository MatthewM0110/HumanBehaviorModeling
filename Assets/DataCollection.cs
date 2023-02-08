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
    DataTable data = new DataTable();
    string filePath = Application.dataPath + "/Data/data.csv";

    // Start is called before the first frame update
    private float timeT = 0;
    void Start()
    {
        data.Columns.Add("Time to Evacuate");
        data.Columns.Add("Age");
        data.Columns.Add("Gender");
    }

    // Update is called once per frame
    void Update()
    {
        if (isDataCollecting && simulationManager.simIsRunning) {
            timeT += Time.deltaTime;
        }
        timeElapsed.text = (timeT).ToString();
        if (Input.GetKeyDown(KeyCode.D)){
            ExportDataTableToCSV(data, filePath);
        }

    }

    public void beginDataCollection() {
        isDataCollecting = true;
    }
    public void addDataRow(float timeToEvacuate, float age ,AgentParameterGeneration.Gender gender) {

        data.Rows.Add(timeToEvacuate.ToString(), age.ToString(), gender.ToString() );
    }

    private void ExportDataTableToCSV(DataTable dataTable, string filePath) {
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
    }

}

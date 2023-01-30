using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
public class AgentInitialSpeedGeneration : MonoBehaviour
{
    string maleCSVFilePath = "maleSpeedPerAge.csv";
    string femaleCSVFilePath = "femaleSpeedPerAge.csv";
    public Dictionary<int, float> maleSpeedPerAge = new Dictionary<int, float>();
    public Dictionary<int, float> femaleSpeedPerAge = new Dictionary<int, float>();

    // Start is called before the first frame update
    void Start()
    {
        
        maleSpeedPerAge = LoadCSV(maleCSVFilePath);
        femaleSpeedPerAge = LoadCSV(femaleCSVFilePath);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private Dictionary<int, float> LoadCSV(string fileName) {

        Dictionary<int, float> temp = new Dictionary<int, float>();
        string filePath = Path.Combine(Application.dataPath,"Resources/"+ fileName);
        print(filePath);
        if (File.Exists(filePath)) {
            string[] csvLines = File.ReadAllLines(filePath);

            for (int i = 0; i < csvLines.Length; i++) {
                string[] lineData = csvLines[i].Split(',');
                int key = int.Parse(lineData[0]);
                float value = float.Parse(lineData[1]);

                temp.Add(key, value);
            }
            //PrintDictionary(temp);
            return temp;
        } else {
            Debug.LogError("File not found: " + filePath);
        }
        return null;
    }

    private void PrintDictionary(Dictionary<int, float> dictionary) {
        foreach (KeyValuePair<int, float> pair in dictionary) {
            Debug.Log(pair.Key + ": " + pair.Value);
        }
    }

    public float getMaleValueGivenKey(int age) {

        maleSpeedPerAge.TryGetValue(age, out float value);
        return value;


    }
    public float getFemaleValueGivenKey(int age) {

        femaleSpeedPerAge.TryGetValue(age, out float value);
        return value;

    }





}

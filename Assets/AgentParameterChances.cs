using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AgentParameterChances : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [Header("Gender")]
    [SerializeField]
    Slider genderPercentageSlider;
    [SerializeField]
    TMP_Text genderPercentageDisplay;
    [Range(1, 99)]
    public float malePercentage;
    [Range(1, 99)]
    public float femalePercentage;

    public void setGenderPercentage(float percent)
    {
        malePercentage = percent;
        femalePercentage = 100 - percent;
        genderPercentageDisplay.text = "Male Percentage: " + malePercentage.ToString();

    }

    [Header("Age")]
    [SerializeField]
    Slider minAgeSlider;
    [SerializeField]
    TMP_Text minAgeDisplay;
    [SerializeField]
    Slider maxAgeSlider;
    [SerializeField]
    TMP_Text maxAgeDisplay;
    [Range(10, 99)]
    public float minAge = 10;
    [Range(10, 99)]
    public float maxAge = 99;
    public void setMinAge(float newMinAge) {
        minAge = newMinAge;
        maxAgeSlider.minValue = minAge;
        minAgeSlider.maxValue = maxAge;
        minAgeDisplay.text = "Minimum Age: " + minAge.ToString();

    }
    public void setMaxAge(float newMaxAge) {
        maxAge = newMaxAge;
        maxAgeSlider.minValue = minAge;
        minAgeSlider.maxValue = maxAge;
        maxAgeDisplay.text = "Maxiumum Age: " + maxAge.ToString();

    }


    float meanAge;
    float averageAge;
    public void setMeanAge(float newMeanAge) {

        meanAge = newMeanAge;
    }
    public void setAverageAge(float newAverageAge) {
        averageAge = newAverageAge;
    }

    [Header("Disabilities")]
    [SerializeField]
    Slider maxPercentOfDisabledAgentsSlider;
    [SerializeField]
    TMP_Text maxPercentOfDisabledAgentsDisplay;
    [Range(0, 10)]
    public float maxPercentOfDisabledAgents = 10;

    public void setMaxPerDisabledAgents(float newPercent) {

        maxPercentOfDisabledAgents = newPercent;
        maxPercentOfDisabledAgentsDisplay.text = "Maximum % of Disabled Agents: " + maxPercentOfDisabledAgents.ToString();

    }
   


}

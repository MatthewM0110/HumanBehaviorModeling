using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DataCollection : MonoBehaviour
{
    [SerializeField]
    public TMP_Text timeElapsed;
    private bool isDataCollecting;
    // Start is called before the first frame update
    private float timeT;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (isDataCollecting) {
            timeT += Time.deltaTime;
            timeElapsed.text = (timeT).ToString();

        }
    }

    public void beginDataCollection() {
        isDataCollecting = true;
    }
}

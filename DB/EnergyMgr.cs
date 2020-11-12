using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyMgr : MonoBehaviour
{
    public bool isFull;

    int energy;

    float refill_Time;

    public Text txtEnergy;
    public Text l_time;
    
    void Start()
    {
        l_time.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFull)
        {
            
        }
    }

    void reFilled()
    {
        isFull = true;
        l_time.text = "";
    }

    //////////////// 나중에 완성하는 걸로 //////////////////////
}

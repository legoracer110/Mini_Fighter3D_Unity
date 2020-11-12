using Shgames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Last : MonoBehaviour
{
    GameObject enemy;
    GameObject TutMgr;
    void Start()
    {
        enemy = GameObject.Find("enemyAI_Berserker");
        TutMgr = GameObject.Find("TutorialMgr");
    }

    
    void Update()
    {
        if(enemy.GetComponent<aiCtrl>().isFall || enemy.GetComponent<aiCtrl>().isDie)
        {
            TutMgr.GetComponent<TutorialMgr>().Tutclear();
            this.gameObject.SetActive(false);
        }
    }
}

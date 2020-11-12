using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{    
    int rand;
    bool isDone = false;
    float fTime = 3f;

    public bool isTest;

    AsyncOperation async_operation;

    public GameObject[] advice;

    void Start()
    {
        rand = Random.Range(0, 5);
        advice[rand].SetActive(true);

        switch (Game.current.getMode())
        {
            // 0 : mainMenu
            // 1 : practice
            // 2 : tutorial
            // 3 : upgrade
            // 4 : story
            // 5 : tournament
            // 6 : custom
            // 7 : online            
            // 10 : customize
            case 0:
                // Main Menu
                StartCoroutine(StartLoad("scMain"));
                break;
            case 1:
                // Practice
                StartCoroutine(StartLoad("scPractice"));
                break;
            case 2:
                // Tutorial
                StartCoroutine(StartLoad("scTutorial"));
                break;
            case 3:
                // Upgrade
                StartCoroutine(StartLoad("scUpgrade"));
                break;
            case 4:
                // Story
                StartCoroutine(StartLoad("scStory"));
                break;
            case 5:
                // Tournament (Not Yet)
                break;
            case 6:
                // Custom Battle
                // Energy(피로도) 소모
                if (userData.data.getEnergy() == 10)
                {
                    userData.data.setKeyTime(System.DateTime.Now);
                }
                userData.data.decEnergy();
                if (Game.current.getStage()==0)                
                    StartCoroutine(StartLoad("map_Octagon"));
                else if (Game.current.getStage() == 1)
                    StartCoroutine(StartLoad("map_Skycity"));
                else if (Game.current.getStage() == 2)
                    StartCoroutine(StartLoad("map_Matrix"));
                break;
            case 7:
                // Online-Mode (Not Yet)
                break;
            case 10:
                StartCoroutine(StartLoad("scCustomize"));
                break;
        }
    }

    private void Update()
    {
        if (isTest)
        {
            fTime += Time.deltaTime;

            if (fTime >= 3)
            {
                async_operation.allowSceneActivation = true;
            }
        }
        else
        {
            async_operation.allowSceneActivation = true;
        }
    }

    public IEnumerator StartLoad(string strSceneName)
    {
        async_operation = SceneManager.LoadSceneAsync(strSceneName);
        async_operation.allowSceneActivation = false;

        if(isDone == false)
        {
            isDone = true;
            while (async_operation.progress < 0.9f)
                yield return true;
        }
    }
}

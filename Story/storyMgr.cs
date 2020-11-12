using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class storyMgr : MonoBehaviour
{
    public bool isPause;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    public GameObject comboPanel;
    public GameObject[] comboList;

    int stageNum = 1;

    public GameObject[] stages;

    void Start()
    {
        //storyData.currStory = new storyData();

        stageNum = storyData.currStory.getStage();

        stages[stageNum - 1].SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
            {
                isPause = false;
                Time.timeScale = 1;
                pausePanel.SetActive(false);
            }
            else
            {
                isPause = true;
                pausePanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void OnClickResume()
    {
        isPause = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void OnClickComboList()
    {
        switch (stageNum)
        {
            case 1: //  BLUE
                comboList[0].SetActive(true);
                break;
            case 2: //  RED
                comboList[1].SetActive(true);
                break;
            case 3: //  ORANGE
                comboList[2].SetActive(true);
                break;
            case 4: //  RED
                comboList[1].SetActive(true);
                break;
        }
        
        comboPanel.SetActive(true);
    }

    public void OnClickCancel()
    {
        comboPanel.SetActive(false);
    }

    public void Retry()
    {
        storyData.currStory.inc_GameOver();
        Game.current.setMode(4);
        SceneManager.LoadScene("scLoading");
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        Invoke("retryGame", 3f);    
    }

    void retryGame()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void OnClickGoToMainMenu()
    {
        Game.current.setMode(0);
        storyData.currStory.reset_GameOver();
        SceneManager.LoadScene("scLoading");
        Time.timeScale = 1;
    }
}

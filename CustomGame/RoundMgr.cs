using Shgames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoundMgr : MonoBehaviour
{
    public bool isTest;
    public bool isWinTest;
    public bool isLoseTest;

    int playerIndex;
    int enemyIndex;

    int roundNum;    

    public Text txtRound;
    
    public GameObject[] players;
    public GameObject[] enemies;

    public Transform playerPos;
    public Transform enemyPos;

    GameObject playerChar;
    GameObject playerModel;
    GameObject enemyChar;

    public GameObject canvas;
    Animator animator;

    public GameObject resultPanel;

    public GameObject scEnd;
    public GameObject[] models;
    GameObject endModel;

    public GameObject pausePanel;
    bool isPause;

    public GameObject img_Combo;
    public GameObject[] imgCombo;

    public GameObject[] RED_HEAD;    
    public GameObject[] RED_TOP;
    public GameObject[] RED_ACC;
    public GameObject[] RED_ETC;
    public GameObject[] RED_ETC2;

    public GameObject[] BLUE_HEAD;
    public GameObject[] BLUE_TOP;
    public GameObject[] BLUE_ACC;
    public GameObject[] BLUE_ETC;
    public GameObject[] BLUE_ETC2;

    public GameObject[] ORANGE_HEAD;
    public GameObject[] ORANGE_TOP;
    public GameObject[] ORANGE_ACC;
    public GameObject[] ORANGE_ETC;
    public GameObject[] ORANGE_ETC2;

    void tmpSetting()
    {
        // Test용 Setting
        Game.current = new Game();
        userData.data = new userData();

        Game.current.setRound(1);
        Game.current.setSelected(1);
        Game.current.setEnemy(0);
    }

    void winTest()
    {
        playerWin();
    }

    void loseTest()
    {
        enemyWin();
    }

    void InitGame()
    {
        roundNum = roundDB.rdb.getTotalRound();
        txtRound.text = "Round " + (roundNum+1);

        playerIndex = Game.current.getSelected();
        enemyIndex = Game.current.getEnemy();

        playerChar = players[playerIndex];
        enemyChar = enemies[enemyIndex];
        playerChar.SetActive(true);
        playerModel = GameObject.Find("Player");

        playerModel.transform.position = playerPos.position;
        enemyChar.transform.position = enemyPos.position;

        enemyChar.SetActive(true);

        Game.current.setLock(false);

        endCostumeSetting(playerIndex);
    }

    void endCostumeSetting(int charIndex)
    {
        // 앤딩 세레모니에 코스튬 적용
        int[] equip;
        switch (charIndex)
        {
            case 0:
                equip = userData.data.blue.getEquip();
                BLUE_HEAD[equip[0]].SetActive(true);
                BLUE_TOP[equip[1]].SetActive(true);
                BLUE_ACC[equip[2]].SetActive(true);
                BLUE_ETC[equip[3]].SetActive(true);
                BLUE_ETC2[equip[4]].SetActive(true);
                break;
            case 1:
                equip = userData.data.red.getEquip();
                RED_HEAD[equip[0]].SetActive(true);
                RED_TOP[equip[1]].SetActive(true);
                if (equip[2] == 1 || equip[2] == 3)
                {
                    RED_ACC[equip[2]].SetActive(true);
                    RED_ACC[equip[2] + 1].SetActive(true);
                }
                RED_ETC[equip[3]].SetActive(true);
                RED_ETC2[equip[4]].SetActive(true);
                break;
            case 2:
                equip = userData.data.orange.getEquip();
                ORANGE_HEAD[equip[0]].SetActive(true);
                ORANGE_TOP[equip[1]].SetActive(true);
                ORANGE_ACC[equip[2]].SetActive(true);
                ORANGE_ETC[equip[3]].SetActive(true);
                ORANGE_ETC2[equip[4]].SetActive(true);
                break;
        }
    }

    private void Start()
    {
        if (isTest)
            tmpSetting();

        roundDB.rdb = new roundDB();

        animator = canvas.GetComponent<Animator>();

        InitGame();

        if (isWinTest)
        {
            winTest();
        }

        if (isLoseTest)
        {
            loseTest();
        }
    }

    private void Update()
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

    public void playerWin()
    {
        if (!Game.current.getLock())
        {
            Game.current.setLock(true);
            animator.SetTrigger("win");
            roundDB.rdb.setPlayerWin();

            if (roundDB.rdb.getPlayerWin() < Game.current.getRound())
                Invoke("NextRound", 5f);
            else
            {
                roundDB.rdb.setVictory();
                Invoke("GameOver", 5f);
            }
        }
    }

    public void enemyWin()
    {
        if (!Game.current.getLock())
        {
            Game.current.setLock(true);
            animator.SetTrigger("lose");
            roundDB.rdb.setEnemyWin();

            if (roundDB.rdb.getEnemyWin() < Game.current.getRound())
                Invoke("NextRound", 5f);
            else
                Invoke("GameOver", 5f);
        }
    }

    void GameOver()
    {        
        animator.SetTrigger("reset");       
       
        Invoke("celemony", 3f);
    }    

    void celemony()
    {
        scEnd.SetActive(true);
        playerChar.SetActive(false);
        enemyChar.SetActive(false);

        switch (playerIndex)
        {
            case 0:
                endModel = models[0];
                break;
            case 1:
                endModel = models[1];
                break;
            case 2:
                endModel = models[2];
                break;
            default:
                break;
        }

        endModel.SetActive(true);

        if (roundDB.rdb.getVictory())
            endModel.GetComponent<Animator>().SetTrigger("win");
        else
            endModel.GetComponent<Animator>().SetTrigger("lose");

        Invoke("Calculate", 3f);
    }

    void Calculate()
    {
        resultPanel.SetActive(true);
        resultPanel.GetComponent<GameOverPanel>().Calculate();
    }    

    void NextRound()
    {
        // 다음 라운드로 (체력과 위치 초기화)
        animator.SetTrigger("reset");

        //Game.current.setRound(1);
        

        playerChar.SetActive(false);
        enemyChar.SetActive(false);

        InitGame();
    }    

    public void lvUp()
    {
        animator.SetTrigger("LV_UP");
    }

    public void OnClickGoToMainMenu()
    {
        Time.timeScale = 1;
        Game.current.setMode(0);
        SceneManager.LoadScene("scLoading");
    }

    public void OnClickResume()
    {
        isPause = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }    

    public void OnClickComboList()
    {        
        imgCombo[playerIndex].SetActive(true);
        img_Combo.SetActive(true);
    }

    public void OnClickCancel()
    {
        img_Combo.SetActive(false);    
    }
}

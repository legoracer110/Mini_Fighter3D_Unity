using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public bool isTest;
    // GameOver Panel
    // 초상화
    public GameObject[] imgPlayer;
    public GameObject[] imgEnemy;

    public Text playerName;
    public Text enemyName;
    public GameObject txtWin;
    public GameObject txtLose;
    public Transform trPlayer;
    public Transform trEnemy;

    public Text txtScore;

    public Text txtDmgPlayer;
    public Text txtDmgEnemy;
    public Text txtGrdPlayer;
    public Text txtGrdEnemy;
    public Text txtRefPlayer;
    public Text txtRefEnemy;

    public Image imgDmgBar_player;
    public Image imgDmgBar_enemy;

    public Image imgGrdBar_player;
    public Image imgGrdBar_enemy;

    public Image imgRefBar_player;
    public Image imgRefBar_enemy;

    float dmg_player;
    float dmg_enemy;
    float dmg_rate;

    float grd_player;
    float grd_enemy;
    float grd_rate;

    float ref_player;
    float ref_enemy;
    float ref_rate;

    public bool calculate;

    public GameObject lvPanel;

    // Start is called before the first frame update
    void Start()
    {
        if (isTest)
            tmpRoundData();

        int enemyIndex = Game.current.getEnemy();
        // 초상화 세팅
        imgPlayer[Game.current.getSelected()].SetActive(true);
        imgEnemy[Game.current.getEnemy()].SetActive(true);

        playerName.text = "" + userData.data.getID();
        switch (enemyIndex)
        {            
            case 0: enemyName.text = "BLUE"; break;
            case 1: enemyName.text = "RED"; break;
            case 2: enemyName.text = "ORANGE"; break;
            case 3: enemyName.text = "GRAY"; break;
            default: break;
        }
        txtScore.text = "" + roundDB.rdb.getPlayerWin() + " : " + roundDB.rdb.getEnemyWin();

        if (!roundDB.rdb.getVictory())
        {
            txtWin.transform.position = trEnemy.position;
            txtLose.transform.position = trPlayer.position;
        }

        dmg_player = roundDB.rdb.getDmgPlayer();
        dmg_enemy = roundDB.rdb.getDmgEnemy();
        grd_player = roundDB.rdb.getGrdPlayer();
        grd_enemy = roundDB.rdb.getGrdEnemy();
        ref_player = roundDB.rdb.getRefPlayer();
        ref_enemy = roundDB.rdb.getRefEnemy();

        txtDmgPlayer.text = "" + (int)dmg_player;
        txtDmgEnemy.text = "" + (int)dmg_enemy;
        txtGrdPlayer.text = "" + (int)grd_player;
        txtGrdEnemy.text = "" + (int)grd_enemy;
        txtRefPlayer.text = "" + (int)ref_player;
        txtRefEnemy.text = "" + (int)ref_enemy;

        dmg_rate = dmg_player / (dmg_player + dmg_enemy);
        grd_rate = grd_player / (grd_player + grd_enemy);
        ref_rate = ref_player / (ref_player + ref_enemy);
    }

    void tmpRoundData()
    {
        userData.data = new userData();
        
        Game.current = new Game();
        Game.current.setSelected(1);
        Game.current.setEnemy(0);

        roundDB.rdb = new roundDB();
        roundDB.rdb.setPlayerWin();
        roundDB.rdb.setEnemyWin();
        roundDB.rdb.setPlayerWin();

        roundDB.rdb.setVictory();

        roundDB.rdb.addDmgPlayer(120);
        roundDB.rdb.addDmgEnemy(93);
        for(int i=0; i<9; i++)
            roundDB.rdb.addGrdPlayer();
        for(int i=0; i<6; i++)
            roundDB.rdb.addGrdEnemy();
        for (int i = 0; i < 3; i++)
            roundDB.rdb.addRefPlayer();
        for (int i = 0; i < 2; i++)
            roundDB.rdb.addRefEnemy();
    }
    
    void Update()
    {
        if (calculate)
        {
            if (imgDmgBar_player.fillAmount < dmg_rate)
                imgDmgBar_player.fillAmount += Time.deltaTime * 0.7f;
            if (imgDmgBar_enemy.fillAmount < (1 - dmg_rate))
                imgDmgBar_enemy.fillAmount += Time.deltaTime * 0.7f;

            if (imgGrdBar_player.fillAmount < grd_rate)
                imgGrdBar_player.fillAmount += Time.deltaTime * 0.7f;
            if (imgGrdBar_enemy.fillAmount < (1 - grd_rate))
                imgGrdBar_enemy.fillAmount += Time.deltaTime * 0.7f;

            if (imgRefBar_player.fillAmount < ref_rate)
                imgRefBar_player.fillAmount += Time.deltaTime * 0.7f;
            if (imgRefBar_enemy.fillAmount < (1 - ref_rate))
                imgRefBar_enemy.fillAmount += Time.deltaTime * 0.7f;

        }
    }

    public void Calculate()
    {
        calculate = true;
        Invoke("displayLV_Panel", 5f);
    }

    void displayLV_Panel()
    {
        lvPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectMgr : MonoBehaviour
{
    public enum select
    {
        Character,
        Enemy,
        Stage,
        Difficulty
    }

    public bool isTest;

    public select current;

    public GameObject[] diff_selected;

    public GameObject[] cha;    
    public GameObject imgSelected;
    public Transform[] btnTr;

    public GameObject[] enemy;    
    public GameObject imgSelected_e;
    public Transform[] btnTr_e;

    int characterIndex;
    int enemyIndex;
    int stageIndex;

    // BLUE TMP STAT
    int b_lv;
    int b_power;
    int b_health;
    int b_trick;

    // RED TMP STAT
    int r_lv;
    int r_power;
    int r_health;
    int r_trick;

    // ORANGE TMP STAT
    int o_lv;
    int o_power;
    int o_health;
    int o_trick;

    public Text txtName;
    public Text txtLv;
    public Text txtRank;

    public Text txtEnemyName;

    public Image img_pBar;
    public Image img_hBar;
    public Image img_tBar;


    public GameObject[] nextBtn;

    GameObject cam;
    public Transform camPos;

    GameObject canvas;

    GameObject stage;
    public GameObject[] maps;
    public Text stageName;

    int[] equip;
    public GameObject[] RED_HEAD;
    // 0 : Nothing
    // 1 : HeadGear (Red_Head)
    // 2 : Cap (Red_Head)
    public GameObject[] RED_TOP;
    // 0 : Nothing
    // 1 : Plate Armor (Red_Top)
    public GameObject[] RED_ACC;
    // 0 : Nothing
    // 1 : BoxingGloves_L (Red_Acc)
    // 2 : BoxingGloves_R (Red_Acc)
    // 3 : HandGears_L (Red_Acc)
    // 4 : HandGears_R (Red_Acc)
    public GameObject[] RED_ETC;
    // 0 : Nothing
    // 1 : Bat (Red_Etc)
    // 2 : Shuriken (Red_Etc)
    public GameObject[] RED_AURA;
    // 0 : Nothing
    // 1 : 

    public GameObject[] BLUE_HEAD;
    // 0 : Nothing
    // 1 : Bike Helmet (Blue_Head)
    // 2 : Cap (Blue_Head)
    public GameObject[] BLUE_TOP;
    // 0 : Nothing
    // 1 : Down Vest (Blue_Top)
    public GameObject[] BLUE_ACC;
    // 0 : Nothing
    // 1 :
    public GameObject[] BLUE_ETC;
    // 0 : Nothing
    // 1 : Bat (Blue_Etc)
    // 2 : Shuriken (Blue_Etc)
    public GameObject[] BLUE_AURA;
    // 0 : Nothing
    // 1 : 

    public GameObject[] ORANGE_HEAD;
    public GameObject[] ORANGE_TOP;
    public GameObject[] ORANGE_ACC;
    public GameObject[] ORANGE_ETC;
    public GameObject[] ORANGE_AURA;

    void Start()
    {
        if(isTest)
            tmpNewGame();
        LoadData();
        setDataToPanel();
        setBtnNext();
        cam = GameObject.Find("Main Camera");
        canvas = GameObject.Find("Canvas");

        current = select.Character;
        stage = maps[0];

        CostumeSetting();
    }

    void CostumeSetting()
    {
        // 추후 캐릭터 슬롯 업데이트시
        // 보유 캐릭터만 코스튬 적용 (보유 캐릭터의 데이터만 로드)

        equip = userData.data.blue.getEquip();
        BLUE_HEAD[equip[0]].SetActive(true);
        BLUE_TOP[equip[1]].SetActive(true);
        BLUE_ACC[equip[2]].SetActive(true);
        BLUE_ETC[equip[3]].SetActive(true);
        BLUE_AURA[equip[4]].SetActive(true);

        equip = userData.data.red.getEquip();
        RED_HEAD[equip[0]].SetActive(true);
        RED_TOP[equip[1]].SetActive(true);
        if (equip[2] == 1 || equip[2] == 3)
        {
            RED_ACC[equip[2]].SetActive(true);
            RED_ACC[equip[2] + 1].SetActive(true);
        }
        RED_ETC[equip[3]].SetActive(true);
        RED_AURA[equip[4]].SetActive(true);

        equip = userData.data.orange.getEquip();
        ORANGE_HEAD[equip[0]].SetActive(true);
        ORANGE_TOP[equip[1]].SetActive(true);
        ORANGE_ACC[equip[2]].SetActive(true);
        ORANGE_ETC[equip[3]].SetActive(true);
        ORANGE_AURA[equip[4]].SetActive(true);
    }

    private void Update()
    {
        if (current == select.Enemy)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, camPos.position, Time.deltaTime*3f);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, camPos.rotation, Time.deltaTime*3f);
        }else if(current == select.Stage)
        {
            stage.transform.Rotate(Vector3.down, Time.deltaTime * 10f);
        }
    }

    void tmpNewGame()
    {
        // Main Menu 에서 생성할 Game과 userData 클래스 임시 생성 (테스트용)
        Game.current = new Game();
        userData.data = new userData();

        Game.current.setMode(6);
        // 6이 커스텀 배틀 (ENEMY도 설정)        
    }

    void setBtnNext()
    {
        int mode = Game.current.getMode();
        if (mode == 6)        
            nextBtn[0].SetActive(true);
        else
            nextBtn[1].SetActive(true);
    }

    void LoadData()
    {
        b_lv = userData.data.blue.getLv();
        b_power = userData.data.blue.getPower();
        b_health = userData.data.blue.getHealth();
        b_trick = userData.data.blue.getTrick();

        r_lv = userData.data.red.getLv();
        r_power = userData.data.red.getPower();
        r_health = userData.data.red.getHealth();
        r_trick = userData.data.red.getTrick();

        o_lv = userData.data.orange.getLv();
        o_power = userData.data.orange.getPower();
        o_health = userData.data.orange.getHealth();
        o_trick = userData.data.orange.getTrick();
    }

    void setDataToPanel()
    {
        float powerStat = 0;
        float healthStat = 0;
        float trickStat = 0;

        int lv = 0;

        switch (characterIndex)
        {
            case 0:
                powerStat = (float)b_power / 10;
                healthStat = (float)b_health / 10;
                trickStat = (float)b_trick / 2;
                txtName.text = "BLUE";
                lv = b_lv;
                break;
            case 1:
                powerStat = (float)r_power / 10;
                healthStat = (float)r_health / 10;
                trickStat = (float)r_trick / 2;
                txtName.text = "RED";
                lv = r_lv;
                break;
            case 2:
                powerStat = (float)o_power / 10;
                healthStat = (float)o_health / 10;
                trickStat = (float)o_trick / 2;
                txtName.text = "ORANGE";
                lv = o_lv;
                break;
            default:
                break;
        }

        img_pBar.fillAmount = powerStat;
        img_hBar.fillAmount = healthStat;
        img_tBar.fillAmount = trickStat;

        txtLv.text = "lv " + lv;
    }

    void setCharacter()
    {
        for(int i=0; i<3; i++)
        {
            if (i == characterIndex)
                cha[i].SetActive(true);
            else
                cha[i].SetActive(false);
        }
    }

    public void onSelectPlayerBlue()
    {
        characterIndex = 0;
        setDataToPanel();
        imgSelected.transform.position = btnTr[0].position;
        setCharacter();
    }

    public void onSelectPlayerRed()
    {
        characterIndex = 1;
        setDataToPanel();
        imgSelected.transform.position = btnTr[1].position;
        setCharacter();
    }

    public void onSelectPlayerOrange()
    {
        characterIndex = 2;
        setDataToPanel();
        imgSelected.transform.position = btnTr[2].position;
        setCharacter();
    }

    public void onSelectPlayerGray()
    {
        characterIndex = 3;
        setDataToPanel();
        imgSelected.transform.position = btnTr[3].position;
        //setCharacter();
    }

    public void onSelectPlayerUnknown()
    {
        characterIndex = 4;
        setDataToPanel();
        imgSelected.transform.position = btnTr[4].position;
        //setCharacter();
    }

    public void onClickSelectEnemy()
    {
        current = select.Enemy;
        //cam.transform.position = Vector3.Lerp(cam.transform.position, camPos.position, Time.deltaTime);
        //cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, camPos.rotation, Time.deltaTime);
        canvas.GetComponent<Animator>().SetTrigger("select_enemy");
        nextBtn[0].SetActive(false);
        nextBtn[1].SetActive(true);
    }

    public void onSelectEnemyBlue()
    {
        for (int i = 0; i < 4; i++)        
            if (i == 0)
                enemy[i].SetActive(true);
            else
                enemy[i].SetActive(false);
        imgSelected_e.transform.position = btnTr_e[0].position;
        txtEnemyName.text = "BLUE";

        enemyIndex = 0;
    }

    public void onSelectEnemyRed()
    {
        for (int i = 0; i < 4; i++)
            if (i == 1)
                enemy[i].SetActive(true);
            else
                enemy[i].SetActive(false);
        imgSelected_e.transform.position = btnTr_e[1].position;
        txtEnemyName.text = "RED";

        enemyIndex = 1;
    }

    public void onSelectEnemyOrange()
    {
        for (int i = 0; i < 4; i++)
            if (i == 2)
                enemy[i].SetActive(true);
            else
                enemy[i].SetActive(false);
        imgSelected_e.transform.position = btnTr_e[2].position;
        txtEnemyName.text = "ORANGE";

        enemyIndex = 2;
    }

    public void onSelectEnemyGray()
    {
        for (int i = 0; i < 4; i++)
            if (i == 3)
                enemy[i].SetActive(true);
            else
                enemy[i].SetActive(false);
        imgSelected_e.transform.position = btnTr_e[3].position;
        txtEnemyName.text = "GRAY";

        enemyIndex = 3;
    }

    public void onClickSelectStage()
    {
        current = select.Stage;
        nextBtn[0].SetActive(false);
        nextBtn[1].SetActive(false);
        nextBtn[2].SetActive(true);
        canvas.GetComponent<Animator>().SetTrigger("select_stage");        
    }

    public void onClickStage_ARENA()
    {
        stage = maps[0];
        for(int i=0; i<3; i++)        
            if (i == 0)
                maps[i].SetActive(true);
            else
                maps[i].SetActive(false);
        stageName.text = "O C T A G O N";

        stageIndex = 0;
    }

    public void onClickStage_SKYCITY()
    {
        stage = maps[1];
        for (int i = 0; i < 3; i++)
            if (i == 1)
                maps[i].SetActive(true);
            else
                maps[i].SetActive(false);
        stageName.text = "S K Y C I T Y";

        stageIndex = 1;
    }

    public void onClickStage_MATRIX()
    {
        stage = maps[2];
        for (int i = 0; i < 3; i++)
            if (i == 2)
                maps[i].SetActive(true);
            else
                maps[i].SetActive(false);
        stageName.text = "M A T R I X";

        stageIndex = 2;
    }

    public void onClickSelectDifficulty()
    {
        current = select.Difficulty;
        nextBtn[2].SetActive(false);
        nextBtn[3].SetActive(true);
        canvas.GetComponent<Animator>().SetTrigger("select_difficulty");
    }

    public void onClickEasy()
    {
        diff_selected[0].SetActive(true);
        diff_selected[1].SetActive(false);
        diff_selected[2].SetActive(false);
        Game.current.setDifficulty(0);
    }

    public void onClickNormal()
    {
        diff_selected[0].SetActive(false);
        diff_selected[1].SetActive(true);
        diff_selected[2].SetActive(false);
        Game.current.setDifficulty(1);
    }

    public void onClickHard()
    {
        diff_selected[0].SetActive(false);
        diff_selected[1].SetActive(false);
        diff_selected[2].SetActive(true);
        Game.current.setDifficulty(2);
    }

    public void onClickReturnToMain()
    {
        Game.current.setMode(0);
        SceneManager.LoadScene("scLoading");
    }

    public void onClickStartMatch()
    {
        Game.current.setSelected(characterIndex);
        Game.current.setEnemy(enemyIndex);
        Game.current.setStage(stageIndex);
        SceneManager.LoadScene("scLoading");
    }
}

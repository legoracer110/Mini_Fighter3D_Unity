using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lvPanel : MonoBehaviour
{
    public bool isTest;

    public bool isActive;
    bool secondInit;

    public GameObject rdbMgr;

    public GameObject[] imgPlayer;

    private int currLv;
    private int exp;
    private int req_exp;

    private float exp_rate;

    public Text currLv1;
    public Text currLv2;

    public Text nextLv1;
    public Text nextLv2;

    public Text txtExp;
    public Text txtMoney;

    public Image expBar;

    bool victory;
    int diff;
    int Exp;
    int money;

    public GameObject lvUp_Panel;

    public GameObject rank_Panel;

    public GameObject menuBtns;

    void Start()
    {
        if (isTest)
            tmpSetting();
        diff = Game.current.getDifficulty();
        victory = roundDB.rdb.getVictory();

        if (!victory)
        {
            Exp = 600;
            money = 1000;
        }
        else
        {
            money = 5000;
            switch (diff)
            {
                case 0: Exp = 1500; break;
                case 1: Exp = 1800; money = 7000;  break;
                case 2: Exp = 2300; money = 12000; break;
                default: Exp = 0; break;
            }
        }
        txtExp.text = "+ exp " + Exp;
        txtMoney.text = "+ " + money + " gold";

        InitDataPanel();

        //Debug.Log("Before EXP : " + exp);
        exp += Exp;
        //Debug.Log("/ After EXP : "+exp);

        isActive = true;
    }

    void InitDataPanel()
    {      
        switch (Game.current.getSelected())
        {
            case 0:
                currLv = userData.data.blue.getLv(); exp = userData.data.blue.getLvExp();
                req_exp = userData.data.blue.getLvReq();
                imgPlayer[0].SetActive(true);
                break;
            case 1:
                currLv = userData.data.red.getLv(); exp = userData.data.red.getLvExp();
                req_exp = userData.data.red.getLvReq();
                imgPlayer[1].SetActive(true);
                break;
            case 2:
                currLv = userData.data.orange.getLv(); exp = userData.data.orange.getLvExp();
                req_exp = userData.data.orange.getLvReq();
                imgPlayer[2].SetActive(true);
                break;
            default: break;
        }

        int gold = userData.data.getGold();
        

        currLv1.text = "lv" + currLv;
        currLv2.text = "" + currLv;

        nextLv1.text = "lv" + (currLv + 1);
        nextLv2.text = "" + (currLv + 1);

        if (!secondInit)
        {
            userData.data.setGold(gold + money);
            exp_rate = (float)exp / (float)req_exp;
            expBar.fillAmount = exp_rate;
            secondInit = true;
        }        
    }

    void fillExpBar()
    {
        exp_rate = (float)exp / (float)req_exp;
        //Debug.Log("exp : " + exp + " / req_exp : " + req_exp + " / exp_rate : " + exp_rate);

        if (expBar.fillAmount >= 1)
            lvUp();

        if (expBar.fillAmount < exp_rate)
            expBar.fillAmount += Time.deltaTime * 0.6f;
        else
            Invoke("goToRankPanel", 2f);
         
    }

    void goToRankPanel()
    {
        isActive = false;

        //Debug.Log("goToRankPanel");
        /*
        rank_Panel.SetActive(true);
        this.gameObject.SetActive(false);
        */

        menuBtns.SetActive(true);
    }

    void lvUp()
    {
        // 레벨 업 시 이벤트
        // 레벨 업 패널 띄워줬다가 터치 후 제거
        isActive = false;
        //Debug.Log("Before : " + userData.data.red.getTotalExp());
        switch (Game.current.getSelected())
        {
            case 0: userData.data.blue.increaseExp(Exp); break;
            case 1: userData.data.red.increaseExp(Exp); break;
            case 2: userData.data.orange.increaseExp(Exp); break;
            default: break;
        }

        //Debug.Log("After : " + userData.data.red.getTotalExp());

        expBar.fillAmount = 0;

        rdbMgr.GetComponent<RoundMgr>().lvUp();
        
        //lvUp_Panel.SetActive(true);
        
    }

    public void OnClickLvUpPanel()
    {
        lvUp_Panel.SetActive(false);
        InitDataPanel();
        isActive = true;
    }

    void tmpSetting()
    {
        Game.current = new Game();
        userData.data = new userData();
        roundDB.rdb = new roundDB();

        Game.current.setSelected(1);
        userData.data.red.setLvExp(1200);
        roundDB.rdb.setVictory();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            fillExpBar();
        }
    }
}

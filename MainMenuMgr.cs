using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuMgr : MonoBehaviour
{
    public GameObject canvas;
    Animator animator;

    public Text userID;     // 유저 이름
    public Text txtLv;      // 유저 LV = 각 캐릭터 LV 합산
    int mainIndex;    // 초상화 인덱스

    public GameObject[] pics;

    public GameObject exitPanel;

    //public GameObject resPanel;
        
    public GameObject[] model;
    public GameObject panelSelectChar; // 캐릭터 선택 패널

    int[] equip;

    
    // 메인케릭터 코스튬 세팅을 위한 데이터
    
    /// ////////////////////////////////////////////
    
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
    public GameObject[] RED_AURA;
    // 0 : Nothing
    // 1 : 

    public GameObject[] BLUE_HEAD;
    public GameObject[] BLUE_TOP;
    public GameObject[] BLUE_ACC;
    public GameObject[] BLUE_ETC;
    public GameObject[] BLUE_AURA;

    public GameObject[] ORANGE_HEAD;
    public GameObject[] ORANGE_TOP;
    public GameObject[] ORANGE_ACC;
    public GameObject[] ORANGE_ETC;
    public GameObject[] ORANGE_AURA;

    public GameObject panelFail;

    ///////////////////////////////////////////////

    public enum Menu
    {
        Main,
        NewGame,
        SinglePlay,
        Story,
        Upgrade,
        Practice,
        Tutorial,
        Customize
    }

    public Menu currentMenu;

    void Start()
    {
                
        // Load User Data & Init
        userID.text = "" + userData.data.getID();
        int lv = userData.data.blue.getLv() + userData.data.red.getLv() + userData.data.orange.getLv();
        txtLv.text = "" + lv;
        mainIndex = userData.data.getMainIndex();

        switch (mainIndex)
        {
            case 0:
                onClickBlue();
                break;
            case 1:
                onClickRed();
                break;
            case 2:
                onClickOrange();
                break;
        }

        setUserProfile();

        currentMenu = Menu.Main;
        animator = canvas.GetComponent<Animator>();
        //resPanel.GetComponent<ResPanelMgr>().UpdateMoney();
    }    

    void setUserProfile()
    {
        if (mainIndex == 0)
        {
            pics[0].SetActive(true);
            pics[1].SetActive(false);
            pics[2].SetActive(false);

            model[0].SetActive(true);
            model[1].SetActive(false);
            model[2].SetActive(false);
        }
        else if (mainIndex == 1)
        {
            pics[0].SetActive(false);
            pics[1].SetActive(true);
            pics[2].SetActive(false);

            model[0].SetActive(false);
            model[1].SetActive(true);
            model[2].SetActive(false);
        }            
        else if (mainIndex == 2)
        {
            pics[0].SetActive(false);
            pics[1].SetActive(false);
            pics[2].SetActive(true);

            model[0].SetActive(false);
            model[1].SetActive(false);
            model[2].SetActive(true);
        }        
    }   

    void tmpGame()
    {
        // 시작타이틀 화면 에서 생성할 Game 및 userData 객체 임시 생성 (테스트용)
        Game.current = new Game();
        userData.data = new userData();        
    }

    // 0 : mainMenu
    // 1 : practice
    // 2 : tutorial
    // 3 : upgrade
    // 4 : story
    // 5 : tournament
    // 6 : custom
    // 7 : online

    // 10 : select

    public void onClickGamePlay()
    {
        //Debug.Log("onClickGamePlay");
        currentMenu = Menu.NewGame;
        animator.SetTrigger("onGamePlay");        
    }

    public void onClickUpgrade()
    {
        currentMenu = Menu.Upgrade;
        animator.SetTrigger("onUpgrade");
        Game.current.setMode(3);
        Invoke("scLoad", 3f);
    }

    public void onClickCustomize()
    {
        currentMenu = Menu.Customize;
        animator.SetTrigger("onCustomize");
        Game.current.setMode(10);
        Invoke("scLoad", 3f);
    }

    public void onClickTutorial()
    {
        currentMenu = Menu.Tutorial;
        animator.SetTrigger("onTutorial");
        Game.current.setMode(2);
        Invoke("scLoad", 3f); 
    }

    public void onClickPractice()
    {
        currentMenu = Menu.Practice;
        animator.SetTrigger("onPractice");
        Game.current.setMode(1);
        Invoke("scLoad", 3f);
    }

    public void onClickSingle()
    {
        currentMenu = Menu.SinglePlay;
        animator.SetTrigger("onSinglePlay");        
    }

    public void onClickStory()
    {
        currentMenu = Menu.Story;
        animator.SetTrigger("onStory");
        Game.current.setMode(4);
        Invoke("scLoad", 3f);
    }

    public void onClickTournament()
    {

    }

    public void onClickCustom()
    {        
        int currEnergy = userData.data.getEnergy();
        Debug.Log("현재 에너지 : "+currEnergy);
        // 피로도가 2 이상일 때만
        if (currEnergy < 2)
            panelFail.SetActive(true);
        else
        {            
            animator.SetTrigger("onCustomPlay");
            Game.current.setMode(6);
            Invoke("selectSettings", 3f);
        }
    }

    public void onClickConfirmFail()
    {
        panelFail.SetActive(false);
    }

    public void onClickCancel()
    {
        if (currentMenu == Menu.SinglePlay)
            currentMenu = Menu.NewGame;
        else
            currentMenu = Menu.Main;
        animator.SetTrigger("cancel");
    }

    public void onClickShop()
    {
        // 상점 버튼 눌렀을때
        SceneManager.LoadScene("scShop");
    }

    public void onClickModelChange()
    {
        // 메인 캐릭터 변경 버튼 눌렀을 때
        panelSelectChar.SetActive(true);
    }

    public void onClickBlue()
    {        
        mainIndex = 0;

        equip = userData.data.blue.getEquip();
        BLUE_HEAD[equip[0]].SetActive(true);
        BLUE_TOP[equip[1]].SetActive(true);
        BLUE_ACC[equip[2]].SetActive(true);
        BLUE_ETC[equip[3]].SetActive(true);
        BLUE_AURA[equip[4]].SetActive(true);

        panelSelectChar.SetActive(false);

        userData.data.setMainIndex(0);

        setUserProfile();
    }

    public void onClickRed()
    {
        mainIndex = 1;

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

        panelSelectChar.SetActive(false);

        userData.data.setMainIndex(1);

        setUserProfile();
    }

    public void onClickOrange()
    {
        mainIndex = 2;

        equip = userData.data.orange.getEquip();
        ORANGE_HEAD[equip[0]].SetActive(true);
        ORANGE_TOP[equip[1]].SetActive(true);
        ORANGE_ACC[equip[2]].SetActive(true);
        ORANGE_ETC[equip[3]].SetActive(true);
        ORANGE_AURA[equip[4]].SetActive(true);

        panelSelectChar.SetActive(false);

        userData.data.setMainIndex(2);

        setUserProfile();
    }


    public void onClickExit()
    {
        //Application.Quit();
        exitPanel.SetActive(true);
    }

    public void onClickCancelExit()
    {
        exitPanel.SetActive(false);
    }

    public void onClickExitConfirm()
    {
        Application.Quit();
    }

    void selectSettings()
    {
        SceneManager.LoadScene("scSelect");
    }

    void scLoad()
    {
        SceneManager.LoadScene("scLoading");
    }
}

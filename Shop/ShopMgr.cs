using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopMgr : MonoBehaviour
{
    public bool isTest;

    public GameObject resPanel;

    int buyIndex;
    // 구매 리스트
    // 0 : 에너지 +5          (10루비)
    // 1 : 골드 +5,000        (10루비)
    // 2 : 골드 +30,000       (50루비)
    // 3 : 골드 +375,000      (500루비)
    // 4 : 루비 +30           (3,000원)
    // 5 : 루비 +55           (5,000원)
    // 6 : 루비 +120          (10,000원)
    // 7 : 루비 +450          (30,000원)
    // 8 : 패키지 1
    // 9 : 패키지 2
    // 10 : 패키지 3

    public GameObject[] panel;  // 탭 매뉴들

    public GameObject errorPanel; // 돈 모자를 때 경고패널
    public GameObject confirmPanel; // 결제 재확인 패널

    public GameObject alarmPanel; // 결제 결과 알림 패널

    public Text txtName;
    public Text txtPrice;

    int kind;
    int price;
    // 필요한 재화 (골드/루비) 가 충분한지 확인 후 결과값 반환
    // 1. kind 종류
    //      case 0 : 골드
    //      case 1 : 루비
    // 2. price 가격

    int gold;   //  골드 소지량
    int ruby;   //  루비 소지량

    int money;  //  해당 재화 소지량

    void Start()
    {
        if (isTest)
        {
            testInit();
        }

        gold = userData.data.getGold();
        ruby = userData.data.getRuby();

        money = gold;
    }
    
    // TEST 용 Setting
    void testInit()
    {
        
        userData.data = new userData();

        userData.data.setGold(50000);
        userData.data.setRuby(400);
    }

    // 패키지 탭 클릭
    public void OnClickPackageTab()
    {        
        for(int i=0; i<4; i++)
        {
            if (i == 0)
                panel[i].SetActive(true);
            else
                panel[i].SetActive(false);
        }
    }

    // 에너지 탭 클릭
    public void OnClickEnergyTab()
    {
        
        for (int i = 0; i < 4; i++)
        {
            if (i == 1)
                panel[i].SetActive(true);
            else
                panel[i].SetActive(false);
        }
    }

    // 골드 탭 클릭
    public void OnClickGoldTab()
    {        
        for (int i = 0; i < 4; i++)
        {
            if (i == 2)
                panel[i].SetActive(true);
            else
                panel[i].SetActive(false);
        }
    }
    
    // 루비 탭 클릭
    public void OnClickRubyTab()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == 3)
                panel[i].SetActive(true);
            else
                panel[i].SetActive(false);
        }
    }

    // 재화가 충분할 경우 TRUE, 모자르면 FALSE   
    bool checkMoneyEnough()
    {
        gold = userData.data.getGold();
        ruby = userData.data.getRuby();

        if (kind == 0)
        {
            // 골드 상품
            money = gold;
        }
        else
        {
            // 루비 상품
            money = ruby;
        }

        if (money >= price)
            return true;
        else
            return false;
    }
    
    // 소지량 모자르거나 구매 취소시
    public void OnClickCancel()
    {
        errorPanel.SetActive(false);
        confirmPanel.SetActive(false);
        alarmPanel.SetActive(false);
    }

    // 구매 확정 버튼 눌렀을 때
    public void OnClickConfirmPurchase()
    {       
        if (kind == 0)
        {
            // 골드 상품
            userData.data.setGold(money - price);
        }else
        {
            // 루비 상품
            userData.data.setRuby(money - price);
        }

        int tmp;

        switch (buyIndex)
        {
            // 구매 리스트
            // 0 : 에너지 +5          (10루비)
            // 1 : 골드 +5,000        (10루비)
            // 2 : 골드 +30,000       (50루비)
            // 3 : 골드 +375,000      (500루비)
            // 4 : 루비 +30           (3,000원)
            // 5 : 루비 +55           (5,000원)
            // 6 : 루비 +120          (10,000원)
            // 7 : 루비 +450          (30,000원)
            // 8 : 패키지 1
            // 9 : 패키지 2
            // 10 : 패키지 3
            case 0:
                // 0 : 에너지 +5 
                tmp = userData.data.getEnergy();
                if(tmp+5>=10)
                    userData.data.setEnergy(10);
                else
                    userData.data.setEnergy(tmp + 5);
                break;
            case 1:
                // 1 : 골드 +5,000
                tmp = userData.data.getGold();
                userData.data.setGold(tmp + 5000);
                break;
            case 2:
                // 2 : 골드 +30,000   
                tmp = userData.data.getGold();
                userData.data.setGold(tmp + 30000);
                break;
            case 3:
                // 3 : 골드 +375,000  
                tmp = userData.data.getGold();
                userData.data.setGold(tmp + 375000);
                break;
            case 4:
                // 4 : 루비 +30
                tmp = userData.data.getRuby();
                userData.data.setRuby(tmp + 30);
                break;
            case 5:
                // 5 : 루비 +55 
                tmp = userData.data.getRuby();
                userData.data.setRuby(tmp + 55);
                break;
            case 6:
                // 6 : 루비 +120
                tmp = userData.data.getRuby();
                userData.data.setRuby(tmp + 120);
                break;
            case 7:
                // 7 : 루비 +450    
                tmp = userData.data.getRuby();
                userData.data.setRuby(tmp + 450);
                break;
            case 8:
                // 8 : 패키지 1
                break;
            case 9:
                // 9 : 패키지 2
                break;
            case 10:
                // 10 : 패키지 3
                break;
        }        

        resPanel.GetComponent<ResPanelMgr>().UpdateMoney();
        alarmPanel.SetActive(true);
    }

    // Energy 구매
    // 10 루비 -> 5 에너지
    public void OnClickBuy0()
    {        
        kind = 1;
        price = 10;
        buyIndex = 0;

        txtName.text = "[<color=#FFD900>에너지 5개</color><color=#FFFFFF>] 를</color>";
        txtPrice.text = "<color=#FF00F3>10 루비</color>";

        if (!checkMoneyEnough())
            errorPanel.SetActive(true);
        else        
            confirmPanel.SetActive(true);    
    }
    
    // 골드 구매
    // 10 루비 -> 5,000 골드
    public void OnClickBuy1()
    {
        kind = 1;
        price = 10;
        buyIndex = 1;

        txtName.text = "[<color=#FFD900>5,000 골드</color><color=#FFFFFF>] 를</color>";
        txtPrice.text = "<color=#FF00F3>10 루비</color>";

        if (!checkMoneyEnough())
            errorPanel.SetActive(true);
        else
            confirmPanel.SetActive(true);
    }

    // 골드 구매
    // 50 루비 -> 30,000 골드
    public void OnClickBuy2()
    {       
        kind = 1;
        price = 50;
        buyIndex = 2;

        txtName.text = "[<color=#FFD900>30,000 골드</color><color=#FFFFFF>] 를</color>";
        txtPrice.text = "<color=#FF00F3>50 루비</color>";

        if (!checkMoneyEnough())
            errorPanel.SetActive(true);
        else
            confirmPanel.SetActive(true);
    }
    
    // 골드 구매
    // 500 루비 -> 375,000 골드
    public void OnClickBuy3()
    {
        kind = 1;
        price = 500;
        buyIndex = 3;

        txtName.text = "[<color=#FFD900>375,000 골드</color><color=#FFFFFF>] 를</color>";
        txtPrice.text = "<color=#FF00F3>500 루비</color>";

        if (!checkMoneyEnough())
            errorPanel.SetActive(true);
        else
            confirmPanel.SetActive(true);
    }

    // 루비 구매
    // 3,000 원 -> 30 루비
    public void OnClickBuy4()
    {
        

        // 구글 스토어 연동        
    }

    public void OnClickBuy5()
    {
        // 루비 구매
        // 5,000 원 -> 55 루비

        // 구글 스토어 연동        
    }

    public void OnClickBuy6()
    {
        // 루비 구매
        // 10,000 원 -> 120 루비

        // 구글 스토어 연동        
    }

    public void OnClickBuy7()
    {
        // 루비 구매
        // 30,000 원 -> 450 루비

        // 구글 스토어 연동        
    }

    // 패키지 1 구매
    public void OnClickBuy8()
    {
        
    }

    // 광고 시청하기 (+5에너지)
    public void OnClickPlayAds()
    {
        // 광고 시청연동
        Debug.Log("Ads Play");

        int tmp = userData.data.getEnergy();
        if (tmp == 10)
        {
            // Nothing (Energy Full)
        }
        else
        {
            // Play Ads
            if (tmp + 5 >= 10)
                userData.data.setEnergy(10);
            else
                userData.data.setEnergy(tmp + 5);
        }
    }

    // 홈으로 돌아가기
    public void OnClickHomeKey()
    {
        Game.current.setMode(0);
        SceneManager.LoadScene("scLoading");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradeMgr : MonoBehaviour
{
    public GameObject[] panel;
    public GameObject[] cha;
    public GameObject comboPanel;
    public GameObject[] c_panel;
    // 0 : BLUE
    // 1 : RED
    // 2 : ORANGE
    // 3 : GRAY (not yet)

    public int char_index;

    public GameObject checkPanel;
    public GameObject errPanel;
    public GameObject err1;
    public GameObject err2;

    public GameObject resPanel;

    public int statIndex;
    public bool up;

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

    /*
    // GRAY TMP STAT
    int g_lv;
    int g_power;
    int g_health;
    int g_trick;
    */

    public Image imgB_pBar;
    public Image imgB_hBar;
    public Image imgB_tBar;

    public Text txtLv;
    public Text txtPower;
    public Text txtHealth;
    public Text txtTrick;

    public Text txtPoint;

    public Image imgR_pBar;
    public Image imgR_hBar;
    public Image imgR_tBar;

    public Image imgO_pBar;
    public Image imgO_hBar;
    public Image imgO_tBar;

    int b_point;
    int r_point;
    int o_point;


    void Start()
    {
        //tmpNewGame();

        char_index = 0;

        LoadData();
        setDataToPanels();
    }

    void tmpNewGame()
    {
        // Main Menu 에서 생성할 userData 클래스 임시 생성 (테스트용)
        userData.data = new userData();                
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

    void setDataToPanels()
    {
        // 로드한 데이터 기반으로 패널 초기화 기입 작업
        imgB_pBar.fillAmount = (float)b_power / 10;
        imgB_hBar.fillAmount = (float)b_health / 10;
        imgB_tBar.fillAmount = (float)b_trick / 2;

        imgR_pBar.fillAmount = (float)r_power / 10;
        imgR_hBar.fillAmount = (float)r_health / 10;
        imgR_tBar.fillAmount = (float)r_trick / 2;

        imgO_pBar.fillAmount = (float)o_power / 10;
        imgO_hBar.fillAmount = (float)o_health / 10;
        imgO_tBar.fillAmount = (float)o_trick / 2;

        txtLv.text = "LV " + b_lv;
        txtPower.text = "" + b_power;
        txtHealth.text = "" + b_health;
        txtTrick.text = "" + b_trick;
                
        b_point = b_lv - b_power - b_health - b_trick+3;
        r_point = r_lv - r_power - r_health - r_trick+3;
        o_point = o_lv - o_power - o_health - o_trick+3;
        txtPoint.text = "(잔여포인트 : " + b_point + ")";
    }

    public void setUpgradeStat(int statIndex, bool up)
    {
        this.statIndex = statIndex;
        this.up = up;

        if (up)
            checkPanel.SetActive(true);
        else
            changeStat();
    }

    public void changeStat()
    {
        // statIndex 는 Power / Health / Trick
        // up 은 + / -        

        //Debug.Log("B포인트 : " + b_point);
        //Debug.Log("R포인트 : " + r_point);
        //Debug.Log("O포인트 : " + o_point);

        if (statIndex == 0)
        {
            // Power
            switch (char_index)
            {
                case 0:
                    // Blue
                    if (up && b_power < 10 && b_point > 0)
                    {
                        b_power++;
                        b_point--;                        
                    }
                    if (!up && b_power > 0)
                    {
                        b_power--;
                        b_point++;
                    }
                    imgB_pBar.fillAmount = (float)b_power / 10;
                    txtPower.text = "" + b_power;
                    txtPoint.text = "(잔여포인트 : " + b_point + ")";
                    break;
                case 1:
                    // Red
                    if (up && r_power < 10 && r_point > 0)
                    {
                        r_power++;
                        r_point--;                        
                    }
                    if (!up && r_power > 0)
                    {
                        r_power--;
                        r_point++;
                    }
                    imgR_pBar.fillAmount = (float)r_power / 10;
                    txtPower.text = "" + r_power;
                    txtPoint.text = "(잔여포인트 : " + r_point + ")";
                    break;
                case 2:
                    // Orange
                    if (up && o_power < 10 && o_point > 0)
                    {
                        o_power++;
                        o_point--;                        
                    }
                    if (!up && o_power > 0)
                    {
                        o_power--;
                        o_point++;
                    }
                    imgO_pBar.fillAmount = (float)o_power / 10;
                    txtPower.text = "" + o_power;
                    txtPoint.text = "(잔여포인트 : " + o_point + ")";
                    break;
                default:
                    break;
            }
        }else if(statIndex == 1)
        {
            // Health
            switch (char_index)
            {
                case 0:
                    // Blue
                    if (up && b_health < 10 && b_point > 0)
                    {
                        b_health++;
                        b_point--;                        
                    }
                    if (!up && b_health > 0)
                    {
                        b_health--;
                        b_point++;
                    }
                    imgB_hBar.fillAmount = (float)b_health / 10;
                    txtHealth.text = "" + b_health;
                    txtPoint.text = "(잔여포인트 : " + b_point + ")";
                    break;
                case 1:
                    // Red
                    if (up && r_health < 10 && r_point > 0)
                    {
                        r_health++;
                        r_point--;
                    }
                    if (!up && r_health > 0)
                    {
                        r_health--;
                        r_point++;
                    }
                    imgR_hBar.fillAmount = (float)r_health / 10;
                    txtHealth.text = "" + r_health;
                    txtPoint.text = "(잔여포인트 : " + r_point + ")";
                    break;
                case 2:
                    // Orange
                    if (up && o_health < 10 && o_point > 0)
                    {
                        o_health++;
                        o_point--;                        
                    }
                    if (!up && o_health > 0)
                    {
                        o_health--;
                        o_point++;
                    }
                    imgO_hBar.fillAmount = (float)o_health / 10;
                    txtHealth.text = "" + o_health;
                    txtPoint.text = "(잔여포인트 : " + o_point + ")";
                    break;
                default:
                    break;
            }
        }
        else
        {
            // Trick
            switch (char_index)
            {
                case 0:
                    // Blue
                    if (up && b_trick < 2 && b_point > 0)
                    {
                        b_trick++;
                        b_point--;                        
                    }
                    if (!up && b_trick > 0)
                    {
                        b_trick--;
                        b_point++;
                    }
                    imgB_tBar.fillAmount = (float)b_trick / 2;
                    txtTrick.text = "" + b_trick;
                    txtPoint.text = "(잔여포인트 : " + b_point + ")";
                    break;
                case 1:
                    // Red
                    if (up && r_trick < 2 && r_point > 0)
                    {
                        r_trick++;
                        r_point--;                        
                    }
                    if (!up && r_trick > 0)
                    {
                        r_trick--;
                        r_point++;
                    }
                    imgR_tBar.fillAmount = (float)r_trick / 2;
                    txtTrick.text = "" + r_trick;
                    txtPoint.text = "(잔여포인트 : " + r_point + ")";
                    break;
                case 2:
                    // Orange
                    if (up && o_trick < 2 && o_point > 0)
                    {
                        o_trick++;
                        o_point--;
                    }
                    if (!up && o_trick > 0)
                    {
                        o_trick--;
                        o_point++;
                    }
                    imgO_tBar.fillAmount = (float)o_trick / 2;
                    txtTrick.text = "" + o_trick;
                    txtPoint.text = "(잔여포인트 : " + o_point + ")";
                    break;
                default:
                    break;
            }
        }
        checkPanel.SetActive(false);
        errPanel.SetActive(false);
    }
    
    void displayPanels(int index)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == index)
            {
                panel[i].SetActive(true);
                cha[i].SetActive(true);
            }
            else { 
                panel[i].SetActive(false);
                cha[i].SetActive(false);
            }
        }        
    }

    void displayC_Panels(int index)
    {
        comboPanel.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            if (i == index)
                c_panel[i].SetActive(true);            
            else
                c_panel[i].SetActive(false);
             
        }
    }

    public void setBlueInfo()
    {
        displayPanels(0);
        char_index = 0;
        txtLv.text = "LV " + b_lv;
        txtPower.text = ""+b_power;
        txtHealth.text = ""+b_health;
        txtTrick.text = ""+b_trick;

        b_point = b_lv - b_power - b_health - b_trick+3;
        txtPoint.text = "(잔여포인트 : " + b_point + ")";
    }

    public void setRedInfo()
    {
        displayPanels(1);
        char_index = 1;
        txtLv.text = "LV " + r_lv;
        txtPower.text = "" + r_power;
        txtHealth.text = "" + r_health;
        txtTrick.text = "" + r_trick;

        r_point = r_lv - r_power - r_health - r_trick+3;
        txtPoint.text = "(잔여포인트 : " + r_point + ")";
    }

    public void setOrangeInfo()
    {
        displayPanels(2);
        char_index = 2;
        txtLv.text = "LV " + o_lv;
        txtPower.text = "" + o_power;
        txtHealth.text = "" + o_health;
        txtTrick.text = "" + o_trick;

        o_point = o_lv - o_power - o_health - o_trick+3;
        txtPoint.text = "(잔여포인트 : " + o_point + ")";
    }

    public void onDisplayCombo()
    {
        displayC_Panels(char_index);
    }

    public void onCancel()
    {
        comboPanel.SetActive(false);        
    }

    public void onExitUpgrade()
    {
        SaveData();
        // 씬 전환
        Game.current.setMode(0);
        SceneManager.LoadScene("scLoading");
    }

    void SaveData()
    {
        userData.data.blue.setPower(b_power);
        userData.data.blue.setHealth(b_health);
        userData.data.blue.setTrick(b_trick);

        userData.data.red.setPower(r_power);
        userData.data.red.setHealth(r_health);
        userData.data.red.setTrick(r_trick);

        userData.data.orange.setPower(o_power);
        userData.data.orange.setHealth(o_health);
        userData.data.orange.setTrick(o_trick);
    }

    public void OnClickBtn_ConfirmUpgrade()
    {
        //Debug.Log("b_point : " + b_point + " / r_point : " + r_point + " / o_point : " + o_point);

        if ((char_index == 0 && b_point <= 0) || (char_index == 1 && r_point <= 0) || (char_index == 2 && o_point <= 0))
        {
            errPanel.SetActive(true);
            err1.SetActive(true);
        }
        else
        {
            int money = userData.data.getGold();
            if (money >= 5000)
            {
                userData.data.setGold(money - 5000);
                resPanel.GetComponent<ResPanelMgr>().UpdateMoney();
                changeStat();
            }
            else
            {
                errPanel.SetActive(true);
                err2.SetActive(true);
            }
        }
    }

    public void OnClickBtn_Cancel()
    {
        checkPanel.SetActive(false);
        errPanel.SetActive(false);
        err1.SetActive(false);
        err2.SetActive(false);
    }
   
}

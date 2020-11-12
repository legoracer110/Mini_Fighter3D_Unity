using Shgames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PracticeMgr : MonoBehaviour
{
    public Text txtDmg;
    public Text txtCombo;
    public Text txtState;

    int playerIndex;

    //public bool dummyAttack;    
    public GameObject[] players;
    public GameObject dummy;

    public Transform playerPos;
    public Transform dummyPos;

    bool isPause;
    public GameObject pausePanel;

    GameObject playerChar;

    public GameObject[] dropDowns;

    public Dropdown dropdown_BLUE;
    public Dropdown dropdown_RED;
    public Dropdown dropdown_ORANGE;

    public GameObject[] comboPanel;

    void Start()
    {
        dropdown_BLUE.onValueChanged.AddListener(delegate
        {
            DropdownValueChangedHandler(dropdown_BLUE);
        });

        dropdown_RED.onValueChanged.AddListener(delegate
        {
            DropdownValueChangedHandler(dropdown_RED);
        });

        dropdown_ORANGE.onValueChanged.AddListener(delegate
        {
            DropdownValueChangedHandler(dropdown_ORANGE);
        });
    }

    private void DropdownValueChangedHandler(Dropdown target)
    {
        if (target.value == 0)
        {
            foreach (GameObject panel in comboPanel)
                panel.SetActive(false);
        }
        else
        {
            switch (playerIndex)
            {
                case 0:
                    displayComboPanel(target.value - 1);
                    break;
                case 1:
                    displayComboPanel(target.value + 4);
                    break;
                case 2:
                    displayComboPanel(target.value + 8);
                    break;
            }
        }
    }

    void displayComboPanel(int index)
    {
        for(int i=0; i<15; i++)
        {
            if (i == index)
                comboPanel[i].SetActive(true);
            else
                comboPanel[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
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

    public void setDmg(int dmg)
    {
        txtDmg.text = "" + dmg;
    }

    public void setComboDmg(int comboDmg)
    {
        if (comboDmg >= 80)
            txtCombo.color = Color.red;
        else if (comboDmg >= 50)
            txtCombo.color = Color.yellow;
        else
            txtCombo.color = Color.white;
        txtCombo.text = "" + comboDmg;
    }

    public void setState(string st)
    {
        if (st == "NORMAL")
            txtState.color = Color.white;
        else if (st == "STUN")
            txtState.color = Color.yellow;
        else if(st == "AERIAL")
            txtState.color = Color.cyan;
        txtState.text = st;
    }

    void ResetPos()
    {
        playerChar.transform.position = playerPos.position;
        dummy.transform.position = dummyPos.position;        
    }

    public void onClickResetPos()
    {
        ResetPos();
    }

    public void onClickBtn_Blue()
    {        
        playerIndex = 0;
        players[0].SetActive(true);
        players[1].SetActive(false);
        players[2].SetActive(false);

        dropDowns[0].SetActive(true);
        dropDowns[1].SetActive(false);
        dropDowns[2].SetActive(false);

        displayComboPanel(0);
        dropdown_BLUE.SetValueWithoutNotify(1);

        playerChar = GameObject.Find("Player");
        dummy.GetComponent<aiCtrl>().playerTr = playerChar.GetComponent<Transform>();
        ResetPos();
    }

    public void onClickBtn_Red()
    {        
        playerIndex = 1;
        players[0].SetActive(false);
        players[1].SetActive(true);
        players[2].SetActive(false);

        dropDowns[0].SetActive(false);
        dropDowns[1].SetActive(true);
        dropDowns[2].SetActive(false);

        displayComboPanel(5);
        dropdown_RED.SetValueWithoutNotify(1);

        playerChar = GameObject.Find("Player");
        dummy.GetComponent<aiCtrl>().playerTr = playerChar.GetComponent<Transform>();
        ResetPos();
    }

    public void onClickBtn_Orange()
    {        
        playerIndex = 2;
        players[0].SetActive(false);
        players[1].SetActive(false);
        players[2].SetActive(true);

        dropDowns[0].SetActive(false);
        dropDowns[1].SetActive(false);
        dropDowns[2].SetActive(true);

        displayComboPanel(9);
        dropdown_ORANGE.SetValueWithoutNotify(1);

        playerChar = GameObject.Find("Player");
        dummy.GetComponent<aiCtrl>().playerTr = playerChar.GetComponent<Transform>();
        ResetPos();
    }    

    public void OnClickResume()
    {
        isPause = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void OnClickBtnReturnToMain()
    {
        Time.timeScale = 1;
        Game.current.setMode(0);
        SceneManager.LoadScene("scLoading");
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CustomizeMgr : MonoBehaviour
{
    public GameObject canvas;
    Animator animator;

    public Text txtName;

    public GameObject model;
    public GameObject[] panels;
    public GameObject[] models;

    public int charIndex;
    // 0 : BLUE
    // 1 : RED
    // 2 : ORANGE
    int itemIndex;
    // 0 : HEAD
    // 1 : TOP
    // 2 : ACC
    // 3 : ETC
    // 4 : AURA

    public GameObject itemMgr;

    public int rotState;
    // 0 : stop
    // 1 : clockwise
    // 2 : counterclockwise

    /*
    void Start()
    {
        charIndex = 0;
        itemIndex = 0;
    }
    */
    public enum MenuState
    {
        // 0, 1, 2, 3, 4,
        BLUE_HEAD, BLUE_TOP, BLUE_ACC, BLUE_ETC, BLUE_AURA,
        // 5, 6, 7, 8, 9,
        RED_HEAD, RED_TOP, RED_ACC, RED_ETC, RED_AURA,
        // 10, 11, 12, 13, 14
        ORANGE_HEAD, ORANGE_TOP, ORANGE_ACC, ORANGE_ETC, ORANGE_AURA
    }

    public MenuState current = MenuState.BLUE_HEAD;

    void Start()
    {
        charIndex = 0;
        itemIndex = 0;
        animator = canvas.GetComponent<Animator>();

        
    }

    void Update()
    {
        if (rotState == 1)        
            model.transform.Rotate(Vector3.down, Time.deltaTime*(-180f));
        else if (rotState == -1)
            model.transform.Rotate(Vector3.down, Time.deltaTime * 180f);
    }

    public void OnClickBlue()
    {
        txtName.text = "BLUE";

        foreach (GameObject panel in panels)
        {
            if(panel.activeSelf)
                panel.SetActive(false);
        }

        foreach (GameObject model in models)
        {
            if (model.activeSelf)
                model.SetActive(false);
        }

        panels[0].SetActive(true);
        models[0].SetActive(true);
        charIndex = 0;
        itemIndex = 0;
        current = MenuState.BLUE_HEAD;

        itemMgr.GetComponent<ItemMgr>().setAvataEquip(0);

        animator.SetTrigger("head");
    }

    public void OnClickRed()
    {
        txtName.text = "RED";

        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
                panel.SetActive(false);
        }

        foreach(GameObject model in models)
        {
            if (model.activeSelf)
                model.SetActive(false);
        }

        panels[5].SetActive(true);
        models[1].SetActive(true);
        charIndex = 1;
        itemIndex = 0;
        current = MenuState.RED_HEAD;

        itemMgr.GetComponent<ItemMgr>().setAvataEquip(1);

        animator.SetTrigger("head");
    }

    public void OnClickOrange()
    {
        txtName.text = "ORANGE";

        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
                panel.SetActive(false);
        }

        foreach (GameObject model in models)
        {
            if (model.activeSelf)
                model.SetActive(false);
        }

        panels[10].SetActive(true);
        models[2].SetActive(true);
        charIndex = 2;
        itemIndex = 0;
        current = MenuState.ORANGE_HEAD;

        itemMgr.GetComponent<ItemMgr>().setAvataEquip(2);

        animator.SetTrigger("head");
    }

    public void OnClickHead()
    {
        itemIndex = 0;
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
                panel.SetActive(false);
        }
        switch (charIndex)
        {
            case 0:
                panels[0].SetActive(true);                                
                current = MenuState.BLUE_HEAD;
                break;
            case 1:
                panels[5].SetActive(true);
                current = MenuState.RED_HEAD;
                break;
            case 2:
                panels[10].SetActive(true);
                current = MenuState.ORANGE_HEAD;
                break;
        }
        animator.SetTrigger("head");
    }

    public void OnClickTop()
    {
        itemIndex = 1;
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
                panel.SetActive(false);
        }
        switch (charIndex)
        {
            case 0:
                panels[1].SetActive(true);
                current = MenuState.BLUE_TOP;
                break;
            case 1:
                panels[6].SetActive(true);
                current = MenuState.RED_TOP;
                break;
            case 2:
                panels[11].SetActive(true);
                current = MenuState.ORANGE_TOP;
                break;
        }
        animator.SetTrigger("top");
    }

    public void OnClickAcc()
    {
        itemIndex = 2;
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
                panel.SetActive(false);
        }
        switch (charIndex)
        {
            case 0:
                panels[2].SetActive(true);
                current = MenuState.BLUE_ACC;
                break;
            case 1:
                panels[7].SetActive(true);
                current = MenuState.RED_ACC;
                break;
            case 2:
                panels[12].SetActive(true);
                current = MenuState.ORANGE_ACC;
                break;
        }
        animator.SetTrigger("acc");
    }

    public void OnClickEtc()
    {
        itemIndex = 3;
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
                panel.SetActive(false);
        }
        switch (charIndex)
        {
            case 0:
                panels[3].SetActive(true);
                current = MenuState.BLUE_ETC;
                break;
            case 1:
                panels[8].SetActive(true);
                current = MenuState.RED_ETC;
                break;
            case 2:
                panels[13].SetActive(true);
                current = MenuState.ORANGE_ETC;
                break;
        }
        animator.SetTrigger("etc");
    }

    public void OnClickAura()
    {
        itemIndex = 4;
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
                panel.SetActive(false);
        }
        switch (charIndex)
        {
            case 0:
                panels[4].SetActive(true);
                current = MenuState.BLUE_AURA;
                break;
            case 1:
                panels[9].SetActive(true);
                current = MenuState.RED_AURA;
                break;
            case 2:
                panels[14].SetActive(true);
                current = MenuState.ORANGE_AURA;
                break;
        }
        animator.SetTrigger("aura");
    }
    

    public void onExitCustomize()
    {
        //SaveData();
        Game.current.setMode(0);
        SceneManager.LoadScene("scLoading");
    }

    void SaveData()
    {

    }
}

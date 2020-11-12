using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class upgradePanelMgr : MonoBehaviour
{
    GameObject UpgradeMgr;

    public int power;
    public int health;
    public int trick;

    public Text txtPower;
    public Text txtHealth;
    public Text txtTrick;

    void Start()
    {
        UpgradeMgr = GameObject.Find("UpgradeMgr");

    }



    public void onClickPowerUp()
    {

    }
}

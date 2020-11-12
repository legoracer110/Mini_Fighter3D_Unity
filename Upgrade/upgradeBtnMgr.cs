using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradeBtnMgr : MonoBehaviour
{
    public GameObject upgradeMgr;
    public void onClick_PowerUp()
    {        
        upgradeMgr.GetComponent<UpgradeMgr>().setUpgradeStat(0, true);        
    }

    public void onClick_PowerDown()
    {
        upgradeMgr.GetComponent<UpgradeMgr>().setUpgradeStat(0, false);
    }

    public void onClick_HealthUp()
    {
        upgradeMgr.GetComponent<UpgradeMgr>().setUpgradeStat(1, true);
    }

    public void onClick_HealthDown()
    {
        upgradeMgr.GetComponent<UpgradeMgr>().setUpgradeStat(1, false);
    }
    public void onClick_TrickUp()
    {
        upgradeMgr.GetComponent<UpgradeMgr>().setUpgradeStat(2, true);
    }

    public void onClick_TrickDown()
    {
        upgradeMgr.GetComponent<UpgradeMgr>().setUpgradeStat(2, false);
    }

    public void onClick_ExitUpgrade()
    {
        upgradeMgr.GetComponent<UpgradeMgr>().onExitUpgrade();
    }
}

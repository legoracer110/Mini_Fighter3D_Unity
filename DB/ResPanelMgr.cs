using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class ResPanelMgr : MonoBehaviour
{
    public Text txtEnergy;
    public Text txtGold;
    public Text txtRuby;

    string strTime;

    public GameObject panelSettings;   

    void Start()
    {
        UpdateMoney();
        //StartCoroutine(this.GetNowTime());        
        strTime = "";
    }

    private void Update()
    {
        if (userData.data.getEnergy() < 10)
        {
            System.DateTime now = System.DateTime.Now;
            System.DateTime keyTime = userData.data.getKeyTime();
            System.TimeSpan timeCal = now - keyTime;
                      

            int timeSliceSeconds = timeCal.Seconds;
            int timeSliceMinutes = timeCal.Minutes;

            if(timeSliceSeconds<10)
                strTime = "0"+ timeSliceSeconds;
            else
                strTime = "" + timeSliceSeconds;
            //Debug.Log(timeSliceMinutes);
            //Debug.Log(timeSliceSeconds);
            int tmpEnergy = userData.data.getEnergy();
            txtEnergy.text = "" + tmpEnergy + "/10" + "<size=10> (0"+
                timeSliceMinutes+":"+strTime + "/05:00)</size>";
            
            // 8 / 10 (03:34/05:00)

            if (timeSliceMinutes >= 5)
            {
                Debug.Log("60초 경과 >> 에너지 증가");
                userData.data.incEnergy(1);
                userData.data.setKeyTime(System.DateTime.Now);                            
            }
        }
        else
        {
            txtEnergy.text = "10/10";
        }
    }

    public void OnClickCharge()
    {
        // 에너지|골드|보석 충전(+) 버튼을 눌렀을 때
        // 상점 페이지로 이동
        SceneManager.LoadScene("scShop");
    }

    public void OnClickSettings()
    {
        panelSettings.SetActive(true);
    }

    public void UpdateMoney()
    {
        // 재화 상황 최신화 갱신

        int tmpGold = userData.data.getGold();
        int tmpRuby = userData.data.getRuby();

        //int tmpEnergy = userData.data.getEnergy();

        txtGold.text = "" + tmpGold;
        txtRuby.text = "" + tmpRuby;

        //txtEnergy.text = "" + tmpEnergy + "/10";
    }    
    /*
    public IEnumerator GetNowTime()
    {
        WWW www = new WWW("http://localhost/getnow.php");
        yield return www;
        Debug.Log(www.text);
    }
    */        
    
}

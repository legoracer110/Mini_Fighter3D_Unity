using Shgames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class TutorialMgr : MonoBehaviour
{
    public int tutLv;                   // 미션 인덱스
    public int count;                   // 미션 성공 카운트
    public GameObject[] lvPanel;        // 미션 패널
    bool countType;
    // true : 1회 미션
    // false : 3회 미션
    public GameObject[] img_Check;      // 카운트 체크 이미지
    public GameObject[] img_Check_Box;  // 카운트 체크 박스

    GameObject player;
    GameObject dummy;
    public GameObject[] dummys;

    public Transform[] playerPos;

    public GameObject lastMgr;

    bool isPause;
    public GameObject pausePanel;

    private void Start()
    {
        //tutLv = 0;
        countType = true;
        player = GameObject.Find("Player");
        //dummy = GameObject.Find("Dummy");
        dummy = dummys[0];
        //enemy = GameObject.Find("Enemy");
        //lastMgr = GameObject.Find("lastMissionMgr");

        updateLv();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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

    void setImgChecker()
    {
        // 체크 모두 해제
        for (int i = 0; i < 3; i++)
            img_Check[i].SetActive(false);

        if (countType)
        {
            img_Check_Box[0].SetActive(false);
            img_Check_Box[1].SetActive(true);
            img_Check_Box[2].SetActive(false);
        }
        else
        {
            img_Check_Box[0].SetActive(true);
            img_Check_Box[1].SetActive(true);
            img_Check_Box[2].SetActive(true);
        }
        /*
        // 카운트 check 이미지 (1회/3회) 교체 및 색상 초기화 (white)
        img_Check[0].GetComponent<Image>().color = Color.white;
        img_Check[1].GetComponent<Image>().color = Color.white;
        img_Check[2].GetComponent<Image>().color = Color.white;

        if (countType)
        {
            img_Check[0].SetActive(false);
            img_Check[1].SetActive(true);
            img_Check[2].SetActive(false);
        }
        else
        {
            img_Check[0].SetActive(true);
            img_Check[1].SetActive(true);
            img_Check[2].SetActive(true);
        }
        */
    }
    
    public void updateProcess()
    {
        // Tutorial 플레이어 및 AI 스크립트에서 호출(미션 성공 체크)
        count++;
        setProcess();
    }

    void setProcess()
    {        
        // 미션 진행도에 따라 UI 및 미션 업데이트 진행
        switch (count)
        {
            case 0:                
                break;
            case 1:
                if (countType)
                {
                    img_Check[1].SetActive(true);
                    tutLv++;
                    Invoke("updateLv", 3f);
                }
                else
                {
                    img_Check[0].SetActive(true);
                }
                break;
            case 2:
                img_Check[1].SetActive(true);
                break;
            case 3:
                img_Check[2].SetActive(true);
                tutLv++;
                Invoke("updateLv", 3f);                
                break;
        }
    }

    void changeArea(int index)
    {
        player.transform.position = (playerPos[index].position);
        player.transform.rotation = (playerPos[index].rotation);        
    }

    void updatePanel()
    {     
        for(int i=0; i<13; i++)
        {
            lvPanel[i].SetActive(false);
        }
        switch (tutLv)
        {
            case 0:
                lvPanel[0].SetActive(true);
                break;
            case 1:                
                lvPanel[1].SetActive(true);
                break;
            case 2:                
                lvPanel[2].SetActive(true);
                break;
            case 3:
                lvPanel[3].SetActive(true);
                break;
            case 4:
                lvPanel[4].SetActive(true);
                break;
            case 5:
                lvPanel[5].SetActive(true);
                break;
            case 6:
                lvPanel[6].SetActive(true);
                break;
            case 7:
                lvPanel[7].SetActive(true);
                break;
            case 8:
                lvPanel[8].SetActive(true);
                break;
            case 9:
                lvPanel[9].SetActive(true);
                break;
            case 10:
                lvPanel[10].SetActive(true);
                break;
            case 11:
                lvPanel[11].SetActive(true);
                break;
            case 12:
                lvPanel[12].SetActive(true);
                break;
            case 100:
                // clear
                lvPanel[13].SetActive(true);
                break;
        }
    }

    void updateLv()
    {
        count = 0;
        switch (tutLv)
        {  
                // lv0 : 이동
            case 1:
                // LV1 : AAAA 콤보 3번
                countType = false;
                setImgChecker();
                //lvPanel[0].SetActive(false);
                //lvPanel[1].SetActive(true);
                updatePanel();
                break;
            case 2:
                // LV2 : AAAB 콤보 3번
                player.GetComponent<Tutorial_Ctrl>().setTutLv(tutLv);
                countType = false;
                setImgChecker();
                //lvPanel[1].SetActive(false);
                //lvPanel[2].SetActive(true);
                updatePanel();
                break;
            case 3:
                // LV3 : B 3번
                player.GetComponent<Tutorial_Ctrl>().setTutLv(tutLv);
                countType = false;
                setImgChecker();
                //lvPanel[2].SetActive(false);
                //lvPanel[3].SetActive(true);
                updatePanel();
                break;
            case 4:
                // LV4 : AABBG 콤보 1번
                player.GetComponent<Tutorial_Ctrl>().setTutLv(tutLv);
                countType = true;
                setImgChecker();
                //lvPanel[3].SetActive(false);
                //lvPanel[4].SetActive(true);
                updatePanel();
                break;
            case 5:
                // LV5 : B 공격 가드 3번
                player.GetComponent<Tutorial_Ctrl>().setTutLv(tutLv);
                dummy.GetComponent<aiTutorialCtrl>().setTutLv(tutLv);
                countType = false;
                setImgChecker();
                //lvPanel[4].SetActive(false);
                //lvPanel[5].SetActive(true);
                updatePanel();
                break;
            case 6:
                // LV6 : B 공격 반격 3번
                player.GetComponent<Tutorial_Ctrl>().setTutLv(tutLv);
                dummy.GetComponent<aiTutorialCtrl>().setTutLv(tutLv);
                countType = false;
                setImgChecker();
                //lvPanel[5].SetActive(false);
                //lvPanel[6].SetActive(true);
                updatePanel();
                break;
            case 7:
                // LV7 : AAAB 반격 3번
                player.GetComponent<Tutorial_Ctrl>().setTutLv(tutLv);
                dummy.GetComponent<aiTutorialCtrl>().setTutLv(tutLv);
                countType = false;
                setImgChecker();
                //lvPanel[6].SetActive(false);
                //lvPanel[7].SetActive(true);
                updatePanel();
                break;
            case 8:
                // LV8 : 회피/캔슬 1번
                player.GetComponent<Tutorial_Ctrl>().setTutLv(tutLv);
                dummy.GetComponent<aiTutorialCtrl>().setTutLv(tutLv);
                countType = true;
                setImgChecker();
                //lvPanel[7].SetActive(false);
                //lvPanel[8].SetActive(true);
                updatePanel();
                break;
            case 9:
                // LV9 : AAAB로 벽꽝 3번
                player.GetComponent<Tutorial_Ctrl>().setTutLv(tutLv);
                dummy.GetComponent<aiTutorialCtrl>().setTutLv(tutLv);
                countType = false;
                setImgChecker();
                //lvPanel[8].SetActive(false);
                //lvPanel[9].SetActive(true);
                updatePanel();
                break;
            case 10:
                // LV10 : BB로 벽꽝 후 재벽꽝 1번
                player.GetComponent<Tutorial_Ctrl>().setTutLv(tutLv);
                dummy.GetComponent<aiTutorialCtrl>().setTutLv(tutLv);
                countType = true;
                setImgChecker();
                //lvPanel[9].SetActive(false);
                //lvPanel[10].SetActive(true);
                updatePanel();
                break;
            case 11:
                // LV11 : BB로 번지 1번
                player.GetComponent<Tutorial_Ctrl>().setTutLv(tutLv);
                dummy.SetActive(false);
                dummy = dummys[1];
                dummy.SetActive(true);
                dummy.GetComponent<aiTutorialCtrl>().setTutLv(tutLv);

                changeArea(0);                

                countType = true;
                setImgChecker();
                //lvPanel[10].SetActive(false);
                //lvPanel[11].SetActive(true);
                updatePanel();
                break;
            case 12:
                // LV12 : AI 대전
                player.GetComponent<Tutorial_Ctrl>().setTutLv(tutLv);
                lastMgr.SetActive(true);
                changeArea(1);

                //dummy.SetActive(false);
                dummy = dummys[2];
                dummy.SetActive(true);                

                countType = true;
                setImgChecker();
                //lvPanel[11].SetActive(false);
                //lvPanel[12].SetActive(true);
                updatePanel();
                break;
            default:
                break;
        }
    }

    public void Tutclear()
    {
        tutLv = 100;
        for (int i = 0; i < 3; i++)
            img_Check[i].SetActive(false);
        updatePanel();
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

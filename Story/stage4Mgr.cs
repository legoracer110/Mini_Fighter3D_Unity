using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shgames
{

    public class stage4Mgr : MonoBehaviour
    {
        public GameObject storyMgr;
        public bool isTest;

        public GameObject mainCam;

        public bool isStart;
        public Transform stage4CamTr;
                
        public GameObject dummies;
        public GameObject storyRoundMgr;

        public GameObject enemy;
        public GameObject player;

        public GameObject endMgr;

        void Start()
        {
            if (isTest)
                testSetting();
            storyData.currStory.setStage(4);
            Invoke("StoryStart", 2.5f);
        }

        void testSetting()
        {
            storyData.currStory = new storyData();
        }

        void StoryStart()
        {
            isStart = true;
        }

        
        void Update()
        {
            if (isStart)
            {
                mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, stage4CamTr.position, Time.deltaTime);
                mainCam.transform.rotation = Quaternion.Slerp(mainCam.transform.rotation, stage4CamTr.rotation, Time.deltaTime);

                Invoke("RoundStart", 5f);
            }

            
            if (enemy.GetComponent<aiBossCtrl>().isDie)
            {
                Invoke("StageEnd", 3f);
            }

            if (player.GetComponent<playerB_Ctrl>().playerHp <= 0)
            {
                storyMgr.GetComponent<storyMgr>().GameOver();
            }
        }

        void RoundStart()
        {
            isStart = false;
            dummies.SetActive(false);
            storyRoundMgr.SetActive(true);
            mainCam.SetActive(false);
        }

        void StageEnd()
        {
            storyRoundMgr.SetActive(false);
            endMgr.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
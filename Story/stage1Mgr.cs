using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shgames
{

    public class stage1Mgr : MonoBehaviour
    {
        public GameObject storyMgr;
        public bool isTest;
        
        public GameObject mainCam;

        public bool isStart;
        public Transform stage1CamTr;

        public GameObject dummies;
        public GameObject endDummy;
        public GameObject storyRoundMgr;

        public GameObject enemy;
        public GameObject player;

        public GameObject stage2Mgr;

        void Start()
        {
            if(isTest)
                tmpSetting();
            
            //Game.current.setRound(0);
            Game.current.setSelected(0);
            Game.current.setEnemy(1);
            storyData.currStory.setStage(1);
            Invoke("StoryStart", 1f);
        }

        void tmpSetting()
        {
            // Test용 Setting
            Game.current = new Game();
            userData.data = new userData();
            //storyData.currStory = new storyData();
        }

        void StoryStart()
        {
            isStart = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (isStart)
            {
                mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, stage1CamTr.position, Time.deltaTime);
                mainCam.transform.rotation = Quaternion.Slerp(mainCam.transform.rotation, stage1CamTr.rotation, Time.deltaTime);

                Invoke("RoundStart", 5f);
            }

            
            if (enemy.GetComponent<aiRedCtrl>().isDie)
            {
                Invoke("NextStage", 3f);
            }

            if (player.GetComponent<playerK_Ctrl>().playerHp <= 0)
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

        void NextStage()
        {
            //endDummy.GetComponent<Animator>().SetBool("dead", true);
            storyRoundMgr.SetActive(false);
            stage2Mgr.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
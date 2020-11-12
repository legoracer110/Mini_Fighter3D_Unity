using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shgames
{

    public class stage3Mgr : MonoBehaviour
    {
        public GameObject storyMgr;
        public GameObject mainCam;

        public bool isTest;
        public bool isStart;
        public Transform stage3CamTr;

        public GameObject orangeDummy;
        public GameObject redDummy;
        public GameObject dummies;
        public GameObject storyRoundMgr;

        public GameObject enemy;
        public GameObject player;

        public GameObject story4Mgr;

        void Start()
        {
            if (isTest)
                testSetting();
            storyData.currStory.setStage(3);
            orangeDummy.GetComponent<Animator>().SetBool("crouch", true);
            redDummy.GetComponent<Animator>().SetTrigger("red");
            Invoke("StoryStart", 3.5f);
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
                mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, stage3CamTr.position, Time.deltaTime);
                mainCam.transform.rotation = Quaternion.Slerp(mainCam.transform.rotation, stage3CamTr.rotation, Time.deltaTime);

                Invoke("RoundStart", 5f);
            }

            
            if (enemy.GetComponent<aiCtrl>().isFall)
            {
                Invoke("NextStage", 3f);
            }

            if (player.GetComponent<playerL_Ctrl>().playerHp <= 0)
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
            storyRoundMgr.SetActive(false);
            story4Mgr.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
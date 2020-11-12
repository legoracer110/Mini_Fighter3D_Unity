using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shgames
{

    public class stage2Mgr : MonoBehaviour
    {
        public GameObject storyMgr;
        public GameObject mainCam;

        public bool isStart;
        public Transform stage2CamTr;

        public GameObject startDummy;
        public GameObject dummies;
        public GameObject storyRoundMgr;

        public GameObject enemy;
        public GameObject player;

        public GameObject stage3Mgr;

        void Start()
        {
            storyData.currStory.setStage(2);
            startDummy.GetComponent<Animator>().SetBool("dead", true);
            Invoke("StoryStart", 2.5f);
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
                mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, stage2CamTr.position, Time.deltaTime);
                mainCam.transform.rotation = Quaternion.Slerp(mainCam.transform.rotation, stage2CamTr.rotation, Time.deltaTime);

                Invoke("RoundStart", 5f);
            }

            
            if (enemy.GetComponent<aiOrangeCtrl>().isDie)
            {
                Invoke("NextStage", 3f);
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

        void NextStage()
        {
            storyRoundMgr.SetActive(false);
            stage3Mgr.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
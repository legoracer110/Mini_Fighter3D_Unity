using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shgames
{

    public class storyEndMgr : MonoBehaviour
    {
        public bool isTest;

        public bool isStart;

        public GameObject panelEnd;

        public bool isCalc;

        public Text txtGameOver;
        public Text txtReward;

        public GameObject btnMain;

        void Start()
        {
            if (isTest)
                testSetting();
            
            Invoke("storySettlement", 2.5f);
        }

        void testSetting()
        {
            //storyData.currStory = new storyData();
            storyData.currStory.inc_GameOver();
        }

        void storySettlement()
        {
            isStart = true;
            panelEnd.SetActive(true);
            Invoke("calcReward", 3f);
        }


        void calcReward()
        {
            isCalc = true;

            txtGameOver.text = ""+storyData.currStory.getGameOver();

            Debug.Log(storyData.currStory.getGameOver());

            int totReward = storyData.currStory.calc_reward();
            txtReward.text = "<size=27>20000</size>" + "<color=#FF0000> - " + storyData.currStory.getGameOver() + "</color>x2000" + " =   " + "<size=35><color=ff0000>"+storyData.currStory.calc_reward()+ "</color></size>";

            userData.data.setGold(userData.data.getGold() + totReward);

            btnMain.SetActive(true);

            storyData.currStory.setStage(1);
        }

        
    }
}
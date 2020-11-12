using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Game
{
    public static Game current;
        
    string version;
    string[] coupons;
    int couponNum;
    // iimin :     +500,000 gold   +500 ruby
    // batcastle :  +100 ruby (10,000원 상당)
    // jinp12 :     +100,000 gold
    // jjangcyc :   +100,000 gold   +100 ruby
    // energetic :  +5energy

    bool vibrate;   // 진동 여부
    int bgm;        // 배경음 크기
    int efm;        // 효과음 크기


    int selected;
    // 플레이어가 선택한 캐릭터
    // 0 : blue
    // 1 : red
    // 2 : orange
    int enemy;
    // 상대(Enemy)가 선택한 캐릭터    
    // 0 : blue
    // 1 : red
    // 2 : orange
    // 3 : gray
    int mode;
    // 0 : mainMenu
    // 1 : practice
    // 2 : tutorial
    // 3 : upgrade
    // 4 : story
    // 5 : tournament
    // 6 : custom
    // 7 : online
    // 10 : customize
    int stage;
    // 0 : ARENA
    // 1 : SKY CITY
    // 2 : MATRIX
    int round;
    // 선승
    // 1/2 Rounds
    // Setting에서 설정 가능하도록
    bool isLock;
    // 승리 중복판정 안되게

    int difficulty;
    // 난이도
    // 0 : 쉬움
    // 1 : 중간
    // 2 : 어려움

    /*
    int roundState;
    // 0 : 초기
    // 1 : 플레이어 승리
    // 2 : 플레이어 패배
      */ 

    public Game()
    {
        ////// 버젼 업데이트시마다 갱신 //////
        version = "v1.04f";

        coupons = new string[5];
        coupons[0] = "iimin";
        coupons[1] = "batcastle";
        coupons[2] = "jinp12";
        coupons[3] = "jjangcyc";
        coupons[4] = "energetic";

        couponNum = 0;

        //////////////////////////////////////

        vibrate = true;
        bgm = 100;
        efm = 100;


        selected = 0;
        enemy = 0;
        mode = 0;
        stage = 0;
        round = 2;
        //roundState = 0;
        difficulty = 0;

        isLock = false;

        storyData.currStory = new storyData();
    }

    public string getVersion()
    {
        return version;
    }

    public bool getVibrate()
    {
        return vibrate;
    }

    public int getBgm()
    {
        return bgm;
    }

    public int getEfm()
    {
        return efm;
    }

    public void setVibrate(bool vibrate)
    {
        this.vibrate = vibrate;
    }

    public void setBgm(int bgm)
    {
        this.bgm = bgm;
    }

    public void setEfm(int efm)
    {
        this.efm = efm;
    }

    public int getSelected()
    {
        return selected;
    }

    public int getEnemy()    
    {
        return enemy;
    }

    public int getMode()
    {
        return mode;
    }

    public int getStage()
    {
        return stage;
    }

    public int getRound()
    {
        return round;
    }
    /*
    public int getRoundState()
    {
        return roundState;
    }
    */
    
    public int getDifficulty()
    {
        return this.difficulty;
    }

    public void setSelected(int selected)
    {
        this.selected = selected;
    }

    public void setEnemy(int enemy)
    {
        this.enemy = enemy;
    }

    public void setMode(int mode)
    {
        this.mode = mode;
    }

    public void setStage(int stage)
    {
        this.stage = stage;
    }

    public void setRound(int round)
    {
        this.round = round;
    }
    /*
    public void setRoundState(int roundState)
    {
        this.roundState = roundState;
    }
    */

    public void setDifficulty(int difficulty)
    {
        this.difficulty = difficulty;
    }

    public bool CheckCoupon(string input)
    {
        for(int i=0; i<coupons.Length; i++)
        {
            if (input == coupons[i])
            {
                couponNum = i;
                return true;
            }
        }
        return false;
    }

    public int getCouponNum()
    {
        return couponNum;
    }

    public bool getLock()
    {
        return isLock;
    }

    public void setLock(bool isLock)
    {
        this.isLock = isLock;
    }
}

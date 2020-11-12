using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class userData
{
    string userID;          //  유저 아이디 (구글)
    string userNum;         //  구글에서 배정된 유저번호
    int mainIndex;          //  매인케릭터 인덱스    
                            //  0 : BLUE
                            //  1 : RED
                            //  2 : ORANGE

    int maxRank;            //  달성 최고계급

    public static userData data;    

    public dataStatCharacter blue = new dataStatCharacter(0);
    public dataStatCharacter red = new dataStatCharacter(1);
    public dataStatCharacter orange = new dataStatCharacter(2);

    int energy;     //  에너지
    int gold;       //  골드
    int ruby;       //  루비

    bool[] couponed;     //  쿠폰 사용 여부

    System.DateTime keyTime;

    public userData()
    {
        userID = "legoracer";

        // 임시 //
        userNum = "d0154677ef486";

        couponed = new bool[5];

        mainIndex = 1;

        energy = 10;
        gold = 0;
        ruby = 0;

        maxRank = 0;
    }

    public string getID()
    {
        return userID;
    }

    public string getUserNum()
    {
        return userNum;
    }

    public int getMainIndex()
    {
        return mainIndex;
    }

    public void setID(string userID)
    {
        this.userID = userID;
    }

    public void setMainIndex(int mainIndex)
    {
        this.mainIndex = mainIndex;
    }

    public int getEnergy()
    {
        return energy;
    }

    public int getGold()
    {
        return gold;
    }

    public int getRuby()
    {
        return ruby;
    }

    public void setEnergy(int energy)
    {
        this.energy = energy;
    }

    public void incEnergy(int energy)
    {
        this.energy += energy;
    }

    public void decEnergy()
    {
        energy -= 2;
    }

    public void setGold(int gold)
    {
        //Debug.Log(gold);
        this.gold = gold;
    }

    public void setRuby(int ruby)
    {
        this.ruby = ruby;
    }

    public void incGold(int gold)
    {
        this.gold += gold;
    }

    public void incRuby(int ruby)
    {
        this.ruby += ruby;
    }

    public bool getUsedCoupon(int num)
    {        
        // 쿠폰을 사용했는지 여부 반환
        if (couponed[num])
            return true;
        else
            return false;        
    }

    public void useCoupon(int num)
    {
        // 쿠폰 사용 체크
        couponed[num] = true;
    }


    public System.DateTime getKeyTime()
    {
        return keyTime;
    }

    public void setKeyTime(System.DateTime t)
    {
        keyTime = t;
    }

    public int getMaxRank()
    {
        return maxRank;
    }

    public void setMaxRank(int rank)
    {
        maxRank = rank;
    }
}

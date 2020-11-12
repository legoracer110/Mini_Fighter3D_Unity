using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataStatCharacter
{
    public int index;

    private int lv;
    private int lv_tot_exp;
    private int lv_exp;

    private int lv_req_exp;     // 다음 레벨업 경험치 필요량

    private int power;
    private int health;
    private int trick;

    // Customize Data
    private bool[] head_own;
    private bool[] top_own;
    private bool[] acc_own;
    private bool[] etc_own;
    private bool[] etc2_own;

    private int[] equip;


    private int rank;
    private int rank_exp;

    public dataStatCharacter(int index)
    {
        this.index = index;
        lv = 1;
        power = 0;
        health = 0;
        trick = 0;
             
        lv_exp = 0;
        lv_req_exp = 1000;
        rank_exp = 0;
        //Debug.Log("testCharacter Created : " + index);

        head_own = new bool[5];
        top_own = new bool[5];
        acc_own = new bool[5];
        etc_own = new bool[5];
        etc2_own = new bool[5];

        equip = new int[5];

        rank = 0;
        rank_exp = 0;
    }

    public int getLv()
    {
        return lv;
    }

    public int getLvExp()
    {
        return lv_exp;
    }

    public void setLvExp(int exp)
    {
        lv_tot_exp = exp;
        cal_lv();
    }

    public void increaseExp(int exp)
    {
        lv_tot_exp += exp;
        //lv_exp += exp;
        cal_lv();
    }


    public int getTotalExp()
    {
        return lv_tot_exp;
    }

    public int getLvReq()
    {
        return lv_req_exp;
    }    

    public int getPower()
    {
        return power;
    }

    public int getHealth()
    {
        return health;
    }

    public int getTrick()
    {
        return trick;
    }

    public void setLv(int lv)
    {
        this.lv = lv;
    }

    public void setPower(int power)
    {
        this.power = power;
    }

    public void setHealth(int health)
    {
        this.health = health;
    }

    public void setTrick(int trick)
    {
        this.trick = trick;
    }

    public void cal_lv()
    {
        // lv 계산
        //  싱글플레이 EXP 증가량
        //​		    	승      패       총합
        //   EASY : +1,500    +600
        // NORMAL : +1,800    +600
        //   HARD : +2,300    +600
        //  LV0 ~LV1   :	1,000       1,000
        //  LV1 ~LV2   :	1,400       2,400
        //  LV2 ~LV3   :	2,000       4,400
        //  LV3 ~LV4   :	3,200       7,600
        //  LV4 ~LV5   :	4,200       11,800  < Special Mission >
        //  LV5 ~LV6   :	5,400       17,200
        //  LV6 ~LV7   :	6,800       24,000
        //  LV7 ~LV8   :	8,400       32,400
        //  LV8 ~LV9   :	10,200      42,600
        //  LV9 ~LV10  :	15,000      57,600  < Special Mission >
        if (lv_tot_exp >= 57600)
        {
            lv = 11;
            lv_exp = lv_tot_exp - 57600;
            //lv_req_exp = 99999;
        }
        else if (lv_tot_exp >= 42600)
        {
            lv = 10;
            lv_exp = lv_tot_exp - 42600;
            lv_req_exp = 15000;
        }
        else if (lv_tot_exp >= 32400)
        {
            lv = 9;
            lv_exp = lv_tot_exp - 32400;
            lv_req_exp = 10200;
        }
        else if (lv_tot_exp >= 24000)
        {
            lv = 8;
            lv_exp = lv_tot_exp - 24000;
            lv_req_exp = 8400;
        }
        else if (lv_tot_exp >= 17200)
        {
            lv = 7;
            lv_exp = lv_tot_exp - 17200;
            lv_req_exp = 6800;
        }
        else if (lv_tot_exp >= 11800)
        {
            lv = 6;
            lv_exp = lv_tot_exp - 11800;
            lv_req_exp = 5400;
        }
        else if (lv_tot_exp >= 7600)
        {
            lv = 5;
            lv_exp = lv_tot_exp - 7600;
            lv_req_exp = 4200;
        }
        else if (lv_tot_exp >= 4400)
        {
            lv = 4;
            lv_exp = lv_tot_exp - 4400;
            lv_req_exp = 3200;
        }
        else if (lv_tot_exp >= 2400)
        {
            lv = 3;
            lv_exp = lv_tot_exp - 2400;
            lv_req_exp = 2000;
        }
        else if (lv_tot_exp >= 1000)
        {
            lv = 2;
            lv_exp = lv_tot_exp - 1000;
            lv_req_exp = 1400;
        }
        else
        {
            lv = 1;
            lv_exp = lv_tot_exp;
            //lv_req_exp = 1000;
        }
    }

    public bool[] getHead()
    {
        return head_own;
    }

    public bool[] getTop()
    {
        return top_own;
    }

    public bool[] getAcc()
    {
        return acc_own;
    }

    public bool[] getEtc()
    {
        return etc_own;
    }

    public bool[] getEtc2()
    {
        return etc2_own;
    }

    public int[] getEquip()
    {
        return equip;
    }

    public void setHeadOwn(int index)
    {
        head_own[index] = true;
    }

    public void setTopOwn(int index)
    {
        top_own[index] = true;
    }

    public void setAccOwn(int index)
    {
        acc_own[index] = true;
    }

    public void setEtcOwn(int index)
    {
        etc_own[index] = true;
    }

    public void setEtc2Own(int index)
    {
        etc2_own[index] = true;
    }

    public void setEquip(int a, int b, int c, int d, int e)
    {
        equip[0] = a;
        equip[1] = b;
        equip[2] = c;
        equip[3] = d;
        equip[4] = e;
    }

    // RANK SYSTEM
    // 1. 포인트 (rank_exp) 획득/손실량
    //                     승    패
    //      1) 동계급      +5    -5          
    //      2) 1계급차     +3    -4
    //      3) 2계급차     +1    -2
    //      4) 그 이상     +0    -0
    //
    // 2. 승급 강등 필요 포인트
    //     cf> rank1 (Trainee) 밑으로는 강등 안댐 (비기너로 강등 X)
    // rank         rank_exp 요구량    칭호          승급시 획득      누적포인트
    //  0           0                  Beginner          
    //  1           5                  Trainee          +0                5         (1판 이기면 승급)
    //  2           20                 Intermediate     +10               30        (3판 더 이기면 승급)
    //  3           45                 Expert           +10               55        (5판 더 이기면 승급)
    //  4           95                 Master           +10               105       (8판 더 이기면 승급)     
    //  5           155                Champion         +5                160       (10판 더 이기면 승급)

    public int getRank()
    {
        return rank;
    }

    public int getRankExp()
    {
        return rank_exp;
    }

    // 랭크 승/강단 계산 및 적용
    public void calc_rank()
    {        
        if(rank_exp >= 155)
        {
            if(rank != 5)
            {
                // 랭크 [4->5] 로 승단 (승단시 포인트 초기화)
                rank = 5;
                rank_exp = 160;
            }           
        }else if (rank_exp >= 95)
        {
            if(rank != 4)
            {
                // 랭크 [5->4] 로 강등 (강등시 포인트 초기화)
                // 랭크 [3->4] 로 승단 (승단시 포인트 초기화)
                rank = 4;
                rank_exp = 105;
            }
        }else if(rank_exp >= 45)
        {
            if (rank != 3)
            {
                // 랭크 [4->3] 로 강등 (강등시 포인트 초기화)
                // 랭크 [2->3] 로 승단 (승단시 포인트 초기화)
                rank = 3;
                rank_exp = 55;
            }
        }else if(rank_exp >= 20)
        {
            if (rank != 2)
            {
                // 랭크 [3->2] 로 강등 (강등시 포인트 초기화)
                // 랭크 [1->2] 로 승단 (승단시 포인트 초기화)
                rank = 2;
                rank_exp = 30;
            }
        }else if(rank_exp >= 5)
        {
            if (rank != 1)
            {
                // 랭크 [3->2] 로 강등 (강등시 포인트 초기화)
                // 랭크 [1->2] 로 승단 (승단시 포인트 초기화)
                rank = 1;
                rank_exp = 5;
            }
        }/*        
        else{
                // Beginner에 대한 강등 처리는 하지 않음
        }
        */
    }

    // 랭크 Exp포인트 증감
    public void setRankExp(bool win, int gap)
    {
        
        if (win)
        {
            // 승리시
            switch (gap)
            {
                case 0:
                    // 동계급(계급차이 없을 때)
                    rank_exp += 5;
                    break;
                case 1:
                    // 1계급차
                    rank_exp += 3;
                    break;
                case 2:
                    // 2계급차
                    rank_exp += 1;
                    break;
                default:
                    // 그 외
                    break;
            }
        }
        else
        {
            // 패배시
            switch (gap)
            {
                case 0:
                    // 동계급(계급차이 없을 때)
                    rank_exp -= 5;
                    break;
                case 1:
                    // 1계급차
                    rank_exp -= 4;
                    break;
                case 2:
                    // 2계급차
                    rank_exp -= 2;
                    break;
                default:
                    // 그 외
                    break;
            }
        }

        calc_rank();
    }


    // 승급 또는 강등 매치 여부 확인
    public int checkRankMatch(int dif)
    {
        // diff
        // 0 : 일반 매치
        // 1 : 승급 매치
        // 2 : 강등 매치

        int check = 0;

        if (dif >= 3)
            return check;

        switch (dif)
        {
            case 0:
                // 동계급                
                if( (rank == 4 && rank_exp + 5 >= 155)
                    || (rank == 3 && rank_exp + 5 >= 95)
                    || (rank == 2 && rank_exp + 5 >= 45)
                    || (rank == 1 && rank_exp + 5 >= 20)
                    || (rank == 0)
                    )
                {
                    // 승급 조건들
                    check = 1;
                }
                
                if( (rank == 5 && rank_exp - 5 <= 155)
                    || (rank == 4 && rank_exp - 5 <= 95)
                    || (rank == 3 && rank_exp - 5 <= 45)
                    || (rank == 2 && rank_exp - 5 <= 20)                    
                    )
                {
                    // 패배 조건들
                    check = 2;
                }
                break;

            case 1:
                // 1계급 차                
                if ((rank == 4 && rank_exp + 3 >= 155)
                    || (rank == 3 && rank_exp + 3 >= 95)
                    || (rank == 2 && rank_exp + 3 >= 45)
                    || (rank == 1 && rank_exp + 3 >= 20)
                    || (rank == 0)
                    )
                {
                    // 승급 조건들
                    check = 1;
                }

                if ((rank == 5 && rank_exp - 4 <= 155)
                    || (rank == 4 && rank_exp - 4 <= 95)
                    || (rank == 3 && rank_exp - 4 <= 45)
                    || (rank == 2 && rank_exp - 4 <= 20)
                    )
                {
                    // 패배 조건들
                    check = 2;
                }
                break;
            case 2:
                // 2계급 차                
                if ((rank == 4 && rank_exp + 1 >= 155)
                    || (rank == 3 && rank_exp + 1 >= 95)
                    || (rank == 2 && rank_exp + 1 >= 45)
                    || (rank == 1 && rank_exp + 1 >= 20)
                    || (rank == 0)
                    )
                {
                    // 승급 조건들
                    check = 1;
                }

                if ((rank == 5 && rank_exp - 2 <= 155)
                    || (rank == 4 && rank_exp - 2 <= 95)
                    || (rank == 3 && rank_exp - 2 <= 45)
                    || (rank == 2 && rank_exp - 2 <= 20)
                    )
                {
                    // 패배 조건들
                    check = 2;
                }
                break;
        }
        

        return check;
    }
}

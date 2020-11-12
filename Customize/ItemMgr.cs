using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMgr : MonoBehaviour
{
    //public GameObject[] model;
    public GameObject resPanel;     // ResourcePanel

    public int charIndex;
    public int itemIndex; // 현재 커서가 놓인 아이템

    // 장착 중인 아이템
    public int[] equip;
    // 유저가 이미 가지고 있는 아이템 목록 (DB에서 가져와서 초기화)
    public bool[] head_own;
    public bool[] top_own;
    public bool[] acc_own;
    public bool[] etc_own;
    public bool[] etc2_own;

    public GameObject buyPanel;
    public GameObject error;
    public GameObject error2;

    public GameObject confirmPanel;

    public Sprite[] imgPriceList = new Sprite[2];
    public Sprite[] imgItemList = new Sprite[10];
    public Text txtName;
    public Image imgPrice;
    public Image imgItem;
    public Text txtPrice;

    public Text[] txtPriceChange;

    public GameObject[] txtEquips;

    /*
    ITEM INDEX
    // 0 : Nothing
    // 1 : HeadGear (Red_Head)
    // 2 : Cap (Red_Head)
    // 3 : Plate Armor (Red_Top)
    // 4 : BoxingGloves_L (Red_Acc)
    // 5 : BoxingGloves_R (Red_Acc)
    // 6 : HandGears_L (Red_Acc)
    // 7 : HandGears_R (Red_Acc)
    // 8 : Bat (Red_Etc)
    // 9 : Shuriken (Red_Etc)
    // 10 :     MagHelmet (Red_Head)
    // 11 :     Kendo Sword (Red_Etc)
    // 12 :     Sunglasses (Red_Etc2)
    // 13 :     HeartSunglasses (Red_Etc2)
    // 14 :
    // 15 : Bike Helmet (Blue_Head)
    // 16 : Cap (Blue_Head)
    // 17 : Down Vest (Blue_Top)
    // 18 :     MagHelmet (Blue_Head) 
    // 19 : 
    // 20 : Bat (Blue_Etc)
    // 21 : Shuriken (Blue_Etc)
    // 22 :     Kendo Sword (Blue_Etc)
    // 23 :     Sunglasses (Blue_Etc2)
    // 24 :     HeartSunglasses (Blue_Etc2)
    // 25 :
    // 26 :
    // 27 :
    // 28 :
    // 29 :
    // 30 :     Kendo Helmet (Orange_Head)
    // 31 :     HeadPhone (Orange_Head)
    // 32 :     MagHelmet (Orange_Head)
    // 33 :     Nunchaku (Orange_Acc)
    // 34 :     Kendo Sword_side (Orange_Acc)
    // 35 :     Bat (Orange_Etc)
    // 36 :     Shuriken (Orange_Etc)
    // 37 :     Kendo Sword (Orange_Etc)
    // 38 :     Sunglasses (Orange_Etc2)
    // 39 :     HeartSunglasses (Orange_Etc2)
    // 40 :     Plate Armor (Orange_Top)

    
    */

    public GameObject[] RED_HEAD;
    // 0 : Nothing
    // 1 : HeadGear (Red_Head)
    // 2 : Cap (Red_Head)
    // 3 :      MegHelmet (Red_Head)
    public GameObject[] RED_TOP;
    // 0 : Nothing
    // 1 : Plate Armor (Red_Top)
    public GameObject[] RED_ACC;
    // 0 : Nothing
    // 1 : BoxingGloves_L (Red_Acc)
    // 2 : BoxingGloves_R (Red_Acc)
    // 3 : HandGears_L (Red_Acc)
    // 4 : HandGears_R (Red_Acc)
    public GameObject[] RED_ETC;
    // 0 : Nothing
    // 1 : Bat (Red_Etc)
    // 2 : Shuriken (Red_Etc)
    // 3 :      Kendo Sword (Red_Etc)
    public GameObject[] RED_ETC2;
    // 0 : Nothing
    // 1 :      Sunglasses (Red_Etc2)
    // 2 :      Heart Sunglasses (Red_Etc2)

    public GameObject[] BLUE_HEAD;
    // 0 : Nothing
    // 1 : Bike Helmet (Blue_Head)
    // 2 : Cap (Blue_Head)
    // 3 :      MegHelmet (Blue_Head)
    public GameObject[] BLUE_TOP;
    // 0 : Nothing
    // 1 : Down Vest (Blue_Top)
    public GameObject[] BLUE_ACC;
    // 0 : Nothing
    // 1 :
    public GameObject[] BLUE_ETC;
    // 0 : Nothing
    // 1 : Bat (Blue_Etc)
    // 2 : Shuriken (Blue_Etc)
    // 3 :      Kendo Sword (Blue_Etc)
    public GameObject[] BLUE_ETC2;
    // 0 : Nothing
    // 1 :      Sunglasses (Blue_Etc2)
    // 2 :      Heart Sunglasses (Blue_Etc2)

    public GameObject[] ORANGE_HEAD;
    // 0 : Nothing
    // 1 :     Kendo Helmet (Orange_Head)
    // 2 :     HeadPhone (Orange_Head)
    // 3 :     MagHelmet (Orange_Head)
    public GameObject[] ORANGE_TOP;
    // 0 : Nothing
    // 1 :     Plate Armor (Orange_Top)
    public GameObject[] ORANGE_ACC;
    // 0 : Nothing
    // 1 :     Nunchaku (Orange_Acc)
    // 2 :     Kendo Sword_side (Orange_Acc)
    public GameObject[] ORANGE_ETC;
    // 0 : Nothing
    // 1 :     Bat (Orange_Etc)
    // 2 :     Shuriken (Orange_Etc)
    // 3 :     Kendo Sword (Orange_Etc)
    public GameObject[] ORANGE_ETC2;
    // 0 : Nothing
    // 1 :     Sunglasses (Orange_Etc2)
    // 2 :     HeartSunglasses (Orange_Etc2)

    public GameObject buyBtn;
    public GameObject equipBtn;
    public GameObject unEquipBtn;


    bool isGold;
    int price;

    void Start()
    {
        // DB에서 유저 아이템 리스트 가져와서 own에 초기화
        //testInit1();

        charIndex = 0;
        equip = userData.data.blue.getEquip();

        setAvataEquip(0);

        //resPanel.GetComponent<ResPanelMgr>().UpdateMoney();
    }

    void testInit1()
    {
        userData.data = new userData();
        
        userData.data.blue.setEquip(0, 0, 0, 0, 0);
        userData.data.red.setEquip(0, 0, 0, 0, 0);
        userData.data.orange.setEquip(0, 0, 0, 0, 0);

        //Debug.Log("현재 재화 : " + userData.data.getGold() + " 골드 / " + userData.data.getRuby() + " 루비");
    }

    void testInit2()
    {        
        userData.data = new userData();

        userData.data.red.setHeadOwn(1);
        userData.data.red.setTopOwn(1);

        userData.data.blue.setEquip(0, 0, 0, 0, 0);
        userData.data.red.setEquip(1, 0, 0, 0, 0);
        userData.data.orange.setEquip(0, 0, 0, 0, 0);        
    }

    // 아바타에게 장착 목록 입히기
    public void setAvataEquip(int charIndex)
    {
        this.charIndex = charIndex;
        switch (charIndex)
        {
            case 0:
                equip = userData.data.blue.getEquip();
                resetAvata(0);
                BLUE_HEAD[equip[0]].SetActive(true);
                BLUE_TOP[equip[1]].SetActive(true);
                BLUE_ACC[equip[2]].SetActive(true);
                BLUE_ETC[equip[3]].SetActive(true);
                BLUE_ETC2[equip[4]].SetActive(true);
                head_own = userData.data.blue.getHead();
                top_own = userData.data.blue.getTop();
                acc_own = userData.data.blue.getAcc();
                etc_own = userData.data.blue.getEtc();
                etc2_own = userData.data.blue.getEtc2();
                break;
            case 1:
                equip = userData.data.red.getEquip();
                resetAvata(1);
                RED_HEAD[equip[0]].SetActive(true);
                RED_TOP[equip[1]].SetActive(true);
                if (equip[2] == 1 || equip[2] == 3)
                {
                    RED_ACC[equip[2]].SetActive(true);
                    RED_ACC[equip[2]+1].SetActive(true);
                }
                RED_ETC[equip[3]].SetActive(true);
                RED_ETC2[equip[4]].SetActive(true);
                head_own = userData.data.red.getHead();
                top_own = userData.data.red.getTop();
                acc_own = userData.data.red.getAcc();
                etc_own = userData.data.red.getEtc();
                etc2_own = userData.data.red.getEtc2();
                break;
            case 2:
                equip = userData.data.orange.getEquip();
                resetAvata(2);
                ORANGE_HEAD[equip[0]].SetActive(true);
                ORANGE_TOP[equip[1]].SetActive(true);
                ORANGE_ACC[equip[2]].SetActive(true);
                ORANGE_ETC[equip[3]].SetActive(true);
                ORANGE_ETC2[equip[4]].SetActive(true);
                head_own = userData.data.orange.getHead();
                top_own = userData.data.orange.getTop();
                acc_own = userData.data.orange.getAcc();
                etc_own = userData.data.orange.getEtc();
                etc2_own = userData.data.orange.getEtc2();
                break;
        }        

    }

    /////////// RED ////////////

    // 1 : 헤드기어 선택시 (RED)
    public void OnClick_HeadGear_RED()
    {
        itemIndex = 1;

        if (RED_HEAD[1].activeSelf)
        {
            RED_HEAD[1].SetActive(false);
            if (head_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
            
        }
        else
        {
            foreach (GameObject head in RED_HEAD)
            {
                head.SetActive(false);
            }

            RED_HEAD[1].SetActive(true);
            if (head_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }            
        }        
    }

    // 2 : 캡 선택시 (RED)
    public void OnClick_Cap_RED()
    {
        itemIndex = 2;
        if (RED_HEAD[2].activeSelf)
        {
            RED_HEAD[2].SetActive(false);
            if (head_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject head in RED_HEAD)
            {
                head.SetActive(false);
            }

            RED_HEAD[2].SetActive(true);
            if (head_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 3 : 플레이트 아머 선택시 (RED)
    public void OnClick_PlateArmor_RED()
    {
        itemIndex = 3;
        if (RED_TOP[1].activeSelf)
        {
            RED_TOP[1].SetActive(false);
            if (top_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject top in RED_TOP)
            {
                top.SetActive(false);
            }

            RED_TOP[1].SetActive(true);
            if (top_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 4(~5) : 복싱글러브 선택시 (RED)
    public void OnClick_BoxingGloves_RED()
    {
        itemIndex = 4;
        if (RED_ACC[1].activeSelf)
        {
            RED_ACC[1].SetActive(false);
            RED_ACC[2].SetActive(false);
            if (acc_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject acc in RED_ACC)
            {
                acc.SetActive(false);
            }

            RED_ACC[1].SetActive(true);
            RED_ACC[2].SetActive(true);
            if (acc_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 6(~7) : 핸드기어 선택시 (RED)
    public void OnClick_HandGears_RED()
    {
        itemIndex = 6;
        if (RED_ACC[3].activeSelf)
        {
            RED_ACC[3].SetActive(false);
            RED_ACC[4].SetActive(false);
            if (acc_own[3])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject acc in RED_ACC)
            {
                acc.SetActive(false);
            }

            RED_ACC[3].SetActive(true);
            RED_ACC[4].SetActive(true);
            if (acc_own[3])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 8 : 야구배트 선택시 (RED)
    public void OnClick_Bat_RED()
    {
        itemIndex = 8;
        if (RED_ETC[1].activeSelf)
        {
            RED_ETC[1].SetActive(false);            
            if (etc_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject etc in RED_ETC)
            {
                etc.SetActive(false);
            }

            RED_ETC[1].SetActive(true);
            if (etc_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 9 : 수리검 선택시 (RED)
    public void OnClick_Shuriken_RED()
    {
        itemIndex = 9;
        if (RED_ETC[2].activeSelf)
        {
            RED_ETC[2].SetActive(false);
            if (etc_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject etc in RED_ETC)
            {
                etc.SetActive(false);
            }

            RED_ETC[2].SetActive(true);
            if (etc_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 10 : 매그니토 헬맷 선택시 (RED)
    public void OnClick_MagHelmet_RED()
    {
        itemIndex = 10;

        if (RED_HEAD[3].activeSelf)
        {
            RED_HEAD[3].SetActive(false);
            if (head_own[3])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject head in RED_HEAD)
            {
                head.SetActive(false);
            }

            RED_HEAD[3].SetActive(true);
            if (head_own[3])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 11 : 죽도 선택시 (RED)
    public void OnClick_KendoSword_RED()
    {
        itemIndex = 11;
        if (RED_ETC[3].activeSelf)
        {
            RED_ETC[3].SetActive(false);
            if (etc_own[3])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject etc in RED_ETC)
            {
                etc.SetActive(false);
            }

            RED_ETC[3].SetActive(true);
            if (etc_own[3])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 12 : 썬글라스 선택시 (RED)
    public void OnClick_Sunglasses_RED()
    {
        itemIndex = 12;
        if (RED_ETC2[1].activeSelf)
        {
            RED_ETC2[1].SetActive(false);
            if (etc2_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject etc2 in RED_ETC2)
            {
                etc2.SetActive(false);
            }

            RED_ETC2[1].SetActive(true);
            if (etc2_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 13 : 하트 썬글라스 선택시 (RED)
    public void OnClick_HeartSunglasses_RED()
    {
        itemIndex = 13;
        if (RED_ETC2[2].activeSelf)
        {
            RED_ETC2[2].SetActive(false);
            if (etc2_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject etc2 in RED_ETC2)
            {
                etc2.SetActive(false);
            }

            RED_ETC2[2].SetActive(true);
            if (etc2_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    /////////// BLUE ////////////

    // 15 : 바이크헬멧 선택시 (BLUE)
    public void OnClick_BikeHelmet_BLUE()
    {
        itemIndex = 15;

        if (BLUE_HEAD[1].activeSelf)
        {
            BLUE_HEAD[1].SetActive(false);
            if (head_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject head in BLUE_HEAD)
            {
                head.SetActive(false);
            }

            BLUE_HEAD[1].SetActive(true);
            if (head_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 16 : 캡 선택시 (BLUE)
    public void OnClick_Cap_BLUE()
    {
        itemIndex = 16;
        if (BLUE_HEAD[2].activeSelf)
        {
            BLUE_HEAD[2].SetActive(false);
            if (head_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject head in BLUE_HEAD)
            {
                head.SetActive(false);
            }

            BLUE_HEAD[2].SetActive(true);
            if (head_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 17 : 다운 베스트 선택시 (BLUE)
    public void OnClick_DownVest_BLUE()
    {
        itemIndex = 17;
        if (BLUE_TOP[1].activeSelf)
        {
            BLUE_TOP[1].SetActive(false);
            if (top_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject top in BLUE_TOP)
            {
                top.SetActive(false);
            }

            BLUE_TOP[1].SetActive(true);
            if (top_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 18 : 매그니토 헬맷 선택시 (BLUE)
    public void OnClick_MagHelmet_BLUE()
    {
        itemIndex = 18;

        if (BLUE_HEAD[3].activeSelf)
        {
            BLUE_HEAD[3].SetActive(false);
            if (head_own[3])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject head in BLUE_HEAD)
            {
                head.SetActive(false);
            }

            BLUE_HEAD[3].SetActive(true);
            if (head_own[3])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 20 : 야구배트 선택시 (BLUE)
    public void OnClick_Bat_BLUE()
    {
        itemIndex = 20;
        if (BLUE_ETC[1].activeSelf)
        {
            BLUE_ETC[1].SetActive(false);
            if (etc_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject etc in BLUE_ETC)
            {
                etc.SetActive(false);
            }

            BLUE_ETC[1].SetActive(true);
            if (etc_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 21 : 수리검 선택시 (BLUE)
    public void OnClick_Shuriken_BLUE()
    {
        itemIndex = 21;
        if (BLUE_ETC[2].activeSelf)
        {
            BLUE_ETC[2].SetActive(false);
            if (etc_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject etc in BLUE_ETC)
            {
                etc.SetActive(false);
            }

            BLUE_ETC[2].SetActive(true);
            if (etc_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 22 : 죽도 선택시 (BLUE)
    public void OnClick_KendoSword_BLUE()
    {
        itemIndex = 22;
        if (BLUE_ETC[3].activeSelf)
        {
            BLUE_ETC[3].SetActive(false);
            if (etc_own[3])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject etc in BLUE_ETC)
            {
                etc.SetActive(false);
            }

            BLUE_ETC[3].SetActive(true);
            if (etc_own[3])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 23 : 썬글라스 선택시 (BLUE)
    public void OnClick_Sunglasses_BLUE()
    {
        itemIndex = 23;
        if (BLUE_ETC2[1].activeSelf)
        {
            BLUE_ETC2[1].SetActive(false);
            if (etc2_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject etc2 in BLUE_ETC2)
            {
                etc2.SetActive(false);
            }

            BLUE_ETC2[1].SetActive(true);
            if (etc2_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 24 : 하트 썬글라스 선택시 (BLUE)
    public void OnClick_HeartSunglasses_BLUE()
    {
        itemIndex = 24;
        if (BLUE_ETC2[2].activeSelf)
        {
            BLUE_ETC2[2].SetActive(false);
            if (etc2_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject etc2 in BLUE_ETC2)
            {
                etc2.SetActive(false);
            }

            BLUE_ETC2[2].SetActive(true);
            if (etc2_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    /////////// BLUE ////////////

    // 30 : 검도 헬맷 선택시 (ORANGE)
    public void OnClick_KendoHelmet_Orange()
    {
        itemIndex = 30;

        if (ORANGE_HEAD[1].activeSelf)
        {
            ORANGE_HEAD[1].SetActive(false);
            if (head_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject head in ORANGE_HEAD)
            {
                head.SetActive(false);
            }

            ORANGE_HEAD[1].SetActive(true);
            if (head_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 31 : 해드폰 선택시 (ORANGE)
    public void OnClick_HeadPhone_Orange()
    {
        itemIndex = 31;

        if (ORANGE_HEAD[2].activeSelf)
        {
            ORANGE_HEAD[2].SetActive(false);
            if (head_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject head in ORANGE_HEAD)
            {
                head.SetActive(false);
            }

            ORANGE_HEAD[2].SetActive(true);
            if (head_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 32 : 매그니토 헬맷 선택시 (ORANGE)
    public void OnClick_MagHelmet_Orange()
    {
        itemIndex = 32;

        if (ORANGE_HEAD[3].activeSelf)
        {
            ORANGE_HEAD[3].SetActive(false);
            if (head_own[3])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject head in ORANGE_HEAD)
            {
                head.SetActive(false);
            }

            ORANGE_HEAD[3].SetActive(true);
            if (head_own[3])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 33 : 쌍절곤 선택시 (ORANGE)
    public void OnClick_Nunchaku_Orange()
    {
        itemIndex = 33;
        if (ORANGE_ACC[1].activeSelf)
        {
            ORANGE_ACC[1].SetActive(false);
            if (acc_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject acc in ORANGE_ACC)
            {
                acc.SetActive(false);
            }

            ORANGE_ACC[1].SetActive(true);
            if (acc_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 34 : 죽도(등) 선택시 (ORANGE)
    public void OnClick_KendoSwordSide_Orange()
    {
        itemIndex = 34;
        if (ORANGE_ACC[2].activeSelf)
        {
            ORANGE_ACC[2].SetActive(false);
            if (acc_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject acc in ORANGE_ACC)
            {
                acc.SetActive(false);
            }

            ORANGE_ACC[2].SetActive(true);
            if (acc_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 35 : 야구배트 선택시 (ORANGE)
    public void OnClick_Bat_ORANGE()
    {
        itemIndex = 35;
        if (ORANGE_ETC[1].activeSelf)
        {
            ORANGE_ETC[1].SetActive(false);
            if (etc_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject etc in ORANGE_ETC)
            {
                etc.SetActive(false);
            }

            ORANGE_ETC[1].SetActive(true);
            if (etc_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 36 : 수리검 선택시 (ORANGE)
    public void OnClick_Shuriken_ORANGE()
    {
        itemIndex = 36;
        if (ORANGE_ETC[2].activeSelf)
        {
            ORANGE_ETC[2].SetActive(false);
            if (etc_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject etc in ORANGE_ETC)
            {
                etc.SetActive(false);
            }

            ORANGE_ETC[2].SetActive(true);
            if (etc_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 37 : 죽도 선택시 (ORANGE)
    public void OnClick_KendoSword_ORANGE()
    {
        itemIndex = 37;
        if (ORANGE_ETC[3].activeSelf)
        {
            ORANGE_ETC[3].SetActive(false);
            if (etc_own[3])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject etc in ORANGE_ETC)
            {
                etc.SetActive(false);
            }

            ORANGE_ETC[3].SetActive(true);
            if (etc_own[3])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 38 : 썬글라스 선택시 (ORANGE)
    public void OnClick_Sunglasses_ORANGE()
    {
        itemIndex = 38;
        if (ORANGE_ETC2[1].activeSelf)
        {
            ORANGE_ETC2[1].SetActive(false);
            if (etc2_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject etc2 in ORANGE_ETC2)
            {
                etc2.SetActive(false);
            }

            ORANGE_ETC2[1].SetActive(true);
            if (etc2_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 39 : 하트 썬글라스 선택시 (ORANGE)
    public void OnClick_HeartSunglasses_ORANGE()
    {
        itemIndex = 39;
        if (ORANGE_ETC2[2].activeSelf)
        {
            ORANGE_ETC2[2].SetActive(false);
            if (etc2_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject etc2 in ORANGE_ETC2)
            {
                etc2.SetActive(false);
            }

            ORANGE_ETC2[2].SetActive(true);
            if (etc2_own[2])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }

    // 40 : 플레이트 아머 선택시 (ORANGE)
    public void OnClick_PlateArmor_ORANGE()
    {
        itemIndex = 40;
        if (ORANGE_TOP[1].activeSelf)
        {
            ORANGE_TOP[1].SetActive(false);
            if (top_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }

        }
        else
        {
            foreach (GameObject top in ORANGE_TOP)
            {
                top.SetActive(false);
            }

            ORANGE_TOP[1].SetActive(true);
            if (top_own[1])
            {
                buyBtn.SetActive(false);
                equipBtn.SetActive(true);
                unEquipBtn.SetActive(false);
            }
            else
            {
                buyBtn.SetActive(true);
                equipBtn.SetActive(false);
                unEquipBtn.SetActive(false);
            }
        }
    }


    // 장착 초기화 선택시
    public void OnClickResetAvata()
    {
        resetAvata(charIndex);
    }

    // 모든 장착 해제 (비활성화)
    void resetAvata(int charIndex)
    {
        switch (charIndex)
        {
            case 0:
                foreach (GameObject head in BLUE_HEAD)
                {
                    head.SetActive(false);
                }
                foreach (GameObject top in BLUE_TOP)
                {
                    top.SetActive(false);
                }
                foreach (GameObject acc in BLUE_ACC)
                {
                    acc.SetActive(false);
                }
                foreach (GameObject etc in BLUE_ETC)
                {
                    etc.SetActive(false);
                }
                foreach (GameObject etc2 in BLUE_ETC2)
                {
                    etc2.SetActive(false);
                }
                break;
            case 1:
                foreach (GameObject head in RED_HEAD)
                {
                    head.SetActive(false);
                }
                foreach (GameObject top in RED_TOP)
                {
                    top.SetActive(false);
                }
                foreach (GameObject acc in RED_ACC)
                {
                    acc.SetActive(false);
                }
                foreach (GameObject etc in RED_ETC)
                {
                    etc.SetActive(false);
                }
                foreach (GameObject etc2 in RED_ETC2)
                {
                    etc2.SetActive(false);
                }
                break;
            case 2:
                foreach (GameObject head in ORANGE_HEAD)
                {
                    head.SetActive(false);
                }
                foreach (GameObject top in ORANGE_TOP)
                {
                    top.SetActive(false);
                }
                foreach (GameObject acc in ORANGE_ACC)
                {
                    acc.SetActive(false);
                }
                foreach (GameObject etc in ORANGE_ETC)
                {
                    etc.SetActive(false);
                }
                foreach (GameObject etc2 in ORANGE_ETC2)
                {
                    etc2.SetActive(false);
                }
                break;
        }
       
    }        

    void initEquipMark()
    {
        // [장착중] 문구 초기세팅
        /*
        if ()
        equip[0]
        */
    }

    public void OnClickEquip()
    {
        switch (itemIndex)
        {
            case 0:
                break;
            case 1:
                RED_HEAD[1].SetActive(true);
                equip[0] = 1;
                txtEquips[1].SetActive(true);
                txtEquips[2].SetActive(false);
                txtEquips[10].SetActive(false);
                break;
            case 2:
                RED_HEAD[2].SetActive(true);
                equip[0] = 2;
                txtEquips[1].SetActive(false);
                txtEquips[2].SetActive(true);
                txtEquips[10].SetActive(false);
                break;
            case 3:
                RED_TOP[1].SetActive(true);
                equip[1] = 1;
                txtEquips[3].SetActive(true);                
                break;
            case 4:
                RED_ACC[1].SetActive(true);
                RED_ACC[2].SetActive(true);
                equip[2] = 1;
                txtEquips[4].SetActive(true);
                txtEquips[6].SetActive(false);
                break;
            case 6:
                RED_ACC[3].SetActive(true);
                RED_ACC[4].SetActive(true);
                equip[2] = 3;
                txtEquips[4].SetActive(false);
                txtEquips[6].SetActive(true);
                break;
            case 8:
                RED_ETC[1].SetActive(true);
                equip[3] = 1;
                txtEquips[8].SetActive(true);
                txtEquips[9].SetActive(false);
                txtEquips[11].SetActive(false);
                break;
            case 9:
                RED_ETC[2].SetActive(true);
                equip[3] = 2;
                txtEquips[8].SetActive(false);
                txtEquips[9].SetActive(true);
                txtEquips[11].SetActive(false);
                break;

            // 신규 추가 (20.10.31) //      txtEquips는 22번부터
            case 10:
                RED_HEAD[3].SetActive(true);
                equip[0] = 3;
                txtEquips[1].SetActive(false);
                txtEquips[2].SetActive(false);
                txtEquips[10].SetActive(true);
                break;
            case 11:
                RED_ETC[3].SetActive(true);
                equip[3] = 3;
                txtEquips[8].SetActive(false);
                txtEquips[9].SetActive(false);
                txtEquips[11].SetActive(true);
                break;
            case 12:
                RED_ETC2[1].SetActive(true);
                equip[4] = 1;
                txtEquips[12].SetActive(true);
                txtEquips[13].SetActive(false);
                break;
            case 13:
                RED_ETC2[2].SetActive(true);
                equip[4] = 2;
                txtEquips[12].SetActive(false);
                txtEquips[13].SetActive(true);
                break;
            ///////////////////////////

            case 15:
                BLUE_HEAD[1].SetActive(true);
                equip[0] = 1;
                txtEquips[15].SetActive(true);
                txtEquips[16].SetActive(false);
                txtEquips[18].SetActive(false);
                break;
            case 16:
                BLUE_HEAD[2].SetActive(true);
                equip[0] = 2;
                txtEquips[15].SetActive(false);
                txtEquips[16].SetActive(true);
                txtEquips[18].SetActive(false);
                break;
            case 17:
                BLUE_TOP[1].SetActive(true);
                equip[1] = 1;
                txtEquips[17].SetActive(true);
                break;
            // 신규 추가 (20.10.31) //      txtEquips는 26번부터
            case 18:
                BLUE_HEAD[1].SetActive(true);
                equip[0] = 3;
                txtEquips[15].SetActive(false);
                txtEquips[16].SetActive(false);
                txtEquips[18].SetActive(true);
                break;
            ///////////////////////////
            case 20:
                BLUE_ETC[1].SetActive(true);
                equip[3] = 1;
                txtEquips[20].SetActive(true);
                txtEquips[21].SetActive(false);
                txtEquips[22].SetActive(false);
                break;
            case 21:
                BLUE_ETC[2].SetActive(true);
                equip[3] = 2;
                txtEquips[20].SetActive(false);
                txtEquips[21].SetActive(true);
                txtEquips[22].SetActive(false);
                break;
            // 신규 추가 (20.10.31) //      txtEquips는 27번부터
            case 22:
                BLUE_ETC[3].SetActive(true);
                equip[3] = 3;
                txtEquips[20].SetActive(false);
                txtEquips[21].SetActive(false);
                txtEquips[22].SetActive(true);
                break;
            case 23:
                BLUE_ETC2[1].SetActive(true);
                equip[4] = 1;
                txtEquips[23].SetActive(true);
                txtEquips[24].SetActive(false);
                break;
            case 24:
                BLUE_ETC2[2].SetActive(true);
                equip[4] = 2;
                txtEquips[23].SetActive(false);
                txtEquips[24].SetActive(true);
                break;
            case 30:
                ORANGE_HEAD[1].SetActive(true);
                equip[0] = 1;
                txtEquips[30].SetActive(true);
                txtEquips[31].SetActive(false);
                txtEquips[32].SetActive(false);
                break;
            case 31:
                ORANGE_HEAD[2].SetActive(true);
                equip[0] = 2;
                txtEquips[30].SetActive(false);
                txtEquips[31].SetActive(true);
                txtEquips[32].SetActive(false);
                break;
            case 32:
                ORANGE_HEAD[3].SetActive(true);
                equip[0] = 3;
                txtEquips[30].SetActive(false);
                txtEquips[31].SetActive(false) ;
                txtEquips[32].SetActive(true);
                break;
            case 33:
                ORANGE_ACC[1].SetActive(true);
                equip[2] = 1;
                txtEquips[33].SetActive(true);
                txtEquips[34].SetActive(false);
                break;
            case 34:
                ORANGE_ACC[2].SetActive(true);
                equip[2] = 2;
                txtEquips[33].SetActive(false);
                txtEquips[34].SetActive(true);
                break;
            case 35:
                ORANGE_ETC[1].SetActive(true);
                equip[3] = 1;
                txtEquips[35].SetActive(true);
                txtEquips[36].SetActive(false);
                txtEquips[37].SetActive(false);
                break;
            case 36:
                ORANGE_ETC[2].SetActive(true);
                equip[3] = 2;
                txtEquips[35].SetActive(false);
                txtEquips[36].SetActive(true);
                txtEquips[37].SetActive(false);
                break;
            case 37:
                ORANGE_ETC[3].SetActive(true);
                equip[3] = 3;
                txtEquips[35].SetActive(false);
                txtEquips[36].SetActive(false);
                txtEquips[37].SetActive(true);
                break;
            case 38:
                ORANGE_ETC2[1].SetActive(true);
                equip[4] = 1;
                txtEquips[38].SetActive(true);
                txtEquips[39].SetActive(false);
                break;
            case 39:
                ORANGE_ETC2[2].SetActive(true);
                equip[4] = 2;
                txtEquips[38].SetActive(false);
                txtEquips[39].SetActive(true);
                break;
            case 40:
                ORANGE_TOP[1].SetActive(true);
                equip[1] = 1;
                txtEquips[40].SetActive(true);
                break;

                ///////////////////////////
        }


        switch (charIndex)
        {
            case 0:
                userData.data.blue.setEquip(equip[0], equip[1], equip[2], equip[3], equip[4]);
                break;
            case 1:
                userData.data.red.setEquip(equip[0], equip[1], equip[2], equip[3], equip[4]);
                break;
            case 2:
                userData.data.orange.setEquip(equip[0], equip[1], equip[2], equip[3], equip[4]);
                break;
        }

        equipBtn.SetActive(false);
        unEquipBtn.SetActive(true);

    }

    public void OnClickUnEquip(int charIndex)
    {
        switch (itemIndex)
        {
            case 0:
                break;
            case 1:
                RED_HEAD[1].SetActive(false);
                equip[0] = 0;
                txtEquips[1].SetActive(false);
                break;
            case 2:
                RED_HEAD[2].SetActive(false);
                equip[0] = 0;
                txtEquips[2].SetActive(false);
                break;
            case 3:
                RED_TOP[1].SetActive(false);
                equip[1] = 0;
                txtEquips[3].SetActive(false);
                break;
            case 4:
                RED_ACC[1].SetActive(false);
                RED_ACC[2].SetActive(false);
                equip[2] = 0;
                txtEquips[4].SetActive(false);
                break;
            case 6:
                RED_ACC[3].SetActive(false);
                RED_ACC[4].SetActive(false);
                equip[2] = 0;
                txtEquips[6].SetActive(false);
                break;
            case 8:
                RED_ETC[1].SetActive(false);
                equip[3] = 0;
                txtEquips[8].SetActive(false);
                break;
            case 9:
                RED_ETC[2].SetActive(false);
                equip[3] = 0;                
                txtEquips[9].SetActive(false);
                break;
            ////////////////////////////
            case 10:
                RED_HEAD[3].SetActive(false);
                equip[0] = 0;                
                txtEquips[22].SetActive(false);
                break;
            case 11:
                RED_ETC[3].SetActive(false);
                equip[3] = 0;
                txtEquips[23].SetActive(false);
                break;
            case 12:
                RED_ETC2[1].SetActive(false);
                equip[4] = 0;
                txtEquips[24].SetActive(false);
                break;
            case 13:
                RED_ETC2[2].SetActive(false);
                equip[4] = 0;
                txtEquips[25].SetActive(false);
                break;
            ////////////////////////////
            case 15:
                BLUE_HEAD[1].SetActive(false);
                equip[0] = 0;
                txtEquips[15].SetActive(false);
                break;
            case 16:
                BLUE_HEAD[2].SetActive(false);
                equip[0] = 0;
                txtEquips[16].SetActive(false);
                break;
            case 17:
                BLUE_TOP[1].SetActive(false);
                equip[1] = 0;
                txtEquips[17].SetActive(false);
                break;
            ////////////////////////////
            case 18:
                BLUE_HEAD[3].SetActive(false);
                equip[0] = 0;
                txtEquips[26].SetActive(false);
                break;
            ////////////////////////////
            case 20:
                BLUE_ETC[1].SetActive(false);
                equip[3] = 0;
                txtEquips[20].SetActive(false);
                break;
            case 21:
                BLUE_ETC[2].SetActive(false);
                equip[3] = 0;
                txtEquips[21].SetActive(false);
                break;
            ////////////////////////////
            case 22:
                BLUE_ETC[3].SetActive(false);
                equip[3] = 0;
                txtEquips[27].SetActive(false);
                break;
            case 23:
                BLUE_ETC2[1].SetActive(false);
                equip[4] = 0;
                txtEquips[28].SetActive(false);
                break;
            case 24:
                BLUE_ETC2[2].SetActive(false);
                equip[4] = 0;
                txtEquips[29].SetActive(false);
                break;            
            case 30:
                ORANGE_HEAD[1].SetActive(false);
                equip[0] = 0;
                txtEquips[30].SetActive(false);
                break;
            case 31:
                ORANGE_HEAD[2].SetActive(false);
                equip[0] = 0;
                txtEquips[31].SetActive(false);                
                break;
            case 32:
                ORANGE_HEAD[3].SetActive(false);
                equip[0] = 0;
                txtEquips[32].SetActive(false);
                break;
            case 33:
                ORANGE_ACC[1].SetActive(false);
                equip[2] = 0;
                txtEquips[33].SetActive(false);
                break;
            case 34:
                ORANGE_ACC[2].SetActive(false);
                equip[2] = 0;
                txtEquips[34].SetActive(false);
                break;
            case 35:
                ORANGE_ETC[1].SetActive(false);
                equip[3] = 0;
                txtEquips[35].SetActive(false);
                break;
            case 36:
                ORANGE_ETC[2].SetActive(false);
                equip[3] = 0;                
                txtEquips[36].SetActive(false);
                break;
            case 37:
                ORANGE_ETC[3].SetActive(false);
                equip[3] = 0;
                txtEquips[37].SetActive(false);
                break;
            case 38:
                ORANGE_ETC2[1].SetActive(false);
                equip[4] = 0;
                txtEquips[38].SetActive(false);
                break;
            case 39:
                ORANGE_ETC2[2].SetActive(false);
                equip[4] = 0;
                txtEquips[39].SetActive(false);
                break;
            case 40:
                ORANGE_TOP[1].SetActive(false);
                equip[1] = 0;
                txtEquips[40].SetActive(false);
                break;
                ////////////////////////////

        }


        switch (charIndex)
        {
            case 0:
                userData.data.blue.setEquip(equip[0], equip[1], equip[2], equip[3], equip[4]);
                break;
            case 1:
                userData.data.red.setEquip(equip[0], equip[1], equip[2], equip[3], equip[4]);
                break;
            case 2:
                userData.data.orange.setEquip(equip[0], equip[1], equip[2], equip[3], equip[4]);
                break;
        }

        equipBtn.SetActive(true);
        unEquipBtn.SetActive(false);
    }

    public void OnClickBuy()
    {
        switch (itemIndex)
        {
            case 0:
                error.SetActive(true);
                break;
            case 1:
                imgItem.sprite = imgItemList[1];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "헤드기어";
                txtPrice.text = "<color=#FFF540>35,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 35000;
                break;
            case 2:
                imgItem.sprite = imgItemList[2];
                imgPrice.sprite = imgPriceList[1];
                txtName.text = "캡 모자";
                txtPrice.text = "<color=#FF00DF>50</color>";                
                buyPanel.SetActive(true);
                isGold = false;
                price = 50;
                break;
            case 3:
                imgItem.sprite = imgItemList[3];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "플레이트 아머";
                txtPrice.text = "<color=#FFF540>40,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 40000;
                break;
            case 4:
                imgItem.sprite = imgItemList[4];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "복싱 글러브";
                txtPrice.text = "<color=#FFF540>120,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 120000;
                break;
            case 6:
                imgItem.sprite = imgItemList[5];
                imgPrice.sprite = imgPriceList[1];
                txtName.text = "핸드 기어";
                txtPrice.text = "<color=#FF00DF>80</color>";
                buyPanel.SetActive(true);
                isGold = false;
                price = 80;
                break;
            case 8:
                imgItem.sprite = imgItemList[6];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "야구 배트";
                txtPrice.text = "<color=#FFF540>60,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 60000;
                break;
            case 9:
                imgItem.sprite = imgItemList[7];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "수리검";
                txtPrice.text = "<color=#FFF540>40,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 40000;
                break;
            case 10:
                imgItem.sprite = imgItemList[8];
                imgPrice.sprite = imgPriceList[1];
                txtName.text = "맥 투구";
                txtPrice.text = "<color=#FF00DF>60</color>";
                buyPanel.SetActive(true);
                isGold = false;
                price = 60;
                break;
            case 11:
                imgItem.sprite = imgItemList[15];
                imgPrice.sprite = imgPriceList[1];
                txtName.text = "죽도";
                txtPrice.text = "<color=#FF00DF>50</color>";
                buyPanel.SetActive(true);
                isGold = false;
                price = 50;
                break;
            case 12:
                imgItem.sprite = imgItemList[10];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "썬글라스";
                txtPrice.text = "<color=#FFF540>20,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 20000;
                break;
            case 13:
                imgItem.sprite = imgItemList[11];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "하트 썬글라스";
                txtPrice.text = "<color=#FFF540>25,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 25000;
                break;
            case 15:
                imgItem.sprite = imgItemList[8];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "바이크 헬멧";
                txtPrice.text = "<color=#FFF540>55,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 55000;
                break;
            case 16:
                imgItem.sprite = imgItemList[2];
                imgPrice.sprite = imgPriceList[1];
                txtName.text = "캡 모자";
                txtPrice.text = "<color=#FF00DF>50</color>";
                buyPanel.SetActive(true);
                isGold = false;
                price = 50;
                break;
            case 17:
                imgItem.sprite = imgItemList[9];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "다운 베스트";
                txtPrice.text = "<color=#FFF540>60,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 60000;
                break;
            case 18:
                imgItem.sprite = imgItemList[16];
                imgPrice.sprite = imgPriceList[1];
                txtName.text = "맥 투구";
                txtPrice.text = "<color=#FF00DF>60</color>";
                buyPanel.SetActive(true);
                isGold = false;
                price = 60;
                break;
            case 20:
                imgItem.sprite = imgItemList[6];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "야구 배트";
                txtPrice.text = "<color=#FFF540>60,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 60000;
                break;
            case 21:
                imgItem.sprite = imgItemList[7];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "수리검";
                txtPrice.text = "<color=#FFF540>40,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 40000;
                break;
            case 22:
                imgItem.sprite = imgItemList[15];
                imgPrice.sprite = imgPriceList[1];
                txtName.text = "죽도";
                txtPrice.text = "<color=#FF00DF>50</color>";
                buyPanel.SetActive(true);
                isGold = false;
                price = 50;
                break;
            case 23:
                imgItem.sprite = imgItemList[10];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "썬글라스";
                txtPrice.text = "<color=#FFF540>20,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 20000;
                break;
            case 24:
                imgItem.sprite = imgItemList[11];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "하트 썬글라스";
                txtPrice.text = "<color=#FFF540>25,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 25000;
                break;
            case 30:
                imgItem.sprite = imgItemList[12];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "검도 헬맷";
                txtPrice.text = "<color=#FFF540>35,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 35000;
                break;
            case 31:
                imgItem.sprite = imgItemList[13];
                imgPrice.sprite = imgPriceList[1];
                txtName.text = "해드폰";
                txtPrice.text = "<color=#FF00DF>50</color>";
                buyPanel.SetActive(true);
                isGold = false;
                price = 50;
                break;
            case 32:
                imgItem.sprite = imgItemList[8];
                imgPrice.sprite = imgPriceList[1];
                txtName.text = "맥 투구";
                txtPrice.text = "<color=#FF00DF>60</color>";
                buyPanel.SetActive(true);
                isGold = false;
                price = 60;
                break;
            case 33:
                imgItem.sprite = imgItemList[14];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "쌍절곤";
                txtPrice.text = "<color=#FFF540>100,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 100000;
                break;
            case 34:
                imgItem.sprite = imgItemList[15];
                imgPrice.sprite = imgPriceList[1];
                txtName.text = "죽도(허리)";
                txtPrice.text = "<color=#FF00DF>80</color>";
                buyPanel.SetActive(true);
                isGold = false;
                price = 80;
                break;
            case 35:
                imgItem.sprite = imgItemList[6];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "야구 배트";
                txtPrice.text = "<color=#FFF540>60,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 60000;
                break;
            case 36:
                imgItem.sprite = imgItemList[7];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "수리검";
                txtPrice.text = "<color=#FFF540>40,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 40000;
                break;
            case 37:
                imgItem.sprite = imgItemList[15];
                imgPrice.sprite = imgPriceList[1];
                txtName.text = "죽도";
                txtPrice.text = "<color=#FF00DF>50</color>";
                buyPanel.SetActive(true);
                isGold = false;
                price = 50;
                break;
            case 38:
                imgItem.sprite = imgItemList[10];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "썬글라스";
                txtPrice.text = "<color=#FFF540>20,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 20000;
                break;
            case 39:
                imgItem.sprite = imgItemList[11];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "하트 썬글라스";
                txtPrice.text = "<color=#FFF540>25,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 25000;
                break;
            case 40:
                imgItem.sprite = imgItemList[3];
                imgPrice.sprite = imgPriceList[0];
                txtName.text = "플레이트 아머";
                txtPrice.text = "<color=#FFF540>40,000</color>";
                buyPanel.SetActive(true);
                isGold = true;
                price = 40000;
                break;
            default:
                break;          
        }
    }

    public void OnBtnCancel()
    {
        error.SetActive(false);
        buyPanel.SetActive(false);
    }

    public void OnBtnNoMoney()
    {
        error2.SetActive(false);
    }

    public void OnBtnOK()
    {
        confirmPanel.SetActive(false);
        buyBtn.SetActive(false);
        equipBtn.SetActive(true);
        unEquipBtn.SetActive(false);

        txtPriceChange[itemIndex].text = "보유 중";
    }

    public void OnClickPurchaseConfirm()
    {  
        if (isGold) // 골드 상품일 경우
        {           
            int tmpGold = userData.data.getGold();
            if (tmpGold >= price)
            {
                switch (itemIndex)
                {
                    case 1:
                        // RED : 헤드기어
                        userData.data.red.setHeadOwn(1);
                        break;                   
                    case 3:
                        // RED : 플레이트 아머
                        userData.data.red.setTopOwn(1);
                        break;
                    case 4:
                        // RED : 복싱 글러브
                        userData.data.red.setAccOwn(1);
                        userData.data.red.setAccOwn(2);
                        break;                   
                    case 8:
                        // RED : 야구 배트
                        userData.data.red.setEtcOwn(1);
                        break;
                    case 9:
                        // RED : 수리검
                        userData.data.red.setEtcOwn(2);
                        break;
                    case 12:
                        // RED : 썬글라스
                        userData.data.red.setEtc2Own(1);
                        break;
                    case 13:
                        // RED : 하트 썬글라스
                        userData.data.red.setEtc2Own(2);
                        break;
                    case 15:
                        // BLUE : 바이크 헬멧
                        userData.data.blue.setHeadOwn(1);
                        break;
                    case 17:
                        // BLUE : 다운 베스트
                        userData.data.blue.setTopOwn(1);
                        break;
                    case 20:
                        // BLUE : 야구 배트
                        userData.data.blue.setEtcOwn(1);
                        break;
                    case 21:
                        // BLUE : 수리검
                        userData.data.blue.setEtcOwn(2);
                        break;
                    case 23:
                        // BLUE : 썬글라스
                        userData.data.blue.setEtc2Own(1);
                        break;
                    case 24:
                        // BLUE : 하트 썬글라스
                        userData.data.blue.setEtc2Own(2);
                        break;
                    case 30:
                        // ORANGE : 검도 헬맷
                        userData.data.orange.setHeadOwn(1);
                        break;
                    case 33:
                        // ORANGE : 쌍절곤
                        userData.data.orange.setAccOwn(1);
                        break;
                    case 35:
                        // ORANGE : 야구 배트
                        userData.data.orange.setEtcOwn(1);
                        break;
                    case 36:
                        // ORANGE : 수리검
                        userData.data.orange.setEtcOwn(2);
                        break;
                    case 38:
                        // ORANGE : 썬글라스
                        userData.data.orange.setEtc2Own(1);
                        break;
                    case 39:
                        // ORANGE : 하트 썬글라스
                        userData.data.orange.setEtc2Own(2);
                        break;
                    case 40:
                        // ORANGE : 플레이트 아머
                        userData.data.orange.setTopOwn(1);
                        break;
                }
                userData.data.setGold(tmpGold - price);

                resPanel.GetComponent<ResPanelMgr>().UpdateMoney();

                buyPanel.SetActive(false);
                confirmPanel.SetActive(true);
            }
            else
            {
                // 돈 모자를 경우
                error2.SetActive(true);
            }
        }else // 보석 상품일 경우
        {
            int tmpRuby = userData.data.getRuby();
            if (tmpRuby >= price)
            {
                switch (itemIndex)
                {                   
                    case 2:
                        // RED : 캡 모자
                        userData.data.red.setHeadOwn(2);
                        break; 
                    case 6:
                        // RED : 핸드 기어
                        userData.data.red.setAccOwn(3);
                        userData.data.red.setAccOwn(4);
                        break;
                    case 10:
                        // RED : 맥 투구
                        userData.data.red.setHeadOwn(3);
                        break;
                    case 11:
                        // RED : 죽도
                        userData.data.red.setEtcOwn(3);
                        break;
                    case 16:
                        // BLUE : 캡 모자
                        userData.data.blue.setHeadOwn(2);
                        break;
                    case 18:
                        // BLUE : 맥 투구
                        userData.data.blue.setHeadOwn(3);
                        break;
                    case 22:
                        // BLUE : 죽도
                        userData.data.blue.setEtcOwn(3);
                        break;
                    case 31:
                        // ORANGE : 해드폰
                        userData.data.orange.setHeadOwn(2);
                        break;
                    case 32:
                        // ORANGE : 맥 투구
                        userData.data.orange.setHeadOwn(3);
                        break;
                    case 34:
                        // ORANGE : 죽도(사이드)
                        userData.data.orange.setAccOwn(2);
                        break;
                    case 37:
                        // ORANGE : 죽도
                        userData.data.orange.setEtcOwn(3);
                        break;
                }
                userData.data.setRuby(tmpRuby - price);

                resPanel.GetComponent<ResPanelMgr>().UpdateMoney();

                buyPanel.SetActive(false);
                confirmPanel.SetActive(true);
            }
            else
            {
                // 돈 모자를 경우
                error2.SetActive(true);
            }
        }
        //Debug.Log("현재 재화 : " + userData.data.getGold() + " 골드 / " + userData.data.getRuby() + " 루비");        
    }
    
}

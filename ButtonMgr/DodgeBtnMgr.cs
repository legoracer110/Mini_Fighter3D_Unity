using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DodgeBtnMgr : MonoBehaviour
{
    public bool isTutorial;

    public int playerIndex;

    private GameObject player;

    public GameObject ct_image;
    public Image ct_fill;
    private float ct_rate;
    private bool isBreak;

    bool active;

    public Image dodgeBtnImage;
    private Color originColor;    
    
    void Start()
    {
        player = GameObject.Find("Player");
        //dodgeBtnImage = GetComponent<Image>();
        originColor = dodgeBtnImage.color;
    }

    void Update()
    {
        if (ct_rate >= 100f)
        {
            dodgeBtnImage.color = originColor;
            ct_image.SetActive(false);
            active = true;  
        }
        else
        {
            ct_image.SetActive(true);
            dodgeBtnImage.color = Color.gray;
            ct_rate += Time.deltaTime * 20f;
            ct_fill.fillAmount = ct_rate / 100f;
            active = false;
        }
    } 

    public void onClick_DodgeBtn()
    {
        if (active) {
            if (isTutorial)
            {
                player.GetComponent<Tutorial_Ctrl>().Dodge();
            }
            else
            {

                switch (playerIndex)
                {
                    case 0: // K
                        player.GetComponent<playerK_Ctrl>().Dodge();
                        break;
                    case 1: // B
                        player.GetComponent<playerB_Ctrl>().Dodge();
                        break;
                    case 2: // L
                        player.GetComponent<playerL_Ctrl>().Dodge();
                        break;
                    default:
                        break;
                }
            }
            ct_rate = 0;
        }
    }
    
}

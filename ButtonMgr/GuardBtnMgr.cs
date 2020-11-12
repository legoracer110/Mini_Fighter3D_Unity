using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GuardBtnMgr : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isTutorial;

    public int playerIndex;

    private GameObject player;

    public GameObject ct_image;
    public Image ct_fill;
    private float ct_rate;
    private bool isBreak;

    private Image shieldBtnImage;
    private Color originColor;    
    
    void Start()
    {
        player = GameObject.Find("Player");
        shieldBtnImage = GetComponent<Image>();
        originColor = shieldBtnImage.color;
    }
    
    public void setGuardGuage(float ct_rate)
    {
        this.ct_rate = ct_rate;
        ct_fill.fillAmount = ct_rate;
    }

    public void setGuardBreak(bool key)
    {
        isBreak = key;
        if (isBreak)
            shieldBtnImage.color = Color.gray;
        else
            shieldBtnImage.color = originColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isBreak)
        {
            if (isTutorial)
                player.GetComponent<Tutorial_Ctrl>().isGuard = true;
            else
            {
                switch (playerIndex)
                {
                    case 1:
                        player.GetComponent<playerK_Ctrl>().isGuard = true;
                        break;
                    case 2:
                        player.GetComponent<playerB_Ctrl>().isGuard = true;
                        break;
                    case 3:
                        player.GetComponent<playerL_Ctrl>().isGuard = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isTutorial)
            player.GetComponent<Tutorial_Ctrl>().isGuard = false;
        else
        {
            switch (playerIndex)
            {
                case 1:
                    player.GetComponent<playerK_Ctrl>().isGuard = false;
                    break;
                case 2:
                    player.GetComponent<playerB_Ctrl>().isGuard = false;
                    break;
                case 3:
                    player.GetComponent<playerL_Ctrl>().isGuard = false;
                    break;
                default:
                    break;
            }
        }
    }
}

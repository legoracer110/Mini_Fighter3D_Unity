using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnCtrlMgr : MonoBehaviour
{
	public bool isTutorial;
	GameObject player;
	GameObject gameMgr;
	public int playerIndex;
    void Start()
    {
		player = GameObject.Find("Player");
		//gameMgr = GameObject.Find("GameDB");
		//playerIndex = 1;
		// playerIndex = gameMgr.GetComponent<gameMgr>().getChar();
	}

	public void onClick_A_Btn(){
		if (isTutorial)
			player.GetComponent<Tutorial_Ctrl>().attack_A();
		else
		{
			switch (playerIndex)
			{
				case 0: // K
					player.GetComponent<playerK_Ctrl>().attack_A();
					break;
				case 1: // B
					player.GetComponent<playerB_Ctrl>().attack_A();
					break;
				case 2: // L
					player.GetComponent<playerL_Ctrl>().attack_A();
					break;
				default:
					break;
			}
		}
	}

	public void onClick_B_Btn(){
		if (isTutorial)
			player.GetComponent<Tutorial_Ctrl>().attack_B();
		else
		{
			switch (playerIndex)
			{
				case 0: // K
					player.GetComponent<playerK_Ctrl>().attack_B();
					break;
				case 1: // B
					player.GetComponent<playerB_Ctrl>().attack_B();
					break;
				case 2: // L
					player.GetComponent<playerL_Ctrl>().attack_B();
					break;
				default:
					break;
			}
		}
	}
}

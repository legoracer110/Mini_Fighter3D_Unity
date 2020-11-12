using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Specialized;
using TMPro;

public class playerK_Ctrl : MonoBehaviour
{
	public bool isTest;

	public bool isMobile;
	public bool isManualRot;

	private float h = 0.0f;
	private float v = 0.0f;

	public Animator animator;

	public Transform camPos;

	public enum PlayerState { idle, run, attack, hit, aerial, guard, dodge, down, dead };
	/*
	idle	: 일반 상태
	run		: 달리기
	attack	: 공격 ing
	hit		: 지상피격 ing
	aerial	: 공중피격 ing
	guard	: 가드 ing
	dodge	: 회피 ing
	down	: 넘어짐 ing
	dead	: 사망
	*/

	public PlayerState playerState = PlayerState.idle;

	public bool isCombo;
	// 공격 중 여부
	public float comboTimer;
	// 다음콤보 연계 가능 시간
	public int comboIndex;

    public bool isAir;
    public bool isDown;

	public bool isDelay;
	public bool moveDelay;
	public bool rotDelay;

	public bool invincibility;

	// PLAYER STAT
	public int lvHP;
	public int lvATTACK;
	public int lvSKILL;

	
	// Active Shield when Player Guards
	public GameObject GuardMgr;
    public int guardHp;
    public bool isGuard;
	public float guardTimer;
	public bool guardBreak;


	public int attackTimer;
	public float skillTimer;

	public float moveDelayTimer;

	//public GameObject bloodEffect;
	//public GameObject ShockWave;

	private Transform tr;
	public float moveSpeed;

	// playerHp UI variable
	public float playerHp;
	private float initHp;
	public Image imgHpbar;

	public Vector3 MoveVector { set; get; }
	public VirtualJoyStick joystick;

	public GameObject hurtBox;

	public GameObject hitBox1;	//	약공
	public GameObject hitBox2;	//	강공
	public GameObject hitBox3;  //	띄우기
	public GameObject hitBox4;  //	날리기
	public GameObject hitBox5;  //	멀리날리기
	public GameObject hitBox6;  //	공중잡기

	/*
	// DualSword Skill Effects
	public float skill_1Timer;
	public bool isSkill_1;
	public GameObject Buff_Effect_body;
	public GameObject Buff_L_Hand;
	public GameObject Buff_R_Hand;

	public GameObject dualSlash;

	// GreatSword Skill Effects
	public GameObject GreatSwordSkill;
	public GameObject ExpEffect;
	public Transform ExpPos;

	// Slayer Skill Effects
	public GameObject SlayerSlash;
	*/

	// Dodge Activate
	float tempV;
	float tempH;
	public GameObject dodgeTrail;
	public bool isDodge;

	public bool isHit;
	public bool isDead;
	public bool isFall;

	public GameObject img_chance;
    public bool counterChance;

	public GameObject roundMgr;

	public GameObject[] cos_head;   // HEAD 코스튬
	public GameObject[] cos_top;    // TOP 코스튬
	public GameObject[] cos_acc;    // ACC 코스튬
	public GameObject[] cos_etc;    // ETC 코스튬
	public GameObject[] cos_aura;   // AURA 코스튬


	//public bool isUpper;
	// 띄우기류 (B) 쓰고 난 뒤 캔슬해서 콤보 가능하도록 하기 위함

	void Awake()
	{
		//playerHp = 100;
		tr = GetComponent<Transform>();

		animator = GetComponent<Animator>();

		initHp = playerHp;

		isMobile = true;

		if(!isTest)
			CostumeSetting();
	}

	void CostumeSetting()
	{
		// 코스튬 세팅
		int[] equip = userData.data.blue.getEquip();

		cos_head[equip[0]].SetActive(true);
		cos_top[equip[1]].SetActive(true);
		cos_acc[equip[2]].SetActive(true);
		cos_etc[equip[3]].SetActive(true);
		cos_aura[equip[4]].SetActive(true);
	}

	private void OnEnable()
	{
		isDead = false;
		playerHp = initHp;

		imgHpbar.fillAmount = playerHp / initHp;
	}


	void Update()
	{

		if (Input.GetKeyDown(KeyCode.J))
		{
			attack_A();
		}

		if (Input.GetKeyDown(KeyCode.K))
		{
			attack_B();
		}

		if (Input.GetKey(KeyCode.M))
		{
			if(!guardBreak)
				isGuard = true;
		}

		if (Input.GetKeyUp(KeyCode.M))
		{
			isGuard = false;
		}

        if (Input.GetKeyDown(KeyCode.Comma))
        {
			Dodge();
        }

        if (isDead)
        {
            playerState = PlayerState.dead;
            animator.SetTrigger("dead");
            //Invoke ("gameOver", 4);
            //GetComponent<Collider> ().enabled = false;
        }
        else if (isDown)
        {

        }
        else if (isHit)
        {
            animator.SetTrigger("isFall");
            Camera.main.GetComponent<Rotate_CamCtrl>().fix = true;
            Invoke("gameOver", 1);
        }
        else if (!isAir && isGuard)
        {
            if (!isDelay && !moveDelay && !invincibility)
            {
                animator.SetBool("isGuard", true);
                guardTimer = 5f;
                // 가드 게이지가 다시 풀로 차기까지 5초
            }
        }
        else
        {
            animator.SetBool("isGuard", false);

            if (guardTimer > 0)
                guardTimer -= Time.deltaTime;
            else
            {
                guardHp = 10;
                guardBreak = false;
                GuardMgr.GetComponent<GuardBtnMgr>().setGuardBreak(false);
                GuardMgr.GetComponent<GuardBtnMgr>().setGuardGuage(1f);
            }

            Vector3 moveDir = Vector3.zero;
            Transform CameraTransform = Camera.main.transform;
            Vector3 forwardVector3 = CameraTransform.forward;
            forwardVector3.y = 0;
            forwardVector3.Normalize();
            Vector3 rightVector3 = new Vector3(forwardVector3.z, 0, -forwardVector3.x);

            if (isMobile)
            {
                //moveDir.x = joystick.Horizontal();
                //moveDir.z = joystick.Vertical();
                moveDir = forwardVector3 * joystick.Vertical() + rightVector3 * joystick.Horizontal();
            }
            else
            {
                h = Input.GetAxis("Horizontal");
                v = Input.GetAxis("Vertical");
                moveDir = (forwardVector3 * v) + (rightVector3 * h);
            }

            v = moveDir.z;
            h = moveDir.x;

            if (v >= 0.1f || v <= -0.1f || h >= 0.1f || h <= -0.1f)
            {
                playerState = PlayerState.run;
                animator.SetBool("isRun", true);
                moveSpeed = 2.5f;
            }
            else
            {
                //Debug.Log ("Doing Nothing");
                playerState = PlayerState.idle;
                animator.SetBool("isRun", false);
                moveSpeed = 0f;
            }

            if (isCombo)
            {
                animator.SetBool("isDelay", true);
                animator.SetBool("isRun", false);
                moveSpeed = 0f;

                playerState = PlayerState.attack;

                comboTimer -= Time.deltaTime;

            }

            if (comboTimer <= 0)
            {
                animator.SetBool("isDelay", false);
                isCombo = false;
                isDelay = false;
                rotDelay = false;
                comboTimer = 0;
                comboIndex = 0;
                motionEnd(0);
            }



            if (!rotDelay && !isAir && moveDir != Vector3.zero)
                tr.rotation = Quaternion.LookRotation(moveDir);

            if (moveDelay)
            {
                moveDelayTimer -= Time.deltaTime;
                if (moveDelayTimer <= 0)
                {
                    animator.SetBool("isDelay", false);
                    moveDelay = false;
                    rotDelay = false;
                }
            }
            else if (!isAir)
            {
                //animator.SetBool("isDelay", false);
                tr.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                camPos.transform.position = new Vector3(tr.position.x - 0.01f, tr.position.y + 1.87f, tr.position.z - 2.64f);
            }

            this.GetComponent<Rigidbody>().isKinematic = false;

            if (playerHp / initHp < 0.2f)
                imgHpbar.color = Color.red;
            else if (playerHp / initHp < 0.5f)
                imgHpbar.color = Color.yellow;
            else
                imgHpbar.color = Color.green;
            imgHpbar.fillAmount = playerHp / initHp;

        }
	}

	public void attack_A()
	{
        if (isAir)
            animator.SetTrigger("wake");
		else if (!isDelay && !moveDelay)
		{
			switch (comboIndex)
			{
				case 0:
					animator.SetInteger("rootIndex", 1);
					animator.SetTrigger("atk");
					comboIndex = 1;
					break;
				case 1:
					animator.SetTrigger("atk");
					comboIndex = 3;
					break;
				case 2:
					if (lvSKILL == 2)
					{
						animator.SetInteger("rootIndex", 4);
						animator.SetTrigger("atk");
						comboIndex = 6;
					}
					break;
				case 3:
					animator.SetInteger("rootIndex", 1);
					animator.SetTrigger("atk");
					comboIndex = 5;
					break;
				case 5:
					animator.SetInteger("rootIndex", 1);
					animator.SetTrigger("atk");
					comboIndex = 0;
					break;
				default:
					return;
			}

			isDelay = true;
			rotDelay = true;
			animator.SetBool("isRun", false);
			moveSpeed = 0f;

			comboTimer = 5f;
			isCombo = true;
		}
	}

	public void attack_B(){        

		if (!isDelay && !moveDelay && !isDown && !isAir)
		{
			if (counterChance)
			{
				if (roundDB.rdb != null)
					roundDB.rdb.addRefPlayer();
				animator.SetTrigger("counter");
				counterChance = false;
				//Debug.Log("REFLECT!");
			}
			else
			{
				switch (comboIndex)
				{
					case 0:
						animator.SetInteger("rootIndex", 4);
						animator.SetTrigger("atk");
						comboIndex = 2;
						break;
					case 2:
						if (lvSKILL >= 1)
						{
							animator.SetInteger("rootIndex", 5);
							animator.SetTrigger("atk");
							comboIndex = 0;
						}
						break;
					case 3:
						animator.SetInteger("rootIndex", 3);
						animator.SetTrigger("atk");
						comboIndex = 4;
						break;
					case 4:
						if (lvSKILL == 2)
						{
							animator.SetTrigger("atk");
							comboIndex = 0;
						}
						break;
					case 5:
						if (lvSKILL >= 1)
						{
							animator.SetInteger("rootIndex", 2);
							animator.SetTrigger("atk");
							comboIndex = 0;
						}
						break;
					case 6:
						animator.SetTrigger("atk");
						comboIndex = 0;
						break;
					default:
						return;
				}
			}
			
			isDelay = true;
			rotDelay = true;
			animator.SetBool("isRun", false);
			moveSpeed = 0f;

			comboTimer = 5f;
			isCombo = true;
		}
	}

	public void motionStart(int index)
	{
		//isUpper = false;
		//rotDelay = true;
		switch (index)
		{
			case 1: hitBox1.SetActive(true); break;
			case 2: hitBox2.SetActive(true); break;
			case 3: hitBox3.SetActive(true); break;
			case 4: hitBox4.SetActive(true); break;
			case 5: hitBox5.SetActive(true); break;
			case 6: hitBox6.SetActive(true);
				rotDelay = true;
				break;
		}
	}

	public void motionEnd(int index)
	{
		//animator.SetBool("isDelay", true);
		isDelay = false;
		comboTimer = 0.7f;

		hitBox1.SetActive(false);
		hitBox2.SetActive(false);
		hitBox3.SetActive(false);
		hitBox4.SetActive(false);
		hitBox5.SetActive(false);
		hitBox6.SetActive(false);
		rotDelay = false;
		if(index!=6)
			animator.SetBool("isGrab", false);

	}

	public void reflectStart()
	{

	}

	public void reflectEnd()
	{

	}

	public void endCombo()
	{
		moveDelay = true;
		moveDelayTimer = 0.4f;
		
	}

	public void specialGrab()
	{

	}

	public void standUp()
	{
        isDown = false;
        isAir = false;
        animator.SetBool("isAir", false);
        animator.SetBool("guardDelay", false);
        animator.SetBool("hadWall", false);
        animator.SetBool("isDown", false);
    }

    void OnTriggerEnter(Collider coll)
    {
        if (!isDead)
        {
            if (coll.tag == "HITBOX")
            {
                if (!invincibility)
                {
                    Vector3 tmp = coll.gameObject.GetComponentInParent<Transform>().position;
                    Vector3 attacker = new Vector3(tmp.x, tr.position.y, tmp.z);
                    tr.LookAt(attacker);
                    if (isGuard)
                    {
                        animator.SetTrigger("guardHit");
                        // CreateGuardEffect(coll.transform.position);
                        guardHp -= 2;

                        if (roundDB.rdb != null)
                            roundDB.rdb.addGrdPlayer();

                        //guardHp -= 1;
                        float guardGuage = (float)guardHp / 10;
                        GuardMgr.GetComponent<GuardBtnMgr>().setGuardGuage(guardGuage);

                        if (guardHp <= 0)
                        {
                            moveDelayTimer = 3f;
                            moveDelay = true;
                            rotDelay = true;
                            GuardMgr.GetComponent<GuardBtnMgr>().setGuardBreak(true);

                            animator.SetBool("guardDelay", true);
                            animator.SetTrigger("guardCrush");
                            guardBreak = true;
                            isGuard = false;
                        }

                    }
                    else
                    {
                        //isHit = true;						

                        int hitDmg = coll.gameObject.GetComponent<HitBoxMgr>().damage;

                        playerHp -= hitDmg;
                        if (roundDB.rdb != null)
                            roundDB.rdb.addDmgEnemy(hitDmg);

                        if (playerHp <= 0)
                            playerDie();

                        float hpRate = (float)playerHp / (float)initHp;

                        if (hpRate < 0.3f)
                            imgHpbar.color = Color.red;
                        else if (hpRate < 0.5f)
                            imgHpbar.color = Color.yellow;
                        else
                            imgHpbar.color = Color.green;

                        imgHpbar.fillAmount = hpRate;

                        if (playerHp <= 0)
                        {
                            isDead = true;
                            // dead call
                        }

                        if (isAir && coll.gameObject.GetComponent<HitBoxMgr>().grab)
                        {
                            Debug.Log("GRAB HIT!");

                            isAir = true;
                            tr.position = coll.gameObject.GetComponent<Transform>().position;
                            animator.SetTrigger("airGrab");
                            animator.SetBool("isDown", true);
                        }
                        else if (coll.gameObject.GetComponent<HitBoxMgr>().d_upper)
                        {
                            Debug.Log("DOUBLE UPPER HIT!");
                            animator.SetBool("isDown", true);
                            animator.SetBool("isAir", true);
                            isAir = true;
                            animator.SetTrigger("doubleUpper");
                        }
                        else if (coll.gameObject.GetComponent<HitBoxMgr>().upper)
                        {
                            Debug.Log("UPPER HIT!");
                            animator.SetBool("isDown", true);
                            animator.SetBool("isAir", true);
                            isAir = true;
                            animator.SetTrigger("hit");
                        }
                        else if (coll.gameObject.GetComponent<HitBoxMgr>().smash)
                        {
                            Debug.Log("SMASH HIT!");
                            animator.SetBool("isDown", true);
                            float power = coll.gameObject.GetComponent<HitBoxMgr>().power;
                            isAir = true;

                            if (power < 3000)
                                animator.SetTrigger("strike_hit");
                            else
                                animator.SetTrigger("strike_far");
                        }
                        else
                        {
                            Debug.Log("NORMAL HIT!");

                            animator.SetTrigger("hit");
                        }

                        animator.SetBool("isDelay", true);
                        animator.SetBool("guardDelay", false);


                        isDelay = false;
                        comboTimer = 0.7f;
                        moveDelayTimer = 1f;
                        moveDelay = true;

                        rotDelay = true;

                    }
                }
            }
            if (coll.tag == "REFLECT")
            {
                //Debug.Log("CHANCE!");
                img_chance.SetActive(true);
                counterChance = true;
            }
        }
    }

    private void OnTriggerExit(Collider coll)
	{
		if (coll.tag == "REFLECT")
		{
			img_chance.SetActive(false);
			//Debug.Log("CHANCE!");
			counterChance = false;
		}
	}

	public void Dodge()
    {
        if(!isAir)
		    animator.SetTrigger("dodge");
	}

	public void dodgeStart()
    {
		motionEnd(0);
		comboTimer = 10f;
		invincibility = true;
		animator.SetBool("isDelay", true);
    }

	public void dodgeEnd()
    {
		comboTimer = 0;
		animator.SetBool("isDelay", false);
		invincibility = false;
	}

	/*
	
	void OnParticleCollision(GameObject other)
	{
		playerHp -= 50f;
		animator.SetTrigger("isHit");

		if (playerHp <= 0)
		{
			isDead = true;
			// dead call
		}
	}

	void CreateBloodEffect(Vector3 pos)
	{
		GameObject blood1 = (GameObject)Instantiate(bloodEffect, pos, Quaternion.identity);
		Destroy(blood1, 2.0f);
	}

	void CreateShockWave(Vector3 pos)
	{
		GameObject shockwave1 = (GameObject)Instantiate(ShockWave, pos, Quaternion.identity);
		Destroy(shockwave1, 2.0f);
	}	

	public void Skill_0()
	{

	}
	

	void Slash()
	{
		Collider[] nearObj = Physics.OverlapSphere(transform.position, 4f);

		//Debug.Log ("Skill 1");
		CreateExp(ExpPos.position);
		foreach (Collider coll in nearObj)
		{

			if (coll.tag == "ENEMY" || coll.tag == "HQ" || coll.tag == "BOSS")
			{
				//Debug.Log ("Skill 2 Send!");
				object[] _params = new object[3];
				_params[0] = coll.transform.position;
				_params[2] = 90;
				coll.gameObject.SendMessage("Slash_Hit", _params, SendMessageOptions.DontRequireReceiver);
			}

		}
	}

	void Charging()
	{
		GreatSwordSkill.SetActive(true);
	}
	

	void FadeOut()
	{
		//Debug.Log ("Fade!");
		GreatSwordSkill.SetActive(false);
		isSkill = false;
	}

	void collActive(int m_index)
	{
		attacking = true;
		animator.SetBool("attacking", true);
		//Debug.Log ("Active");
		this.GetComponent<Re_ItemMgr2>().ActiveWeaponCollider(true);
		switch (m_index)
		{
			case 1:
				this.GetComponent<CCCsystemMgr>().Combo1();
				break;
			case 2:
				this.GetComponent<CCCsystemMgr>().Combo2();
				break;
			case 3:
				this.GetComponent<CCCsystemMgr>().Combo3();
				break;
			case 4:
				this.GetComponent<CCCsystemMgr>().Combo4();
				break;
			case 10:
				this.GetComponent<CCCsystemMgr>().Combo5();
				break;
			default:
				break;
		}
	}

	void ExpDamage(object[] _params)
	{
		CreateBloodEffect((Vector3)_params[0]);
		playerHp -= (int)_params[1];

		if (playerHp / initHp < 0.2f)
			imgHpbar.color = Color.red;
		else if (playerHp / initHp < 0.5f)
			imgHpbar.color = Color.yellow;
		else
			imgHpbar.color = Color.green;

		imgHpbar.fillAmount = playerHp / initHp;

		if (playerHp <= 0)
		{
			isDead = true;
			// dead call
		}
	}

	void colldeActive()
	{
		attacking = false;
		animator.SetBool("attacking", false);
		//Debug.Log ("deActive");
		this.GetComponent<Re_ItemMgr2>().ActiveWeaponCollider(false);
	}

	public void DodgeFunction()
	{
		moveSpeed = 8f;
		attacking = false;

		animator.SetBool("attacking", false);
		this.GetComponent<CCCsystemMgr>().chainTimer = 2f;

		if (tempH <= -0.1f)
			animator.SetTrigger("dodgeL");
		else if (tempH >= 0.1f)
			animator.SetTrigger("dodgeR");
		else if (tempV >= 0.1f)
			animator.SetTrigger("dodgeF");
		else
			animator.SetTrigger("dodgeB");
	}

	void dodgeStart()
	{
		animator.SetBool("isDodge", true);
		dodgeTrail.SetActive(true);
		isDodge = false;
		//isDodge2 = true;
	}

	void dodgeEnd()
	{
		animator.SetBool("isDodge", false);
		isDodge = false;
		dodgeTrail.SetActive(false);
		//isDodge2 = false;
	}

	void Dual_Slash()
	{
		GameObject slashEff = dualSlash;
		slashEff.transform.position = shieldPos.position;
		slashEff.transform.rotation = shieldPos.rotation;
		slashEff.SetActiveRecursively(true);
	}

	void PowerSlash()
	{
		GameObject slashEff = SlayerSlash;
		slashEff.transform.position = shieldPos.position;
		slashEff.transform.rotation = shieldPos.rotation;
		slashEff.SetActiveRecursively(true);
	}
	*/
    public void groundDown()
    {
        isDown = true;
        animator.SetBool("isDown", true);
    }

	void playerDie()
	{
		if (roundMgr != null)
			roundMgr.GetComponent<RoundMgr>().enemyWin();
		animator.SetTrigger("dead");
	}

	void gameOver()
	{
		//GameObject.Find("GameUI").GetComponent<Re_GameUI_Mele>().gameOver = true;
	}
}

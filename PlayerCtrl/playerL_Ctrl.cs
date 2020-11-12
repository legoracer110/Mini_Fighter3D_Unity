using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Specialized;

public class playerL_Ctrl : MonoBehaviour
{
	public bool isTest;
	public bool isMobile;
	public bool isManualRot;

	private float h = 0.0f;
	private float v = 0.0f;

	public Animator animator;

	public Transform camPos;

	public enum PlayerState { idle, run, attack, hit, aerial, guard, evade, down, dead };
	/*
	idle	: 일반 상태
	run		: 달리기
	attack	: 공격 ing
	hit		: 지상피격 ing
	aerial	: 공중피격 ing
	guard	: 가드 ing
	evade	: 회피 ing
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

	public bool isHit;
	public bool isDead;
	public bool isFall;

	public GameObject img_chance;
	public bool counterChance;

	public bool isParry;

	public GameObject roundMgr;

	public GameObject[] cos_head;   // HEAD 코스튬
	public GameObject[] cos_top;    // TOP 코스튬
	public GameObject[] cos_acc;    // ACC 코스튬
	public GameObject[] cos_etc;    // ETC 코스튬
	public GameObject[] cos_etc2;   // ETC2 코스튬

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
		int[] equip = userData.data.orange.getEquip();

		cos_head[equip[0]].SetActive(true);
		cos_top[equip[1]].SetActive(true);
		cos_acc[equip[2]].SetActive(true);
		cos_etc[equip[3]].SetActive(true);
		cos_etc2[equip[4]].SetActive(true);
	}

	private void OnEnable()
    {
		isDead = false;
		playerHp = initHp ;

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
			if (!guardBreak)
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
                //moveDir = (Vector3.forward * v) + (Vector3.right * h);
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
					comboIndex = 2;
					break;
				case 2:
					animator.SetTrigger("atk");
					comboIndex = 3;					
					break;
				case 3:					
					animator.SetTrigger("atk");
					comboIndex = 0;
					break;
				case 5:
					if (lvSKILL >= 2)
					{
						animator.SetInteger("rootIndex", 5);
						animator.SetTrigger("atk");
						comboIndex = 6;						
					}
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
						comboIndex = 5;
						break;
					case 1:
						if (lvSKILL >= 1)
						{
							animator.SetInteger("rootIndex", 3);
							animator.SetTrigger("atk");
							comboIndex = 4;
						}
						break;
					case 3:
						if (lvSKILL >= 1)
						{
							animator.SetInteger("rootIndex", 2);
							animator.SetTrigger("atk");
							comboIndex = 0;
						}
						break;
					case 4:
						animator.SetTrigger("atk");
						comboIndex = 0;
						break;
					case 5:
						animator.SetTrigger("atk");
						comboIndex = 0;
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
			case 7:
				isParry = true;
				break;
			default:
				break;
		}
	}

	public void motionEnd(int index)
	{
		isDelay = false;
		isParry = false;
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
					if (isParry)
					{
						motionStart(1);
					}
					else if (isGuard)
					{
						animator.SetTrigger("guardHit");
						// CreateGuardEffect(coll.transform.position);
						guardHp -= 2;
						//guardHp -= 1;

						if (roundDB.rdb != null)
							roundDB.rdb.addGrdPlayer();


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

                            float power = coll.gameObject.GetComponent<HitBoxMgr>().power;
                            isAir = true;
                            animator.SetBool("isDown", true);
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

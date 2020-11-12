using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Specialized;

public class playerCtrl : MonoBehaviour
{

	public bool isMobile;
	public bool isManualRot;

	private float h = 0.0f;
	private float v = 0.0f;

	public Animator animator;

	public Transform camPos;

	public enum PlayerState { idle, run, attack, hit, aerial, guard, evade, dead };
	/*
	idle	: 일반 상태
	run		: 달리기
	attack	: 공격 ing
	hit		: 지상피격 ing
	aerial	: 공중피격 ing
	guard	: 가드 ing
	evade	: 회피 ing
	dead	: 사망
	*/

	public PlayerState playerState = PlayerState.idle;

	public bool isCombo;
	// 공격 중 여부
	public float comboTimer;
	// 다음콤보 연계 가능 시간
	public int comboIndex;

	public bool isDelay;

	// PLAYER STAT
	public int lvHP;
	public int lvATTACK;
	public int lvSKILL;

	/*
	// Active Shield when Player Guards
	public GameObject GuardMgr;
	public Re_shield playerShield;
	public Transform shieldPos;
	*/

	public int attackTimer;
	public float skillTimer;

	//public GameObject bloodEffect;
	//public GameObject ShockWave;

	private Transform tr;
	public float moveSpeed;

	/*
	public GameObject lastTarget;
	public bool targetChange;
	*/

	// playerHp UI variable
	public float playerHp;
	private float initHp;
	public Image imgHpbar;

	public Vector3 MoveVector { set; get; }
	public VirtualJoyStick joystick;
	
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

	public bool isDead;
	public bool isFall;

	void Start()
	{
		//playerHp = 100;
		tr = GetComponent<Transform>();

		animator = GetComponent<Animator>();

		initHp = playerHp;

		isMobile = true;
	}


	void Update()
	{
		if (isDead) {
			playerState = PlayerState.dead;
			animator.SetTrigger ("isDead");
			Invoke ("gameOver", 4);
			//GetComponent<Collider> ().enabled = false;
		} else if (isFall) {
			animator.SetTrigger ("isFall");
			Camera.main.GetComponent<Rotate_CamCtrl> ().fix = true;
			Invoke ("gameOver", 1);
		} else {

			Vector3 moveDir = Vector3.zero;

			if (isMobile) {
				moveDir.x = joystick.Horizontal ();
				moveDir.z = joystick.Vertical ();
			} else {
				h = Input.GetAxis ("Horizontal");
				v = Input.GetAxis ("Vertical");
				moveDir = (Vector3.forward * v) + (Vector3.right * h);
			}

			v = moveDir.z;
			h = moveDir.x;

			if (v >= 0.1f || v <= -0.1f || h >= 0.1f || h <= -0.1f) {
				playerState = PlayerState.run;
				animator.SetBool ("isRun", true);
				moveSpeed = 2.5f;
			} else {
				//Debug.Log ("Doing Nothing");
				playerState = PlayerState.idle;
				animator.SetBool ("isRun", false);
				moveSpeed = 0f;
			}

			if (moveDir != Vector3.zero) {
				tr.rotation = Quaternion.LookRotation (moveDir);
			}
				
			if (isCombo) {
				
				playerState = PlayerState.attack;

				comboTimer -= Time.deltaTime;

				if (comboTimer <= 0) {
					isCombo = false;
					comboTimer = 0;
					comboIndex = 0;
					motionEnd ();
				}

			} 

			if (isDelay) {
				animator.SetBool ("isRun", false);
				moveSpeed = 0f;
			} else {
				tr.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
				camPos.transform.position = new Vector3 (tr.position.x - 0.01f, tr.position.y + 1.87f, tr.position.z - 2.64f);
			}

			//Collider[] nearObj = Physics.OverlapSphere (transform.position, 2.5f);

			/*
			if (autoAttack)
			{
				isAttack = true;
			}
			else
			{
				isAttack = false;
			}
			*/

			/*
			// isSkill_1 : DualSword Skill (Buff)
			if (isSkill_1)
			{
				this.gameObject.GetComponent<Re_ItemMgr2>().Buff(0, true);
				this.gameObject.GetComponent<Re_ItemMgr2>().Buff(1, true);
				Buff_Effect_body.SetActive(true);
				Buff_L_Hand.SetActive(true);
				Buff_R_Hand.SetActive(true);
				skill_1Timer += Time.deltaTime;
				if (skill_1Timer > 8f)
				{
					isSkill_1 = false;
				}
			}

			else
			{
				this.gameObject.GetComponent<Re_ItemMgr2>().Buff(0, false);
				this.gameObject.GetComponent<Re_ItemMgr2>().Buff(1, false);
				Buff_Effect_body.SetActive(false);
				Buff_L_Hand.SetActive(false);
				Buff_R_Hand.SetActive(false);
			}

			// isSkill_1 : GreatSword Skill (PowerSmash)
			if (isSkill)
			{
				if (this.gameObject.GetComponent<Re_ItemMgr2>().meleIndex == 0)
				{
					skillTimer += Time.deltaTime;
					skill_1Timer = 0;
					if (skillTimer > 3f)
					{
						isSkill = false;
						isSkill_1 = true;
						skillTimer = 0;
					}
				}
			}

			//////// Guard //////////
			else if (isGuard)
			{
				//_animation.CrossFade (anim.guard.name, 0.3f);
				animator.SetBool("isGuard", true);
				if (isBreak)
				{
					animator.SetTrigger("guardBreak");
					GuardMgr.GetComponent<ReGuardBtnMgr>().isBreak = true;
					playerShield.GetComponent<Re_shield>().DisableShield();
					isBreak = false;
				}

			}
			else
			{
				animator.SetBool("isGuard", false);
				if (playerShield != null)
				{
					playerShield.GetComponent<Re_shield>().DisableShield();
				}
				if (isAttack)
				{

					//this.GetComponent<Re_ItemMgr2> ().ActiveWeaponCollider (true);

					moveSpeed = 1.0f;
					animator.SetBool("isAttack", true);
					attackTimer += 1;
					if (attackTimer == 20)
					{
						isAttack = false;
						attackTimer = 0;
					}
				}
				else if (manualAttack)
				{
					animator.SetTrigger("manualAttack");
					manualAttack = false;
				}
				else
				{

					//this.GetComponent<Re_ItemMgr2> ().ActiveWeaponCollider (false);
					animator.SetBool("isAttack", false);

					if (v >= 0.1f)
					{
						animator.SetBool("isRun", true);
						animator.SetBool("isMove", false);
						if (isSkill_1)
							moveSpeed = 8f;
						else if (attacking)
							moveSpeed = 0.0f;
						else
							moveSpeed = 5.5f;
					}
					else if (v <= -0.1f)
					{
						animator.SetBool("isRun", false);
						animator.SetBool("isMove", true);
						if (isSkill_1)
							moveSpeed = 7f;
						else if (attacking)
							moveSpeed = 0.0f;
						else
							moveSpeed = 4.0f;
					}
					else if (h >= 0.1f || h <= -0.1f)
					{
						animator.SetBool("isRun", false);
						animator.SetBool("isMove", true);
					}
					else
					{
						//Debug.Log ("Doing Nothing");
						animator.SetBool("isMove", false);
						animator.SetBool("isRun", false);
					}

				}
				tr.Translate(moveDir * Time.deltaTime * moveSpeed, Space.Self);

			}
		}
		*/
			this.GetComponent<Rigidbody> ().isKinematic = false;

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
		if (!isDelay) {
			animator.SetBool ("isRun", false);
			moveSpeed = 0f;

			comboTimer = 2f;
			isCombo = true;
		
			switch (comboIndex) {
			case 0:
				animator.SetBool ("A", true);
				comboIndex = 1;
				break;
			case 1:
				animator.SetBool ("AA", true);
				comboIndex = 3;
				break;
			case 2:
				if (lvSKILL == 2) {
					animator.SetBool ("BA", true);
					comboIndex = 0;
				}
				break;
			case 3:
				animator.SetBool ("AAA", true);
				if (lvSKILL >= 1)
					comboIndex = 5;
				else
					comboIndex = 0;
				break;
			case 4:
				if (lvSKILL == 2) {
					animator.SetBool ("ABA", true);
					comboIndex = 0;
				}
				break;
			case 5:
				break;
			case 6:
				if (lvSKILL == 2) {
					animator.SetBool ("AAABA", true);
					comboIndex = 0;
				}
				break;
			default:
				break;
			}		
		}
	}

	public void attack_B(){
		if (!isDelay) {
			animator.SetBool ("isRun", false);
			moveSpeed = 0f;

			comboTimer = 2f;
			isCombo = true;

			switch (comboIndex) {
			case 0:
				animator.SetBool ("B", true);
				if (lvSKILL == 2)
					comboIndex = 2;
				else
					comboIndex = 0;
				break;
			case 1:
				animator.SetBool ("AB", true);
				if (lvSKILL == 2)
					comboIndex = 4;
				else
					comboIndex = 0;
				break;
			case 2:
				if (lvSKILL == 2) {
					animator.SetBool ("BA", true);
					comboIndex = 0;
				}
				break;
			case 3:
				animator.SetBool ("AAB", true);
				comboIndex = 0;
				break;
			case 4:
				break;
			case 5:
				if (lvSKILL >= 1) {
					animator.SetBool ("AAAB", true);
					if (lvSKILL == 2)
						comboIndex = 6;
					else
						comboIndex = 0;
				}
				break;
			default:
				break;
			}		
		}
	}

public void motionStart(){
		isDelay = true;
}

public void motionEnd(){
		isDelay = false;
		animator.SetBool ("A", false);
		animator.SetBool ("AA", false);
		animator.SetBool ("AAA", false);
		animator.SetBool ("AAAB", false);
		animator.SetBool ("AAABA", false);
		animator.SetBool ("AAB", false);
		animator.SetBool ("AB", false);
		animator.SetBool ("ABA", false);
		animator.SetBool ("B", false);
		animator.SetBool ("BA", false);
	}

public void reflectStart(){

}

public void reflectEnd(){

}

	/*
	void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject.tag == "ATTACK")
		{

			//isHit = true;

			//animator.SetTrigger ("isHit");


			CreateBloodEffect(coll.transform.position);
			playerHp -= 2f;
			//isHit = false;

			if (playerHp <= 0)
			{
				isDead = true;
			}
		}
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "ATTACK")
		{

			//Debug.Log ("Hit!");
			//isHit = true;

			//animator.SetTrigger ("isHit");


			CreateBloodEffect(coll.transform.position);
			playerHp -= 1f;
			//isHit = false;

			if (playerHp <= 0)
			{
				isDead = true;
				// dead call
			}
		}
	}

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

	void CreateExp(Vector3 pos)
	{
		GameObject Exp1 = (GameObject)Instantiate(ExpEffect, pos, Quaternion.identity);
		Destroy(Exp1, 0.8f);
	}

	void hit()
	{
		Collider[] nearObj = Physics.OverlapSphere(transform.position, 2f);

		foreach (Collider coll in nearObj)
		{

			if (coll.tag == "ENEMY" || coll.tag == "HQ" || coll.tag == "BOSS")
			{
				//Debug.Log ("Kick Send");
				CreateShockWave(shieldPos.position);
				object[] _params = new object[2];
				_params[0] = coll.transform.position;
				_params[1] = -6000f;
				coll.gameObject.SendMessage("KickFlow", _params, SendMessageOptions.DontRequireReceiver);
			}

		}
	}

	public void Skill_0()
	{

	}

	public void Skill_1()
	{
		Collider[] nearObj = Physics.OverlapSphere(transform.position, 4f);

		CreateShockWave(shieldPos.position);
		foreach (Collider coll in nearObj)
		{

			if (coll.tag == "ENEMY" || coll.tag == "HQ" || coll.tag == "BOSS")
			{
				//Debug.Log ("Skill 2 Send!");
				object[] _params = new object[3];
				_params[0] = coll.transform.position;
				_params[1] = -12000f;
				_params[2] = 10;
				coll.gameObject.SendMessage("M_Skill_1_Hit", _params, SendMessageOptions.DontRequireReceiver);
			}
			// Needs to be Fix...
		}
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
	

	void gameOver()
	{
		//GameObject.Find("GameUI").GetComponent<Re_GameUI_Mele>().gameOver = true;
	}
}

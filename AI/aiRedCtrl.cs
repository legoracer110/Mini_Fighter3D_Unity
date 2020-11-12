using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shgames
{
    public class aiRedCtrl : MonoBehaviour
    {
        public bool isSpecial;
        public bool isPractice;
        public GameObject practiceMgr;
        public bool attackMode1;
        public bool attackMode2;

        public GameObject roundMgr;

        public enum EnemyState { idle, run, attack, hit, aerial, guard, evade, down, dead, fall };

        public EnemyState enemyState = EnemyState.idle;

        /*
        idle	: 일반 상태
        run		: 달리기
        attack1	: 콤보1
        attack2	: 콤보2
        attack3	: 콤보3
        hit		: 지상피격 ing
        aerial	: 공중피격 ing
        guard	: 가드 ing
        evade	: 회피 ing
        down	: 넘어짐 ing
        dead	: 사망
        fall    : 번지
        */

        private UnityEngine.AI.NavMeshAgent nvAgent;
        private Animator animator;

        //private Collider hurtBox;
        //public Transform hurtPos;

        public int fullHp;
        public int currHp;

        private Transform enemyTr;
        public Transform playerTr;
        public Transform targetTr;

        public bool isFall;
        public bool isDie;
        public bool isHit;
        public bool isGuardBreak;
        public bool isAir;
        public bool isDown;
        public bool isGuard;

        public float guardTimer;

        public bool moveDelay;
        public bool isDelay;
        private float delayTimer;

        public int guardHp;

        //public bool isCombo;
        //private float comboTimer;

        public bool wallHit; // 벽꽝 여부 : 1콤보(다운 전까지)당 1번 벽꽝 가능

        public float attackDist;
        public float coolTime;
        public float stunTime;
        private float sTime;

        public float wakeTime;

        // Enemy Status UI
        public Image imgHpbar;
        public Text txtName;

        public int combatIndex;
        

        //GameObject camTest;

        public GameObject[] hitBox;
        public GameObject reflectZone;
        public Transform[] rf_pos;

        //public GameObject[] hitEffect;

        // Start is called before the first frame update
        void Awake()
        {
            currHp = fullHp;
            guardHp = fullHp;

            sTime = stunTime;
            delayTimer = 1.5f;

            enemyTr = GetComponent<Transform>();
            playerTr = GameObject.Find("Player").GetComponent<Transform>();

            if (!isSpecial)
                targetTr = playerTr;

            nvAgent = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            animator = this.gameObject.GetComponent<Animator>();
            //hurtBox = this.gameObject.GetComponent<Collider>();

            //camTest = GameObject.Find("CamTest");                                                             

            revive();

            StartCoroutine(this.CheckEnemyState());
            StartCoroutine(this.EnemyAction());            
        }

        private void OnEnable()
        {
            revive();

            StartCoroutine(this.CheckEnemyState());
            StartCoroutine(this.EnemyAction());
        }

        void Update()
        {
            if (isHit && !isAir)
            {
                sTime -= Time.deltaTime;
                if (sTime <= 0)
                {
                    sTime = stunTime;
                    isHit = false;
                    moveDelay = false;
                }
            }

            if(isDelay)
            {                
                delayTimer -= Time.deltaTime;
                if (delayTimer <= 0)
                {
                    delayTimer = 1.5f;
                    isDelay = false;
                    animator.SetBool("isDelay", false);                    
                    moveDelay = false;
                }
            }

            if (isGuard)
            {
                guardTimer -= Time.deltaTime;
                if (guardTimer <= 0)
                {
                    guardTimer = 2f;
                    animator.SetBool("isGuard", false);
                    isGuard = false;
                    combatIndex = 60;
                    enemyState = EnemyState.attack;
                }
            }
            /*
            if (isCombo)
            {
                comboTimer -= Time.deltaTime;
                if (comboTimer <= 0)
                {
                    comboTimer = 5f;
                    isCombo = false;
                }
            }
            */
        }

        IEnumerator CheckEnemyState()
        {
            while (!isDie&&!isFall)
            {
                yield return new WaitForSeconds(0.2f);

                float tempDist = Vector3.Distance(targetTr.position, enemyTr.position);

                if (!isDelay)
                {
                    if (isDown)
                    {
                        //this.GetComponent<Collider>().enabled = false;
                        enemyState = EnemyState.down;
                        if (isPractice)
                        {
                            practiceMgr.GetComponent<PracticeMgr>().setState("DOWN");
                        }
                    }
                    else if (isAir)
                    {
                        enemyState = EnemyState.aerial;
                        if (isPractice)
                        {
                            practiceMgr.GetComponent<PracticeMgr>().setState("AERIAL");
                        }
                    }
                    else if (isHit || isGuardBreak)
                    {
                        enemyState = EnemyState.hit;
                        if (isPractice)
                        {
                            practiceMgr.GetComponent<PracticeMgr>().setState("STUN");
                        }
                    }
                    else
                    {
                        animator.SetBool("hadWall", false);
                        if (isPractice)
                        {
                            revive();
                            practiceMgr.GetComponent<PracticeMgr>().setState("NORMAL");
                        }
                        if (tempDist < attackDist)
                        {
                            if (isPractice)
                            {
                                if (attackMode1)
                                {
                                    animator.SetInteger("comboRoot", 1);
                                    enemyState = EnemyState.attack;
                                }
                                else if (attackMode2)
                                {
                                    animator.SetInteger("comboRoot", 4);
                                    enemyState = EnemyState.attack;
                                }
                                else
                                    enemyState = EnemyState.idle;
                            }
                            else
                            {
                                if (combatIndex >= 80) {
                                    if (!isGuardBreak)
                                    {
                                        enemyState = EnemyState.guard;
                                        
                                    }
                                    else
                                    {
                                        animator.SetInteger("comboRoot", 5);
                                        enemyState = EnemyState.attack;
                                    }
                                }

                                else
                                {
                                    if (combatIndex < 80 && combatIndex >= 50)
                                        animator.SetInteger("comboRoot", 1);
                                    else if (combatIndex < 50 && combatIndex >= 30)
                                        animator.SetInteger("comboRoot", 2);
                                    else if (combatIndex < 30 && combatIndex >= 15)
                                        animator.SetInteger("comboRoot", 3);
                                    else if (combatIndex < 15 && combatIndex >= 5)
                                        animator.SetInteger("comboRoot", 4);
                                    else
                                        animator.SetInteger("comboRoot", 5);
                                    enemyState = EnemyState.attack;
                                }
                            }
                        }
                        else
                        {
                            enemyState = EnemyState.run;
                        }
                    }
                }
            }
        }

        IEnumerator EnemyAction()
        {
            while (!isDie && !isFall)
            {
                switch (enemyState)
                {
                    case EnemyState.idle:
                        
                        animator.SetBool("isRun", false);
                        nvAgent.isStopped = true;
                        isGuard = false;
                        break;
                    case EnemyState.run:
                        animator.SetBool("isRun", true);
                        animator.SetBool("isGuard", false);
                        
                        nvAgent.destination = targetTr.position;
                        if(!moveDelay&&!isDelay)
                            nvAgent.isStopped = false;
                        combatIndex = Random.Range(0, 99);
                        isGuard = false;
                        break;
                    case EnemyState.hit:
                        
                        animator.SetBool("isRun", false);
                        nvAgent.isStopped = true;
                        // animator.SetTrigger("hit");
                        isGuard = false;
                        break;
                    case EnemyState.aerial:
                        isGuard = false;
                        animator.SetBool("isRun", false);
                        nvAgent.isStopped = true;
                        break;
                    case EnemyState.down:
                        //hurtBox.enabled = false;
                        guardHp = 100;
                        isGuard = false;
                        guardTimer = Random.Range(2f, 4f);
                        yield return new WaitForSeconds(wakeTime);
                        animator.SetTrigger("wake");
                        break;
                    case EnemyState.attack:
                        isGuard = false;
                        nvAgent.isStopped = true;
                        enemyTr.LookAt(targetTr.position);
                        if(!moveDelay)
                            animator.SetTrigger("atk");
                        animator.SetBool("isRun", false);                        
                        yield return new WaitForSeconds(1f);
                        break;                    
                    case EnemyState.guard:
                        isGuard = true;
                        nvAgent.isStopped = true;
                        Vector3 tmpVec = new Vector3(targetTr.position.x, enemyTr.position.y, targetTr.position.z);
                        enemyTr.LookAt(tmpVec);
                        animator.SetBool("isGuard", true);
                        animator.SetBool("isRun", false);
                        
                        break;
                    case EnemyState.evade:
                        break;
                    case EnemyState.dead:
                        
                        break;
                    case EnemyState.fall:

                        break;
                    default:

                        break;
                }
                yield return null;
            }
        }


        public void motionStart(int index)
        {
            enemyTr.LookAt(targetTr.position);            
            hitBox[index-1].SetActive(true);
            moveDelay = true;
            animator.SetBool("attackDelay", true);
        }

        public void motionEnd()
        {
            moveDelay = false;

            for (int i=0; i<7; i++)
                hitBox[i].SetActive(false);

            // 움직임 후딜
            isDelay = true;
            animator.SetBool("attackDelay", false);
            animator.SetBool("isDelay", true);
        }

        void OnTriggerEnter(Collider coll)
        {          
            if(coll.tag == "FALL")
            {
                isFall = true;
                transform.rotation = coll.transform.rotation;
                transform.Rotate(new Vector3(0, 1, 0), 180.0f);
                Fallen();
            }else if (coll.tag == "HITBOX" && !isDown && !isDie)
            {
                //Vector3 hitPos = new Vector3(coll.transform.position.x, this.transform.position.y, coll.transform.position.z);
                reflectZone.transform.position = Vector3.Lerp(reflectZone.transform.position, rf_pos[1].position, 1);
                enemyTr.LookAt(targetTr.position);

                if (isGuard)
                {
                    animator.SetTrigger("guardHit");
                    guardHp -= coll.gameObject.GetComponent<HitBoxMgr>().damage;

                    if (roundDB.rdb != null)
                        roundDB.rdb.addGrdEnemy();

                    if (guardHp <= 0)
                    {
                        animator.SetBool("isGuard", false);
                        animator.SetTrigger("guardCrush");
                        isGuard = false;
                        isGuardBreak = true;
                    }
                }
                else
                {

                    isHit = true;

                    int hitDmg = coll.gameObject.GetComponent<HitBoxMgr>().damage;
                    currHp -= hitDmg;
                    if (roundDB.rdb != null)
                        roundDB.rdb.addDmgPlayer(hitDmg);

                    if (currHp <= 0)
                        EnemyDie();

                    if (isPractice)
                    {
                        practiceMgr.GetComponent<PracticeMgr>().setDmg(hitDmg);
                        practiceMgr.GetComponent<PracticeMgr>().setComboDmg(fullHp - currHp);
                    }

                    float hpRate = (float)currHp / (float)fullHp;

                    if (hpRate < 0.3f)
                        imgHpbar.color = Color.red;
                    else if (hpRate < 0.5f)
                        imgHpbar.color = Color.yellow;
                    else
                        imgHpbar.color = Color.green;

                    imgHpbar.fillAmount = hpRate;


                    if (isAir && coll.gameObject.GetComponent<HitBoxMgr>().grab)
                    {
                        //Debug.Log("GRAB");
                        isAir = true;
                        enemyTr.position = coll.gameObject.GetComponent<Transform>().position;
                        enemyTr.LookAt(targetTr.position);
                        animator.SetTrigger("airGrab");

                        //hurtBox.enabled = false;



                    }
                    else if (coll.gameObject.GetComponent<HitBoxMgr>().d_upper)
                    {
                        CameraMgr.Instance.ShakeHardCamera(0.05f);
                        //Debug.Log("UPPER");                
                        //GetComponent<Rigidbody>().AddForce(transform.up * 4000f);
                        //GetComponent<Rigidbody>().AddForce(transform.forward * 1000f);
                        animator.SetBool("isAir", true);
                        isAir = true;
                        animator.SetTrigger("doubleUpper");
                    }
                    else if (coll.gameObject.GetComponent<HitBoxMgr>().upper)
                    {
                        CameraMgr.Instance.ShakeCamera(0.05f);
                        //Debug.Log("UPPER");                
                        //GetComponent<Rigidbody>().AddForce(transform.up * 4000f);
                        //GetComponent<Rigidbody>().AddForce(transform.forward * 1000f);
                        animator.SetBool("isAir", true);
                        isAir = true;
                        animator.SetTrigger("hit");
                    }
                    else if (coll.gameObject.GetComponent<HitBoxMgr>().smash)
                    {
                        CameraMgr.Instance.ShakeHardCamera(0.1f);
                        Debug.Log("SMASH");
                        float power = coll.gameObject.GetComponent<HitBoxMgr>().power;
                        isAir = true;
                        //GetComponent<Rigidbody>().AddForce(coll.gameObject.transform.forward * power);
                        //2000 4000
                        if (power < 3000)
                            animator.SetTrigger("strike_hit");
                        else
                            animator.SetTrigger("strike_far");
                    }
                    else
                    {
                        //CreateHitEffect(hitPos, 0);

                        CameraMgr.Instance.ShakeCamera(0.02f);


                        sTime = stunTime;
                        animator.SetTrigger("hit");
                    }
                }
            }
            else if (coll.tag == "WALL" && isAir)
            {
                isHit = true;
                isAir = false;
                Debug.Log("AI RED WALL HIT");
                animator.SetTrigger("wallHit");
            }
        }

        public void standUp()
        {
            animator.SetBool("isAir", false);
            isAir = false;
            isDown = false;
            animator.SetBool("hadWall", false);
            animator.SetBool("isDown", false);
            moveDelay = false;
            //hurtBox.enabled = true;
                        
            //this.GetComponent<Collider>().enabled = true;

            if (isPractice)
                revive();
        }

        public void groundDown()
        {
            Debug.Log("GROUND");
            isDown = true;
            animator.SetBool("isDown", true);
            enemyState = EnemyState.down;
        }

        public void wallBound(int index)
        {
            isDown = false;
            currHp -= 5;
            if (roundDB.rdb != null)
                roundDB.rdb.addDmgPlayer(5);

            float hpRate = (float)currHp / (float)fullHp;

            if (isPractice)
                practiceMgr.GetComponent<PracticeMgr>().setComboDmg(fullHp - currHp);

            if (hpRate < 0.3f)
                imgHpbar.color = Color.red;
            else if (hpRate < 0.5f)
                imgHpbar.color = Color.yellow;
            else
                imgHpbar.color = Color.green;

            imgHpbar.fillAmount = hpRate;

            if (currHp <= 0)
                EnemyDie();

            if (index == 0)
            {
                isAir = false;
                isHit = true;
                animator.SetBool("hadWall", true);
                animator.SetBool("isAir", false);
            }
            else
            {
                //hurtBox.enabled = false;
                
            }

        }

        public void revive()
        {
            animator.SetBool("dead", false);

            currHp = fullHp;
            float hpRate = (float)currHp / (float)fullHp;
            imgHpbar.color = Color.green;
            imgHpbar.fillAmount = hpRate;

            isHit = false;
            isAir = false;
            isDown = false;

            isDie = false;
            nvAgent.enabled = true;
        }

        /*
        void CreateHitEffect(Vector3 pos, int hitType)
        {
            GameObject hit = (GameObject)Instantiate(hitEffect[0], pos, Quaternion.identity);

            switch (hitType)
            {                
                case 0:
                    hit = (GameObject)Instantiate(hitEffect[0], pos, Quaternion.identity);
                    break;
                case 1:
                    hit = (GameObject)Instantiate(hitEffect[1], pos, Quaternion.identity);
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                default:
                    break;
            }
            
            Destroy(hit, 1.0f);
        }
        */
                
        void EnemyDie()
        {
            isDie = true;
            enemyState = EnemyState.dead;
            animator.SetBool("dead", true);

            animator.SetBool("isRun", false);
            animator.SetBool("isDelay", false);
            animator.SetBool("isDown", false);
            animator.SetBool("isGrab", false);
            animator.SetBool("isGuard", false);
            animator.SetBool("hadWall", false);
            animator.SetBool("isAir", false);
            animator.SetBool("guardDelay", false);

            StopAllCoroutines();

            if (roundMgr != null)
                roundMgr.GetComponent<RoundMgr>().playerWin();
        }               

        public void reflectStart()
        {
            //reflectZone.SetActive(true);
            //Vector3 tmpPos = new Vector3(reflectZone.transform.position.x, reflectZone.transform.position.y + 400f, reflectZone.transform.position.z);
            //reflectZone.transform.position = Vector3.Lerp(reflectZone.transform.position, tmpPos, Time.deltaTime * 1f);
            reflectZone.transform.position = Vector3.Lerp(reflectZone.transform.position, rf_pos[0].position, 1);
        }

        public void reflectEnd()
        {
            //Vector3 tmpPos = new Vector3(reflectZone.transform.position.x, reflectZone.transform.position.y - 400f, reflectZone.transform.position.z);
            //reflectZone.transform.position = Vector3.Lerp(reflectZone.transform.position, tmpPos, Time.deltaTime * 1f);
            reflectZone.transform.position = Vector3.Lerp(reflectZone.transform.position, rf_pos[1].position, 1);
        }

        void Fallen()
        {
            // 번지 낙사
            enemyState = EnemyState.fall;
            nvAgent.enabled = false;
            animator.SetTrigger("fall");

            if (roundMgr != null)
                roundMgr.GetComponent<RoundMgr>().playerWin();
        }

        public void activeGravity()
        {
            this.GetComponent<Rigidbody>().useGravity = true;
            //tutMgr.GetComponent<TutorialMgr>().updateProcess();
            Destroy(this.gameObject, 1f);
        }

        public void endCombo()
        {
            combatIndex = Random.Range(0, 99);
        }
    }    

}

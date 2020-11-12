using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shgames
{
    public class aiCtrl : MonoBehaviour
    {
        public bool isPractice;
        public GameObject practiceMgr;
        public bool attackMode1;
        public bool attackMode2;

        public GameObject roundMgr;

        public enum EnemyState { idle, run, attack1, attack2, attack3, hit, aerial, guard, evade, down, dead, fall };

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

        public bool isFall;
        public bool isDie;
        public bool isHit;
        public bool isAir;
        public bool isDown;
        public bool isGuard;

        public bool wallHit; // 벽꽝 여부 : 1콤보(다운 전까지)당 1번 벽꽝 가능

        public float attackDist;
        public float coolTime;
        public float stunTime;
        public float sTime;

        public float wakeTime;

        // Enemy Status UI
        public Image imgHpbar;
        public Text txtName;

        private int combatIndex;

        //GameObject camTest;

        public GameObject hitBox1;
        public GameObject hitBox2;
        public GameObject reflectZone;
        public Transform[] rf_pos;

        //public GameObject[] hitEffect;

        // Start is called before the first frame update
        void Awake()
        {
            currHp = fullHp;

            sTime = stunTime;

            enemyTr = GetComponent<Transform>();
            playerTr = GameObject.Find("Player").GetComponent<Transform>();

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
                }
            }

        }

        IEnumerator CheckEnemyState()
        {
            while (!isDie&&!isFall)
            {
                yield return new WaitForSeconds(0.2f);

                float tempDist = Vector3.Distance(playerTr.position, enemyTr.position);

                if (isDown)
                {
                    //this.GetComponent<Collider>().enabled = false;
                    enemyState = EnemyState.down;
                    if (isPractice)
                    {
                        practiceMgr.GetComponent<PracticeMgr>().setState("DOWN");
                    }
                }else if (isAir)
                {
                    enemyState = EnemyState.aerial;
                    if (isPractice)
                    {
                        practiceMgr.GetComponent<PracticeMgr>().setState("AERIAL");
                    }
                }
                else if (isHit)
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
                            if(attackMode1)
                                enemyState = EnemyState.attack1;
                            else if (attackMode2)
                                enemyState = EnemyState.attack2;
                            else
                                enemyState = EnemyState.idle;
                        }
                        else
                        {
                            combatIndex = Random.Range(0, 99);

                            if (combatIndex >= 90)
                                enemyState = EnemyState.guard;
                            else if (combatIndex < 90 && combatIndex >= 60)
                                enemyState = EnemyState.attack1;
                            else if (combatIndex < 60 && combatIndex >= 30)
                                enemyState = EnemyState.attack2;
                            else
                                enemyState = EnemyState.attack3;
                        }      
                    }
                    else
                    {
                        enemyState = EnemyState.run;
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
                        //enemyTr.LookAt(playerTr.position);
                        animator.SetBool("isRun", false);
                        nvAgent.isStopped = true;
                        break;
                    case EnemyState.run:
                        animator.SetBool("isRun", true);
                        //enemyTr.LookAt(playerTr.position);
                        nvAgent.destination = playerTr.position;
                        nvAgent.isStopped = false;
                        break;
                    case EnemyState.hit:
                        //enemyTr.LookAt(playerTr.position);
                        animator.SetBool("isAttack1", false);
                        animator.SetBool("isRun", false);
                        nvAgent.isStopped = true;
                        // animator.SetTrigger("hit");

                        break;
                    case EnemyState.aerial:
                        animator.SetBool("isRun", false);
                        nvAgent.isStopped = true;
                        break;
                    case EnemyState.down:
                        //hurtBox.enabled = false;
                        yield return new WaitForSeconds(wakeTime);
                        animator.SetTrigger("wake");
                        break;
                    case EnemyState.attack1:
                        nvAgent.isStopped = true;
                        enemyTr.LookAt(playerTr.position);
                        animator.SetBool("isAttack1", true);
                        animator.SetBool("isRun", false);                        
                        yield return new WaitForSeconds(8f);
                        break;
                    case EnemyState.attack2:
                        nvAgent.isStopped = true;
                        enemyTr.LookAt(playerTr.position);
                        animator.SetBool("isAttack1", false);
                        animator.SetTrigger("isAttack2");
                        animator.SetBool("isRun", false);
                        yield return new WaitForSeconds(5f);
                        break;
                    case EnemyState.attack3:
                        enemyTr.LookAt(playerTr.position);
                        animator.SetBool("isRun", false);
                        nvAgent.isStopped = true;

                        break;
                    case EnemyState.guard:
                        enemyTr.LookAt(playerTr.position);
                        animator.SetBool("isRun", false);
                        nvAgent.isStopped = true;

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

        void OnTriggerEnter(Collider coll)
        {          
            if(coll.tag == "FALL")
            {
                isFall = true;
                transform.rotation = coll.transform.rotation;
                //transform.Rotate(new Vector3(0, 1, 0), 180.0f);
                Vector3 frontVec = this.transform.position + (transform.forward * .5f);
                transform.position = Vector3.Lerp(transform.position, frontVec,1);
                Fallen();
            }else if (coll.tag == "HITBOX" && !isDown && !isDie)
            {
                //Vector3 hitPos = new Vector3(coll.transform.position.x, this.transform.position.y, coll.transform.position.z);
                reflectZone.transform.position = Vector3.Lerp(reflectZone.transform.position, rf_pos[1].position, 1);

                isHit = true;

                enemyTr.LookAt(playerTr.position);

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
                    enemyTr.LookAt(playerTr.position);
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
                    //Debug.Log("SMASH");
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
            else if (coll.tag == "WALL" && isAir)
            {
                isHit = true;
                isAir = false;
                animator.SetTrigger("wallHit");
                isDown = false;
            }
        }

        public void standUp()
        {
            animator.SetBool("isAir", false);
            isAir = false;
            isDown = false;
            animator.SetBool("hadWall", false);
            animator.SetBool("isDown", false);
            //hurtBox.enabled = true;
                        
            //this.GetComponent<Collider>().enabled = true;

            if (isPractice)
                revive();
        }

        public void groundDown()
        {
            //Debug.Log("GROUND");
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
            this.GetComponent<Rigidbody>().useGravity = false;
            enemyState = EnemyState.dead;
            animator.SetBool("dead", true);

            animator.SetBool("isRun", false);
            animator.SetBool("isDelay", false);
            animator.SetBool("isDown", false);
            animator.SetBool("isGrab", false);
            animator.SetBool("isGuard", false);
            animator.SetBool("hadWall", false);
            animator.SetBool("isAir", false);
            animator.SetBool("guradDelay", false);

            StopAllCoroutines();

            if (roundMgr != null)
                roundMgr.GetComponent<RoundMgr>().playerWin();
        }

        public void motionStart(int index)
        {
            enemyTr.LookAt(playerTr.position);
            switch (index)
            {
                case 1:
                    hitBox1.SetActive(true);
                    break;
                case 2:
                    hitBox2.SetActive(true);
                    break;
                default:
                    hitBox1.SetActive(true);
                    break;
            }
            
        }

        public void motionEnd()
        {
            hitBox1.SetActive(false);
            hitBox2.SetActive(false);
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
            //Destroy(this.gameObject, 1f);
            if (!isFall)
                Invoke("EnemyDie", 1f);
        }
    }    

}

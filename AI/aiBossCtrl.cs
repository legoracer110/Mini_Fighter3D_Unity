using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shgames
{
    public class aiBossCtrl : MonoBehaviour
    {     
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


        public int fullHp;
        public int currHp;

        private Transform enemyTr;
        public Transform playerTr;
        public Transform aiTr;
        public Transform aiTr2;

        public Transform targetTr;

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

        public GameObject hitBox1;
        public GameObject hitBox2;
        public GameObject hitBoxRoar;
        public GameObject reflectZone;
        public Transform[] rf_pos;

        void Awake()
        {
            currHp = fullHp;

            sTime = stunTime;

            enemyTr = GetComponent<Transform>();
            playerTr = GameObject.Find("Player").GetComponent<Transform>();

            targetTr = playerTr;

            nvAgent = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            animator = this.gameObject.GetComponent<Animator>();                                            

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

                int rand = Random.Range(0, 3);
                if (rand == 0)
                    targetTr = playerTr;
                else if (rand == 1 && !aiTr.gameObject.GetComponent<aiBlueCtrl>().isDie)
                {
                    targetTr = aiTr;
                }
                else if(!aiTr2.gameObject.GetComponent<aiOrangeCtrl>().isDie)
                {
                    targetTr = aiTr2;
                }

                float tempDist = Vector3.Distance(targetTr.position, enemyTr.position);


                if (isDown)
                {
                    enemyState = EnemyState.down;
                    
                }else if (isAir)
                {
                    enemyState = EnemyState.aerial;                    
                }
                else if (isHit)
                {
                    enemyState = EnemyState.hit;                    
                }

                else
                {
                    animator.SetBool("hadWall", false);

                    if (tempDist < attackDist)
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
                        animator.SetBool("isRun", false);
                        nvAgent.isStopped = true;
                        break;
                    case EnemyState.run:
                        animator.SetBool("isRun", true);
                        nvAgent.destination = targetTr.position;
                        nvAgent.isStopped = false;
                        break;
                    case EnemyState.hit:
                        animator.SetBool("isAttack1", false);
                        animator.SetBool("isRun", false);
                        nvAgent.isStopped = true;

                        break;
                    case EnemyState.aerial:
                        animator.SetBool("isRun", false);
                        nvAgent.isStopped = true;
                        break;
                    case EnemyState.down:
                        yield return new WaitForSeconds(wakeTime);
                        animator.SetTrigger("wake");
                        break;
                    case EnemyState.attack1:
                        nvAgent.isStopped = true;
                        enemyTr.LookAt(targetTr.position);
                        animator.SetBool("isAttack1", true);
                        animator.SetBool("isRun", false);                        
                        yield return new WaitForSeconds(6f);
                        break;
                    case EnemyState.attack2:
                        nvAgent.isStopped = true;
                        enemyTr.LookAt(targetTr.position);
                        animator.SetBool("isAttack1", false);
                        animator.SetTrigger("isAttack2");
                        animator.SetBool("isRun", false);
                        yield return new WaitForSeconds(3.5f);
                        break;
                    case EnemyState.attack3:
                        enemyTr.LookAt(targetTr.position);

                        animator.SetBool("isAttack1", false);

                        animator.SetTrigger("roar");

                        animator.SetBool("isRun", false);
                        nvAgent.isStopped = true;

                        break;
                    case EnemyState.guard:
                        enemyTr.LookAt(targetTr.position);
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
                transform.Rotate(new Vector3(0, 1, 0), 180.0f);
                Fallen();
            }else if (coll.tag == "HITBOX" && !isDown && !isDie)
            {
                reflectZone.transform.position = Vector3.Lerp(reflectZone.transform.position, rf_pos[1].position, 1);

                isHit = true;

                enemyTr.LookAt(targetTr.position);

                int hitDmg = coll.gameObject.GetComponent<HitBoxMgr>().damage;
                currHp -= hitDmg;
                if (roundDB.rdb != null)
                    roundDB.rdb.addDmgPlayer(hitDmg);

                if (currHp <= 0)
                    EnemyDie();

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
                    //isAir = true;
                    enemyTr.position = coll.gameObject.GetComponent<Transform>().position;
                    enemyTr.LookAt(targetTr.position);
                    //animator.SetTrigger("airGrab");

                }
                else if (coll.gameObject.GetComponent<HitBoxMgr>().d_upper)
                {
                    //CameraMgr.Instance.ShakeHardCamera(0.1f);                   
                    //animator.SetBool("isAir", true);
                    //isAir = true;
                    //animator.SetTrigger("doubleUpper");
                }
                else if (coll.gameObject.GetComponent<HitBoxMgr>().upper)
                {
                    //CameraMgr.Instance.ShakeCamera(0.1f);
                    //animator.SetBool("isAir", true);
                    //isAir = true;
                    //animator.SetTrigger("hit");
                }
                else if (coll.gameObject.GetComponent<HitBoxMgr>().smash)
                {
                    CameraMgr.Instance.ShakeCamera(0.02f);
                    animator.SetTrigger("hit");
                    /*
                    float power = coll.gameObject.GetComponent<HitBoxMgr>().power;
                    isAir = true;
                    if (power > 3000)
                        animator.SetTrigger("strike_hit");
                    
                    */
                }
                else
                {                    
                    //CameraMgr.Instance.ShakeCamera(0.13f);

                    sTime = stunTime;
                    //animator.SetTrigger("hit");
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
        /*
        public void standUp()
        {
            animator.SetBool("isAir", false);
            isAir = false;
            isDown = false;
            animator.SetBool("hadWall", false);
            animator.SetBool("isDown", false);

        }

        public void groundDown()
        {
            isDown = true;
            animator.SetBool("isDown", true);
            enemyState = EnemyState.down;
        }

        public void wallBound(int index)
        {
            currHp -= 5;
            if (roundDB.rdb != null)
                roundDB.rdb.addDmgPlayer(5);

            float hpRate = (float)currHp / (float)fullHp;

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

            }

        }
        */
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
            animator.SetBool("guradDelay", false);

            StopAllCoroutines();

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

        public void isRoaring()
        {
            motionEnd();
            hitBoxRoar.SetActive(true);
        }

        public void roarEnd()
        {
            hitBoxRoar.SetActive(false);
        }

        public void reflectStart()
        {
            reflectZone.transform.position = Vector3.Lerp(reflectZone.transform.position, rf_pos[0].position, 1);
        }

        public void reflectEnd()
        {
            reflectZone.transform.position = Vector3.Lerp(reflectZone.transform.position, rf_pos[1].position, 1);
        }

        void Fallen()
        {
            // 번지 낙사
            enemyState = EnemyState.fall;
            nvAgent.enabled = false;
            animator.SetTrigger("fall");

        }

        public void activeGravity()
        {
            this.GetComponent<Rigidbody>().useGravity = true;
            Destroy(this.gameObject, 1f);
        }
    }    

}

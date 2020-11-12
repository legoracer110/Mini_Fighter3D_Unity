using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Shgames
{
    public class aiTutorialCtrl : MonoBehaviour
    {        
        public GameObject tutMgr;


        public int tutLv;
        // LV1 : AAAA 콤보 3번
        // LV2 : AAAB 콤보 3번
        // LV3 : B 3번
        // LV4 : AABBG 콤보 1번
        // LV5 : B 공격 가드 3번
        // LV6 : B 공격 반격 3번
        // LV7 : AAAB 반격 3번
        // LV8 : AAAB로 벽꽝 3번
        // LV9 : BB로 벽꽝 후 재벽꽝 1번
        // LV10 : BB로 번지 3번
        // LV11 : AI 대전

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
        private Transform playerTr;

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
        private float sTime;

        public float wakeTime;

        // Enemy Status UI
        public Image imgHpbar;
        public Text txtName;

        private int combatIndex;

        public GameObject hitBox1;
        public GameObject hitBox2;
        public GameObject reflectZone;
        public Transform[] rf_pos;

        void Start()
        {
            currHp = fullHp;

            sTime = stunTime;

            enemyTr = GetComponent<Transform>();
            playerTr = GameObject.Find("Player").GetComponent<Transform>();

            nvAgent = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            animator = this.gameObject.GetComponent<Animator>();                                                               

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
            while (!isDie && !isFall)
            {
                yield return new WaitForSeconds(0.2f);

                float tempDist = Vector3.Distance(playerTr.position, enemyTr.position);

                if(tutLv==8 && currHp<=65)
                    tutMgr.GetComponent<TutorialMgr>().updateProcess();

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

                // LV0 : 이동
                // LV1 : AAAA 콤보 3번
                // LV2 : AAAB 콤보 3번
                // LV3 : B 3번
                // LV4 : AABBG 콤보 1번
                // LV5 : B 공격 가드 3번
                // LV6 : B 공격 반격 3번
                // LV7 : AAAB 반격 3번
                // LV8 : 회피/캔슬
                // LV9 : AAAB로 벽꽝 3번
                // LV10 : BB로 벽꽝 후 재벽꽝 1번
                // LV11 : BB로 번지 1번
                // LV12 : AI 대전

                else
                {
                    animator.SetBool("hadWall", false);                    
                    if (tempDist < attackDist)
                    {
                        if (tutLv == 5)
                            enemyState = EnemyState.attack2;
                        else if (tutLv == 6)
                            enemyState = EnemyState.attack2;
                        else if (tutLv == 7)
                            enemyState = EnemyState.attack1;
                        else
                            enemyState = EnemyState.idle ;
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
                        animator.SetBool("isAttack1", false);                        
                        nvAgent.isStopped = true;
                        revive();
                        break;
                    case EnemyState.run:
                        animator.SetBool("isRun", true);
                        nvAgent.destination = playerTr.position;
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
                transform.Rotate(new Vector3(0, 1, 0), 180.0f);                                
                Fallen();
            }else if (coll.tag == "HITBOX" && !isDown && !isDie)
            {                
                reflectZone.transform.position = Vector3.Lerp(reflectZone.transform.position, rf_pos[1].position, 1);

                isHit = true;

                enemyTr.LookAt(playerTr.position);

                int hitDmg = coll.gameObject.GetComponent<HitBoxMgr>().damage;
                currHp -= hitDmg;
                        

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
                    isAir = true;
                    enemyTr.position = coll.gameObject.GetComponent<Transform>().position;
                    enemyTr.LookAt(playerTr.position);
                    animator.SetTrigger("airGrab");
                }
                else if (coll.gameObject.GetComponent<HitBoxMgr>().d_upper)
                {
                    CameraMgr.Instance.ShakeCamera(0.2f);
                    animator.SetBool("isAir", true);
                    isAir = true;
                    animator.SetTrigger("doubleUpper");
                }
                else if (coll.gameObject.GetComponent<HitBoxMgr>().upper)
                {
                    CameraMgr.Instance.ShakeCamera(0.2f);                    
                    animator.SetBool("isAir", true);
                    isAir = true;
                    animator.SetTrigger("hit");
                }
                else if (coll.gameObject.GetComponent<HitBoxMgr>().smash)
                {
                    CameraMgr.Instance.ShakeCamera(0.25f);
                    Debug.Log("SMASH");
                    float power = coll.gameObject.GetComponent<HitBoxMgr>().power;
                    isAir = true;
                    if (power < 3000)
                        animator.SetTrigger("strike_hit");
                    else
                        animator.SetTrigger("strike_far");
                }
                else
                {
                    sTime = stunTime;
                    animator.SetTrigger("hit");
                }
            }
            else if (coll.tag == "WALL" && isAir)
            {
                isHit = true;
                isAir = false;
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
            currHp -= 5;

            float hpRate = (float)currHp / (float)fullHp;                        

            if (hpRate < 0.3f)
                imgHpbar.color = Color.red;
            else if (hpRate < 0.5f)
                imgHpbar.color = Color.yellow;
            else
                imgHpbar.color = Color.green;

            imgHpbar.fillAmount = hpRate;

            if (index == 0)
            {
                isAir = false;
                isHit = true;
                animator.SetBool("hadWall", true);
                animator.SetBool("isAir", false);
                if(tutLv==9)
                    tutMgr.GetComponent<TutorialMgr>().updateProcess();
            }
            else
            {
               
                if(tutLv==10)
                    tutMgr.GetComponent<TutorialMgr>().updateProcess();
            }

        }

        public void revive()
        {
            currHp = fullHp;
            float hpRate = (float)currHp / (float)fullHp;
            imgHpbar.color = Color.green;
            imgHpbar.fillAmount = hpRate;
        }   

        public void setTutLv(int lv)
        {
            tutLv = lv;
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
            tutMgr.GetComponent<TutorialMgr>().updateProcess();
            Destroy(this.gameObject, 1f);
        }
    }    

}

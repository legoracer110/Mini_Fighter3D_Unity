using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxMgr : MonoBehaviour
{
    public bool isTutorial;
    public bool isEnemy;

    public int damage;
    public bool smash;
    public bool upper;
    public bool d_upper;
    public float power;
    public bool grab;

    float timer;

    public GameObject hitEffect;

    void Start()
    {
        timer = 0.4f;
        if (!isEnemy)
        {
            if (Game.current.getSelected() == 0)
                damage += (int)(userData.data.blue.getPower() * 0.3f);
            else if (Game.current.getSelected() == 1)
                damage += (int)(userData.data.red.getPower() * 0.3f);
            else if (Game.current.getSelected() == 2)
                damage += (int)(userData.data.orange.getPower() * 0.3f);
        }
    }

    void Update()
    {
        if (grab)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = 0.4f;
                this.gameObject.SetActive(false);
            }            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ENEMY")
        {            
            if (grab)
            {
                if(isTutorial)
                    GameObject.Find("Player").GetComponent<Tutorial_Ctrl>().animator.SetBool("isGrab", true);
                else
                    GameObject.Find("Player").GetComponent<playerK_Ctrl>().animator.SetBool("isGrab", true);
            }

            CreateHitEffect(this.transform.position);
        }
        
        this.gameObject.SetActive(false);        
    }

    void CreateHitEffect(Vector3 pos)
    {

        GameObject hit = (GameObject)Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(hit, 1.0f);

    }
}

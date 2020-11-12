using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMgr : MonoBehaviour
{

    public GameObject model1;
    public GameObject model2;

    public Transform initPos1;
    public Transform initPos2;

    Animator animator1;
    Animator animator2;

    void Start()
    {
        animator1 = model1.GetComponent<Animator>();
        animator2 = model2.GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator1.SetTrigger("atk");
            animator2.SetTrigger("atk");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            resetPos();
        }
    }

    void resetPos()
    {
        Debug.Log("reset");
        //model1.transform.position = Vector3.Lerp(model1.transform.position, tr1.position, 1);
        //model2.transform.position = Vector3.Lerp(model2.transform.position, tr2.position, 1);
        model1.transform.position = initPos1.position;
        model2.transform.position = initPos2.position;
    }
}

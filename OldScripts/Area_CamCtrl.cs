using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.EventSystems;

public class Area_CamCtrl : MonoBehaviour
{

	public Vector3 RotateVector { set; get; }

	private Transform targetTr;
	public Transform normaltargetTr;
	//public Transform sniperTargetTr;

	//public float dist;
	//public float height;
	public float dampTrace;

	public int mode;
	private Quaternion rot;

	private Transform tr;
	GameObject Char;

	public bool ResetBtn = false;

	private Transform initPos;
	private Transform backupPos;

	public bool fix;

	void Start()
	{
		targetTr = normaltargetTr;
		tr = GetComponent<Transform>();
		//dist = 3.8f;
		//height = 1.0f;

		Char = GameObject.Find("Player");

		ResetCamera();

		initPos = tr;
	}

	void LateUpdate()
	{
		tr.position = Vector3.Lerp(tr.position, targetTr.position, Time.deltaTime * dampTrace);
		//tr.LookAt(targetTr.position);
	}

	void ResetCamera()
	{
		//tr.position = Vector3.Lerp(tr.position, targetTr.position - (targetTr.forward * dist) + (Vector3.up * height), 1);
		//tr.LookAt(targetTr.position);
	}

	public void ResetPos()
	{
		tr.position = initPos.position;
	}
}

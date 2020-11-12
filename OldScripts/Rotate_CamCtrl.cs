using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.EventSystems;

public class Rotate_CamCtrl : MonoBehaviour
{

	public Vector3 RotateVector { set; get; }

	//public VirtualJoyStick2 joystick;
	public TouchAreaCtrl Area;

	private Transform targetTr;
	public Transform normaltargetTr;
	//public Transform sniperTargetTr;

	public float dist;
	public float height;

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

	void Update()
	{

		if (fix)
		{
			tr.position = backupPos.position;
			tr.LookAt(targetTr.position);
		}
		else
		{

			Vector3 rotateDir = Vector3.zero;
			rotateDir.x = Area.Horizontal() * 6;

			Char.transform.Rotate(Vector3.down, rotateDir.x);
			//Camera.main.transform.RotateAround (Char.transform.position, Vector3.up, rotateDir.x/2);

			Vector3 rotateDir2 = Vector3.zero;
			rotateDir2.z = Area.Vertical() * 2;

			if (Camera.main.transform.eulerAngles.x >= 0f && Camera.main.transform.eulerAngles.x < 37f
			   || Camera.main.transform.eulerAngles.x >= 326 && Camera.main.transform.eulerAngles.x < 340f)
			{
				//Debug.Log ("Normal Rot");
				//Debug.Log (Area.Vertical());
				Camera.main.transform.RotateAround(targetTr.position, Vector3.right, rotateDir2.z);
				targetTr.Rotate(Vector3.right, rotateDir2.z);
			}
			else if (Camera.main.transform.eulerAngles.x >= 17f && Camera.main.transform.eulerAngles.x < 70f)
			{
				if (rotateDir2.z < 0)
				{
					//Debug.Log ("Over Rot");

					Camera.main.transform.RotateAround(targetTr.position, Vector3.right, rotateDir2.z);
					targetTr.Rotate(Vector3.right, rotateDir2.z);
				}
			}
			else if (Camera.main.transform.eulerAngles.x >= 280f && Camera.main.transform.eulerAngles.x < 400f)
			{
				if (rotateDir2.z > 0)
				{
					//Debug.Log ("Under Rot");

					Camera.main.transform.RotateAround(targetTr.position, Vector3.right, rotateDir2.z);
					targetTr.Rotate(Vector3.right, rotateDir2.z);
				}
			}
			else
			{
				//Debug.Log (Camera.main.transform.eulerAngles.x);
			}

			tr.LookAt(targetTr.position);

			targetTr = normaltargetTr;

			ResetCamera();
			//dist = 3.8f;
			//height = 1.0f;
			Camera.main.fieldOfView = 60f;

			backupPos = tr;
		}
	}

	void ResetCamera()
	{
		tr.position = Vector3.Lerp(tr.position, targetTr.position - (targetTr.forward * dist) + (Vector3.up * height), 1);
		tr.LookAt(targetTr.position);
	}

	public void ResetPos()
	{
		tr.position = initPos.position;
	}
}

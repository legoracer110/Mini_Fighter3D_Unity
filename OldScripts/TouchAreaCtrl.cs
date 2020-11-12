using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchAreaCtrl : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
{

	public GameObject pointerImg;

	public Vector3 firstPos;
	public Vector3 lastPos;

	public Vector3 inputVector;

	public virtual void OnPointerDown(PointerEventData ped)
	{

		firstPos = ped.position;
		OnDrag(ped);

		//joystick.transform.position = ped.position;
		//joystick.gameObject.SetActive (true);
		pointerImg.SetActive(true);
	}

	public virtual void OnDrag(PointerEventData ped)
	{

		lastPos = ped.position;
		inputVector = firstPos - lastPos;
		inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

		pointerImg.transform.position = ped.position;
	}

	public virtual void OnPointerUp(PointerEventData ped)
	{
		//joystick.gameObject.SetActive (false);
		inputVector = Vector3.zero;
		pointerImg.SetActive(false);
	}

	public float Horizontal()
	{
		if (inputVector.x != 0)
			return inputVector.x;
		else
			return 0;
	}

	public float Vertical()
	{
		if (inputVector.y != 0)
			return inputVector.y;
		else
			return 0;
	}
}

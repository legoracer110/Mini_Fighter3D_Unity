using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Shgames
{
    public class CameraMgr : Singleton<CameraMgr>
    {
        public bool isOn;
        public float sec;


        private CameraCtrl cameraCtrl;
        private Coroutine routine;


        public CameraCtrl CAM_CONTROLLER
        {
            get
            {
                if (cameraCtrl == null)
                {
                    cameraCtrl = GameObject.FindObjectOfType<CameraCtrl>();
                }
                return cameraCtrl;
            }
        }

        IEnumerator _CamShake(float sec)
        {
            CAM_CONTROLLER.TriggerCamera(CameraTrigger.Shake);
            yield return new WaitForSeconds(sec);
            CAM_CONTROLLER.TriggerCamera(CameraTrigger.Default);
        }

        IEnumerator _HardCamShake(float sec)
        {
            CAM_CONTROLLER.TriggerCamera(CameraTrigger.HardShake);
            yield return new WaitForSeconds(sec);
            CAM_CONTROLLER.TriggerCamera(CameraTrigger.Default);
        }

        public void ShakeCamera(float sec)
        {
            if (routine != null)
            {
                StopCoroutine(routine);
            }
            routine = StartCoroutine(_CamShake(sec));
        }

        public void ShakeHardCamera(float sec)
        {
            if (routine != null)
            {
                StopCoroutine(routine);
            }
            routine = StartCoroutine(_HardCamShake(sec));
        }

    }
}


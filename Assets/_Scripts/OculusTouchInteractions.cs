using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusTouchInteractions : MonoBehaviour
{
    private const float ThrustSpeed = 10.0f;

    public GameObject CameraGameObject;
    public OVRInput.Controller LeftController;
    public OVRInput.Controller RightController;

    private Rigidbody _rb;

    // Use this for initialization
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.rotation = CameraGameObject.transform.rotation;
        var leftTriggerInput = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, LeftController);
        var rightTriggerInput = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, RightController);
        if (leftTriggerInput > 0)
        {
            var leftControllerRotation = OVRInput.GetLocalControllerRotation(LeftController);
            Debug.Log("LEFT Input! I:" + leftTriggerInput + ", R:" + leftControllerRotation);
            _rb.AddForce(leftControllerRotation * Vector3.one * ThrustSpeed);
//            Debug.DrawLine(Vector3.zero, leftControllerRotation * Vector3.one);
//            _rb.AddForce(0, Math.Max(leftTriggerInput, rightTriggerInput) * ThrustSpeed, 0);
        }
        else if (rightTriggerInput > 0)
        {
            var rightControllerRotation = OVRInput.GetLocalControllerRotation(RightController);
            Debug.Log("RIGHT Input! I:" + rightTriggerInput + ", R:" + rightControllerRotation);
//            Debug.DrawLine(Vector3.zero, rightControllerRotation * Vector3.one);
//            _rb.AddForce(0, Math.Max(rightTriggerInput, rightTriggerInput) * ThrustSpeed, 0);
        }
    }
}
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
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = CameraGameObject.transform.rotation;
        float leftTriggerInput = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);
        float rightTriggerInput = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);
        if (leftTriggerInput > 0 || rightTriggerInput > 0)
        {
            Debug.Log("Trigger input! L" + leftTriggerInput + ", R" + rightTriggerInput + ", Z Rotation: " + transform.rotation.z);
            _rb.AddForce(0, Math.Max(leftTriggerInput, rightTriggerInput) * ThrustSpeed, 0);
        }
    }
}
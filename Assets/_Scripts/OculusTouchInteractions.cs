using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusTouchInteractions : MonoBehaviour
{
    private const float ThrustSpeed = 5.0f;

    public GameObject CameraGameObject;
    public OVRInput.Controller LeftController;
    public OVRInput.Controller RightController;
    public GameObject DebugLeftArrow;
    public GameObject DebugRightArrow;
    public GameObject DebugAverageArrow;

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
        Quaternion leftControllerRotation = Quaternion.identity,
            rightControllerRotation = Quaternion.identity,
            averageControllerRotation = Quaternion.identity;

        DebugAverageArrow.SetActive(false);

        DebugLeftArrow.SetActive(false);
        if (leftTriggerInput > 0)
        {
            DebugLeftArrow.SetActive(true);
            DebugAverageArrow.SetActive(true);
            leftControllerRotation = OVRInput.GetLocalControllerRotation(LeftController);
            averageControllerRotation = leftControllerRotation;
            _rb.AddForce(leftControllerRotation * Quaternion.Euler(-45, 0, 45) * Vector3.one * leftTriggerInput * ThrustSpeed);
            Debug.DrawLine(Vector3.zero, leftControllerRotation * Quaternion.Euler(-45, 0, 45) * Vector3.one, Color.red);
            DebugLeftArrow.transform.rotation = leftControllerRotation;
        }

        DebugRightArrow.SetActive(false);
        if (rightTriggerInput > 0)
        {
            DebugRightArrow.SetActive(true);
            DebugAverageArrow.SetActive(true);
            rightControllerRotation = OVRInput.GetLocalControllerRotation(RightController);
            averageControllerRotation = rightControllerRotation;
            _rb.AddForce(rightControllerRotation * Quaternion.Euler(-45, 0, 45) * Vector3.one * rightTriggerInput * ThrustSpeed);
            Debug.DrawLine(Vector3.zero, rightControllerRotation * Quaternion.Euler(-45, 0, 45) * Vector3.one, Color.blue);
            DebugRightArrow.transform.rotation = rightControllerRotation;
        }

        if (leftTriggerInput > 0 && rightTriggerInput > 0)
        {
            averageControllerRotation = Quaternion.Lerp(leftControllerRotation, rightControllerRotation, 0.5f);
        }
        if (leftTriggerInput > 0 || rightTriggerInput > 0)
        {
            DebugAverageArrow.transform.rotation = averageControllerRotation;
            Debug.DrawLine(Vector3.zero, averageControllerRotation * Quaternion.Euler(-45, 0, 45) * Vector3.one, Color.white);
        }
    }
}
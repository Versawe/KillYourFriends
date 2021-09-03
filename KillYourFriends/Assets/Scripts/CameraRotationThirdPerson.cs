using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationThirdPerson : MonoBehaviour
{
    private Camera cam;

    private float yawSensitivity = 8.5f;
    private float pitchSensitivity = 5.5f;

    public GameObject Snout;
    Quaternion SnoutFollowCam;
    Quaternion StartRotationSnout;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        // gets main camera
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;

        StartRotationSnout = Snout.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //has rig set to focus on character
        transform.position = target.position;

        RotateCamera();
    }

    private void RotateCamera()
    {
        //saves axis movement of x and y mouse movement
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // yaw and pitch values change determined on 
        // mousex and mousey movement and applied sensitivity to both
        yaw += mx * yawSensitivity;
        pitch -= my * pitchSensitivity;

        //clamp pitch, so camera doesn't rotate too far low or high to seem weird
        float pitch_clamped = Mathf.Clamp(pitch, -5f, 90f);

        // use the clamped pitch and yaw to rotate camera rig, entered in as euler angles through Quaternion class 
        transform.rotation = Quaternion.Euler(pitch_clamped, yaw, 0);

        Snout.transform.localRotation = Quaternion.Euler(pitch_clamped, target.rotation.y, target.rotation.z);
    }
}

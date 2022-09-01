using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public bool steer;
    public bool invertSteer;
    public bool power;

    public float SteerAngle {get; set;}
    public float Torque {get; set;}
    public float TorqueBrake {get; set;}

    public float speed;

    private WheelCollider wheelCollider;
    private Transform wheelTransform;
    private float circumference;

    // Start is called before the first frame update
    void Start()
    {
        wheelCollider = GetComponentInChildren<WheelCollider>();
        wheelTransform = GetComponentInChildren<MeshRenderer>().GetComponent<Transform>();
        circumference = 2 * 3.14f * wheelCollider.radius;
    }

    // Update is called once per frame
    void Update()
    {
        wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;

        speed = circumference * wheelCollider.rpm * 6 / 100;
    }

    private void FixedUpdate()
    {
        if(steer){
            wheelCollider.steerAngle = SteerAngle * (invertSteer ? -1 : 1);
        }

        if(power){
            wheelCollider.motorTorque = Torque;
        }

        wheelCollider.brakeTorque = TorqueBrake;
    }

    public void set(float steer, float throttle, float brake){
        SteerAngle = steer;
        Torque = throttle;
        TorqueBrake = brake;
    }
}

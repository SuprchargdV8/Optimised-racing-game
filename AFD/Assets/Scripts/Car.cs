using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float maxGas;
    public bool remGas;
    public float currGass;

    private Rigidbody rb;
    private Wheel[] wheels;
    private AudioSource engineNoise;

    public GameObject pointer;
    private Transform parentTransf;
    public Transform centerOfMass;

    public float torqueGas = 900f;
    public float torqueBrake = 400f;
    public float maxSteer = 40f;
    public float speedMultiplier = 5;
    public int gearing;
    public float sounder;

    public float Steer {get; set;}
    public float Throttle {get; set;}
    public float Brake {get; set;}
    public int DownforceMulti;

    public float Speed {get; private set;}
    public float DriftAngle {get; private set;}
    public float wheelSpeed;

    void Start(){
        getGas();
        engineNoise = GetComponent<AudioSource>();
        wheels = GetComponentsInChildren<Wheel>();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
        parentTransf = gameObject.GetComponent<Transform>();
    }

    public void getGas(){
        if(PlayerPrefs.HasKey($"{name}Gas")){
            currGass = PlayerPrefs.GetInt($"{name}Gas");
        } else {
            PlayerPrefs.SetInt($"{name}Gas", (int) maxGas);
            currGass = maxGas;
        }
    }

    void Update(){

        wheelSpeed = Mathf.Sqrt(wheelSpeed * wheelSpeed);

        DriftAngle = Vector3.Angle(rb.velocity, transform.forward);

        float gas = 0;
        float brake = 0;
        if(currGass > 0){

            if(remGas){
                currGass -= Time.deltaTime;
            }

            engineNoise.pitch = (wheelSpeed / gearing) % sounder + 0.7f * (wheelSpeed / gearing + 1);

            gas = (rb.velocity.magnitude > 0.0001 && Brake == 1) ? -1 : Throttle;
            brake = (rb.velocity.magnitude > 0.0001 && Brake == 1) ? 0 : Brake;
            brake = (rb.velocity.magnitude < 0.1 && Brake == 0) ? Brake : 0;

        } else {
            brake = 3;
            engineNoise.pitch = 0;
        }

        wheelSpeed = 0;
        foreach(var wheel in wheels){
            wheel.set(Steer * maxSteer, gas * torqueGas, brake * torqueBrake);
            wheelSpeed += wheel.speed;
        }
        wheelSpeed = wheelSpeed / wheels.Length;
        rb.AddForce(transform.up * -1 * DownforceMulti * Speed, ForceMode.Force);

        Speed = rb.velocity.magnitude * speedMultiplier;
            
        Quaternion tempRot = Quaternion.Euler(0, parentTransf.localEulerAngles.y, 0);

        pointer.transform.rotation = tempRot;
        pointer.transform.position = parentTransf.position;
    }

}

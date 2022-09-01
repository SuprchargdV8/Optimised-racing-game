using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public int throttle {get; private set;}
    public int brake {get; private set;}
    public float steer {get; private set;}

    void Update()
    {
        if(Input.acceleration.x * 2f > 1){
            steer = 1;
        } else {
            steer = Input.acceleration.x * 2f;
        }
    }

    public void GasDown(){
        throttle = 1;
    }
    public void GasUp(){
        throttle = 0;
    }

    public void BrakeDown(){
        brake = 1;
    }
    public void BrakeUp(){
        brake = 0;
    }

}

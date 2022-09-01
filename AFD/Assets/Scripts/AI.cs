using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    
    private Car Car;

    private float prevSteer;
    
    public GameObject trip;
    private Transform[] checkpointsUN;
    private List<Transform> checkpointsSO = new List<Transform>();
    public int checkpointID;
    private Vector3 targetPosition;
    private int number;

    // Start is called before the first frame update
    void Start()
    {
        prevSteer = 0;
        Car = GetComponent<Car>();
        checkpointsUN = trip.GetComponentsInChildren<Transform>();
        for(var i = 0; i < checkpointsUN.Length; i++){
            foreach(Transform child in checkpointsUN){
                if(child.gameObject.name.Equals($"{i+1}")){
                    checkpointsSO.Add(child);
                }
            }

        }
        number = checkpointsSO.Count;
        setTargetPosition(checkpointsSO[checkpointID].position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        float dot = Vector3.Dot(transform.forward, direction);
        float thr = 0;
        if( dot > 0){
            Car.Throttle = 1.0f;
            thr = 1.0f;
        } else {
            Car.Throttle = -1.0f;
            thr = -1.0f;
        }

        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
            
        float tempSteer = prevSteer;

        float tempAngle = (angle/Car.maxSteer > 1) ? 1 : angle/Car.maxSteer;
        tempAngle = (angle/Car.maxSteer < -1) ? -1 : angle/Car.maxSteer;

        tempSteer = (tempAngle + prevSteer * 7) / 8;

        Car.Steer = thr * tempSteer;

        prevSteer = tempSteer;          
    }

    public void setTargetPosition(Vector3 targetPos){
        this.targetPosition = targetPos;
    }

    private void OnTriggerEnter(Collider other){
        if(other.name.Equals($"{checkpointID+1}")){
            checkpointID = (checkpointID + 1) % number;
            setTargetPosition(checkpointsSO[checkpointID].position);
        }
    }

}

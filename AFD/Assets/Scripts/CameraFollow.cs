using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public string TargetName;

    private GameObject TargetObject;
    private Transform Target;
    private Rigidbody rb;
    private Transform centerOfMass;
    private Transform cameraPivotFront;
    private Transform cameraPivotRear;
    private Transform player;

    public Vector3 offset;
    public Vector3 eulerRotation;
    public float damper;
    public float lookDown;

    private Vector3 pos;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player").transform;

        updateCamera();
    }

    public void updateCamera(){
        
        transform.eulerAngles = eulerRotation;
        TargetObject = GameObject.Find($"{TargetName}");
        Target = TargetObject.transform;
        rb = TargetObject.GetComponent<Rigidbody>();
        centerOfMass = GameObject.Find($"{TargetName}/CameraPivot").transform;
        cameraPivotFront = GameObject.Find($"{TargetName}/CameraPivot/Front").transform;
        cameraPivotRear = GameObject.Find($"{TargetName}/CameraPivot/Rear").transform;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Target == null)
            return;

        Vector3 posFront = cameraPivotFront.position - centerOfMass.position;
        Vector3 posBack = cameraPivotRear.position - centerOfMass.position;
        Vector3 pos = Vector3.zero;
        Vector3 p = Vector3.zero;

        if ((Vector3.Angle(posFront, rb.velocity) > Vector3.Angle(posBack, rb.velocity)) && rb.velocity.magnitude > 1){

            p = cameraPivotRear.position;
            pos = posFront + cameraPivotFront.position;
            p.y = p.y - 2*lookDown;

        } else {
            
            p = cameraPivotFront.position;
            pos = posBack + cameraPivotRear.position;
            p.y = p.y - lookDown;
            
        }
        
        player.position = p;
        
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * damper);
        transform.LookAt(player);
    }
}

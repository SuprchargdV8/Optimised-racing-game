using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScript : MonoBehaviour
{

    private Transform looker;
    public Rigidbody trainRigid;
    public Transform trainTrans;

    public float stop1;
    public float stop2;

    // Start is called before the first frame update
    void Start()
    {
        looker = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(stop2 > 0){  
            trainRigid.AddForce(trainTrans.forward * -50 * Time.deltaTime, ForceMode.Force);
            stop2 -= Time.deltaTime;
        }
        
        Vector3 temp = trainTrans.position;
        temp.x = temp.x - 10;
        temp.z = temp.z -40;

        if(stop1 > 0){
            looker.position = temp;
            stop1 -= Time.deltaTime;
        }
        looker.LookAt(trainTrans);
    }

    public void returnMain(){
        SceneManager.LoadScene(0);
    }
}

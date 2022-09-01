using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashSounds : MonoBehaviour
{
    
    private AudioSource crashSound;
    // Start is called before the first frame update
    void Start()
    {
        crashSound = GetComponent<AudioSource>();
    }
    
    void onCollisionEnter(Collision other){
        if(other.collider.gameObject.tag.Equals("World") || other.collider.gameObject.tag.Equals("Car")){
            crashSound.Play();
        }
    }
}

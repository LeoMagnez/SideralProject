using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    // Start is called before the first frame update
  Rigidbody Objectfracture ;

Rigidbody rgPlayer ;  
    void Start()
    {
        
     Objectfracture = gameObject.GetComponent<Rigidbody>();
     rgPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
      /// Objectfracture.AddExplosionForce(20,transform.position, 30f, 100f);
    //Objectfracture.AddForce(transform.up * 1000f);


    }

 void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            rgPlayer.AddExplosionForce(15f,transform.position, 1000f, 3f , ForceMode.Impulse);
        }
        if(collision.rigidbody){
            collision.rigidbody.AddExplosionForce(5f,transform.position, 1000f, 3f , ForceMode.Impulse);            
        }
    }    
        
}

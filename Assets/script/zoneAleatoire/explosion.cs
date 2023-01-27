using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        


        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody Objectfracture = gameObject.GetComponent<Rigidbody>();
        Objectfracture.AddExplosionForce(5, transform.forward, 7, 4);

            //Objectfracture.AddForce(-transform.up * Random.Range(15 , 20));

    }
}

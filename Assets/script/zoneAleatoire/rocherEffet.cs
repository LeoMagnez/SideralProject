using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocherEffet : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject fracture ; 
    public MeshRenderer visual ; 

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
       if (collision.gameObject.tag == "Player" )//|| collision.rigidbody.tag == "asteroideComplet")
        {
            fracture.SetActive(true);
            visual.enabled= false;
            gameObject.GetComponent<SphereCollider>().enabled = false;
            //gameObject.SetActive(false);
        }
    }    
}

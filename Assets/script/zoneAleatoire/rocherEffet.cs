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
        //fracture.SetActive(true);
      //  gameObject.SetActive(false);
       if (collision.rigidbody.tag == "Player" || collision.rigidbody.tag == "asteroideComplet")
        {
            fracture.SetActive(true);
            gameObject.SetActive(false);

        }
    }    
}

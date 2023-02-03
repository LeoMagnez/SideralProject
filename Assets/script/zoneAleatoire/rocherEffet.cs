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
//            Debug.Log("fekf,") ; 
            fracture.SetActive(true) ; 
            //spwan.poolFracture[0].transform.position = transform.position ; 
           // visual.enabled = false ;
            gameObject.SetActive(false) ;  
    }    
}

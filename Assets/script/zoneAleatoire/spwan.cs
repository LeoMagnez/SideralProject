using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spwan : MonoBehaviour
{


    public static int nombreObj ; 
    public GameObject prefabQuiApparait ;
    // Start is called before the first frame update
    void Start()
    {
        nombreObj = ((int)gameObject.transform.localScale.x +(int)gameObject.transform.localScale.y +(int)gameObject.transform.localScale.z)/3 ; 
        GameObject[] pool = new GameObject[nombreObj];


        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(prefabQuiApparait, new Vector3(Random.Range(gameObject.transform.position.x + -gameObject.transform.localScale.x /2 , gameObject.transform.position.x + gameObject.transform.localScale.x /2),Random.Range(gameObject.transform.position.y -gameObject.transform.localScale.y /2 ,gameObject.transform.position.y + gameObject.transform.localScale.y /2),Random.Range(gameObject.transform.position.z -gameObject.transform.localScale.z /2 , gameObject.transform.position.z + gameObject.transform.localScale.z /2)) , Quaternion.identity ) ;
        }       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

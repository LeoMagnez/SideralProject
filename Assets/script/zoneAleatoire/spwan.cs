using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spwan : MonoBehaviour
{


   public int nombreObj ; 
   public GameObject prefabQuiApparait ;
   public GameObject dossierRangement ;
   public GameObject player ;
   GameObject[] pool ;

   float detectionRadius ;          

    // Start is called before the first frame update
    void Start()
    {
        nombreObj = ((int)gameObject.transform.localScale.x +(int)gameObject.transform.localScale.y +(int)gameObject.transform.localScale.z)/3 ; 

        /////////// SI TU VEUX CHANGER LE NB OBJ DANS LA ZONE //////////
        pool = new GameObject[2000];
        /////////// FIN //////////

        /////////// SI TU VEUX CHANGER LA TAILLE DE DETECTION QU IL Y A ENTRE LE JOUEUR ET LA ZONE //////////
        detectionRadius = (gameObject.transform.localScale.x + gameObject.transform.localScale.y + gameObject.transform.localScale.z)/1.7f ; 
        /////////// FIN //////////

        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(prefabQuiApparait, new Vector3(Random.Range(gameObject.transform.position.x + -gameObject.transform.localScale.x /2 , gameObject.transform.position.x + gameObject.transform.localScale.x /2),Random.Range(gameObject.transform.position.y -gameObject.transform.localScale.y /2 ,gameObject.transform.position.y + gameObject.transform.localScale.y /2),Random.Range(gameObject.transform.position.z -gameObject.transform.localScale.z /2 , gameObject.transform.position.z + gameObject.transform.localScale.z /2)) , Quaternion.identity , dossierRangement.transform) ;
        }       
        
    }

    // Update is called once per frame
    void Update()
    {     
        if(Vector3.Distance(player.transform.position,gameObject.transform.position) > detectionRadius)
        {
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i].SetActive(false) ; 
            } 
        }
        else 
        {
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i].SetActive(true) ; 
            }             
        }
    }

    void OnDrawGizmos(){

        Gizmos.color =  Color.red ;
        Gizmos.DrawWireSphere(transform.position,detectionRadius) ;         
    }     
}

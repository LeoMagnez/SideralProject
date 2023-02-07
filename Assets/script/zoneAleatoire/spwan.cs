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
   float timer = 0;
   bool siIlEstActive = false;
   bool siIlsOntSpawn = false;



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
            pool[i].transform.localScale = new Vector3(0f, 0f, 0f) ;
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
            siIlEstActive = true;

            timer = timer + 1f * Time.deltaTime;
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i].SetActive(true);
            }

            if (siIlEstActive == true && timer < 1) 
            {
                for (int i = 0; i < pool.Length; i++)
                {
                    pool[i].transform.localScale += new Vector3(0.05f,0.05f,0.05f);
                }
            }
            else if(timer > 1 && siIlsOntSpawn == false)
            {
                for (int i = 0; i < pool.Length; i++)
                {
                    pool[i].transform.localScale = new Vector3(Random.Range(1f, 10f), Random.Range(1f, 10f), Random.Range(1f, 10f));
                }
                siIlsOntSpawn = true;
            }
            Debug.Log("timer + " + timer); 
        }
    }

    void OnDrawGizmos(){

        Gizmos.color =  Color.red ;
        Gizmos.DrawWireSphere(transform.position,detectionRadius) ;         
    }     
}

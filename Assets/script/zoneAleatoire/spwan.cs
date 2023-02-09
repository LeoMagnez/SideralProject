using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spwan : MonoBehaviour
{


   public int nombreObj ; 
   public GameObject prefabQuiApparait ;
   public GameObject prefabQuiApparaitDeux ;
   public GameObject dossierRangement ;
   public GameObject player ;
   GameObject[] pool ;
   GameObject[] poolBonus ;


   float detectionRadius ;
   bool coroutinesEstActive = false;   



    // Start is called before the first frame update
    void Start()
    {
        nombreObj = ((int)gameObject.transform.localScale.x +(int)gameObject.transform.localScale.y +(int)gameObject.transform.localScale.z)/3 ; 

        /////////// SI TU VEUX CHANGER LE NB OBJ DANS LA ZONE //////////
        pool = new GameObject[1500];
        poolBonus = new GameObject[1500];

        /////////// FIN //////////

        /////////// SI TU VEUX CHANGER LA TAILLE DE DETECTION QU IL Y A ENTRE LE JOUEUR ET LA ZONE //////////
        detectionRadius = (gameObject.transform.localScale.x + gameObject.transform.localScale.y + gameObject.transform.localScale.z)/3f ; 
        /////////// FIN //////////

        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(prefabQuiApparait, new Vector3(Random.Range(gameObject.transform.position.x + -gameObject.transform.localScale.x /2 , gameObject.transform.position.x + gameObject.transform.localScale.x /2),Random.Range(gameObject.transform.position.y -gameObject.transform.localScale.y /2 ,gameObject.transform.position.y + gameObject.transform.localScale.y /2),Random.Range(gameObject.transform.position.z -gameObject.transform.localScale.z /2 , gameObject.transform.position.z + gameObject.transform.localScale.z /2)) , Quaternion.identity , dossierRangement.transform) ;
            pool[i].SetActive(false) ;         
        }
        for (int i = 0; i < poolBonus.Length; i++)
        {
            poolBonus[i] = Instantiate(prefabQuiApparaitDeux, new Vector3(Random.Range(gameObject.transform.position.x + -gameObject.transform.localScale.x /2 , gameObject.transform.position.x + gameObject.transform.localScale.x /2),Random.Range(gameObject.transform.position.y -gameObject.transform.localScale.y /2 ,gameObject.transform.position.y + gameObject.transform.localScale.y /2),Random.Range(gameObject.transform.position.z -gameObject.transform.localScale.z /2 , gameObject.transform.position.z + gameObject.transform.localScale.z /2)) , Quaternion.identity , dossierRangement.transform) ;
            poolBonus[i].SetActive(false) ;                     
            poolBonus[i].name = "bonus" ;       
        }        
    }

    // Update is called once per frame
    void Update()
    {     
        if(Vector3.Distance(player.transform.position,gameObject.transform.position) > detectionRadius)
        {
            coroutinesEstActive = false ;
            
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i].SetActive(false) ; 
                poolBonus[i].SetActive(false) ; 

            } 
        }
        else if(Vector3.Distance(player.transform.position,gameObject.transform.position) < detectionRadius && coroutinesEstActive == false)
        {
            StartCoroutine(apparitionAsteroide());
        }
    }
    IEnumerator apparitionAsteroide()
    {
            coroutinesEstActive = true;
/////// SI ON GARDE LE MEME NB D'OBJ DANS LES DEUX POOL Y A PAS DE SOUCIS, SINON FAUDRA RE POLISH LE SCRIPT, MAIS YA PAS DE SOUCIS C'EST RAPIDE ////////
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i].SetActive(true);
                poolBonus[i].SetActive(true);

                pool[i].transform.localScale = new Vector3(0f, 0f, 0f) ;
                poolBonus[i].transform.localScale = new Vector3(0f, 0f, 0f) ;

            }
            yield return new WaitForSeconds(0.5f) ; 
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i].transform.localScale += new Vector3(1f,1f,1f);
                poolBonus[i].transform.localScale += new Vector3(0.2f,0.2f,0.2f);

            }
            yield return new WaitForSeconds(0.1f) ; 
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i].transform.localScale += new Vector3(1f,1f,1f);
                poolBonus[i].transform.localScale += new Vector3(0.2f,0.2f,0.2f);
                
            }
            yield return new WaitForSeconds(0.1f) ;
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i].transform.localScale += new Vector3(1f,1f,1f);
                poolBonus[i].transform.localScale += new Vector3(0.2f,0.2f,0.2f);

            }
            yield return new WaitForSeconds(0.1f) ;
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i].transform.localScale += new Vector3(1f,1f,1f);
                poolBonus[i].transform.localScale += new Vector3(0.2f,0.2f,0.2f);

            }
            yield return new WaitForSeconds(0.1f) ;             
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i].transform.localScale += new Vector3(1f,1f,1f);
                poolBonus[i].transform.localScale += new Vector3(0.2f,0.2f,0.2f);

            }
            yield return new WaitForSeconds(0.1f) ;                                                 

            for (int i = 0; i < pool.Length; i++)
            {
                pool[i].transform.localScale = new Vector3(Random.Range(1f, 10f), Random.Range(1f, 10f), Random.Range(1f, 10f));
                poolBonus[i].transform.localScale = new Vector3(10f,10f,10f);
            }
    }
    void OnDrawGizmos()
    {
        Gizmos.color =  Color.red ;
        Gizmos.DrawWireSphere(transform.position,detectionRadius) ;         
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHolePos : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    
    Vector3 posBH;
    Vector3 posBHNormalized;

    // Start is called before the first frame update
    void Start()
    {
        posBH = player.transform.position - gameObject.transform.position;
        posBHNormalized = player.transform.position + posBH.normalized * 1;
        this.gameObject.transform.position = posBHNormalized;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

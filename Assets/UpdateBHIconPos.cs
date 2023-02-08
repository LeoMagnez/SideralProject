using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBHIconPos : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;

    [SerializeField]
    Transform blackHoleTransform;

    [SerializeField]
    float ratio;

    [SerializeField]
    GameObject minimapCam;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        transform.rotation = minimapCam.transform.rotation * Quaternion.Euler(90,0,0);

        Vector3 _playerToBlackHole = blackHoleTransform.position - playerTransform.position;

        transform.position = playerTransform.position;
        transform.position += _playerToBlackHole.normalized * ratio * _playerToBlackHole.magnitude;

        //gameObject.transform.LookAt(minimapCam.transform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetShards : MonoBehaviour
{
    [SerializeField]
    private GameObject blackHole;
    [SerializeField]
    float ratio;

    private bool isDrifting;
    private float timer = 0f;
    private float delay = 0f;
    private float startingZ;

    [SerializeField]
    private float duration = 0f;
    [SerializeField]
    private float randomScale = 0f;
    [SerializeField]
    private float radius;

    /*public float moveDuration = 2f;
    private float elapsedTime;*/

    public void StartDrifting()
    {
        isDrifting = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        startingZ = transform.localPosition.z;
        delay = Mathf.Lerp(0, duration, (startingZ + radius) / (2 * radius)); //depend de la position du shard
        delay += randomScale * Random.value; // desynchronise les shards
    }

    // Update is called once per frame
    void Update()
    {
        //elapsedTime += Time.deltaTime;
        timer += Time.deltaTime;

        if(timer > delay)
        {
            //float movingShards = elapsedTime / moveDuration;
            Vector3 _shardToBlackHole = blackHole.transform.position - transform.localPosition;

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, (_shardToBlackHole.normalized * ratio * _shardToBlackHole.magnitude), (timer * delay / 4));
            //transform.position += _shardToBlackHole.normalized * ratio * _shardToBlackHole.magnitude;
        }
    }
}

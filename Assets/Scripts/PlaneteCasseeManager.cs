using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneteCasseeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject shard1, shard2, shard3, shard4, shard5, shard6, shard7, shard8;

    [SerializeField]
    private Material planeteMat;

    public float timer = 5f;

    bool entered;
    private float span;
    public float shockWaveDuration = 2f;
    private float firstWaveElapsedTime;
    private float secondWaveElapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        shard1.SetActive(false);
        shard2.SetActive(false);
        shard3.SetActive(false);
        shard4.SetActive(false);
        shard5.SetActive(false);
        shard6.SetActive(false);
        shard7.SetActive(false);
        shard8.SetActive(false);
        entered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (entered)
        {
            timer -= Time.deltaTime;
        }

        if (entered && timer >= 1f)
        {
            firstWaveElapsedTime += Time.deltaTime;
            float firstPercentageComplete = firstWaveElapsedTime / shockWaveDuration;
            span = Mathf.MoveTowards(0f, 1f, firstPercentageComplete);
            planeteMat.SetFloat("_Shockwave_Span", span);

        }

        if(timer <= 1f)
        {
            secondWaveElapsedTime += Time.deltaTime;
            float secondPercentageComplete = secondWaveElapsedTime / shockWaveDuration;
            span = Mathf.MoveTowards(1f, 3f, secondPercentageComplete);
            planeteMat.SetFloat("_Shockwave_Span", span);

        }

        if(timer <= 0.01f)
        {
            shard1.SetActive(true);
            shard2.SetActive(true);
            shard3.SetActive(true);
            shard4.SetActive(true);
            shard5.SetActive(true);
            shard6.SetActive(true);
            shard7.SetActive(true);
            shard8.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    { 
        if(other.gameObject.tag == "Player")
        {
            entered = true;
        }
    }
}

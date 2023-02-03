using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light _light;

    public float flickerDuration = 1f;
    public Color whiteColor = Color.white;
    public Color redColor = Color.red;
    public float timer = 5f;


    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Flickering(float duration, float speed)
    {


        float elapsed = 0.0f;

        while(elapsed < duration)
        {
            float t = (Mathf.PingPong(Time.time, flickerDuration) / flickerDuration) * speed;
            _light.color = Color.Lerp(whiteColor, redColor, t);

            elapsed += Time.deltaTime;

            yield return null;
        }

        _light.color = whiteColor;

        
    }


}

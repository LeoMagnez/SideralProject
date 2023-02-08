
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

public class EndOfGameManager : MonoBehaviour
{
    public VisualEffect flamesVFX;
    public Camera cam;
    public Volume volume;
    public LensDistortion distortion;
    public float fishEye;
    public CameraShake shake;

    public float timer = 5f;
    private float amountSpan;
    private float amountElapsedTime;
    private float sizeElapsedTime;
    private float sizeSpan;
    public float duration = 2f;
    private bool entered;
    // Start is called before the first frame update
    void Start()
    {
        entered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(entered)
        {
            timer -= Time.deltaTime;
        }

        if(entered && timer >= 4)
        {
            amountElapsedTime += Time.deltaTime;

            float amountPercentageComplete = amountElapsedTime / duration;
            amountSpan = Mathf.MoveTowards(0f, 1f, amountPercentageComplete * 4f);
            flamesVFX.SetFloat("FlameAmount", amountSpan);
            StartCoroutine(shake.Shake(5f, 0.1f));
        }

        if(entered && timer <= 0.5f)
        {
            sizeElapsedTime += Time.deltaTime;

            float sizePercentageComplete = sizeElapsedTime / duration;
            sizeSpan = Mathf.MoveTowards(0f, 1f, sizePercentageComplete * 1.1f);
            flamesVFX.SetFloat("FlameSize", sizeSpan);
            StartCoroutine(shake.Shake(5f, 0.5f));
            volume.profile.TryGet<LensDistortion>(out distortion);
            fishEye = Mathf.MoveTowards(0f, -1f, sizePercentageComplete * 2f);
            distortion.intensity.Override(fishEye);
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

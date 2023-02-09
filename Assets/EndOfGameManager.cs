
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;
using TMPro;

public class EndOfGameManager : MonoBehaviour
{
    public VisualEffect flamesVFX;
    public Camera cam;
    public Volume volume;
    public LensDistortion distortion;
    public float fishEye;
    public CameraShake shake;
    public Material glitchyHologram;
    public Material glitchyHologram2;
    public Material glitchyHologram3;
    public Material glitchyText;
    public Material glitchyText2;

    public FlickeringLight leftFlickeringLight;
    public FlickeringLight rightFlickeringLight;

    public float timer = 5f;
    private float amountSpan;
    private float amountElapsedTime;
    private float sizeElapsedTime;
    private float sizeSpan;
    public float duration = 2f;
    private bool entered;


    public TextMeshProUGUI errorText;
    // Start is called before the first frame update
    void Start()
    {
        glitchyHologram.SetFloat("_GlitchAmount", 0f);
        glitchyHologram2.SetFloat("_GlitchAmount", 0f);
        glitchyHologram3.SetFloat("_GlitchAmount", 0f);
        glitchyText.SetFloat("_GlitchAmount", 0f);
        glitchyText2.SetFloat("_GlitchAmount", 0f);
        errorText.text = "";
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

            glitchyHologram.SetFloat("_GlitchAmount", Mathf.PingPong(Time.timeScale, 0.3f));
            glitchyHologram2.SetFloat("_GlitchAmount", Mathf.PingPong(Time.timeScale, 0.3f));
            glitchyHologram3.SetFloat("_GlitchAmount", Mathf.PingPong(Time.timeScale, 0.3f));
            glitchyText.SetFloat("_GlitchAmount", Mathf.PingPong(Time.timeScale, 0.4f));
            glitchyText2.SetFloat("_GlitchAmount", Mathf.PingPong(Time.timeScale, 0.4f));

            StartCoroutine(ErrorFlicker());
            leftFlickeringLight.flickerDuration = 0.4f;
            rightFlickeringLight.flickerDuration = 0.4f;
            StartCoroutine(leftFlickeringLight.Flickering(6f, 1f));
            StartCoroutine(rightFlickeringLight.Flickering(6f, 1f));

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

            leftFlickeringLight.flickerDuration = 0.2f;
            rightFlickeringLight.flickerDuration = 0.2f;
            StartCoroutine(leftFlickeringLight.Flickering(5.5f, 1f));
            StartCoroutine(rightFlickeringLight.Flickering(5.5f, 1f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            entered = true;
        }
    }

    public IEnumerator ErrorFlicker()
    {
        while (entered)
        {
            errorText.text = "ERROR";
            yield return new WaitForSeconds(0.8f);
            errorText.text = "";
            yield return new WaitForSeconds(0.8f);
        }



    }
}

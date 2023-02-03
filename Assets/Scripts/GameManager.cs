using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Claude claude;
    public CameraShake camShake;
    public FlickeringLight leftFlickeringLight;
    public FlickeringLight rightFlickeringLight;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClaudeHitsPlayer()
    {
        Debug.Log("Claude/Player collision !");


        StartCoroutine(camShake.Shake(1f, 0.2f));
        player.TakeDamage(new Vector3(player.transform.position.x - claude.GetMinDistanceBetweenClaudeAndThePlayer(), player.transform.position.y, player.transform.position.z), claude.impactForce, claude.damage - 1, claude.GetMinDistanceBetweenClaudeAndThePlayer()*2);
        player.health -= 25;

        if (player.health == 75)
        {
            AudioManager.instance.PlaySFX("Health75");
            leftFlickeringLight.flickerDuration = 0.4f;
            rightFlickeringLight.flickerDuration = 0.4f;
            StartCoroutine(leftFlickeringLight.Flickering(6f, 1f));
            StartCoroutine(rightFlickeringLight.Flickering(6f, 1f));
        }

        if (player.health == 50)
        {
            AudioManager.instance.PlaySFX("Health50");
            leftFlickeringLight.flickerDuration = 0.2f;
            rightFlickeringLight.flickerDuration = 0.2f;
            StartCoroutine(leftFlickeringLight.Flickering(5.5f, 1f));
            StartCoroutine(rightFlickeringLight.Flickering(5.5f, 1f));
        }

        if (player.health == 25)
        {
            AudioManager.instance.PlaySFX("Health25");
            leftFlickeringLight.flickerDuration = 0.05f;
            rightFlickeringLight.flickerDuration = 0.05f;
            StartCoroutine(leftFlickeringLight.Flickering(5.5f, 1f));
            StartCoroutine(rightFlickeringLight.Flickering(5.5f, 1f));
        }

        if(player.health == 0)
        {
            player.Die();
        }

    }
}

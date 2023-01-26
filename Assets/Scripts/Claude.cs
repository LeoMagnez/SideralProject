using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Events;

public class Claude : MonoBehaviour
{
    [Header("Claude properties")]
    public float claudeBaseSpeed;
    public float claudeChasingSpeed;
    public float claudeSlowSpeed;
    public float slowDuration;
    public float impactForce;
    public float damage;

    [Header("Player properties")]
    public GameObject player;
    public float maxDistanceBeweenClaudeAndThePlayer;

    [Header("What happens if Claude hits the player?")]
    public UnityEvent claudeHitsPlayer;


    private VisualEffect VE_Claude_Cloud;
    private VisualEffect VE_Claude_Particles;
    public float minDistanceBetweenClaudeAndThePlayer;
    private Coroutine claudeHitCoroutine;

    private void Start()
    {
        VE_Claude_Particles = transform.GetChild(0).GetComponent<VisualEffect>();
        VE_Claude_Cloud = transform.GetChild(1).GetComponent<VisualEffect>();
        minDistanceBetweenClaudeAndThePlayer = maxDistanceBeweenClaudeAndThePlayer / 10;
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
        UpdateClaudeColor();
    }

    
    void ChasePlayer()
    {
        transform.position = new Vector3(transform.position.x, player.transform.position.y, player.transform.position.z);

        float distanceBetweenClaudeAndThePlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceBetweenClaudeAndThePlayer >= maxDistanceBeweenClaudeAndThePlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Mathf.Lerp(claudeBaseSpeed, claudeChasingSpeed, (distanceBetweenClaudeAndThePlayer - maxDistanceBeweenClaudeAndThePlayer)/maxDistanceBeweenClaudeAndThePlayer) * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, claudeBaseSpeed * Time.deltaTime);
        }

        if (distanceBetweenClaudeAndThePlayer <= minDistanceBetweenClaudeAndThePlayer && claudeHitCoroutine == null)
        {
            claudeHitsPlayer.Invoke();
            claudeHitCoroutine = StartCoroutine(SlowClaudeAfterAHit());
        }
    }

    void UpdateClaudeColor()
    {
        float distanceBetweenClaudeAndThePlayer = Vector3.Distance(transform.position, player.transform.position);
        VE_Claude_Cloud.SetFloat("DangerValue", Mathf.Lerp(1, 0, (distanceBetweenClaudeAndThePlayer - minDistanceBetweenClaudeAndThePlayer) / maxDistanceBeweenClaudeAndThePlayer));
        VE_Claude_Particles.SetFloat("DangerValue", Mathf.Lerp(1, 0, (distanceBetweenClaudeAndThePlayer - minDistanceBetweenClaudeAndThePlayer) / maxDistanceBeweenClaudeAndThePlayer));

        VE_Claude_Cloud.playRate = Mathf.Lerp(2.5f, 1f, (distanceBetweenClaudeAndThePlayer - minDistanceBetweenClaudeAndThePlayer) / maxDistanceBeweenClaudeAndThePlayer);
        VE_Claude_Particles.playRate = Mathf.Lerp(2.5f, 1f, (distanceBetweenClaudeAndThePlayer - minDistanceBetweenClaudeAndThePlayer) / maxDistanceBeweenClaudeAndThePlayer);
    }

    private IEnumerator SlowClaudeAfterAHit()
    {
        float tempSpeed = claudeBaseSpeed;

        claudeBaseSpeed = claudeSlowSpeed;
        yield return new WaitForSeconds(slowDuration);
        claudeBaseSpeed = tempSpeed;
        claudeHitCoroutine = null;

        yield return null;
    }

    public float GetMinDistanceBetweenClaudeAndThePlayer()
    {
        return minDistanceBetweenClaudeAndThePlayer;
    }
}

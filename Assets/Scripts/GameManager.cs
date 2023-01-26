using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Claude claude;


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
        player.TakeDamage(new Vector3(player.transform.position.x - claude.GetMinDistanceBetweenClaudeAndThePlayer(), player.transform.position.y, player.transform.position.z), claude.impactForce, claude.damage, claude.GetMinDistanceBetweenClaudeAndThePlayer()*2);
    }
}

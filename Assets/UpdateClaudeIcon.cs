using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateClaudeIcon : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;

    [SerializeField]
    Transform claudeTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _playerToClaude = claudeTransform.position - playerTransform.position;

        transform.position = playerTransform.position;
        transform.position += _playerToClaude.normalized * 0.9f * _playerToClaude.magnitude;
    }
}

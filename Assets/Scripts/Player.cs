using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    public float maxHealth = 100;

    
    public float health;

    private Rigidbody rb;

    public TextMeshProUGUI healthText;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.SetText(health.ToString());
    }

    public void TakeDamage(Vector3 impactPosition, float impactForce, float damage, float explosionRadius)
    {
        rb.AddExplosionForce(impactForce, impactPosition, explosionRadius);
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

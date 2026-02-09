using System;
using UnityEngine;

public class EnemyHard : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody enemyHardRb;
    private GameObject player;
    private float bottomBorder = -10;
    private float strength = 5.0f;
    private bool isGround;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        enemyHardRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (player.transform.position.y <= 2.0f)
        {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            enemyHardRb.AddForce(lookDirection * speed);
        }
        DestroyEnemyAtBottom();
    }
    
    private void DestroyEnemyAtBottom()
    {
        if (transform.localPosition.y < bottomBorder)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 awayFromEnemy = other.gameObject.transform.position - transform.position;
            enemyHardRb.AddForce(awayFromEnemy * strength, ForceMode.Impulse);
        }
    }
}

using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3.0f;
    private Rigidbody enemyRb;
    private GameObject player;
    private float bottomBorder = -10;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        enemyRb = GetComponent<Rigidbody>();
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
            enemyRb.AddForce(lookDirection * speed);
        }
        DestroyEnemyAtBottom();
    }

    private void DestroyEnemyAtBottom()
    {
        if (transform.localPosition.y <= bottomBorder)
        {
            Destroy(gameObject);
        }
    }
}

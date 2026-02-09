using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private int smallBulletStrength = 30;
    public float bulletSpeed = 10.0f;
    
    private float sqrDetectionRange = 5f;
    
    public float turnSpeed = 10.0f;
    private float bulletBorder = 24;
    
    private void FixedUpdate()
    {
      HomingBullet();
      CheckBulletOutOfBounce();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromBullet = other.gameObject.transform.position - transform.position ;
            enemyRigidbody.AddForce(awayFromBullet * smallBulletStrength, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }
    
    
    private void HomingBullet()
    {
       GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
       GameObject closestEnemy = null;
      

       foreach (var enemy in enemies)
       {
           float sqrDistance = (enemy.transform.position - transform.position).sqrMagnitude;

           if (sqrDistance <= sqrDetectionRange)
           {
               closestEnemy = enemy;
           }
       }

       if (closestEnemy)
       {
           Vector3 lookDirection = (closestEnemy.transform.position - transform.position).normalized;
           transform.Translate(lookDirection * (bulletSpeed * Time.deltaTime), Space.World);
           transform.up = lookDirection;
       }
       else
       {
           transform.Translate(-transform.forward * (bulletSpeed * Time.deltaTime), Space.Self);
       }
    }
    
    private void CheckBulletOutOfBounce()
    {
        if (transform.position.x >= bulletBorder || transform.position.x <= -bulletBorder)
        {
            Destroy(gameObject);
        }
        
        if (transform.position.z >= bulletBorder || transform.position.z <= -bulletBorder)
        {
            Destroy(gameObject);
        }
    } 
}

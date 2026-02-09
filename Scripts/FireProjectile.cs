using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public GameObject smallBullet;
    public List<ParticleSystem> fireParticle;
    [SerializeField] private PlayerController playerController;
    
    private bool bulletDelay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        FireSmallBullet();
        SetFireParticlePosition();
    }
    
    private void FireSmallBullet()
    {
        if (playerController.hasFireUp)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!bulletDelay)
                {
                    bulletDelay = true;
                    var position = BulletPositionAlignment();
                    var rotation = BulletRotationAlignment();
                    
                    StartFireParticle();
              
                    Instantiate(smallBullet, position.frontRight, rotation.standardRotation);
                    Instantiate(smallBullet, position.frontLeft, rotation.standardRotation);
                    Instantiate(smallBullet, position.rightSide, rotation.rightRotation);
                    Instantiate(smallBullet, position.leftSide, rotation.leftRotation);
                
                    StartCoroutine(DelayShotBullets());
                }
            }
        }
    }

    private (Vector3 frontRight, Vector3 frontLeft, Vector3 rightSide, Vector3 leftSide) BulletPositionAlignment()
    {
        Vector3 frontRight = transform.position + transform.forward * 1.0f + transform.right * 0.5f;
        Vector3 frontLeft = transform.position + transform.forward * 1.0f + transform.right * -0.5f;
        Vector3 rightSide = transform.position + transform.right * 1.2f;
        Vector3 leftSide = transform.position + transform.right * -1.2f;

        return (frontRight, frontLeft, rightSide, leftSide);
    }

    private (Quaternion rightRotation, Quaternion leftRotation, Quaternion standardRotation) BulletRotationAlignment()
    {
        Quaternion standardRotation = transform.rotation * smallBullet.transform.rotation;
                
        Quaternion extraRightRotation = Quaternion.Euler(0, 0, -90f);
        Quaternion rightRotation = transform.rotation * smallBullet.transform.rotation * extraRightRotation;
                
        Quaternion extraLeftRotation = Quaternion.Euler(0, 0, 90f);
        Quaternion leftRotation = transform.rotation * smallBullet.transform.rotation * extraLeftRotation;

        return (rightRotation, leftRotation, standardRotation);
    }
    
    
    private void StartFireParticle()
    {
        for (int i = 0; i < 4; i++)
        {
            fireParticle[i].Play();
        }
    }

    private void StopFireParticle()
    {
        for (int i = 0; i < 4; i++)
        {
            fireParticle[i].Stop();
        }
    }
    
    
    private void SetFireParticlePosition()
    {
        var position = BulletPositionAlignment();
        
        fireParticle[0].transform.position = position.frontLeft;
        fireParticle[1].transform.position = position.frontRight;
        fireParticle[2].transform.position = position.leftSide;
        fireParticle[3].transform.position = position.rightSide;
        
    }
    
    IEnumerator DelayShotBullets()
    {
        yield return new WaitForSeconds(0.25f);
        StopFireParticle();
        bulletDelay = false;
    }
}

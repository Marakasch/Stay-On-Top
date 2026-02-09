using System.Collections;
using UnityEngine;
using TMPro;
public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private BoxCollider playerBc;
    private GameObject cameraHolder;
    private GameObject projectileHolder;
    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;
    private SpawnManager spawnManager;
    public TextMeshProUGUI liveText;
    private int playerLives = 3;
    
    [Header("PlayerMovement")]
    public float speed = 4.0f;
    private float currentSpeed;
    private float border = 10;
    private bool isGround;
    
    public bool hasFireUp;

    [Header("PowerUp")]
    public GameObject powerUpIndicator;
    private Rigidbody powerUpRb;
    private Quaternion powerUpstartRotation; 
    private float powerUpStrength = 20.0f;
    private bool hasPowerUp;
    private bool activePowerUpAnimation;
    private int rotationsSpeed = 20;
    
    [Header("JumpUp")] 
    private bool hasJumpUp;
    private float upForce = 20f;
    private float downForce = 40f;
    public ParticleSystem fireParticleUp;
    public ParticleSystem fireParticleRotateBlue;
    public ParticleSystem fireParticleRotateRed;
    public ParticleSystem fireParticleGround;
    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powerUpRb = powerUpIndicator.gameObject.GetComponent<Rigidbody>();
        playerRb = GetComponent<Rigidbody>();
        playerBc = GetComponent<BoxCollider>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        
        cameraHolder = GameObject.Find("Camera Holder");
        projectileHolder = GameObject.Find("Projectile Holder");
        
        playerStartPosition = transform.position;
        playerStartRotation = transform.rotation;
        powerUpstartRotation = powerUpIndicator.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnManager.isGameActive)
        {
            liveText.text = $"Lives left: {playerLives}";
            PlayerControls();
            IndicatorOnPlayerPosition();
            ParticleOnPlayerPosition();
            ResetPlayerPosition();
        }
    }

    private void PlayerControls()
    {
        if (playerLives == 0)
        {
            spawnManager.GameOver();
        }
        
        if (isGround)
        {
            float verticalInput = Input.GetAxis("Vertical");
            currentSpeed = (verticalInput * speed);
            playerRb.AddForce(cameraHolder.transform.forward * currentSpeed);
        }
        
        projectileHolder.transform.position = transform.position + new Vector3(0, 0.4f, 0);
        JumpAndDownSmash();
    }

    private void IndicatorOnPlayerPosition()
    {
        if (hasPowerUp || hasFireUp || hasJumpUp)
        {
            powerUpIndicator.transform.position = transform.position + new Vector3(0, 1.55f, 0);
        }
    }
    
    private void ParticleOnPlayerPosition()
    {
        if (hasJumpUp)
        {
            fireParticleRotateRed.transform.position = transform.position;
            fireParticleRotateBlue.transform.position = transform.position;
        }
    }
    
    private void ResetPlayerPosition()
    {
        if (transform.position.y <= -border)
        {
            playerLives--;
            if(playerLives > 0)
            {
                transform.position = playerStartPosition;
                ResetPlayerForceMovement();
            }
        }
        else if (transform.position.y >= border)
        { 
            transform.position = new Vector3(transform.position.x, border, transform.position.z);
        }
    }
    
    private void ResetPlayerForceMovement()
    {
        playerRb.angularVelocity = Vector3.zero;
        playerRb.linearVelocity = Vector3.zero;
    }

    private void ResetPowerUpTransform()
    {
        powerUpRb.transform.rotation = powerUpstartRotation; 
        powerUpRb.linearVelocity = Vector3.zero;
        powerUpRb.angularVelocity = Vector3.zero;
    }
    
    private void JumpAndDownSmash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && hasJumpUp && isGround)
        {
            isGround = false;
            fireParticleUp.transform.position = transform.position;
            fireParticleUp.Play();
            playerRb.AddForce(cameraHolder.transform.up * upForce, ForceMode.Impulse);
            ResetPlayerForceMovement();
            playerRb.AddTorque(Vector3.right * rotationsSpeed, ForceMode.Impulse);
            StartCoroutine(SmashDown());
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            if(!activePowerUpAnimation && !hasFireUp && !hasJumpUp)
            {
                activePowerUpAnimation = true;
                hasPowerUp = true;
                Destroy(other.gameObject);
                spawnManager.powerUpCounter--;
                powerUpIndicator.gameObject.SetActive(true);
                StartCoroutine(PowerUpCountdown());
            }
        }

        if (other.CompareTag("FireUp"))
        {
            if (!activePowerUpAnimation && !hasPowerUp && !hasJumpUp)
            {
                activePowerUpAnimation = true;
                hasFireUp = true;
                Destroy(other.gameObject);
                spawnManager.fireUpCounter--;
                powerUpIndicator.gameObject.SetActive(true);
                StartCoroutine(PowerUpCountdown());
            }
        }
        
        if (other.CompareTag("JumpUp"))
        {
            if (!activePowerUpAnimation && !hasPowerUp && !hasFireUp)
            {
                activePowerUpAnimation = true;
                hasJumpUp = true;
                Destroy(other.gameObject);
                spawnManager.jumpUpCounter--;
                powerUpIndicator.gameObject.SetActive(true);
                StartCoroutine(PowerUpCountdown());
            }
        }
        
        if (other.gameObject.CompareTag("Enemy") && hasJumpUp)
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;
            enemyRigidbody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
        
        if (other.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;
            enemyRigidbody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
        }
    }

    private void StartParticleOnPlayer()
    {
      fireParticleGround.transform.position = transform.position;
      fireParticleGround.Play();
    }

    private void StopRotation()
    { 
      ResetPlayerForceMovement();
      transform.rotation = playerStartRotation;
    }
    
    private IEnumerator PowerUpCountdown()
    {
      yield return new WaitForSeconds(8);
      powerUpRb.AddTorque(Vector3.right * rotationsSpeed, ForceMode.Impulse);
      
      yield return new WaitForSeconds(2);
      hasPowerUp = false;
      hasFireUp = false;
      hasJumpUp = false;
      powerUpRb.AddForce(Vector3.up * 15, ForceMode.Impulse);
      
      yield return new WaitForSeconds(1);
      activePowerUpAnimation = false;
      powerUpIndicator.gameObject.SetActive(false);
      ResetPowerUpTransform();
    }

    private IEnumerator SmashDown()
    {
        yield return new WaitForSeconds(0.5f);
        fireParticleUp.Stop();
        fireParticleRotateRed.Play();
        
        yield return new WaitForSeconds(0.7f);
        fireParticleRotateRed.Stop();
        fireParticleRotateBlue.Play();
        
        yield return new WaitForSeconds(1f);
        fireParticleRotateBlue.Stop();
        playerRb.AddForce(-Vector3.up * downForce, ForceMode.Impulse);
        playerBc.enabled = true;
        
        yield return new WaitForSeconds(0.25f);
        StopRotation();
        StartParticleOnPlayer();
        
        yield return new WaitForSeconds(1f);
        playerBc.enabled = false;
        fireParticleGround.Stop();
    }
}

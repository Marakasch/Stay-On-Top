using UnityEngine;

public class PowerUpMovement : MonoBehaviour
{

    public float spinningSpeed = 50f;

    private float floatSpeed = 0.25f;

    private float MaxHeight = 0.6f;
    private float MinHeight = 0.1f;

    private bool floating;
    // Update is called once per frame
    void Update()
    { 
        transform.Rotate(Vector3.up, spinningSpeed * Time.deltaTime);
        FloatBehavior();
    }

    private void FloatBehavior()
    {
        if (!floating)
        {
            if (transform.position.y >= MaxHeight)
            {
                floating = true;
            }
            transform.Translate(Vector3.up * (floatSpeed * Time.deltaTime));
        }
        
        if (floating)
        {
            if (transform.position.y <= MinHeight)
            {
                floating = false;
            }
            transform.Translate(Vector3.down * (floatSpeed * Time.deltaTime));
        }
    }
}

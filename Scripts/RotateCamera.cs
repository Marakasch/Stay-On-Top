using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float roationSpeed;
    
    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * roationSpeed * Time.deltaTime);
    }
}

using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Vector3 velocity;
    public Rigidbody rb;

    private void Update()
    {
        rb.linearVelocity = velocity;
    }


}

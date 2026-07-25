using UnityEngine;

public class VelocityTracker : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject tracked;

    private void FixedUpdate()
    {
        rb.MovePosition(tracked.transform.position);
    }



}

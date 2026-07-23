using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public float time;
    public Vector3 velocity;
    public Rigidbody rb;

    private void Start()
    {
        
    }
    void Update()
    {
        time += Time.deltaTime;
        rb.linearVelocity = velocity;
    }
}

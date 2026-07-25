using UnityEngine;

public class TimerScript : MonoBehaviour, ICarrayable
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

    public void TurnPhysicsOff()
    {
        rb.isKinematic = true;
        rb.useGravity = false; 
        rb.detectCollisions = false;
        rb.freezeRotation = true;
    }

    public void TurnPhysicsOn()
    {
        rb.isKinematic = false;
        rb.detectCollisions = true;
        rb.useGravity = true;
        rb.freezeRotation = false;
    }

    public GameObject Pickup()
    {
        TurnPhysicsOff();
        Debug.Log("Picked up " + transform.name);
        return gameObject;
    }

    public GameObject PutDown()
    {
        TurnPhysicsOn();
        Debug.Log("Put down " + transform.name);
        return gameObject;
    }
}

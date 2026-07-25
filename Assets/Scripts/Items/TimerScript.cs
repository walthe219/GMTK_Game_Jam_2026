using UnityEngine;

public class TimerScript : MonoBehaviour, ICarrayable
{
    public float time;
    public Vector3 velocity;
    public Rigidbody rb;
    public CharacterController playeCC;

    bool isCarried = false;

    private void Start()
    {
        
    }

    void Update()
    {
        time += Time.deltaTime;

        if (isCarried)
        {
            //rb.linearVelocity = playeCC.velocity;
        }
        //rb.linearVelocity = velocity;
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
        isCarried = true;
        TurnPhysicsOff();
        Debug.Log("Picked up " + transform.name);
        return gameObject;
    }

    public GameObject PutDown()
    {
        isCarried = false;
        TurnPhysicsOn();
        Debug.Log("Put down " + transform.name);
        return gameObject;
    }
}

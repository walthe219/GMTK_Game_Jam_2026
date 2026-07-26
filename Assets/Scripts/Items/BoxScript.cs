using UnityEngine;
using System;

public class BoxScript : MonoBehaviour, ICarrayable
{
    public Rigidbody rb;

    bool isCarried = false;

    public event Action OnPickup;
    public event Action OnPutDown;

    public void TurnPhysicsOff()
    {
        rb.interpolation = RigidbodyInterpolation.None;
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.detectCollisions = false;
        rb.freezeRotation = true;
    }

    public void TurnPhysicsOn()
    {
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.isKinematic = false;
        rb.detectCollisions = true;
        rb.useGravity = true;
        rb.freezeRotation = false;
    }

    public GameObject Pickup()
    {
        isCarried = true;
        TurnPhysicsOff();
        OnPickup?.Invoke();
        Debug.Log("Picked up " + transform.name);
        return gameObject;
    }

    public GameObject PutDown()
    {
        isCarried = false;
        TurnPhysicsOn();
        OnPutDown?.Invoke();
        Debug.Log("Put down " + transform.name);
        return gameObject;
    }
}

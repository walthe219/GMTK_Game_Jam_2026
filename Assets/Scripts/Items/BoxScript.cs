using UnityEngine;
using System;

public class BoxScript : MonoBehaviour, ICarrayable
{
    public Rigidbody rb;

    bool isCarried = false;

    public event Action OnPickup;
    public event Action OnPutDown;

    [Header("Sounds")]
    public AudioSource audio;
    public AudioClip pickedUpSound;
    public AudioClip putDownSound;

    public void TurnPhysicsOff()
    {
        rb.interpolation = RigidbodyInterpolation.None;
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.detectCollisions = false;
        rb.freezeRotation = true;
        rb.Sleep();
    }

    public void TurnPhysicsOn()
    {
        rb.isKinematic = false;
        rb.detectCollisions = true;
        rb.useGravity = true;
        rb.freezeRotation = false;

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.WakeUp();
    }

    public GameObject Pickup()
    {
        isCarried = true;
        TurnPhysicsOff();
        OnPickup?.Invoke();
        audio.PlayOneShot(pickedUpSound);
        Debug.Log("Picked up " + transform.name);
        return gameObject;
    }

    public GameObject PutDown()
    {
        isCarried = false;
        TurnPhysicsOn();
        OnPutDown?.Invoke();
        audio.PlayOneShot(putDownSound);
        Debug.Log("Put down " + transform.name);
        return gameObject;
    }
}

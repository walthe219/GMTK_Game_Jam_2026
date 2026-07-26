using UnityEngine;
using System;

public class TimerScript : MonoBehaviour
{
    public float time;
    public Vector3 velocity;
    public Rigidbody rb;
    public CharacterController playeCC;


    void Update()
    {
        time += Time.deltaTime;
    }
}

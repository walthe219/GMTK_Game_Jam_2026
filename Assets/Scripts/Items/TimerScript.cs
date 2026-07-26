using System;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public float time;
    public Vector3 velocity;
    public Rigidbody rb;
    public CharacterController playeCC;
    public TextMeshProUGUI timerHUD;


    void Update()
    {
        time += Time.deltaTime;
        timerHUD.text = time.ToString("F2");
    }
}

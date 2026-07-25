using UnityEngine;
using TMPro;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public float timeScale = 1f;
    public float velocityScaler = 0.1f;
    public float velocityRatio = 1;

    public float minTimeScale;
    public float maxTimeScale;
    public float maxSpeed;

    public CharacterController playerCC;
    public Rigidbody timerRB;

    public TextMeshProUGUI timeHud;

    private void Start()
    {
        
    }

    private void Update()
    {
        velocityRatio = (1 + playerCC.velocity.magnitude * velocityScaler) / (1 + timerRB.linearVelocity.magnitude * velocityScaler);
        timeScale =  Mathf.Clamp(velocityRatio,minTimeScale,maxTimeScale);
        Time.timeScale = timeScale;
        timeHud.text = "Time Scale: " + timeScale.ToString("F1");

    }

    IEnumerator UpdateUI()
    {
        while (true)
        {
            //timeHud.text = "Time Scale: " + timeScale.ToString("F1");
            yield return new WaitForSecondsRealtime(1);
        }
    }
}

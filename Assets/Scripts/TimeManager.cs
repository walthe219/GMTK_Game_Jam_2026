using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float timeScale = 1f;
    public float velocityScaler = 0.1f;
    public float velocityRatio = 1;

    public float minTimeScale;
    public float maxTimeScale;
    public float maxSpeed;

    public Rigidbody playerRB;
    public Rigidbody timerRB;

    private void Update()
    {
        velocityRatio = (1 + playerRB.linearVelocity.magnitude * velocityScaler) / (1 + timerRB.linearVelocity.magnitude * velocityScaler);
        timeScale =  Mathf.Clamp(velocityRatio,minTimeScale,maxTimeScale);
        Time.timeScale = timeScale;
    }
}

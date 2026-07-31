using UnityEngine;
using TMPro;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public bool timeDilationActive = true;

    public float timeScale = 1f;
    public float playerVelocityScaler = 0.1f;
    public float timerVelocityScaler = 0.1f;
    public float velocityRatio = 1;

    public float minTimeScale;
    public float maxTimeScale;
    public float maxSpeed;

    public CharacterController playerCC;
    public Rigidbody timerRB;

    public TextMeshProUGUI timeHud;

    bool timerCarried;

    private void Start()
    {
        var boxScript = timerRB.gameObject.GetComponent<BoxScript>();
        boxScript.OnPickup += () => timerCarried = true;
        boxScript.OnPutDown += () => timerCarried = false;
    }

    private void Update()
    {
        if (timeDilationActive)
        {
            velocityRatio = !timerCarried ? (1 + playerCC.velocity.magnitude * playerVelocityScaler) / (1 + timerRB.linearVelocity.magnitude * timerVelocityScaler) : 1;
            timeScale =  Mathf.Clamp(velocityRatio,minTimeScale,maxTimeScale);
            Time.timeScale = timeScale;
            Time.fixedDeltaTime = 0.02f * timeScale;
            timeHud.text = "Time Scale: " + timeScale.ToString("F1");
        }

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

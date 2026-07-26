using UnityEngine;
using System;

public class TimerScript : MonoBehaviour
{
    public float startingTime;
    public float currentTime;
    public bool isActive = false;

    public static event Action OnTimerChange;


    void Update()
    {
        if (isActive)
        {
            OnTimerChange?.Invoke();
            currentTime -= Time.deltaTime;
            if (currentTime < 0)
                SceneLoader.Instance.ReloadScene();
        }
            
    }


    public void ToggleActive()
    {
        isActive = !isActive;
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }
}

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ButtonScript : MonoBehaviour, IInteractable
{
    public UnityEvent OnButtonPress;
    public UnityEvent OnButtonUnpressed;
    public float pressDist = 0.05f;
    public float pressedTime;

    public AudioClip buttonPress;
    public AudioClip clockTick;
    private AudioSource audio;
    

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    bool isPressed;
    public void Interact()
    {
        if (!isPressed)
        {
            isPressed = true;
            OnButtonPress.Invoke();
            StartCoroutine(Pressed(pressedTime));
        }
    }

    IEnumerator Pressed(float duration)
    {
        audio.PlayOneShot(buttonPress);
        var clockTicking = StartCoroutine(RepeatedClockTick());

        transform.position -= new Vector3(0f, pressDist, 0f);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        isPressed = false;
        OnButtonUnpressed.Invoke();
        transform.position += new Vector3(0f, pressDist, 0f);

        StopCoroutine(clockTicking);
        audio.PlayOneShot(buttonPress);
    }

    IEnumerator RepeatedClockTick( )
    {

        while (true)
        {
            yield return new WaitForSeconds(1f);
            //clockTick.pitch = Time.timeScale;
            audio.PlayOneShot(clockTick);
        }
    }
}

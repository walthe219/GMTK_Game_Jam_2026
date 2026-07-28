using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ButtonScript : MonoBehaviour, IInteractable
{
    public UnityEvent OnButtonPress;
    public UnityEvent OnButtonUnpressed;
    public float pressDist = 0.05f;
    public float pressedTime;

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
    }
}

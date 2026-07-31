using UnityEngine;
using UnityEngine.Events;

public class WeightedPlate : MonoBehaviour
{
    [Header("Settings")]
    public float requiredWeight = 1f;
    public float currentWeight = 0f;

    [Header("Visuals")]
    public Transform buttonCap;
    public Vector3 pressedOffset = new Vector3(0, -0.1f, 0);
    private Vector3 startPos;

    [Header("Sounds")]
    public AudioSource audio;
    public AudioClip activteSound;

    [Header("Events")]
    public UnityEvent onActivate;
    public UnityEvent onDeactivate;

    private bool isActivated = false;

    void Start()
    {
        if (buttonCap != null)
            startPos = buttonCap.localPosition;
    }

    void OnTriggerEnter(Collider other) => EvaluateWeight(other, true);
    void OnTriggerExit(Collider other) => EvaluateWeight(other, false);

    void EvaluateWeight(Collider other, bool isEntering)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            currentWeight += isEntering ? rb.mass : -rb.mass;
            CheckActivation();
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player on plate");
            currentWeight += isEntering ? 20 : -20;
            CheckActivation();
        }
    }

    void CheckActivation()
    {
        bool shouldActivate = currentWeight >= requiredWeight;

        if (shouldActivate != isActivated)
        {
            isActivated = shouldActivate;
            if (isActivated)
            {
                audio.PlayOneShot(activteSound);
                onActivate.Invoke();
            }
            else
            {
                audio.PlayOneShot(activteSound);
                onDeactivate.Invoke();
            }
        }

        // Move button visually
        if (buttonCap != null)
        {
            buttonCap.localPosition = isActivated ? startPos + pressedOffset : startPos;
        }
    }

    public void DebugActivate()
    {
        Debug.Log("button activated");
    }

    public void DebugDeactivate()
    {
        Debug.Log("button deactivated");
    }

}

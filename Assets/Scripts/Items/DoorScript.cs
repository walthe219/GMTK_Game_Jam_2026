using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;
    public float slidingDist = 3;
    public float slideTime = 1;

    private bool isOpened = false;
    private int activatorCount = 0;

    private Vector3 leftClosedPos;
    private Vector3 leftOpenedPos;
    private Vector3 rightClosedPos;
    private Vector3 rightOpenedPos;
    private Coroutine leftDoorCoroutine;
    private Coroutine rightDoorCoroutine;

    [Header("Sounds")]
    public AudioSource audio;
    public AudioClip doorOpenedSound;
    public AudioClip doorClosedSound;
    

    private void Start()
    {
        leftClosedPos = leftDoor.transform.position;
        leftOpenedPos = leftDoor.transform.position - transform.right * slidingDist;
        rightClosedPos = rightDoor.transform.position;
        rightOpenedPos = rightDoor.transform.position + transform.right * slidingDist;
    }


    [ContextMenu("OpenDoor()")]
    public void OpenDoor()
    {
        activatorCount++;

        if (activatorCount == 1)
        { 
            StopActiveCoroutines();
            isOpened = true;

            audio.PlayOneShot(doorOpenedSound);
            leftDoorCoroutine = StartCoroutine(slideOut(leftDoor, leftOpenedPos, slideTime));
            rightDoorCoroutine = StartCoroutine(slideOut(rightDoor, rightOpenedPos, slideTime));
        }
    }

    [ContextMenu("CloseDoor()")]
    public void CloseDoor()
    {
        if (activatorCount <= 0) return;

        activatorCount--;

        if(activatorCount == 0)
        {
            StopActiveCoroutines();
            isOpened = false;

            leftDoorCoroutine = StartCoroutine(slideOut(leftDoor, leftClosedPos, slideTime));
            rightDoorCoroutine = StartCoroutine(slideOut(rightDoor, rightClosedPos, slideTime));
            audio.PlayOneShot(doorClosedSound);
        }
    }

    IEnumerator slideOut(GameObject obj, Vector3 target, float time)
    {
        float elapsed = 0f;
        Vector3 start = obj.transform.position;

        while(elapsed < time)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / time;

            obj.transform.position = Vector3.Lerp(start, target, t);

            yield return null;
        }
        obj.transform.position = target;

    }

    private void StopActiveCoroutines()
    {
        if (leftDoorCoroutine != null) StopCoroutine(leftDoorCoroutine);
        if (rightDoorCoroutine != null) StopCoroutine(rightDoorCoroutine);
    }

}

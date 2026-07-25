using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;
    public float slidingDist = 3;
    public float slideTime = 1;

    bool isOpened = false;
    private Vector3 leftClosedPos;
    private Vector3 leftOpenedPos;
    private Vector3 rightClosedPos;
    private Vector3 rightOpenedPos;
    private Coroutine leftDoorCoroutine;
    private Coroutine rightDoorCoroutine;

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
        if (isOpened) return;

        StopActiveCoroutines();
        isOpened = true;

        leftDoorCoroutine = StartCoroutine(slideOut(leftDoor, leftOpenedPos, slideTime));
        rightDoorCoroutine = StartCoroutine(slideOut(rightDoor, rightOpenedPos, slideTime));
    }

    [ContextMenu("CloseDoor()")]
    public void CloseDoor()
    {
        if (!isOpened) return;

        StopActiveCoroutines();
        isOpened = false;

        leftDoorCoroutine = StartCoroutine(slideOut(leftDoor, leftClosedPos, slideTime));
        rightDoorCoroutine = StartCoroutine(slideOut(rightDoor, rightClosedPos, slideTime));
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

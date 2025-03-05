using System.Collections;
using UnityEngine;

public class ClockSegmentController : MonoBehaviour
{
    [SerializeField] private Transform[] segmentTransforms;

    private static readonly int[][] numberPattern = new int[][]
    {
        new int[] {1, 1, 1, 1, 1, 1, 0}, // 0
        new int[] {0, 1, 1, 0, 0, 0, 0}, // 1
        new int[] {1, 1, 0, 1, 1, 0, 1}, // 2
        new int[] {1, 1, 1, 1, 0, 0, 1}, // 3
        new int[] {0, 1, 1, 0, 0, 1, 1}, // 4
        new int[] {1, 0, 1, 1, 0, 1, 1}, // 5
        new int[] {1, 0, 1, 1, 1, 1, 1}, // 6
        new int[] {1, 1, 1, 0, 0, 0, 0}, // 7
        new int[] {1, 1, 1, 1, 1, 1, 1}, // 8
        new int[] {1, 1, 1, 1, 0, 1, 1}  // 9
    };

    private void Start()
    {
        InitializeSegmentRotations();
    }

    private void Update()
    {
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()) || Input.GetKeyDown((KeyCode)((int)KeyCode.Keypad0 + i)))
            {
                SetSegmentDisplay(i);
            }
        }
    }

    private void InitializeSegmentRotations()
    {
        for (int index = 0; index < segmentTransforms.Length; index++)
        {
            if (index == 0 || index == 3 || index == 6)
            {
                segmentTransforms[index].rotation = Quaternion.identity;
            }
            else
            {
                segmentTransforms[index].rotation = Quaternion.Euler(0, 0, 90);
            }
        }
    }

    public void SetSegmentDisplay(int number)
    {
        for (int i = 0; i < segmentTransforms.Length; i++)
        {
            bool isActive = numberPattern[number][i] == 1;
            Vector3 rotationAxis = (i == 0 || i == 3 || i == 6) ? Vector3.right : Vector3.forward;

            StartCoroutine(RotateSegmentCoroutine(segmentTransforms[i], isActive, rotationAxis));
        }
    }

    private IEnumerator RotateSegmentCoroutine(Transform segment, bool active, Vector3 rotationAxis)
    {
        Quaternion startRotation = segment.rotation;
        Quaternion endRotation = GetTargetRotation(rotationAxis, active);

        float elapsedTime = 0f;
        const float rotationDuration = 0.4f;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            segment.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / rotationDuration);
            yield return null;
        }
        segment.rotation = endRotation;
    }

    private Quaternion GetTargetRotation(Vector3 axis, bool isActive)
    {
        if (axis == Vector3.right)
        {
            return isActive ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(-90, 0, 0);
        }
        else
        {
            return isActive ? Quaternion.Euler(0, 0, 90) : Quaternion.Euler(0, 90, 90);
        }
    }
}

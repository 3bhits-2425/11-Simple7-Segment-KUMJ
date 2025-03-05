using UnityEngine;
using System.Collections;

public class SevenSegmentDisplay : MonoBehaviour
{
    [Header("Assign all 7 segments (A-G)")]
    public Transform[] segments; // Die 7 Segmente als Transforms

    [Header("Rotation settings")]
    public float rotationAngle = 90f; // Klappwinkel für inaktive Segmente
    public float rotationSpeed = 5f; // Geschwindigkeit der Drehung
    public Vector3 rotationAxis = Vector3.forward; // Ändere ggf. auf Vector3.up oder Vector3.right

    // Richtige Reihenfolge: [A, B, C, D, E, F, G]
    private readonly bool[,] digitPatterns = new bool[10, 7]
    {
        { true, true, true, true, true, true, false },  // 0
        { false, true, true, false, false, false, false },  // 1
        { true, true, false, true, true, false, true },  // 2
        { true, true, true, true, false, false, true },  // 3
        { false, true, true, false, false, true, true },  // 4
        { true, false, true, true, false, true, true },  // 5
        { true, false, true, true, true, true, true },  // 6
        { true, true, true, false, false, false, false },  // 7
        { true, true, true, true, true, true, true },  // 8
        { true, true, true, true, false, true, true }   // 9
    };

    private void Update()
    {
        // Zahlen 0-9 per Tastendruck setzen
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i)) // Drücke '0'-'9'
            {
                SetNumber(i);
            }
        }
    }

    public void SetNumber(int number)
    {
        if (segments.Length != 7)
        {
            Debug.LogError("Es müssen genau 7 Segmente zugewiesen sein!");
            return;
        }

        if (number < 0 || number > 9)
        {
            Debug.LogError("Die Zahl muss zwischen 0 und 9 liegen!");
            return;
        }

        Debug.Log($"Zahl gesetzt: {number}");

        for (int i = 0; i < segments.Length; i++)
        {
            bool shouldBeOn = digitPatterns[number, i];
            StartCoroutine(RotateSegment(segments[i], shouldBeOn));
        }
    }

    private IEnumerator RotateSegment(Transform segment, bool shouldBeOn)
    {
        Quaternion targetRotation = shouldBeOn
            ? Quaternion.identity
            : Quaternion.Euler(rotationAxis * rotationAngle);

        while (Quaternion.Angle(segment.localRotation, targetRotation) > 0.1f)
        {
            segment.localRotation = Quaternion.Lerp(segment.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        segment.localRotation = targetRotation;
    }
}
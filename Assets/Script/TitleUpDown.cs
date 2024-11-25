using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUpDown : MonoBehaviour
{
    public float floatSpeed = 2f;
    public Vector3 floatDirection = Vector3.up;
    public float floatRange = 5f;

    private RectTransform rectTransform;
    private Vector3 startPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.position;
    }

    void Update()
    {
        rectTransform.position += floatDirection * floatSpeed * Time.deltaTime;

        if (Vector3.Distance(rectTransform.position, startPos) > floatRange)
        {
            floatDirection = -floatDirection;
        }
    }
}

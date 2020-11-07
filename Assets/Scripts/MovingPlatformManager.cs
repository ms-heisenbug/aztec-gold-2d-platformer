using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformManager : MonoBehaviour
{
    [SerializeField] GameObject platform;
    Transform currentPoint;
    [SerializeField] Transform[] sidePoints;
    [SerializeField] int pointSelection;
    [SerializeField] float moveSpeed;

    private void Start()
    {
        currentPoint = sidePoints[pointSelection];
    }

    void Update()
    {
        platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentPoint.position, moveSpeed * Time.deltaTime);

        if(platform.transform.position == currentPoint.position)
        {
            pointSelection++;

            if (pointSelection == sidePoints.Length)
            {
                pointSelection = 0;
            }

            currentPoint = sidePoints[pointSelection];
        }
    }
}

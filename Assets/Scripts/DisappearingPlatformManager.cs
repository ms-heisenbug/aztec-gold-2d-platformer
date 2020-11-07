using System.Collections;
using UnityEngine;

public class DisappearingPlatformManager : MonoBehaviour
{
    [SerializeField] float timeToDisappear;
    BoxCollider2D collider;
    float transparencyThreshold;
    float frequency;
    SpriteRenderer spriteRenderer;
    [SerializeField] float idleTime;
    float startOffset;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        frequency = 0.05f;
        transparencyThreshold = 0.3f;
        startOffset = 1.0f;
    }

    private void Start()
    {
        StartCoroutine("StartStartOffset");
    }

    IEnumerator StartStartOffset()
    {
        yield return new WaitForSeconds(startOffset);
        yield return StartCoroutine("StartIdleBeforeDisappear");
    }


    IEnumerator StartToDisappear()
    {
        Color color = spriteRenderer.color;
        bool isColliderEnabled = true;

        for (float elapsedTime = 0f; elapsedTime < timeToDisappear; elapsedTime += frequency)
        {
            color.a = 1 - elapsedTime / timeToDisappear;
            spriteRenderer.color = color;

            if(color.a < transparencyThreshold && isColliderEnabled)
            {
                collider.enabled = false;
                isColliderEnabled = false;
            }

            yield return new WaitForSeconds(frequency);
        }

        color.a = 0.0f;
        spriteRenderer.color = color;

        yield return StartCoroutine("StartIdleBeforeAppear");
    }

    IEnumerator StartIdleBeforeDisappear()
    {
        for (float elapsedTime = 0f; elapsedTime < idleTime; elapsedTime += frequency)
        {
            yield return new WaitForSeconds(frequency);
        }

        yield return StartCoroutine("StartToDisappear");
    }

    IEnumerator StartIdleBeforeAppear()
    {
        for (float elapsedTime = 0f; elapsedTime < idleTime; elapsedTime += frequency)
        {
            yield return new WaitForSeconds(frequency);
        }

        yield return StartCoroutine("StartToAppear");
    }

    IEnumerator StartToAppear()
    {
        Color col = spriteRenderer.color;
        for (float elapsedTime = 0f; elapsedTime < timeToDisappear; elapsedTime += frequency)
        {
            col.a = elapsedTime / timeToDisappear;
            spriteRenderer.color = col;

            if (spriteRenderer.color.a < transparencyThreshold && !collider.enabled)
            {
                collider.enabled = true;
            }


            yield return new WaitForSeconds(frequency);
        }

        col.a = 1.0f;
        spriteRenderer.color = col;

        yield return StartCoroutine("StartIdleBeforeDisappear");
    }
}

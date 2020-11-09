using System.Collections;
using UnityEngine;

public class MovingLavaBulpManager : MonoBehaviour
{
    [SerializeField] GameObject lavaBulp;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endpoint;
    [SerializeField] float moveSpeed;

    void Update()
    {
        StartCoroutine(MoveBulp());
    }

    IEnumerator MoveBulp()
    {
        lavaBulp.transform.position = Vector3.MoveTowards(lavaBulp.transform.position, endpoint.position, moveSpeed * Time.deltaTime);

        if (lavaBulp.transform.position == endpoint.position)
        {
            yield return new WaitForSeconds(1f);

            lavaBulp.transform.position = startPoint.position;
        }
    }
}

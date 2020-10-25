using System;
using UnityEngine;

public class BgScroll : MonoBehaviour
{
    [SerializeField] GameObject[] backgroundElements;
    private Camera mainCamera;
    private Vector2 screenBounds;
    private float choke;
    private Vector3 lastScreenPosition;
    [SerializeField] Transform sunPosition;


    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        choke = 0.25f;
        lastScreenPosition = transform.position;
    }

    void Start()
    {
        foreach(GameObject obj in backgroundElements)
        {
            LoadChildObjects(obj);
        }
    }

    private void LoadChildObjects(GameObject obj)
    {
        float objWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x - choke;
        int childsNeeded = (int)Mathf.Ceil(screenBounds.x * 2 / objWidth);
        GameObject clone = Instantiate(obj) as GameObject;

        for (int i = 0; i <= childsNeeded; i++)
        {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(obj.transform);
            c.transform.position = new Vector3(objWidth * i, obj.transform.position.y, obj.transform.position.z);
            c.name = obj.name + i;
        }
        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }

    private void LateUpdate()
    {
        foreach(GameObject obj in backgroundElements)
        {
            TweakChildPosition(obj);
            float parallaxSpeed = 1 - Mathf.Clamp01(Mathf.Abs(transform.position.z / obj.transform.position.z));
            float difference = transform.position.x - lastScreenPosition.x;
            obj.transform.Translate(Vector3.right * parallaxSpeed * difference);
        }

        lastScreenPosition = transform.position;

        #region Blood moon position
        Vector3 middle = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height - 50, 100));
        float width = sunPosition.GetComponent<SpriteRenderer>().bounds.extents.x;
        float height = sunPosition.GetComponent<SpriteRenderer>().bounds.extents.y;
        sunPosition.position = new Vector3(middle.x + width / 2, middle.y - height / 2, middle.z);
        #endregion
    }

    private void TweakChildPosition(GameObject obj)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();

        if(children.Length > 1)
        {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            float halfWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x - choke;
            if(transform.position.x + screenBounds.x > lastChild.transform.position.x + halfWidth)
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfWidth * 2, lastChild.transform.position.y, lastChild.transform.position.z);
            }
            else if(transform.position.x - screenBounds.x < firstChild.transform.position.x - halfWidth)
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x - halfWidth * 2, firstChild.transform.position.y, firstChild.transform.position.z);
            }
        }
    }
}

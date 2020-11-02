using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int numberOfTotems;
    int orbsNeeded;
    int orbsFound;
    [SerializeField] Sprite[] totemSprites;
    GameObject totemImgs;

    private void Awake()
    {
        orbsNeeded = 2 * numberOfTotems;
        orbsFound = 0;
        totemImgs = GetTotemImgsGameObj();
    }

    private GameObject GetTotemImgsGameObj()
    {
        GameObject canvasGo = GameObject.Find("UICanvas");

        if(canvasGo != null)
        {
            Debug.Log("Canvas");
            return GameObject.Find("TotemsImgs");
        }
        return null;
    }

    void Start()
    {
    }

    void Update()
    {
        if(orbsFound == orbsNeeded)
        {
            GameObject.Find("Door").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("Door").transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void OrbFound()
    {
        orbsFound += 1;
        orbsNeeded -= 1;
        AddNewTotemImg();
    }

    void AddNewTotemImg()
    {
        Debug.Log("Orbs found = " + orbsFound);
        totemImgs.GetComponent<Image>().sprite = totemSprites[orbsFound];
    }
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int numberOfTotems;
    static int orbsNeeded;
    static int orbsFound;
    [SerializeField] Sprite[] totemSprites;
    GameObject totemImgs;

    float timer;

    //TODO: to nie powinno byc serializable
    //[SerializeField] TextMeshProUGUI timerText;

    [SerializeField] GameObject doorOpened;
    [SerializeField] GameObject doorClosed;

    private void Awake()
    {
        orbsNeeded = 2 * numberOfTotems;
        orbsFound = 0;
        totemImgs = GetTotemImgsGameObj();
        timer = 0;
        doorOpened.SetActive(false);
        doorClosed.SetActive(true);
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
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name.Equals("EndScreen"))
        {
            float t = timer;
            //timerText.text += t.ToString();
        }
        timer = 0;
    }

    void Update()
    {
        if(orbsFound == orbsNeeded)
        {
            doorClosed.SetActive(false);
            doorOpened.SetActive(true);
        }

        timer += Time.deltaTime;
    }

    public void OrbFound()
    {
        orbsFound += 1;
        AddNewTotemImg();
    }

    void AddNewTotemImg()
    {
        Debug.Log("Orbs found = " + orbsFound);
        totemImgs.GetComponent<Image>().sprite = totemSprites[orbsFound];
    }

    internal static void ShowEndScreen()
    {
        if (DoorOpened())
        {
            SceneManager.LoadScene("EndScreen");
        }
    }

    //TODO: zle liczy
    private static bool DoorOpened()
    {
        Debug.Log("orbs found: " + orbsFound);
        return orbsNeeded == orbsFound;
    }
}

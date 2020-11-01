using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbPickupManager : MonoBehaviour
{
    LevelManager lvlManager;

    private void Awake()
    {
        lvlManager = FindObjectOfType<LevelManager>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            lvlManager.OrbFound();
        }
    }
}

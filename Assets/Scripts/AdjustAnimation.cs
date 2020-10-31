using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustAnimation : MonoBehaviour
{
    Animator animator;
    [SerializeField] float animSpeed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        animator.speed = animSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

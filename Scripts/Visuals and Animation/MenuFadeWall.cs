﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFadeWall : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartGame()
    {
        animator.SetTrigger("StartGame");
    }
}

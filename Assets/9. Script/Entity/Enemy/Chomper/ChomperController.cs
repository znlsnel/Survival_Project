using System;
using UnityEngine;
using UnityEngine.AI;

public class ChomperController: MonoBehaviour
{
    private ChomperAnimationHandler _animation;
    private ChomperNavigationHandler _navigation;

    void Awake()
    {
        _animation = GetComponent<ChomperAnimationHandler>();
        _navigation = GetComponent<ChomperNavigationHandler>();
    }

    void FixedUpdate()
    {
        
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassState : MonoBehaviour, IClassState
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MainAttack();
        }
        if(Input.GetMouseButtonDown(1))
        {
            SecondAttack();
        }
    }

    public virtual void MainAttack()
    {
        throw new System.NotImplementedException();
    }

    public virtual void SecondAttack()
    {
        throw new System.NotImplementedException();
    }

    public virtual Type Transition(PlayerClass state)
    {
        throw new System.NotImplementedException();
    }
}

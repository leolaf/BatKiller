using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClassState
{
    public virtual void MainAttack()
    {
        throw new System.NotImplementedException();
    }
    public virtual void SecondAttack()
    {
        throw new System.NotImplementedException();
    }
    public virtual Type Transition(PlayerClass classState)
    {
        throw new System.NotImplementedException();
    }
}

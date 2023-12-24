using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakKnightState : ClassState
{
    public override void MainAttack()
    {
        Debug.Log($"Main attack of {GetType()}");
    }
    public override void SecondAttack()
    {
        Debug.Log($"Second attack of {GetType()}");
    }

    public override Type Transition(PlayerClass playerClass)
    {
        switch (playerClass)
        {
            case PlayerClass.KNIGHT:
                return typeof(StrongKnightState);
            case PlayerClass.MAGE:
                return typeof(WeakMageState);
            case PlayerClass.ROGUE:
                return typeof(WeakRogueState);
            case PlayerClass.WARRIOR:
                return typeof(WeakWarriorState);
        }
        Debug.LogError($"No transition found between {GetType()} and {playerClass}.");
        throw new NotImplementedException();
    }
}

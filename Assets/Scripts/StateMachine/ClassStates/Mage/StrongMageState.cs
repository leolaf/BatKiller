using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongMageState : ClassState
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
                return typeof(GodlikeMageState);
            case PlayerClass.ROGUE:
                return typeof(StrongRogueState);
            case PlayerClass.WARRIOR:
                return typeof(StrongWarriorState);
        }
        Debug.LogError($"No transition found between {GetType()} and {playerClass}.");
        throw new NotImplementedException();
    }
}

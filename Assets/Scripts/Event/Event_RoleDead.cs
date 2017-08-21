using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_RoleDead
{
    private bool isDead;
    public Event_RoleDead(bool _isDead)
    {
        IsDead = _isDead;
    }

    public bool IsDead
    {
        get
        {
            return isDead;
        }

        private set
        {
            isDead = value;
        }
    }
}

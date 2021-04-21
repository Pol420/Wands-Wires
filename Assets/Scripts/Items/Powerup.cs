using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : PickableObject
{
    [SerializeField] private PowerupType type = PowerupType.Invincible;

    protected override void OnPickup()
    {
        switch (type)
        {
            case PowerupType.Invincible: player.MakeInvincible(); break;
            case PowerupType.Infinite: player.MakeInfinite(); break;
            case PowerupType.Deadly: player.MakeDeadly(); break;
            case PowerupType.TEMPslowmo: player.MakeSlowmo(); break;
            default: break;
        }
    }
}
public enum PowerupType { Invincible, Infinite, Deadly, TEMPslowmo }
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
            case PowerupType.Invincible: playerPowers.MakeInvincible(); break;
            case PowerupType.Infinite: playerPowers.MakeInfinite(); break;
            case PowerupType.Deadly: playerPowers.MakeDeadly(); break;
            case PowerupType.TEMPslowmo: playerPowers.MakeSlowmo(); break;
            default: break;
        }
    }
}
public enum PowerupType { Invincible, Infinite, Deadly, TEMPslowmo }
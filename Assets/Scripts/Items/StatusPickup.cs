using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPickup : PickableObject
{
    [SerializeField] private PickupType type = PickupType.Health;
    [SerializeField] private PickupSize size = PickupSize.Medium;

    protected override void OnPickup()
    {
        switch (type)
        {
            case PickupType.Health:
                if (size == PickupSize.Small) player.AddHealth(20f);
                else if (size == PickupSize.Medium) player.AddHealth(50f);
                else player.AddHealth(200f);
                break;
            case PickupType.Shield:
                if (size == PickupSize.Small) player.AddShield(20f);
                else if (size == PickupSize.Medium) player.AddShield(50f);
                else player.AddShield(100f);
                break;
            case PickupType.FireAmmo:
                if (size == PickupSize.Small) player.AddFireAmmo(10);
                else if (size == PickupSize.Medium) player.AddFireAmmo(25);
                else player.AddFireAmmo(64);
                break;
            case PickupType.WaterAmmo:
                if (size == PickupSize.Small) player.AddWaterAmmo(10);
                else if (size == PickupSize.Medium) player.AddWaterAmmo(25);
                else player.AddWaterAmmo(64);
                break;
            case PickupType.TeslaAmmo:
                if (size == PickupSize.Small) player.AddTeslaAmmo(10);
                else if (size == PickupSize.Medium) player.AddTeslaAmmo(25);
                else player.AddTeslaAmmo(64);
                break;
            default: break;
        }
    }
}
public enum PickupType { Health, Shield, FireAmmo, WaterAmmo, TeslaAmmo }
public enum PickupSize { Small, Medium, Big }

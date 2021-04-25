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
                if (size == PickupSize.Small) playerStats.AddHealth(20f);
                else if (size == PickupSize.Medium) playerStats.AddHealth(50f);
                else playerStats.AddHealth(200f);
                break;
            case PickupType.Shield:
                if (size == PickupSize.Small) playerStats.AddShield(20f);
                else if (size == PickupSize.Medium) playerStats.AddShield(50f);
                else playerStats.AddShield(100f);
                break;
            case PickupType.FireAmmo:
                if (size == PickupSize.Small) playerStats.AddFireAmmo(10);
                else if (size == PickupSize.Medium) playerStats.AddFireAmmo(25);
                else playerStats.AddFireAmmo(64);
                break;
            case PickupType.WaterAmmo:
                if (size == PickupSize.Small) playerStats.AddWaterAmmo(10);
                else if (size == PickupSize.Medium) playerStats.AddWaterAmmo(25);
                else playerStats.AddWaterAmmo(64);
                break;
            case PickupType.TeslaAmmo:
                if (size == PickupSize.Small) playerStats.AddTeslaAmmo(10);
                else if (size == PickupSize.Medium) playerStats.AddTeslaAmmo(25);
                else playerStats.AddTeslaAmmo(64);
                break;
            default: break;
        }
    }
}
public enum PickupType { Health, Shield, FireAmmo, WaterAmmo, TeslaAmmo }
public enum PickupSize { Small, Medium, Big }

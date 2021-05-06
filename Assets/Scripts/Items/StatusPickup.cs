using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPickup : PickableObject
{
    [SerializeField] private PickupType type = PickupType.Health;
    [SerializeField] private PickupSize size = PickupSize.Medium;

    private static GameObject statusSample;
    protected override void OnStart() { if (statusSample == null) CreateSample(); }

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

    public static GameObject RandomStatus(Vector3 pos)
    {
        GameObject status = Instantiate(statusSample);
        status.transform.position = pos;
        StatusPickup stat = status.GetComponent<StatusPickup>();
        stat.Activate(true);
        stat.SetType(Random.Range(0, 2));
        stat.SetSize(Random.Range(0, 3));
        return status;
    }
    public static GameObject DropAmmo(Vector3 pos, Ammo ammoType)
    {
        GameObject ammo = Instantiate(statusSample);
        ammo.transform.position = pos;
        StatusPickup stat = ammo.GetComponent<StatusPickup>();
        stat.Activate(true);
        stat.SetType(ammoType == Ammo.Fire? 2:(ammoType == Ammo.Water? 3:4));
        stat.SetSize(Random.Range(0, 3));
        return ammo;
    }

    public void SetType(int typeIndex)
    {
        switch (typeIndex)
        {
            case 0: type = PickupType.Health; break;
            case 1: type = PickupType.Shield; break;
            case 2: type = PickupType.FireAmmo; break;
            case 3: type = PickupType.WaterAmmo; break;
            case 4: type = PickupType.TeslaAmmo; break;
            default: break;
        }
    }
    public void SetSize(int sizeIndex)
    {
        switch (sizeIndex)
        {
            case 0: size = PickupSize.Small; break;
            case 1: size = PickupSize.Medium; break;
            case 2: size = PickupSize.Big; break;
            default: break;
        }
    }
    private void CreateSample()
    {
        statusSample = Instantiate(gameObject);
        statusSample.transform.position = new Vector3(0f, -256f, 0f);
        statusSample.GetComponent<StatusPickup>().Activate(false);
    }
}
public enum PickupType { Health, Shield, FireAmmo, WaterAmmo, TeslaAmmo }
public enum PickupSize { Small, Medium, Big }

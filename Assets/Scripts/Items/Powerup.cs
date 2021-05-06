using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : PickableObject
{
    [SerializeField] private PowerupType type = PowerupType.Invincible;
    private static GameObject powerupSample;
    protected override void OnStart() { if (powerupSample == null) CreateSample(); }

    protected override void OnPickup()
    {
        switch (type)
        {
            case PowerupType.Invincible: playerPowers.MakeInvincible(); break;
            case PowerupType.Infinite: playerPowers.MakeInfinite(); break;
            case PowerupType.Deadly: playerPowers.MakeDeadly(); break;
            default: break;
        }
    }

    public static GameObject RandomPowerup(Vector3 pos)
    {
        GameObject powerup = Instantiate(powerupSample);
        powerup.transform.position = pos;
        Powerup pu = powerup.GetComponent<Powerup>();
        pu.Activate(true);
        pu.SetType(Random.Range(0, 3));
        return powerup;
    }

    private void CreateSample()
    {
        powerupSample = Instantiate(gameObject);
        powerupSample.transform.position = new Vector3(0f, -256f, 0f);
        powerupSample.GetComponent<Powerup>().Activate(false);
    }

    public void SetType(int typeIndex)
    {
        switch (typeIndex)
        {
            case 0: type = PowerupType.Invincible; break;
            case 1: type = PowerupType.Infinite; break;
            case 2: type = PowerupType.Deadly; break;
            default: break;
        }
    }
}
public enum PowerupType { Invincible, Infinite, Deadly }
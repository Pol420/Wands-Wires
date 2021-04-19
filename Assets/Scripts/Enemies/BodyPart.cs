using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] [Range(0f, 3f)] private float damageMultiplier = 1f;
    private GameObject floatyText;
    private Enemy body;
    private Ammo enemyType;

    public void Connect(Enemy parentBody, GameObject floatyText, Ammo type)
    {
        body = parentBody;
        this.floatyText = floatyText;
        enemyType = type;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("PlayerProjectile"))
        {
            Vector3 point = collision.contacts[0].point;
            Projectile projectile = other.GetComponent<Projectile>();
            Hurt(point, (point - transform.position).normalized, Mathf.RoundToInt(projectile.damage * damageMultiplier * GetTypeMultiplier(projectile.type)), projectile.type);
        }
    }

    private float GetTypeMultiplier(Ammo projectileType)
    {
        if (enemyType == projectileType) return 1f;
        switch (enemyType)
        {
            case Ammo.Fire: return (projectileType == Ammo.Water) ? 1.25f : 0.75f;
            case Ammo.Water: return (projectileType == Ammo.Tesla) ? 1.25f : 0.75f;
            case Ammo.Tesla: return (projectileType == Ammo.Fire) ? 1.25f : 0.75f;
            default: return 1f;
        }
    }

    private void Hurt(Vector3 position, Vector3 direction, int damage, Ammo ammoType)
    {
        body.Hurt(damage);
        Instantiate(floatyText).GetComponent<FloatyText>().Init(position, direction, damage, ammoType);
    }

}

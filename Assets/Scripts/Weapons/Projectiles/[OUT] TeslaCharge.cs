/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaCharge : Projectile
{
    private static List<Transform> charges;
    private List<Transform> neighbours;
    
    private float sparkChance;
    private float ttl;

    private bool stuck;
    private GameObject sparkPrefab;
    private GameObject spark;

    public void Shoot(Vector3 position, Vector3 direction, float chance, float life, GameObject spark)
    {
        if (charges == null) charges = new List<Transform>();
        charges.Add(transform);
        body = GetComponent<Rigidbody>();
        neighbours = new List<Transform>();
        sparkPrefab = spark;
        stuck = false;
        sparkChance = chance;
        ttl = life;
        transform.forward = direction;
        transform.position = position;
        body.SetDensity(10f);
        body.AddForce(direction * 100f * Time.deltaTime, ForceMode.Impulse);
    }
    
    private void FindNeighbours()
    {
        neighbours = new List<Transform>();
        foreach (Transform t in charges) if (t != null && !neighbours.Contains(t) && Vector3.Distance(t.position, transform.position) <= 25f) neighbours.Add(t);
    }

    void Update()
    {
        if (stuck)
        {
            if (ttl <= 0) RemoveCharge();
            else
            {
                ttl -= Time.deltaTime;
                if (Random.Range(0f, 100f) <= sparkChance * 100f * Time.deltaTime) Spark();
            }
        }
    }

    private void Spark()
    {
        FindNeighbours();
        if(neighbours.Count > 0)
        {
            Transform otherEnd = neighbours[Random.Range(0, neighbours.Count)];
            ClearSpark();
            spark = Instantiate(sparkPrefab, transform);
            spark.transform.localScale = 2 * Vector3.Distance(transform.position, otherEnd.position) * Vector3.forward + new Vector3(0.2f, 0.2f);
            spark.transform.LookAt(otherEnd, Vector3.right);
            Invoke("ClearSpark", Time.deltaTime * 30f);
            Collider[] hits = Physics.OverlapCapsule(transform.position, otherEnd.position, 0.1f); foreach (Collider c in hits)
            {
                BodyPart part = c.gameObject.GetComponent<BodyPart>();
                if (part != null) part.Hurt(c.ClosestPoint(transform.position), damage, type);
            }
        }
    }

    private void ClearSpark()
    {
        if (spark != null) Destroy(spark.gameObject);
    }

    private void RemoveCharge()
    {
        charges.Remove(transform);
        Destroy(gameObject);
    }

    protected override void OnCollision(Collision collision)
    {
        stuck = true;
        GetComponent<Collider>().enabled = false;
        body.isKinematic = true;
        transform.SetParent(collision.transform);
    }

    protected override void OnDrag(Collision collision) {}
}
*/
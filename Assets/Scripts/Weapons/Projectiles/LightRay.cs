using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LightRay : MonoBehaviour
{

    private LineRenderer lr;

    void Awake() { lr = GetComponent<LineRenderer>(); }
    
    public void SetRay(Vector3 pos, Vector3 dir, float length)
    {
        Vector3[] points = { pos, pos + dir * length };
        lr.SetPositions(points);
    }

    public void SetGirth(float girth)
    {
        lr.startWidth = girth;
        lr.endWidth = girth;
    }

    public void SetMaterial(Material material) { lr.material = material; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatyText : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float highDamageThreshold = 20f;
    private float ttl;

    private Text text;
    private Vector3 direction;
    private Transform view;

    public void Init(Vector3 position, Vector3 direction, int damage, Ammo ammoType)
    {
        ttl = duration;
        text = GetComponentInChildren<Text>();
        view = Camera.main.transform;
        transform.position = position;
        this.direction = direction;
        text.text = damage + "";
        Color color = Color.red;
        if (ammoType == Ammo.Water) color = Color.cyan;
        else if (ammoType == Ammo.Tesla) color = Color.yellow;
        float threshold = Mathf.Min(damage / highDamageThreshold, 1f);
        text.color = new Color(color.r * threshold, color.g * threshold, color.b * threshold);
        text.fontSize = (int) (text.fontSize * (0.5f + threshold));
    }

    private float ColorValue(float amount)
    {
        return Mathf.Max(255f * (1 - amount), 0f);
    }
    
    void Update()
    {
        if (ttl <= 0f) Destroy(gameObject);
        else
        {
            ttl-= Time.deltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, ttl / duration);
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.forward = view.forward;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAltar : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private GameObject weaponModel = null;
    [SerializeField] private WeaponType weaponType = WeaponType.Staff;

    [Header("Trap Settings")]
    [SerializeField] private List<GameObject> trapDoors = new List<GameObject>();
    [SerializeField] private List<GameObject> hiddenEnemies = new List<GameObject>();

    [Header("Other Settings")]
    [SerializeField] [Range(0f, 10f)] private float elevation = 1f;
    [SerializeField] [Range(0f, 10f)] private float hoverSpeed = 1f;
    [SerializeField] [Range(0f, 1f)] private float hoverAmount = 0.5f;
    [SerializeField] [Range(0f, 10f)] private float rotateSpeed = 1f;

    private Transform model;
    private bool inRange;
    private bool spent;

    void Start()
    {
        inRange = false;
        spent = false;
        model = Instantiate(weaponModel, transform).transform;
        model.localScale *= 2f;
        model.position = transform.position + Vector3.up * elevation;
        model.localEulerAngles -= transform.localEulerAngles;
        ActivateDoors(false);
        ShowEnemies(false);
    }

    void Update()
    {
        if (!spent)
        {
            model.RotateAround(model.position, Vector3.up, rotateSpeed);
            model.position = transform.position + Vector3.up * (elevation + Mathf.Sin(Time.time * hoverSpeed) * hoverAmount);
            if (inRange)
            {
                if (Input.GetButton("Fire2"))
                {
                    GetWeapon();
                    ShowEnemies(true);
                    ActivateDoors(true);
                }
            }
        }
    }

    private void ActivateDoors(bool active) { foreach (GameObject door in trapDoors) door.SetActive(active); }
    private void ShowEnemies(bool show) { foreach (GameObject enemy in hiddenEnemies) enemy.SetActive(show); }
    private void GetWeapon()
    {
        spent = true;
        PlayerStats.Instance().UnlockWeapon(weaponType);
        Destroy(model.gameObject);
    }

    private void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) inRange = true; }
    private void OnTriggerExit(Collider other) { if (other.CompareTag("Player")) inRange = false; }
}
public enum WeaponType { Wand, Staff, Grimoir, Prism };
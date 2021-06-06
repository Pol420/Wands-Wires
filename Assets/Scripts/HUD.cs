using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("Ammo")]
    [SerializeField] private Text fireText = null;
    [SerializeField] private Text waterText = null;
    [SerializeField] private Text teslaText = null;
    [SerializeField] private Image selector = null;

    [Header("Status")]
    [SerializeField] private Image healthBar = null;
    [SerializeField] private Image shieldBar = null;

    [Header("Powers & Powerups")]
    [SerializeField] private Image slowCharge = null;
    [SerializeField] private Image secondSlowCharge = null;
    [SerializeField] private Text slowIcon = null;
    [SerializeField] private Image slowmo = null;
    [SerializeField] private Image invincible = null;
    [SerializeField] private Image infinite = null;
    [SerializeField] private Image deadly = null;

    [Header("Items")]
    [SerializeField] private Transform KeyItems = null;
    [SerializeField] [Range(0f, 200f)] private float itemSpacing = 50f;
    private List<GameObject> items;

    private void Start()
    {
        items = new List<GameObject>();
    }

    public void ResetHud() { items = new List<GameObject>(); }

    public void SetFire(int amount) { fireText.text = amount + ""; }
    public void SetWater(int amount) { waterText.text = amount + ""; }
    public void SetTesla(int amount) { teslaText.text = amount + ""; }
    public void SetSelector(float rate) { selector.fillAmount = rate; }
    public void MoveSelector(int pos)
    {
        if (pos == 0) selector.transform.position = fireText.transform.position;
        else if (pos == 1) selector.transform.position = waterText.transform.position;
        else selector.transform.position = teslaText.transform.position;
    }

    public void SetHealth(float proportion) { healthBar.fillAmount = proportion; }
    public void SetShield(float proportion) { shieldBar.fillAmount = proportion; }

    public void SetSlowmo(float proportion, bool active) { slowmo.fillAmount = proportion; slowmo.gameObject.SetActive(active); }
    public void SetInvincible(float proportion, bool active) { invincible.fillAmount = proportion; invincible.gameObject.SetActive(active); }
    public void SetInfinite(float proportion, bool active) { infinite.fillAmount = proportion; infinite.gameObject.SetActive(active); }
    public void SetDeadly(float proportion, bool active) { deadly.fillAmount = proportion; deadly.gameObject.SetActive(active); }
    public void AddKeyItem(GameObject item)
    {
        items.Add(Instantiate(item));
        items[items.Count - 1].transform.localScale *= KeyItems.lossyScale.magnitude * 20f;
        SortItems();
    }
    public void RemoveKeyItem(string code)
    {
        GameObject toRemove = null;
        for (int i = 0; toRemove == null && i < items.Count; ++i) if (items[i].GetComponent<KeyItem>().GetCode().Equals(code)) toRemove = items[i];
        items.Remove(toRemove);
        Destroy(toRemove.gameObject);
        SortItems();
    }
    private void SortItems()
    {
        for (int i = 0; i < items.Count; ++i) items[i].transform.position = KeyItems.position
                + Vector3.right * KeyItems.lossyScale.magnitude * itemSpacing * (i - (items.Count - 1f) / 2f);
    }

    public void SetSlowCharge(float proportion)
    {
        if(proportion <= 1f)
        {
            slowCharge.fillAmount = proportion;
            secondSlowCharge.fillAmount = 0f;
        }
        else
        {
            secondSlowCharge.fillAmount = proportion - 1f;
            slowCharge.fillAmount = 1f;
        }
        slowIcon.CrossFadeAlpha(proportion, Time.deltaTime * 20f, true);
    }
}

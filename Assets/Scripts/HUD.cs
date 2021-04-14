using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Text fireText = null;
    [SerializeField] private Text waterText = null;
    [SerializeField] private Text teslaText = null;
    [SerializeField] private Image selector = null;
    
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
}

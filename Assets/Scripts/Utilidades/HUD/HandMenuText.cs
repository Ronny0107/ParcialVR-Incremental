using TMPro;
using UnityEngine;

public class HandMenuText : MonoBehaviour
{
    [SerializeField] TMP_Text Dinero;
    void Update()
    {
        string texto = GameManager.Instance.Stats.Money.ToString();
        Dinero.text = texto;
        
    }
}

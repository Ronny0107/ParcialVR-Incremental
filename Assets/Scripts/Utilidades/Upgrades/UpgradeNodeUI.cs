using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Solo muestra el estado de un nodo (texto y color). No maneja clics.
// Un boton aparte llama a Buy() desde su OnClick.
public class UpgradeNodeUI : MonoBehaviour
{
    public Upgrades Data;
    public TMP_Text Label;
    public Image Background;
    public Color Locked = new Color(0.3f, 0.3f, 0.3f);
    public Color Available = Color.white;
    public Color Bought = new Color(0.3f, 1f, 0.4f);

    UpgradeManager subscribedTo;

    void OnEnable()
    {
        var m = UpgradeManager.Instance;
        if (m == null) return;

        if (subscribedTo != m)
        {
            m.OnUpgradePurchased += OnChanged;
            m.OnUpgradesReset += Refresh;
            subscribedTo = m;
        }
        Refresh();
    }

    void Start() { OnEnable(); }

    void OnDisable()
    {
        if (subscribedTo == null) return;
        subscribedTo.OnUpgradePurchased -= OnChanged;
        subscribedTo.OnUpgradesReset -= Refresh;
        subscribedTo = null;
    }

    // Se enlaza al OnClick de un boton aparte.
    public void Buy()
    {
        if (UpgradeManager.Instance != null && Data != null)
            UpgradeManager.Instance.BuyUpgrade(Data);
    }

    void OnChanged(Upgrades _) { Refresh(); }

    public void Refresh()
    {
        var m = UpgradeManager.Instance;
        if (m == null || Data == null) return;

        bool maxed = m.IsMaxed(Data);

        if (Label != null)
            Label.text = maxed
                ? Data.DisplayName + "\nMAX"
                : Data.DisplayName + "\n" + m.GetLevel(Data) + "/" + Data.MaxLevel + "  -  " + m.GetCost(Data);

        if (Background != null)
            Background.color = maxed ? Bought : (m.IsUnlocked(Data) ? Available : Locked);
    }
}

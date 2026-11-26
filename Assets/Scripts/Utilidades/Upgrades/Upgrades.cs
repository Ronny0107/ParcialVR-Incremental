using UnityEngine;

public enum UpgradeType
{
    PlayerNewTool,
    PlayerMovement,
    ConveyorSpawnRate,

}

[CreateAssetMenu(menuName = "Upgrades/Upgrade")]
public class Upgrades : ScriptableObject
{
    [System.Serializable]
    public class Requirement
    {
        public Upgrades Node;
        public int MinLevel = 1;
    }

    public UpgradeType Type;
    public string DisplayName;
    [TextArea] public string Description;
    public int BaseCost = 100;
    public float CostMultiplier = 1.5f;   // cada nivel cuesta más
    public float Value = 1f;              // cantidad que aplica por nivel
    public int MaxLevel = 1;              // 1 = compra única

    [Tooltip("Nodos que deben estar comprados (al nivel indicado) antes de poder comprar este.")]
    public Requirement[] Requires = new Requirement[0];

    public int GetCost(int currentLevel)
    {
        return Mathf.RoundToInt(BaseCost * Mathf.Pow(CostMultiplier, currentLevel));
    }
}

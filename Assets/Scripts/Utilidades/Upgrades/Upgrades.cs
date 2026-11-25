using UnityEngine;

public enum UpgradeType
{
    PlayerNewTool,
    PlayerMovement,
    PlayerRange,
    ToolFixSpeed,
    ConveyorSpawnRate,
    ConveyorSpeed
}

[CreateAssetMenu(menuName = "Upgrades/Upgrade")]
public class Upgrades : ScriptableObject
{
    public UpgradeType Type;
    public string DisplayName;
    public int BaseCost = 100;
    public float CostMultiplier = 1.5f;   // cada nivel cuesta más
    public float Value = 1f;              // cantidad que aplica por nivel
    public int MaxLevel = 1;              // 1 = compra única

    public int GetCost(int currentLevel)
    {
        return Mathf.RoundToInt(BaseCost * Mathf.Pow(CostMultiplier, currentLevel));
    }
}
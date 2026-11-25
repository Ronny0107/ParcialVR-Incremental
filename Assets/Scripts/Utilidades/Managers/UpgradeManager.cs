using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance {get; private set;}
    PlayerStatRev psr;
    readonly Dictionary<UpgradeType, int> levels = new();

    public event Action<Upgrades> OnUpgradePurchased;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

public int GetLevel(Upgrades data)
    {
        return levels.TryGetValue(data.Type, out int lvl) ? lvl : 0;
    }

    public bool IsMaxed(Upgrades data)
    {
        return GetLevel(data) >= data.MaxLevel;
    }

    public int GetCost(Upgrades data)
    {
        return data.GetCost(GetLevel(data));
    }

    public bool CanBuy(Upgrades data)
    {
        return !IsMaxed(data) && psr.Money >= GetCost(data);
    }

    public void BuyUpgrade(Upgrades data)
    {
        if (!CanBuy(data)) return;

        psr.Money -= GetCost(data);
        levels[data.Type] = GetLevel(data) + 1;

        ApplyUpgrade(data);
        OnUpgradePurchased?.Invoke(data);
    }

    void ApplyUpgrade(Upgrades data)
    {
        switch (data.Type)
        {
            case UpgradeType.PlayerNewTool:
                // desbloquear herramienta
                break;
            case UpgradeType.PlayerMovement:
                // psr.MoveSpeed += data.Value;
                break;
            case UpgradeType.PlayerRange:
                // psr.Range += data.Value;
                break;
            case UpgradeType.ToolFixSpeed:
                // psr.FixSpeed += data.Value;
                break;
            case UpgradeType.ConveyorSpawnRate:
                // spawner.Rate += data.Value;
                break;
            case UpgradeType.ConveyorSpeed:
                // conveyor.Speed += data.Value;
                break;
        }
    }
}

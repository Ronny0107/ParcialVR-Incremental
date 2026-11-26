using System;
using System.Collections.Generic;
using UnityEngine;


public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance {get; private set;}

    // Nivel por nodo (el asset es la clave), así dos nodos del mismo tipo no se pisan.
    readonly Dictionary<Upgrades, int> levels = new();

    public event Action<Upgrades> OnUpgradePurchased;
    public event Action OnUpgradesReset;

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
        return data != null && levels.TryGetValue(data, out int lvl) ? lvl : 0;
    }

    // Útil para habilitar cosas de compra única (p. ej. una herramienta nueva).
    public bool Has(Upgrades data)
    {
        return GetLevel(data) > 0;
    }

    public bool IsMaxed(Upgrades data)
    {
        return GetLevel(data) >= data.MaxLevel;
    }

    public int GetCost(Upgrades data)
    {
        return data.GetCost(GetLevel(data));
    }

    // Un nodo está desbloqueado cuando todos sus prerrequisitos están al nivel mínimo.
    public bool IsUnlocked(Upgrades data)
    {
        if (data.Requires == null) return true;

        foreach (var req in data.Requires)
        {
            if (req == null || req.Node == null) continue;
            if (GetLevel(req.Node) < Mathf.Max(1, req.MinLevel)) return false;
        }
        return true;
    }

    public bool CanBuy(Upgrades data)
    {
        return IsUnlocked(data)
            && !IsMaxed(data)
            && GameManager.Instance.Stats.Money >= GetCost(data);
    }

    public void BuyUpgrade(Upgrades data)
    {
        if (!CanBuy(data)) return;

        GameManager.Instance.Pay(GetCost(data));
        levels[data] = GetLevel(data) + 1;

        ApplyUpgrade(data);
        OnUpgradePurchased?.Invoke(data);
    }

    // Bono total de un tipo: suma de (nivel x Value) de todos los nodos de ese tipo.
    // Cada sistema consulta esto en vez de que el manager modifique sus valores.
    public float GetBonus(UpgradeType type)
    {
        float total = 0f;
        foreach (var kv in levels)
            if (kv.Key.Type == type)
                total += kv.Key.Value * kv.Value;
        return total;
    }

    public void ResetAll()
    {
        levels.Clear();
        GameManager.Instance.RefreshPlayerSpeed();
        GameManager.Instance.RefreshSpawnTime();
        OnUpgradesReset?.Invoke();
    }

    void ApplyUpgrade(Upgrades data)
    {
        // Los efectos numericos no se acumulan aqui: cada sistema lee GetBonus(tipo).
        // Aqui solo se refrescan los sistemas que necesitan que les empujen el valor.
        switch (data.Type)
        {
            case UpgradeType.PlayerMovement:
                GameManager.Instance.RefreshPlayerSpeed();
                break;

            case UpgradeType.ConveyorSpawnRate:
                GameManager.Instance.RefreshSpawnTime();
                break;

            case UpgradeType.NewObject:
            {
                var spawner = FindFirstObjectByType<ElementSpawner>();
                if (spawner != null) spawner.Desbloquear(data.Unlocks);
                break;
            }
        }
    }
}

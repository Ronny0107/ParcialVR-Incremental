using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public PlayerStatRev Stats {get; private set;} = new PlayerStatRev();
    ContinuousMoveProvider moveProvider;
    float baseMoveSpeed;
    float baseSpawnTime;
    const float MinSpawnTime = 1f;
    public bool DayEnded { get; private set; } = true;
    public event Action OnDayStarted;
    public event Action OnDayEnded;
    void Awake()
    {
        if(Instance == null){
            Instance = this;
        }
    }

    void Start()
    {
        // si no la arrastras en el Inspector, la busca sola
        if (moveProvider == null)
            moveProvider = FindFirstObjectByType<ContinuousMoveProvider>();

        if (moveProvider != null)
            baseMoveSpeed = moveProvider.moveSpeed;

        RefreshPlayerSpeed();
    }

    public void SetDayState(bool ended)
    {
        if (DayEnded == ended) return;
        DayEnded = ended;
        if (ended) OnDayEnded?.Invoke(); else OnDayStarted?.Invoke();
    }

    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void AddFix()
    {
        Stats.Fixed++;
    }

    public void AddDamage()
    {
        Stats.Damaged++;
    }
    public void AddMoney(int value)
    {
        Stats.Money += value;
    }

    public void Pay(int price)
    {
        Stats.Money -= price;
    }

    public void TotalMoney(int money)
    {
        Stats.AllMoney += money;
    }

    public void RefreshPlayerSpeed()
    {
        if (moveProvider == null) return;

        float bonus = UpgradeManager.Instance != null
            ? UpgradeManager.Instance.GetBonus(UpgradeType.PlayerMovement)
            : 0f;

        moveProvider.moveSpeed = baseMoveSpeed + bonus;
        Stats.movementSpeed = moveProvider.moveSpeed;
    }

    // Igual que la velocidad: el valor base se queda fijo y las mejoras RESTAN tiempo.
    public void RefreshSpawnTime()
    {
        if (baseSpawnTime <= 0f) baseSpawnTime = Stats.SpawnTime;

        float bonus = UpgradeManager.Instance != null
            ? UpgradeManager.Instance.GetBonus(UpgradeType.ConveyorSpawnRate)
            : 0f;

        Stats.SpawnTime = Mathf.Max(MinSpawnTime, baseSpawnTime - bonus);
    }
}

public class PlayerStatRev
{
    public int Fixed;
    public int Damaged;
    public int Money;
    public int AllMoney;
    public float movementSpeed = 1f;
    public float SpawnTime = 45f;
    public float DayTime = 100f;
    public float profit = 0f;
}



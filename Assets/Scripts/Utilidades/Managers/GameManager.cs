using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public PlayerStatRev Stats {get; private set;} = new PlayerStatRev();

    void Awake()
    {
        if(Instance == null){
            Instance = this;
        }
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
}

public class PlayerStatRev
{
    public int Fixed;
    public int Damaged;
    public int Money;
    public int AllMoney;
}

public class PlayerInitStats
{
    public float moveSpeed = 1f;
    public float range;
}

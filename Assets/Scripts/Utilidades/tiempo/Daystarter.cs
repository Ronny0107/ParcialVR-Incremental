using UnityEngine;

public class Daystarter : MonoBehaviour
{
    [SerializeField] ElementSpawner es;

    float dayLeft, spawnLeft;
    bool running;

    // Se llama desde un boton (OnClick)
    public void StartDay()
    {
        if (running) return;

        dayLeft = GameManager.Instance.Stats.DayTime;
        spawnLeft = 0f;
        running = true;
        GameManager.Instance.SetDayState(false);
    }

    void Update()
    {
        if (!running) return;

        dayLeft -= Time.deltaTime;
        spawnLeft -= Time.deltaTime;

        if (spawnLeft <= 0f)
        {
            es.SpawnObj();
            spawnLeft = GameManager.Instance.Stats.SpawnTime;
        }

        if (dayLeft <= 0f)
        {
            running = false;
            GameManager.Instance.SetDayState(true);
        }
    }
}

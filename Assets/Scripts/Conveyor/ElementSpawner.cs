using UnityEngine;

public class ElementSpawner : MonoBehaviour
{
    [SerializeField] Transform SpawnScrew;
    [SerializeField] Transform SpawnObjects;
    public int SelectElement = 0;
    [Tooltip("Conjunto de elementos spawneables")]
    [SerializeField] GameObject[] Spawnables;
    [SerializeField] GameObject [] Objetosdispo;
    
    public void Spawnscrew()
    {
        //no hay nada :O
        if(Spawnables == null) return;

        //hay algo y escogiste algo :O
        Instantiate(Spawnables[0], SpawnScrew.position, SpawnScrew.rotation);
    }

    public void SpawnObj()
    {
        if(Spawnables == null) return;

        int i = Random.Range(0, Spawnables.Length);
        Instantiate(Spawnables[i], SpawnObjects.position,SpawnObjects.rotation);
    }
}

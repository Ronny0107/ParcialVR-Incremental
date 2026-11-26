using System.Collections.Generic;
using UnityEngine;

public class ElementSpawner : MonoBehaviour
{
    [SerializeField] Transform SpawnScrew;
    [SerializeField] Transform SpawnObjects;
    public int SelectElement = 0;
    [Tooltip("Conjunto de elementos spawneables")]
    [SerializeField] GameObject[] Spawnables;
    [SerializeField] List<GameObject> Objetosdispo;

    public void Spawnscrew()
    {
        //no hay nada :O
        if(Spawnables == null) return;

        //hay algo y escogiste algo :O
        Instantiate(Spawnables[0], SpawnScrew.position, SpawnScrew.rotation);
    }

    // Lo llama el arbol de mejoras al comprar un nodo NewObject
    public void Desbloquear(GameObject prefab)
    {
        if (prefab == null) return;
        if (Objetosdispo == null) Objetosdispo = new List<GameObject>();
        if (!Objetosdispo.Contains(prefab)) Objetosdispo.Add(prefab);
    }

    public void SpawnObj()
    {
        if(Objetosdispo == null) return;

        int i = Random.Range(0, Objetosdispo.Count);
        Instantiate(Objetosdispo[i], SpawnObjects.position,SpawnObjects.rotation);
    }
}

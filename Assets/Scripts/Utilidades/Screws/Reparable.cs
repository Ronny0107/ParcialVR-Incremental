using UnityEngine;

// Va en la RAIZ del objeto reparable. Sabe si todos sus tornillos estan apretados.
public class Reparable : MonoBehaviour
{
    [Tooltip("Si lo dejas vacio, busca los Screw en los hijos")]
    [SerializeField] Screw[] screws;
    [SerializeField] int recompensa = 100;

    public int Recompensa => recompensa;
    public bool Enviado { get; set; }

    void Awake()
    {
        if (screws == null || screws.Length == 0)
            screws = GetComponentsInChildren<Screw>(true);
    }

    // Se consulta en vivo: si un tornillo se afloja despues, deja de contar.
    public bool Completo
    {
        get
        {
            if (screws == null || screws.Length == 0) return false;
            foreach (var s in screws)
                if (s == null || !s.Apretado) return false;
            return true;
        }
    }
}

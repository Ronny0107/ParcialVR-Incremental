using UnityEngine;

// Va en el elemento que recibe los objetos. Necesita un Collider con Is Trigger activado.
// Si el objeto esta completo: paga y lo elimina.
public class Envio : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var obj = other.GetComponentInParent<Reparable>();
        if (obj == null || obj.Enviado) return;

        // incompleto: no pasa nada
        if (!obj.Completo) return;

        // evita pagar dos veces si el objeto tiene varios colliders
        obj.Enviado = true;

        var gm = GameManager.Instance;
        if (gm != null)
        {
            gm.AddMoney(obj.Recompensa);
            gm.TotalMoney(obj.Recompensa);
            gm.AddFix();
        }

        Destroy(obj.gameObject);
    }
}

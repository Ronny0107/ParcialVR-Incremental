using System.Collections;
using System.Reflection;
using SplineMeshTools.Misc;
using UnityEngine;

public class DetenerConveyor : MonoBehaviour
{
    [Tooltip("Segundos que tarda la cinta en detenerse por completo")]
    [SerializeField] float tiempoFrenado = 2f;
    [Tooltip("Segundos que tarda la cinta en volver a su velocidad normal")]
    [SerializeField] float tiempoArranque = 1f;

    AnimateTextureOffset ato;
    ConveyorBeltMover cbm;

    // campos privados de los scripts del paquete (se accede por reflexion)
    FieldInfo cbmSpeedField;
    FieldInfo atoOriginalOffsetField;
    FieldInfo atoDirectionField;
    FieldInfo atoSpeedField;

    float velocidadCintaOriginal;
    float velocidadTexturaOriginal;
    float factorActual = 1f; 

    Coroutine transicion;

    void Start()
    {
        ato = GetComponentInParent<AnimateTextureOffset>();
        cbm = GetComponentInParent<ConveyorBeltMover>();

        const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        cbmSpeedField = typeof(ConveyorBeltMover).GetField("conveyorSpeed", flags);
        atoOriginalOffsetField = typeof(AnimateTextureOffset).GetField("originalOffset", flags);
        atoDirectionField = typeof(AnimateTextureOffset).GetField("offsetDirection", flags);
        atoSpeedField = typeof(AnimateTextureOffset).GetField("speed", flags);

        if (cbm != null && cbmSpeedField != null) velocidadCintaOriginal = (float)cbmSpeedField.GetValue(cbm);
        if (ato != null && atoSpeedField != null) velocidadTexturaOriginal = (float)atoSpeedField.GetValue(ato);

        var gm = GameManager.Instance;
        if (gm != null)
        {
            gm.OnDayEnded   += Frenar;
            gm.OnDayStarted += Arrancar;

            // estado inicial sin transicion
            AplicarFactor(gm.DayEnded ? 0f : 1f);
        }
    }

    void OnDestroy()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;
        gm.OnDayEnded   -= Frenar;
        gm.OnDayStarted -= Arrancar;
    }

    void Frenar()   { IniciarTransicion(0f, tiempoFrenado); }
    void Arrancar() { IniciarTransicion(1f, tiempoArranque); }

    void IniciarTransicion(float factorFinal, float duracion)
    {
        if (transicion != null) StopCoroutine(transicion);
        transicion = StartCoroutine(CambiarVelocidad(factorFinal, duracion));
    }

    IEnumerator CambiarVelocidad(float factorFinal, float duracion)
    {
        float factorInicial = factorActual;
        float t = 0f;

        while (t < duracion)
        {
            t += Time.deltaTime;
            float progreso = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t / duracion));
            AplicarFactor(Mathf.Lerp(factorInicial, factorFinal, progreso));
            yield return null;
        }

        AplicarFactor(factorFinal);
        transicion = null;
    }

    void AplicarFactor(float factor)
    {
        factorActual = factor;

        if (cbm != null && cbmSpeedField != null)
            cbmSpeedField.SetValue(cbm, velocidadCintaOriginal * factor);

        if (ato != null && atoSpeedField != null)
            SetVelocidadTextura(velocidadTexturaOriginal * factor);
    }

    // Cambia el speed de la textura compensando el offset original para que no haya saltos
    // (AnimateTextureOffset usa: originalOffset + direccion * speed * Time.time)
    void SetVelocidadTextura(float nueva)
    {
        if (atoOriginalOffsetField == null || atoDirectionField == null) return;

        float anterior = (float)atoSpeedField.GetValue(ato);
        Vector2 direccion = (Vector2)atoDirectionField.GetValue(ato);
        Vector2 original = (Vector2)atoOriginalOffsetField.GetValue(ato);

        atoOriginalOffsetField.SetValue(ato, original + direccion * (anterior - nueva) * Time.time);
        atoSpeedField.SetValue(ato, nueva);
    }
}

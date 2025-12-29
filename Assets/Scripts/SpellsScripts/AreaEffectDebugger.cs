using UnityEngine;

[ExecuteAlways]
public class AreaEffectDebugger : MonoBehaviour
{
    [Header("Referencia")]
    public SpellCaster spellCaster;

    [Header("Colores")]
    public Color areaColor = new Color(0f, 0.8f, 1f, 0.35f);
    public Color hitColor = Color.red;
    public Color centerColor = Color.white;

    [Header("Opciones")]
    public bool showHits = true;

    private void OnDrawGizmosSelected()
    {
        if (spellCaster == null || spellCaster.CurrentSpell == null)
            return;

        SpellData data = spellCaster.CurrentSpell.Data;
        if (data == null) return;

        Vector3 center = ResolveCenter();

        foreach (var effect in data.effects)
        {
            if (effect is AreaEffect area)
            {
                DrawArea(center, area.radius);

                if (showHits)
                    DrawHits(center, area.radius);
            }
        }
    }

    // ------------------------------------------------------

    private Vector3 ResolveCenter()
    {
        // Runtime cursor
        Vector3 cursorPos = spellCaster.CursorPosition;

        if (cursorPos != Vector3.zero)
            return cursorPos;

        // Fallback: FirePoint o caster
        if (spellCaster.FirePoint != null)
            return spellCaster.FirePoint.position;

        return spellCaster.transform.position;
    }

    private void DrawArea(Vector3 center, float radius)
    {
        Gizmos.color = areaColor;
        DrawWireCircle(center, radius, 32);

        Gizmos.color = centerColor;
        Gizmos.DrawSphere(center, 0.15f);
    }

    private void DrawHits(Vector3 center, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(center, radius);

        Gizmos.color = hitColor;
        foreach (var hit in hits)
        {
            Gizmos.DrawSphere(hit.transform.position, 0.25f);
        }
    }

    private void DrawWireCircle(Vector3 center, float radius, int segments)
    {
        float step = 360f / segments;
        Vector3 prev = center + Vector3.forward * radius;

        for (int i = 1; i <= segments; i++)
        {
            Vector3 next = center +
                Quaternion.Euler(0f, step * i, 0f) * Vector3.forward * radius;

            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }
}

using UnityEngine;

[ExecuteAlways]
public class AreaEffectDebugger : MonoBehaviour
{
    public SpellCaster spellCaster;

    public Color areaColor = new Color(0f, 0.8f, 1f, 0.25f);
    public Color hitColor = Color.red;

    public bool showHits = true;

    private void OnDrawGizmos()
    {
        if (spellCaster == null || spellCaster.CurrentSpell == null)
            return;

        Vector3 center = spellCaster.CursorPosition;

        foreach (var effect in spellCaster.CurrentSpell.Data.effects)
        {
            if (effect is AreaEffect area)
            {
                DrawArea(center, area.radius);

                if (showHits)
                    DrawHits(area, center);
            }
        }
    }

    private void DrawArea(Vector3 center, float radius)
    {
        Gizmos.color = areaColor;
        DrawWireCircle(center, radius, 32);
        Gizmos.DrawWireSphere(center, 0.15f);
    }

    private void DrawHits(AreaEffect area, Vector3 center)
    {
        foreach (var hit in Physics.OverlapSphere(center, area.radius))
        {
            Gizmos.color = hitColor;
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

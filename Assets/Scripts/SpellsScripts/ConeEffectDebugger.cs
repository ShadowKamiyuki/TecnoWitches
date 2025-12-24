using UnityEngine;

#if UNITY_EDITOR
#endif

[ExecuteAlways]
public class ConeEffectDebugger : MonoBehaviour
{
    [Header("Referencia al SpellCaster")]
    public SpellCaster spellCaster;

    [Header("Opciones")]
    public Color coneColor = new Color(1f, 0.6f, 0f, 0.3f);
    public Color hitColor = Color.red;
    public Color radiusColor = Color.yellow;

    [Header("Debug")]
    public bool showHits = true;

    private void OnDrawGizmos()
    {
        if (spellCaster == null || spellCaster.CurrentSpell == null)
            return;

        // Buscamos un ConeEffect en los efectos del spell actual
        foreach (var effect in spellCaster.CurrentSpell.data.effects)
        {
            if (effect is ConeEffect cone)
            {
                Vector3 origin = spellCaster.FirePoint != null ? spellCaster.FirePoint.position : spellCaster.transform.position;
                Vector3 targetDir = spellCaster.CursorPosition - origin;
                targetDir.y = 0; // proyectar en plano horizontal
                if (targetDir.magnitude < 0.001f) targetDir = spellCaster.transform.forward;

                DrawCone(origin, targetDir.normalized, cone.radius, cone.angle);

                if (showHits)
                    DrawHits(cone, origin, targetDir.normalized);
            }
        }
    }

    private void DrawCone(Vector3 origin, Vector3 forward, float radius, float angle)
    {
        forward = new Vector3(forward.x, 0, forward.z).normalized;
        float halfAngle = angle * 0.5f;

        // Líneas laterales
        Vector3 leftDir = Quaternion.Euler(0f, -halfAngle, 0f) * forward;
        Vector3 rightDir = Quaternion.Euler(0f, halfAngle, 0f) * forward;

        Gizmos.color = coneColor;
        Gizmos.DrawLine(origin, origin + leftDir * radius);
        Gizmos.DrawLine(origin, origin + rightDir * radius);

        // Arco frontal
        int segments = 24;
        Vector3 previousPoint = origin + leftDir * radius;
        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments;
            float currentAngle = Mathf.Lerp(-halfAngle, halfAngle, t);
            Vector3 dir = Quaternion.Euler(0f, currentAngle, 0f) * forward;
            Vector3 point = origin + dir * radius;
            Gizmos.DrawLine(previousPoint, point);
            previousPoint = point;
        }

        // Radio de la esfera
        Gizmos.color = radiusColor;
        Gizmos.DrawWireSphere(origin, radius);
    }

    private void DrawHits(ConeEffect cone, Vector3 origin, Vector3 forward)
    {
        Collider[] hits = Physics.OverlapSphere(origin, cone.radius);
        forward = new Vector3(forward.x, 0, forward.z).normalized;

        foreach (var hit in hits)
        {
            if (hit.transform == spellCaster.transform) continue; // ignorar al caster

            Vector3 dirToTarget = hit.transform.position - origin;
            Vector3 flatDir = new Vector3(dirToTarget.x, 0, dirToTarget.z).normalized;

            // Usamos Vector3.Angle para coincidencia exacta con el Execute
            float flatAngle = Vector3.Angle(forward, flatDir);

            if (flatAngle <= cone.angle / 2f)
            {
                Gizmos.color = hitColor;
                Gizmos.DrawSphere(hit.transform.position, 0.3f);
            }
        }
    }
}

using UnityEngine;

public class ProjectileVisual : MonoBehaviour
{
    private const float SHADOW_POSITION_Y_DIVIDER = 6f;

    [SerializeField] private Projectile m_projectile;

    [Space]

    [SerializeField] private Transform m_projectileVisual;
    [SerializeField] private Transform m_projectileShadow;

    private Transform m_target;
    private Vector3 m_trajectoryStartPoint;

    private void Start()
    {
        m_trajectoryStartPoint = transform.position;
    }

    private void Update()
    {
        UpdateProjectileRotation();
        UpdateShadowPosition();

        float trajectoryProgressMagnitude = (transform.position - m_trajectoryStartPoint).magnitude;
        float trajectoryMagnitude = (m_target.position - m_trajectoryStartPoint).magnitude;

        float trajectoryProgressNormalized = trajectoryProgressMagnitude / trajectoryMagnitude;
        //if (trajectoryProgressNormalized < 0.7f)
        //{
        //    UpdateProjectileShadowRotation();
        //}
        UpdateProjectileShadowRotation();
    }

    private void UpdateShadowPosition()
    {
        Vector3 targetPosition = transform.position;
        Vector3 trajectoryRange = m_target.position - m_trajectoryStartPoint;

        if (Mathf.Abs(trajectoryRange.normalized.x) < Mathf.Abs(trajectoryRange.normalized.y))
        {
            targetPosition.x = m_trajectoryStartPoint.x + m_projectile.GetNextXTrajectoryPosition() / SHADOW_POSITION_Y_DIVIDER + m_projectile.GetNextPositionXTrajectoryAbsolute();
        }
        else
        {
            targetPosition.y = m_trajectoryStartPoint.y + m_projectile.GetNextYTrajectoryPosition() / SHADOW_POSITION_Y_DIVIDER + m_projectile.GetNextPositionYTrajectoryAbsolute();
        }

        m_projectileShadow.position = targetPosition;
    }

    private void UpdateProjectileRotation()
    {
        Vector3 projectileMovementDirection = m_projectile.MovementDirection;
        m_projectileVisual.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(projectileMovementDirection.y, projectileMovementDirection.x) * Mathf.Rad2Deg);
    }

    private void UpdateProjectileShadowRotation()
    {
        Vector3 projectileMovementDirection = m_projectile.MovementDirection;
        m_projectileShadow.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(projectileMovementDirection.y, projectileMovementDirection.x) * Mathf.Rad2Deg);
    }

    public void SetTarget(Transform target)
    {
        m_target = target;
    }
}

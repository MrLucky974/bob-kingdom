using UnityEngine;

public class Projectile : MonoBehaviour
{
    public struct ProjectileData
    {
        public Transform target;
        public float maxSpeed;

        public float trajectoryMaxHeight;
        public AnimationCurve trajectoryAnimationCurve;
        public AnimationCurve axisCorrectionAnimationCurve;
        public AnimationCurve speedAnimationCurve;

        public int damage;
    }

    private const float MINIMUM_TARGET_DISTANCE = 0.4f;

    [SerializeField] private ProjectileVisual m_visual;

    private Transform m_target;
    private Transform m_virtualTarget;

    private float m_movementSpeed;
    private float m_maxMovementSpeed;

    private Vector3 m_trajectoryStartPoint;
    private Vector3 m_projectileMovementDirection;
    private Vector3 m_trajectoryRange;

    private float m_nextYTrajectoryPosition;
    private float m_nextXTrajectoryPosition;

    private float m_nextPositionYCorrectionAbsolute;
    private float m_nextPositionXCorrectionAbsolute;

    private float m_trajectoryMaxRelativeHeight;
    private AnimationCurve m_trajectoryAnimationCurve;
    private AnimationCurve m_axisCorrectionAnimationCurve;
    private AnimationCurve m_speedAnimationCurve;

    private int m_damage;

    private void Start()
    {
        m_trajectoryStartPoint = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, m_virtualTarget.position) < MINIMUM_TARGET_DISTANCE)
        {
            if (m_target != null)
            {
                bool isEnemy = m_target.TryGetComponent<EnemyBehavior>(out var enemy);
                if (isEnemy)
                {
                    enemy.Damage(m_damage);
                }
            }

            Destroy(m_virtualTarget.gameObject);
            Destroy(gameObject);
        }

        if (m_target != null)
        {
            m_virtualTarget.position = m_target.position;
        }

        UpdateProjectilePosition();
    }

    private void UpdateProjectilePosition()
    {
        m_trajectoryRange = m_virtualTarget.position - m_trajectoryStartPoint;

        if (Mathf.Abs(m_trajectoryRange.normalized.x) < Mathf.Abs(m_trajectoryRange.normalized.y))
        {
            if (m_trajectoryRange.y < 0)
            {
                m_movementSpeed = -m_movementSpeed;
            }
            UpdatePositionWithXCurve();
        }
        else
        {
            if (m_trajectoryRange.x < 0)
            {
                m_movementSpeed = -m_movementSpeed;
            }
            UpdatePositionWithYCurve();
        }
    }

    private void UpdatePositionWithXCurve()
    {
        float nextPositionY = transform.position.y + m_movementSpeed * Time.deltaTime;
        float nextPositionYNormalized = (nextPositionY - m_trajectoryStartPoint.y) / m_trajectoryRange.y;

        float nextPositionXNormalized = m_trajectoryAnimationCurve.Evaluate(nextPositionYNormalized);
        m_nextXTrajectoryPosition = nextPositionXNormalized * m_trajectoryMaxRelativeHeight;

        float nextPositionXCorrectionNormalized = m_axisCorrectionAnimationCurve.Evaluate(nextPositionYNormalized);
        m_nextPositionXCorrectionAbsolute = nextPositionXCorrectionNormalized * m_trajectoryRange.x;

        if (m_trajectoryRange.x > 0 && m_trajectoryRange.y > 0)
        {
            m_nextXTrajectoryPosition = -m_nextXTrajectoryPosition;
        }

        if (m_trajectoryRange.x < 0 && m_trajectoryRange.y < 0)
        {
            m_nextXTrajectoryPosition = -m_nextXTrajectoryPosition;
        }

        float nextPositionX = m_trajectoryStartPoint.x + m_nextXTrajectoryPosition + m_nextPositionXCorrectionAbsolute;

        Vector3 targetPosition = new Vector3(nextPositionX, nextPositionY, 0f);

        CalculateNextProjectileSpeed(nextPositionYNormalized);
        m_projectileMovementDirection = targetPosition - transform.position;

        transform.position = targetPosition;
    }

    private void UpdatePositionWithYCurve()
    {
        float nextPositionX = transform.position.x + m_movementSpeed * Time.deltaTime;
        float nextPositionXNormalized = (nextPositionX - m_trajectoryStartPoint.x) / m_trajectoryRange.x;

        float nextPositionYNormalized = m_trajectoryAnimationCurve.Evaluate(nextPositionXNormalized);
        m_nextYTrajectoryPosition = nextPositionYNormalized * m_trajectoryMaxRelativeHeight;

        float nextPositionYCorrectionNormalized = m_axisCorrectionAnimationCurve.Evaluate(nextPositionXNormalized);
        m_nextPositionYCorrectionAbsolute = nextPositionYCorrectionNormalized * m_trajectoryRange.y;

        float nextPositionY = m_trajectoryStartPoint.y + m_nextYTrajectoryPosition + m_nextPositionYCorrectionAbsolute;

        Vector3 targetPosition = new Vector3(nextPositionX, nextPositionY, 0f);

        CalculateNextProjectileSpeed(nextPositionXNormalized);
        m_projectileMovementDirection = targetPosition - transform.position;

        transform.position = targetPosition;
    }

    private void CalculateNextProjectileSpeed(float nextPositionXNormalized)
    {
        float nextMovementSpeedNormalized = m_speedAnimationCurve.Evaluate(nextPositionXNormalized);
        m_movementSpeed = nextMovementSpeedNormalized * m_maxMovementSpeed;
    }

    public void Initialize(ProjectileData data)
    {
        m_target = data.target;
        var gameObject = new GameObject("Projectile Target");
        m_virtualTarget = gameObject.transform;
        m_virtualTarget.position = m_target.position;

        m_maxMovementSpeed = data.maxSpeed;

        float xDistanceToTarget = m_target.position.x - transform.position.x;
        m_trajectoryMaxRelativeHeight = Mathf.Abs(xDistanceToTarget) * data.trajectoryMaxHeight;
        m_trajectoryAnimationCurve = data.trajectoryAnimationCurve;
        m_axisCorrectionAnimationCurve = data.axisCorrectionAnimationCurve;
        m_speedAnimationCurve = data.speedAnimationCurve;

        m_visual.SetTarget(m_virtualTarget);

        m_damage = data.damage;
    }

    public float GetNextPositionYTrajectoryAbsolute()
    {
        return m_nextPositionYCorrectionAbsolute;
    }

    public float GetNextYTrajectoryPosition()
    {
        return m_nextYTrajectoryPosition;
    }

    public float GetNextPositionXTrajectoryAbsolute()
    {
        return m_nextPositionXCorrectionAbsolute;
    }

    public float GetNextXTrajectoryPosition()
    {
        return m_nextXTrajectoryPosition;
    }

    public Vector3 MovementDirection => m_projectileMovementDirection;
}
using LuckiusDev.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoldierCoordinator : Singleton<SoldierCoordinator>
{
    // Keeps track of enemies already targeted by other soldiers.
    private static HashSet<Transform> m_targetedEnemies = new HashSet<Transform>();

    /// <summary>
    /// Finds the closest available target to the given soldier within the specified attack range,
    /// avoiding already targeted enemies when possible. If all enemies are targeted, returns a random target within range.
    /// </summary>
    /// <param name="soldier">The soldier's Transform (usually its position).</param>
    /// <param name="attackDistance">The maximum distance to consider a target "available".</param>
    /// <returns>The Transform of the closest or randomly selected enemy, or null if no enemies are in range.</returns>
    public Transform GetClosestAvailableTarget(Transform soldier, float attackDistance)
    {
        // Find all active enemies in the scene using EnemyBehavior.
        EnemyBehavior[] enemies = FindObjectsOfType<EnemyBehavior>();

        // If no enemies are found, return null.
        if (enemies.Length == 0)
            return null;

        // Filter enemies within attack range.
        List<Transform> enemiesInRange = enemies
            .Where(e => Vector3.Distance(soldier.position, e.transform.position) <= attackDistance)
            .Select(e => e.transform)
            .ToList();

        // If no enemies are in range, return null.
        if (enemiesInRange.Count == 0)
            return null;

        // Try to find the closest enemy that is not already targeted.
        Transform closestAvailable = enemiesInRange
            .Where(e => !m_targetedEnemies.Contains(e))
            .OrderBy(e => Vector3.Distance(soldier.position, e.position))
            .FirstOrDefault();

        // If an available enemy is found, mark it as targeted and return it.
        if (closestAvailable != null)
        {
            m_targetedEnemies.Add(closestAvailable);
            return closestAvailable;
        }

        // If all enemies are already targeted, select the closest one (even if it's already targeted).
        Transform closestEnemy = enemiesInRange
            .OrderBy(e => Vector3.Distance(soldier.position, e.position))
            .FirstOrDefault();

        return closestEnemy;
    }

    /// <summary>
    /// Releases a targeted enemy, making it available for other soldiers to target.
    /// </summary>
    /// <param name="enemy">The enemy to release from the targeted list.</param>
    public static void ReleaseTarget(Transform enemy)
    {
        Debug.Log($"{enemy} released from targetting!");
        m_targetedEnemies.Remove(enemy);
    }
}

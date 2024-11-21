using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class EnemyComposition
{
    [FormerlySerializedAs("_enemies")] public List<GameObject> enemies;
}

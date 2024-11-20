using System.Collections;
using UnityEngine;

public class EnemyWaveSystem : MonoBehaviour
{
    public bool _waveFinished;
    [SerializeField] private bool _waveCanStart;
    [SerializeField] private float _waveCooldown;
    public bool _waveStarted;

    EnemySpawner _enemySpawner;

    private void Start()
    {   
        _enemySpawner = FindObjectOfType<EnemySpawner>();
        _waveFinished = false;
        _waveCanStart = false;
    }


    IEnumerator WaveCooldown()
    {
        yield return new WaitForSeconds(_waveCooldown);
        if (_waveStarted == false) { StartWave(); }
    }


    public void StartWave()
    {
        _waveStarted = true;
        _waveCanStart = true;
        _enemySpawner._activate = true;
        _enemySpawner.IncreaseWaveCount();
    }


    public void WaveFinished()
    {
        _enemySpawner._enemySpawned = 0;
        _enemySpawner._enemyKilled = 0;
        _waveFinished = false;
        _waveCanStart = false;
        _enemySpawner._activate = false;
        StartCoroutine(WaveCooldown());
    }
}

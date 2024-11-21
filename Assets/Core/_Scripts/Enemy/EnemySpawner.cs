using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawnArea1;
    [SerializeField] private GameObject _spawnArea2;

    [SerializeField] private float _spawnCoolDown;
    [SerializeField] private bool _canSpawn;
    public bool _activate;

    [SerializeField] private int _firstWaveEnemyCount;
    public int _enemyCount;
    public int _enemySpawned;
    public int _enemyKilled;

    public List<EnemyComposition> _enemyCompositions;
    public int _waveIndex;

    EnemyWaveSystem _waveSystem;

    int _compositionIndex;
    const int _compEveryWave = 4;
    


    private void Start()
    {
        int InitialCompIndex = 0;  //dire quel est la comp de depart
        var tempWaveIndex = _waveIndex; //notre tempwave = la wave actuel
        
        while (tempWaveIndex >= _compEveryWave) //tant que la tempwave >= 4   -->faire-->   tempwave - 4 et initialCompIndex + 1 
        {
            tempWaveIndex -= _compEveryWave;
            InitialCompIndex++;
        }
        _compositionIndex = InitialCompIndex; //notre compindex = initcompindex
        
        _waveSystem = FindObjectOfType<EnemyWaveSystem>();
        _activate = false;
        _canSpawn = true;
    }
    
    
    private void Update()
    {
        _enemyCount = Mathf.RoundToInt(_firstWaveEnemyCount * Mathf.Pow(1.15f, _waveIndex));
        if (_enemySpawned >= _enemyCount)
        {
            _activate = false;
        }
        if (_enemyKilled == _enemyCount)
        {
            _waveSystem._waveStarted = false;
            _waveSystem.WaveFinished();
        }
        if (_activate == true)
        {
            if (_canSpawn == true)
            {
                StartCoroutine(SpawningCooldown());
                Spawn();
                _enemySpawned++;
            }

        }
    }
    
    
    public void Spawn()
    {
        if(_waveIndex %4 == 0 && _waveIndex > 0)
        {
            _compositionIndex++;
        }

        if (_compositionIndex >= _enemyCompositions.Count)
        {
            _compositionIndex = _enemyCompositions.Count - 1;
        }
        
        EnemyComposition currentComposition = _enemyCompositions[_compositionIndex];
        List<GameObject> enemies = currentComposition._enemies;
        
        var randomPose = new Vector3(transform.position.x,Random.Range(_spawnArea1.transform.position.y, _spawnArea2.transform.position.y),0);

        var randoIndex = Random.Range(0, enemies.Count);

        var enemy = enemies[randoIndex];
        Instantiate(enemy,randomPose,Quaternion.identity);
    }
    
    
    IEnumerator SpawningCooldown()
    {
        _canSpawn = false;
        yield return new WaitForSeconds(_spawnCoolDown);
        _canSpawn = true;
    }
    
    
    public void IncreaseWaveCount()
    {
        _waveIndex++;
    }

}

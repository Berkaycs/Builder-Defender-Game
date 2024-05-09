using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance {  get; private set; }

    public event EventHandler OnWaveNumberChanged;

    [SerializeField] private List<Transform> _spawnPositionTransformList;
    [SerializeField] private Transform _nextWaveSpawnPositionTransform;

    private float _nextWaveSpawnTimer;
    private float _nextEnemySpawnTimer;
    private int _remainingEnemySpawnAmount;
    private int _waveNumber;

    private Vector3 _spawnPosition;

    private State _state;

    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave,
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _state = State.WaitingToSpawnNextWave;
        _spawnPosition = _spawnPositionTransformList[UnityEngine.Random.Range(0, _spawnPositionTransformList.Count)].position;
        _nextWaveSpawnPositionTransform.position = _spawnPosition;
        _nextEnemySpawnTimer = 5f;
    }

    private void Update()
    {
        switch (_state)
        {
            case State.WaitingToSpawnNextWave:
                _nextWaveSpawnTimer -= Time.deltaTime;
                if (_nextWaveSpawnTimer < 0f)
                {
                    SpawnWave();
                }
                break;
            case State.SpawningWave:
                if (_remainingEnemySpawnAmount > 0)
                {
                    _nextEnemySpawnTimer -= Time.deltaTime;
                    if (_nextEnemySpawnTimer < 0f)
                    {
                        _nextEnemySpawnTimer = UnityEngine.Random.Range(0.2f, 0.4f);
                        Enemy.Create(_spawnPosition + UtilitiesClass.GetRandomDir() * UnityEngine.Random.Range(0f, 10f));
                        _remainingEnemySpawnAmount--;

                        if (_remainingEnemySpawnAmount <= 0)
                        {
                            _state = State.WaitingToSpawnNextWave;
                            _spawnPosition = _spawnPositionTransformList[UnityEngine.Random.Range(0, _spawnPositionTransformList.Count)].position;
                            _nextWaveSpawnPositionTransform.position = _spawnPosition;
                            _nextWaveSpawnTimer = 15f;
                        }
                    }
                }
                break;
        }
    }

    private void SpawnWave()
    {
        _remainingEnemySpawnAmount = 5 + 2 * _waveNumber;
        _state = State.SpawningWave;
        _waveNumber++;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetWaveNumber()
    {
        return _waveNumber;
    }

    public float GetNextWaveSpawnTimer()
    {
        return _nextWaveSpawnTimer;
    }

    public Vector3 GetSpawnPosition()
    {
        return _spawnPosition;
    }
}

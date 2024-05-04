using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyWaveUI : MonoBehaviour
{
    [SerializeField] private EnemyWaveManager _enemyWaveManager;
    [SerializeField] private Camera _mainCamera;

    private TextMeshProUGUI _waveNumberText;
    private TextMeshProUGUI _waveMessageText;
    private RectTransform _enemyWaveSpawnPositionIndicator;
    private RectTransform _enemyClosestWaveSpawnPositionIndicator;

    private void Awake()
    {
        _waveNumberText = transform.Find("TxtWaveNumber").GetComponent<TextMeshProUGUI>();
        _waveMessageText = transform.Find("TxtNextWaveSeconds").GetComponent<TextMeshProUGUI>();
        _enemyWaveSpawnPositionIndicator = transform.Find("EnemyWaveSpawnPositionIndicator").GetComponent<RectTransform>();
        _enemyClosestWaveSpawnPositionIndicator = transform.Find("EnemyClosestWaveSpawnPositionIndicator").GetComponent<RectTransform>();
    }

    private void Start()
    {
        _enemyWaveManager.OnWaveNumberChanged += EnemyWaveManager_OnWaveNumberChanged;
        SetWaveNumberText("Wave " + _enemyWaveManager.GetWaveNumber());
    }

    private void EnemyWaveManager_OnWaveNumberChanged(object sender, System.EventArgs e)
    {
        SetWaveNumberText("Wave " + _enemyWaveManager.GetWaveNumber());
    }

    private void Update()
    {
        HandleNextWaveMessage();
        HandleEnemyWaveSpawnPositionIndicator();
        HandleEnemyClosestPositionIndicator();
    }

    private void HandleNextWaveMessage()
    {
        float nextWaveSpawnTimer = _enemyWaveManager.GetNextWaveSpawnTimer();
        if (nextWaveSpawnTimer <= 0f)
        {
            SetMessageText("");
        }
        else
        {
            SetMessageText("Next Wave in " + nextWaveSpawnTimer.ToString("F1") + "s"); // it shows 1 decimal point after comma
        }
    }

    private void HandleEnemyClosestPositionIndicator()
    {
        float targetMaxRadius = 9999f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(_mainCamera.transform.position, targetMaxRadius);

        Enemy _targetEnemy = null;

        foreach (Collider2D collider2D in collider2DArray)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();

            if (enemy != null)
            {
                // Is a building!
                if (_targetEnemy == null)
                {
                    _targetEnemy = enemy;
                }
                else
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) <
                        Vector3.Distance(transform.position, _targetEnemy.transform.position))
                    {
                        // Closer!
                        _targetEnemy = enemy;
                    }
                }
            }
        }

        if (_targetEnemy != null)
        {
            Vector3 dirToClosestSpawnPosition = (_targetEnemy.transform.position - _mainCamera.transform.position).normalized;

            _enemyClosestWaveSpawnPositionIndicator.anchoredPosition = dirToClosestSpawnPosition * 250f;
            _enemyClosestWaveSpawnPositionIndicator.eulerAngles = new Vector3(0, 0, UtilitiesClass.GetAngleFromVector(dirToClosestSpawnPosition));

            float distanceToClosestSpawnPosition = Vector3.Distance(_targetEnemy.transform.position, _mainCamera.transform.position);
            _enemyClosestWaveSpawnPositionIndicator.gameObject.SetActive(distanceToClosestSpawnPosition > _mainCamera.orthographicSize * 1.5f);
        }
        else
        {
            _enemyClosestWaveSpawnPositionIndicator.gameObject.SetActive(false);
        }
    }

    private void HandleEnemyWaveSpawnPositionIndicator()
    {
        Vector3 dirToNextSpawnPosition = (_enemyWaveManager.GetSpawnPosition() - _mainCamera.transform.position).normalized;

        _enemyWaveSpawnPositionIndicator.anchoredPosition = dirToNextSpawnPosition * 300f;
        _enemyWaveSpawnPositionIndicator.eulerAngles = new Vector3(0, 0, UtilitiesClass.GetAngleFromVector(dirToNextSpawnPosition));

        float distanceToNextSpawnPosition = Vector3.Distance(_enemyWaveManager.GetSpawnPosition(), _mainCamera.transform.position);
        _enemyWaveSpawnPositionIndicator.gameObject.SetActive(distanceToNextSpawnPosition > _mainCamera.orthographicSize * 1.5f);
    }

    private void SetMessageText(string message)
    {
        _waveMessageText.SetText(message);
    }

    private void SetWaveNumberText(string text)
    {
        _waveNumberText.SetText(text);
    }
}

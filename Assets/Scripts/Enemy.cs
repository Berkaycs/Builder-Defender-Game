using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Create(Vector3 position)
    {
        Transform enemyPrefab = Resources.Load<Transform>("Enemy");
        Transform enemyTransform = Instantiate(enemyPrefab, position, Quaternion.identity);

        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }

    private Rigidbody2D _rb;
    private Transform _targetTransform;
    private HealthSystem _healthSystem;
    private float _lookForTargetTimer;
    private float _lookForTargetTimerMax = .2f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        // because we can't access the target from projects to serializefield, we get it with script in the scene
        if (BuildManager.Instance.GetHQBuilding() != null )
        {
            _targetTransform = BuildManager.Instance.GetHQBuilding().transform;
        }

        _healthSystem = GetComponent<HealthSystem>();

        _healthSystem.OnDamaged += HealthSystem_OnDamaged;
        _healthSystem.OnDied += HealthSystem_OnDied;

        _lookForTargetTimer = Random.Range(0f, _lookForTargetTimerMax);
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
    }

    private void Update()
    {
        HandleMovement();
        HandleTargeting();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();

        if (building != null)
        {
            // Collided with a building!
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(10);
            Destroy(gameObject);
        }
    }

    private void HandleMovement()
    {
        if (_targetTransform != null)
        {
            Vector3 moveDir = (_targetTransform.position - transform.position).normalized;

            float moveSpeed = 6f;
            _rb.velocity = moveDir * moveSpeed;
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }

    private void HandleTargeting()
    {
        _lookForTargetTimer -= Time.deltaTime;
        if (_lookForTargetTimer < 0)
        {
            _lookForTargetTimer += _lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void LookForTargets()
    {
        float targetMaxRadius = 10f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            Building building = collider2D.GetComponent<Building>();

            if (building != null)
            {
                // Is a building!
                if (_targetTransform == null)
                {
                    _targetTransform = building.transform;
                }
                else
                {
                    if (Vector3.Distance(transform.position, building.transform.position) < 
                        Vector3.Distance(transform.position, _targetTransform.position))
                    {
                        // Closer!
                        _targetTransform = building.transform;
                    }
                }
            }
        }

        if (_targetTransform == null)
        {
            if (BuildManager.Instance.GetHQBuilding() != null)
            {
                _targetTransform = BuildManager.Instance.GetHQBuilding().transform;
            }           
        }
    }
}

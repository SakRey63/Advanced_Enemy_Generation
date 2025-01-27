using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<SpawnPoint> _spawnPoints;
    [SerializeField] private float _delay = 2f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 15;
    
    private WaitForSeconds _waitForSeconds;
    private Enemy _enemy;
    private Transform _target;
    private SpawnPoint _spawnPoint;
    
    private ObjectPool<Enemy> _pool;
    
    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(
            createFunc: () => Instantiate(_enemy),
            actionOnGet: (enemy) => GetAction(enemy),
            actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
            actionOnDestroy: (enemy) => Destroy(enemy),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
        );
    }

    private void Start()
    {
        StartCoroutine(nameof(Spawn), _delay);
    }
    
    private IEnumerator Spawn(float delay)
    {
        _waitForSeconds = new WaitForSeconds(delay);

        while (true) 
        {
            _spawnPoint = GetRandomSpawnPoint();
                
            GetSetting(_spawnPoint);
                
            SpawnEnemy();

            yield return _waitForSeconds;
        }
    }
    
    private SpawnPoint GetRandomSpawnPoint()
    {
        return _spawnPoints[Random.Range(0, _spawnPoints.Count)];
    }

    private void GetSetting(SpawnPoint point)
    {
        _target = point.Target;
        _enemy = point.Enemy;
    }
    
    private void GetAction(Enemy enemy)
    {
        enemy.Triggered += ReleaseEnemy;

        enemy.transform.position = _spawnPoint.transform.position;
        enemy.gameObject.SetActive(true);
        enemy.SetTarget(_target);
    }
    
    private void SpawnEnemy()
    {
        _pool.Get();
    }
    
    private void ReleaseEnemy(Enemy enemy)
    { 
        enemy.Triggered -= ReleaseEnemy;
        
        _pool.Release(enemy);
    }
}

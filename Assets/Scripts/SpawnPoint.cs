using UnityEngine;
using UnityEngine.Pool;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 15;

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
    
    private void GetAction(Enemy enemy)
    {
        enemy.Triggered += EnemyRelease;

        enemy.transform.position = transform.position;
        enemy.gameObject.SetActive(true);
        enemy.SetTarget(_target);
    }
    
    private void EnemyRelease(Enemy enemy)
    { 
        enemy.Triggered -= EnemyRelease;
        
        _pool.Release(enemy);
    }

    public void SpawnEnemy()
    {
        _pool.Get();
    }
}
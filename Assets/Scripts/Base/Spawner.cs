using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Timer))]
public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy poolObject;// test spawn
    [SerializeField] private float maxDistance;
    [SerializeField] private Transform player;
    private Dictionary<Type, IObjectPool<ISpawnable>> pool = new();
    private Timer timer;

    private void Awake()
    {
        if (!pool.ContainsKey(typeof(Enemy)))
        {
            pool.Add(typeof(Enemy),
                new ObjectPool<ISpawnable>(poolObject.OnCreate, poolObject.OnGet, poolObject.OnRelease, poolObject.OnDestroyFromPool));
        }
        timer = GetComponent<Timer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(DoSpawn), 0f, 5f);
        //timer.BeginTimer();
    }

    private void DoSpawn()
    {
        string poolName = poolObject.name.ToUpper();
        if (!pool.ContainsKey(typeof(Enemy))) return;

        Enemy enemy = pool[typeof(Enemy)].Get() as Enemy;

        Vector3 randomPosition = Random.insideUnitCircle * maxDistance;
        Vector3 spawnPosition = new Vector3(randomPosition.x, 1f, randomPosition.y);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPosition, out hit, 10f, 1))
        {
            enemy.transform.position = hit.position;
            enemy.transform.rotation = Quaternion.identity;
        }

        enemy.SetUpPool(pool[typeof(Enemy)]);
        enemy.SetTarget(player);
        enemy.Spawn(new AgentData
        {
            spawnPoint = transform,
            isPartroll = false,
            stats = AgentStats.RESPAWN,
        });
    }
}

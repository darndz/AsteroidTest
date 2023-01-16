using System;
using System.Collections.Generic;
using UnityEngine;
using Asteroids.Core;
using Asteroids.Movables;
using Asteroids.Spawners;
public class SpawnController : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    private int _enemyCount = 0;
    private Dictionary<Spawner, int> _spawnersDict = new Dictionary<Spawner, int>();
    void Start()
    {
        _gameController.OnGameStart += StartGameSpawn;
        _gameController.OnGameEnd += ClearSpawn;
    }
    void StartGameSpawn(object o, EventArgs args)
    {
        AsteroidSpawner asteroidSpawner = new AsteroidSpawner(3);
        asteroidSpawner.OnSpawn += CreateObject;
        asteroidSpawner.OnShardCreate += CreateObject;
        _spawnersDict.Add(asteroidSpawner, 5);

        SaucerSpawner saucerSpawner = new SaucerSpawner(_gameController.Player);
        saucerSpawner.OnSpawn += CreateObject;
        _spawnersDict.Add(saucerSpawner, 2);

        SpawnBois();
    }
    void SpawnBois()
    {
        List<Spawner> keys = new List<Spawner>(_spawnersDict.Keys);
        foreach (Spawner key in keys)
        {
            key.Spawn(_spawnersDict[key]);
            _spawnersDict[key] += 1;
        }
    }
    void ClearSpawn(object o, EventArgs args)
    {
        _spawnersDict.Clear();
    }
    public void CreateObject(object o, SpawnArgs args)
    {
        var movable = args.SpawnMovable;
        var obj = _gameController.CreateObject(movable);
        movable.OnDeath += ObjectDeath;
        _enemyCount += 1;
        if (movable is Asteroid)
        {
            Asteroid asteroid = movable as Asteroid;
            obj.transform.localScale *= asteroid.Size;            
        }
    }
    public void ObjectDeath(object o, EventArgs args)
    {
        _enemyCount -= 1;

        if (_enemyCount <= 0)
        {
            SpawnBois();
        }
    }
}

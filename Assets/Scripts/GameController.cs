using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Asteroids.Core;
using Asteroids.Movables;
using Asteroids.Weapons;
using Asteroids.Score;
using Asteroids.Spawners;
public class GameController : MonoBehaviour
{
    private Dictionary<Transform, Movable> _transformsDict = new Dictionary<Transform, Movable>();
    private Dictionary<Movable,Transform> _movablesDict = new Dictionary<Movable, Transform>();
    private List<Movable> _movablesToDestroy = new List<Movable>();
    private AccelBody _playerMovable;
    public AccelBody Player { get { return _playerMovable; } }
    private Transform _playerTransform;
    public Transform PlayerTransform { get { return _playerTransform; } }
    private Armament _playerArmament;
    public Armament PlayerArmament { get { return _playerArmament; } }
    private DefaultInput _input;
    [SerializeField] private ObjectsViewsData _objectsId;
    [SerializeField] private GameUI _ui;
    [SerializeField] private float _playerAngularSpeed;
    [SerializeField] private float _playerAcceleration;
    private Vector2 _moveInput;
    private Vector4 _screenBorders;
    private ScoreSystem _gameScoreSystem;
    public ScoreSystem GameScoreSystem { get { return _gameScoreSystem; } }
    private AsteroidSpawner _asteroidSpawner;
    private SaucerSpawner _saucerSpawner;
    public EventHandler OnGameStart;
    public EventHandler OnGameEnd;
    void Start()
    {
        _input = new DefaultInput();
    }
    public void StartGame()
    {
        _input.Keyboard.Enable();

        _input.Keyboard.Move.performed += MovePlayer;
        _input.Keyboard.Move.canceled += MovePlayer;
        _input.Keyboard.Fire.performed += Fire;
        _input.Keyboard.AltFire.performed += AltFire;

        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0,0,Camera.main.nearClipPlane));
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1,1,Camera.main.nearClipPlane));
        Movable.SetScreenBorders(leftBorder.x, leftBorder.z, rightBorder.x, rightBorder.z);
        CollisionSender.gameController = this;
        _gameScoreSystem = new ScoreSystem();

        ClearAllMovables();
        CreatePlayer();
        
        OnGameStart?.Invoke(this, EventArgs.Empty);
    }
    void EndGame()
    {
        AddToDestroyQueue(_playerMovable, EventArgs.Empty);
        _playerTransform = null;
        _playerMovable = null;
        _playerArmament = null;
        _input.Keyboard.Move.performed -= MovePlayer;
        _input.Keyboard.Move.canceled -= MovePlayer;
        _input.Keyboard.Fire.performed -= Fire;
        _input.Keyboard.AltFire.performed -= AltFire;

        OnGameEnd?.Invoke(this, EventArgs.Empty);
    }
    void FixedUpdate()
    {
        foreach(KeyValuePair<Transform, Movable> transformPair in _transformsDict)
        {
            UpdateMovableTransform(transformPair.Key, transformPair.Value);
        }
        if (_movablesToDestroy.Count > 0)
        {
            foreach(Movable movable in _movablesToDestroy)
            {
                DestroyMovable(movable);
            }
            _movablesToDestroy.Clear();
        }
    }
    void UpdateMovableTransform(Transform trans, Movable movable)
    {
        movable.Update(Time.deltaTime);
        trans.rotation = Quaternion.AngleAxis(movable.Angle, Vector3.up);
        trans.position = new Vector3(movable.X, 0, movable.Y);
    }
    public GameObject CreateObject(Movable mov)
    {
        GameObject objPrefab;
        if (!_objectsId.GetObject(mov.ViewId, out objPrefab))
            return null;
        var newObject = Instantiate(objPrefab);
        newObject.transform.position = new Vector3(mov.X, 0, mov.Y);
        newObject.transform.rotation = Quaternion.AngleAxis(mov.Angle, Vector3.up);
        _transformsDict.Add(newObject.transform, mov);
        _movablesDict.Add(mov, newObject.transform);
        mov.OnDeath += AddToDestroyQueue;
        if (mov is Destroyable)
            mov.OnDeath += _gameScoreSystem.AddScore;
        return newObject;
    }
    void AddToDestroyQueue(object obj, EventArgs args)
    {
        Movable movable = (Movable)obj;
        if (movable != null)
            _movablesToDestroy.Add(movable);
    }
    void ClearAllMovables()
    {
        foreach(KeyValuePair<Transform, Movable> movable in _transformsDict)
        {
            AddToDestroyQueue(movable.Value, EventArgs.Empty);
        }
    }
    void DestroyMovable(Movable movable)
    {
        if (movable == null)
            return;
        if (!_movablesDict.ContainsKey(movable))
            return;
        var trans = _movablesDict[movable];

        _movablesDict.Remove(movable);
        _transformsDict.Remove(trans);
        Destroy(trans.gameObject);
    }
    public void CollideHandle(Transform transform, Collider collider)
    {
        if (!_transformsDict.ContainsKey(transform) || !_transformsDict.ContainsKey(collider.transform))
            return;
        _transformsDict[transform].Collision();
        _transformsDict[collider.transform].Collision();
    }
    /// Player
    void CreatePlayer()
    {
        var player = new AccelBody(_playerAcceleration, _playerAcceleration * 1.5f, _playerAngularSpeed);
        player.ViewId = 1;
        player.OnCollision += PlayerDeath;
        GameObject obj;

        _objectsId.GetObject(player.ViewId, out obj);
        var playerObj = Instantiate(obj);
        _playerTransform = playerObj.transform;
        _transformsDict.Add(playerObj.transform, player);
        _movablesDict.Add(player, playerObj.transform);
        _playerMovable = player;

        _playerArmament = new Armament(player);

        var primWeapon = new Weapon(_playerArmament, 20f, lifetime: 1f);
        primWeapon.OnShoot += Shoot;

        var secWeapon = new ChargingWeapon(_playerArmament, 100f, 10f, 20f, 0f, true, 0.1f, 4);
        secWeapon.OnShoot += Shoot;

        _playerArmament.AddWeapon(primWeapon);
        _playerArmament.AddWeapon(secWeapon);

        player.AttachedArmament = _playerArmament;
    }
    void PlayerDeath(object o, EventArgs args)
    {
        EndGame();
    }
    void MovePlayer(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
        if (_moveInput.y > 0)
        {
            _playerMovable.Accelerate(1);
        }
        else
        {
            _playerMovable.Accelerate(0);
        }
        _playerMovable.Rotate((int)_moveInput.x);
    }
    void Fire(InputAction.CallbackContext ctx)
    {
        _playerMovable.AttachedArmament?.Shoot(0);
    }
    void AltFire(InputAction.CallbackContext ctx)
    {
        _playerMovable.AttachedArmament?.Shoot(1);
    }
    void Shoot(object weapon, ShootArgs shootArgs)
    {
        Projectile projectile = shootArgs.projectile;
        CreateObject(projectile);
    }
}

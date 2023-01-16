using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Asteroids.Core;
using Asteroids.Weapons;
using Asteroids.Score;
using Asteroids.Movables;
using TMPro;
public class GameUI : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private Image _laserChargeBar;
    [SerializeField] private TextMeshProUGUI _loseTextMesh;
    [SerializeField] private TextMeshProUGUI _shotsTextMesh;
    [SerializeField] private TextMeshProUGUI _scoreTextMesh;
    [SerializeField] private TextMeshProUGUI _angleTextMesh;
    [SerializeField] private TextMeshProUGUI _velocityTextMesh;
    [SerializeField] private TextMeshProUGUI _positionTextMesh;
    [SerializeField] private GameObject _restartButton;
    void Start()
    {
        _gameController.OnGameStart += GameStart;
        _gameController.OnGameEnd += GameEnd;
    }
    public void InitPlayer(AccelBody player)
    {
        player.OnAngleChange += AngleChange;
        player.OnPositionChange += PositionChange;
        player.OnVelocityChange += VelocityChange;
    }
    public void InitWeapon(Armament trackingArmament)
    {
        List<Weapon> weapons = trackingArmament.GetWeapons();
        foreach (Weapon weapon in weapons)
        {
            if (weapon is ChargingWeapon)
            {
                var chargingWeapon = weapon as ChargingWeapon;
                chargingWeapon.OnChargeChange += LaserChargeChange;
            }
        }
    }

    public void LaserChargeChange(object o, ChargeArgs args)
    {
        _laserChargeBar.fillAmount = args.ChargeRatio;
        _shotsTextMesh.text = ((int)(args.CurrentCharge/args.Cost)).ToString();
    }
    public void AngleChange(object o, EventArgs args)
    {
        if (!(o is AccelBody))
            return;
        _angleTextMesh.text = "Угол: " + ((int)(o as AccelBody).Angle).ToString();
    }
    public void PositionChange(object o, EventArgs args)
    {
        if (!(o is AccelBody))
            return;
        AccelBody ab = o as AccelBody;
        _positionTextMesh.text = "Координаты: " + ab.Position.X.ToString("N2") + " " + ab.Position.Y.ToString("N2");
    }
    public void VelocityChange(object o, EventArgs args)
    {
        if (!(o is AccelBody))
            return;
        AccelBody ab = o as AccelBody;
        _velocityTextMesh.text = "Скорость: " + ab.Velocity.X.ToString("N2") + " " + ab.Velocity.Y.ToString("N2");
    }
    public void ScoreTextChange(object o, EventArgs args)
    {
        if (!(o is ScoreSystem))
            return;
        _scoreTextMesh.text = "Очки: " + (o as ScoreSystem).Score.ToString();
    }
    public void GameEnd(object o, EventArgs args)
    {
        _gameController.GameScoreSystem.OnScoreChange -= ScoreTextChange;
        _loseTextMesh.text = "Проигрыш! =)\nСчёт: " + _scoreTextMesh.text;
        _loseTextMesh.enabled = true;
        _restartButton.SetActive(true);
    }
    public void GameStart(object o, EventArgs args)
    {
        _gameController.GameScoreSystem.OnScoreChange += ScoreTextChange;
        InitPlayer(_gameController.Player);
        InitWeapon(_gameController.PlayerArmament);

        _scoreTextMesh.text = "Очки: 0";
        _velocityTextMesh.text = "Скорость: ";
        _positionTextMesh.text = "Координаты: ";
        _angleTextMesh.text = "Угол: ";

        _restartButton.SetActive(false);
    }
}

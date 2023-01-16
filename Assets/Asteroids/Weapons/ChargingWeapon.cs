using System;
using Asteroids.Core;

namespace Asteroids.Weapons
{
    public class ChargeArgs: EventArgs
    {
        public ChargeArgs(float currentCharge, float maxCharge, float cost)
        {
            CurrentCharge = currentCharge;
            MaxCharge = maxCharge;
            Cost = cost;
            ChargeRatio = CurrentCharge / MaxCharge;
        }
        public float CurrentCharge;
        public float MaxCharge;
        public float ChargeRatio;
        public float Cost;
    }
    public class ChargingWeapon : Weapon
    {
        protected bool _charging = true;
        public bool Charging { get { return _charging; } set { _charging = value; } }
        private float _currentCharge;
        protected float _maxCharge;
        protected float _chargeSpeed;
        protected float _shootCost;
        public EventHandler<ChargeArgs> OnChargeChange;
        public ChargingWeapon(Armament attachedTo, float maxCharge, float chargeSpeed, float shootCost, float projectileSpeed = 1f, bool piercing = false, float lifetime = 2f, int viewId = 2)
        {
            _attachedArmament = attachedTo;
            _projectileSpeed = projectileSpeed;
            _maxCharge = maxCharge;
            _chargeSpeed = chargeSpeed;
            _shootCost = shootCost;
            _currentCharge = maxCharge;
            _piercing = piercing;
            _lifetime = lifetime;
            _projectileViewId = viewId;
        }
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (_charging)
                ChangeCharge(_chargeSpeed * deltaTime);
        }
        public override void Shoot()
        {
            if (_currentCharge >= _shootCost)
            {
                ChangeCharge(-_shootCost);
                base.Shoot();
            }
        }
        public virtual void ChangeCharge(float val, bool dif = true)
        {
            if (dif)
                _currentCharge = Math.Clamp(_currentCharge + val, 0, _maxCharge);
            else
                _currentCharge = Math.Clamp(val, 0, _maxCharge);
            OnChargeChange?.Invoke(this, new ChargeArgs(_currentCharge, _maxCharge, _shootCost));
        }
    }
}

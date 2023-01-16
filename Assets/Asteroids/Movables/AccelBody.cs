using System;
using System.Numerics;
using Asteroids.Core;

namespace Asteroids.Movables
{
    public class AccelBody: Movable
    {
        protected float _angularSpeed = 0f;
        protected float _currentAngularSpeed = 0f;
        protected float _acceleration = 0f;
        protected float _currentAcceleration = 0f;
        protected float _maxVelocity = 0f;
        protected Armament _attachedArmament = null;
        public Armament AttachedArmament { get { return _attachedArmament; } set { _attachedArmament = value; } }
        public EventHandler OnVelocityChange;
        public EventHandler OnAngleChange;
        public EventHandler OnPositionChange;
        public AccelBody(float acceleration, float maxVelocity, float angularSpeed)
        {
            _acceleration = acceleration;
            _maxVelocity = maxVelocity;
            _angularSpeed = angularSpeed;
        }
        public void Rotate(int dir)
        {
            _currentAngularSpeed = _angularSpeed * dir;
        }
        public void Accelerate(int dir)
        {
            _currentAcceleration = _acceleration * dir;
        }
        public override void Update(float deltaTime)
        {
            Vector2 prevPos = _position;
            if (_currentAngularSpeed != 0)
            {
                _angle += _currentAngularSpeed * deltaTime;
                OnAngleChange?.Invoke(this, EventArgs.Empty);
            }
            if (_angle < 0)
                _angle += 360;
            if (_angle > 360)
                _angle -= 360;

            if (_currentAcceleration != 0)
            {
                Vector2 newVelocity = new Vector2();
                float cos = MathF.Cos(-_angle * MathF.PI / 180);
                float sin = MathF.Sin(-_angle * MathF.PI / 180);
                newVelocity.X = _velocity.X + cos * _currentAcceleration * deltaTime;
                newVelocity.Y = _velocity.Y + sin * _currentAcceleration * deltaTime;
                float velMagn = MathF.Sqrt( newVelocity.X * newVelocity.X + newVelocity.Y * newVelocity.Y );
                _velocity = newVelocity * (Math.Clamp(velMagn, 0, _maxVelocity) / velMagn);
                OnVelocityChange?.Invoke(this, EventArgs.Empty);
            }
            else if (_velocity != Vector2.Zero)
            {
                _velocity = _velocity - (Vector2.Normalize(_velocity) * deltaTime * _acceleration);
                OnVelocityChange?.Invoke(this, EventArgs.Empty);
                if (_velocity.X >= -0.1f && _velocity.Y >= -0.1f && _velocity.X <= 0.1f && _velocity.Y <= 0.1f)
                    _velocity = Vector2.Zero;
            }
            
            if (_attachedArmament != null)
                _attachedArmament.Update(deltaTime);
            base.Update(deltaTime);

            if (prevPos != _position)
                OnPositionChange?.Invoke(this, EventArgs.Empty);
        }
    }
}

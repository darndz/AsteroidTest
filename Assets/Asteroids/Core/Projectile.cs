using System;
using System.Numerics;
using Asteroids.Core;

namespace Asteroids.Movables
{
    public class Projectile : Movable
    {
        protected float _lifetime;
        protected bool _piercing;
        public Projectile(Vector2 position, Vector2 velocity, float angle = 0f, float lifetime = 2f, bool piercing = false, int viewId = 2)
        {
            _position = position;
            _velocity = velocity;
            _angle = angle;
            _lifetime = lifetime;
            _piercing = piercing;
            _viewId = viewId;
        }
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            _lifetime -= deltaTime;
            if (_lifetime <= 0)
                Die();
        }
        public override void Collision()
        {
            base.Collision();
            if (!_piercing)
                Die();
        }
    }
}
using System;
using System.Numerics;
using Asteroids.Movables;

namespace Asteroids.Core
{
    public class ShootArgs: EventArgs
    {
        public ShootArgs(Projectile proj)
        {
            projectile = proj;
        }
        public Projectile projectile;
    }
    public class Weapon
    {
        public event EventHandler<ShootArgs> OnShoot;
        protected Armament _attachedArmament;
        protected float _projectileSpeed;
        protected bool _piercing;
        protected float _lifetime;
        protected int _projectileViewId;
        public Weapon() {}
        public Weapon(Armament attachedTo, float projectileSpeed = 1f, bool piercing = false, float lifetime = 2f, int viewId = 2)
        {
            _attachedArmament = attachedTo;
            _projectileSpeed = projectileSpeed;
            _piercing = piercing;
            _lifetime = lifetime;
            _projectileViewId = viewId;
            // в принципе, можно было создавать конкретный "прожектайл" и задавать его оружию в конструкторе
            // но в данный момент это излишнее + "база" в виде прожектайла как класса есть для такой реализации
        }
        public virtual void Update(float deltaTime) {}
        public virtual void Shoot()
        {

            Movable attachedMovable = _attachedArmament.AttachedMovable;
            float cos = MathF.Cos(-attachedMovable.Angle * MathF.PI / 180);
            float sin = MathF.Sin(-attachedMovable.Angle * MathF.PI / 180);
            Vector2 velocity = new Vector2(cos * _projectileSpeed, sin * _projectileSpeed);
            Projectile proj = new Projectile(attachedMovable.Position, velocity, attachedMovable.Angle, _lifetime, _piercing, _projectileViewId);
            ShootArgs args = new ShootArgs(proj);
            OnShoot?.Invoke(this, args);
        }
    }
}

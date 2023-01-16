using System;
using System.Collections.Generic;

namespace Asteroids.Core
{
    public class Armament
    {
        protected List<Weapon> _weapons = new List<Weapon>();
        protected Movable _attachedMovable;
        public Movable AttachedMovable { get { return _attachedMovable; } set { _attachedMovable = value; } }
        public EventHandler OnWeaponAdd;
        public EventHandler OnWeaponRemove;

        public Armament(Movable attachedTo)
        {
            _attachedMovable = attachedTo;
        }
        public virtual List<Weapon> GetWeapons()
        {
            return _weapons;
        }
        public virtual void AddWeapon(Weapon weapon)
        {
            _weapons.Add(weapon);
            OnWeaponAdd?.Invoke(this, EventArgs.Empty);
        }
        public virtual void RemoveWeapon(Weapon weapon)
        {
            if (_weapons.Contains(weapon))
            {
                _weapons.Remove(weapon);
                OnWeaponRemove?.Invoke(this, EventArgs.Empty);
            }
        }
        public virtual void Update(float deltaTime)
        {
            foreach (Weapon weapon in _weapons)
                weapon.Update(deltaTime);
        }
        public virtual void Shoot(int index)
        {
            if (index >= _weapons.Count)
                return;
            Shoot(_weapons[index]);
        }
        public virtual void Shoot(Weapon weapon)
        {
            weapon.Shoot();
        }
    }
}

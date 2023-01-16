using System;
using System.Collections.Generic;
using System.Numerics;
using Asteroids.Core;
namespace Asteroids.Spawners
{
    public class SpawnArgs: EventArgs
    {
        public Movable SpawnMovable;
        public SpawnArgs(Movable movable)
        {
            SpawnMovable = movable;
        }
    }
    public class Spawner
    {
        public EventHandler<SpawnArgs> OnSpawn;
        protected SpawnArgs _spawnArgs;
        public virtual void Spawn(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Spawn();
            }
        }
        public virtual void Spawn()
        {
            OnSpawn?.Invoke(this, _spawnArgs);
        }
    }
}

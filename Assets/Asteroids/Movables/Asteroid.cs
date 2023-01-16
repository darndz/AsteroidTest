using System;
using System.Numerics;
using Asteroids.Core;
using Asteroids.Spawners;
using Asteroids.Utils;
namespace Asteroids.Movables
{
    public class Asteroid : Destroyable
    {
        protected int _size;
        public int Size { get { return _size; } set { _size = value; } }
        public EventHandler<SpawnArgs> OnShardCreate;
        public Asteroid(Vector2 velocity, int size, int viewId = 0, int score = 20) : base(viewId, velocity)
        {
            _velocity = velocity;
            _size = size;
            _viewId = viewId;
            _score = score;
        }
        public override void Die(object o, CollisionArgs args)
        {
            int count = 2;
            var rand = new ARandom();
            float velocityMultipler = _velocity.Length() * 3;
            for (int i = 0; i < count; i ++)
            {
                float x = rand.Range(-1, 1);
                float y = rand.Range(-1, 1);
                Vector2 velocity = new Vector2(x, y) * velocityMultipler;
                Movable movableShard;
                if (_size > 2)
                {
                    Asteroid asteroidShard  = new Asteroid(velocity, _size - 1, score: _score * 2);
                    asteroidShard.OnShardCreate = OnShardCreate;
                    movableShard = asteroidShard;
                }
                else
                {
                    movableShard = new Destroyable(0, velocity, _score * 2);
                }
                movableShard.X = X;
                movableShard.Y = Y;
                OnShardCreate?.Invoke(this, new SpawnArgs(movableShard));
            }
            base.Die();
        }
    }
}

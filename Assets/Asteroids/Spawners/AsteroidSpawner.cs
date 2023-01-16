using System;
using System.Numerics;
using Asteroids.Core;
using Asteroids.Movables;
using Asteroids.Utils;
namespace Asteroids.Spawners
{
    public class AsteroidSpawner : Spawner
    {
        private int _startSize;
        private Vector2[] _borders;
        private int _bordersInd;
        public EventHandler<SpawnArgs> OnShardCreate;
        public AsteroidSpawner(int startSize)
        {
            _startSize = startSize;
            Vector4 brd = Movable.ScreenBorders;
            _bordersInd = 0;
            _borders = new Vector2[4] {
                new Vector2(brd.X, brd.Y),
                new Vector2(brd.X, brd.W),
                new Vector2(brd.Z, brd.W),
                new Vector2(brd.Z, brd.Y),
            };
        }
        public override void Spawn()
        {
            var rand = new ARandom();
            Vector2 minVector = _borders[_bordersInd];
            Vector2 maxVector = _borders[(_bordersInd+1)%_borders.Length];
            _bordersInd = (_bordersInd+1)%_borders.Length;
            float x = rand.Range(minVector.X, maxVector.X);
            float y = rand.Range(minVector.Y, maxVector.Y);
            
            Vector2 velocity = new Vector2( rand.Range(-1,1), rand.Range(-1,1) );
            Asteroid asteroid = new Asteroid(velocity, _startSize);
            asteroid.OnShardCreate = OnShardCreate;
            asteroid.X = x;
            asteroid.Y = y;
            _spawnArgs = new SpawnArgs(asteroid);
            base.Spawn();
        }
    }
}

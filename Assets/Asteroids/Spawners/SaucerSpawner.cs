using System;
using System.Numerics;
using Asteroids.Core;
using Asteroids.Movables;
using Asteroids.Utils;
namespace Asteroids.Spawners
{
    public class SaucerSpawner : Spawner
    {
        protected Movable _playerMovable;
        private Vector2[] _borders;
        private int _bordersInd;
        public SaucerSpawner(Movable player)
        {
            _playerMovable = player;
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
            
            Vector2 velocity = new Vector2(2,0);
            Saucer saucer = new Saucer(_playerMovable, velocity, 3);
            saucer.X = x;
            saucer.Y = y;
            _spawnArgs = new SpawnArgs(saucer);
            base.Spawn();
        }
    }
}

using Asteroids.Core;
using System.Numerics;
namespace Asteroids.Movables
{
    public class Saucer: Destroyable
    {
        protected Movable _playerMovable;
        protected float _speed;
        public Saucer(Movable player, Vector2 velocity, int viewId = 3, int score = 150) : base(viewId, velocity)
        {
            _playerMovable = player;
            _velocity = velocity;
            _speed = velocity.Length();
            _viewId = viewId;
            _score = score;
        }
        public override void Update(float deltaTime)
        {
            _velocity = Vector2.Normalize(_playerMovable.Position - _position) * _speed;
            base.Update(deltaTime);
        }
    }
}
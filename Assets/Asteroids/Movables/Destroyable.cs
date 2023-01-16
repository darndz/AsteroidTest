using System.Numerics;
using Asteroids.Core;
namespace Asteroids.Movables
{
    public class Destroyable : Movable
    {
        protected int _score = 0;
        public int Score { get { return _score; } }
        public Destroyable(int viewId, Vector2 velocity, int score = 0, bool dieOnCollision = true)
        {
            _viewId = viewId;
            _velocity = velocity;
            _score += score;
            OnCollision += Die;
        }
        public virtual void Die(object o, CollisionArgs args)
        {
            Die();
        }
    }
}

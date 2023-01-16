using System;
namespace Asteroids.Utils
{
    public class ARandom
    {
        private Random _rnd;
        public ARandom()
        {
            _rnd = new Random();
        }
        public float Range(float min, float max)
        {
            return (float)(_rnd.NextDouble() * (max - min) + min);
        }
    }
}

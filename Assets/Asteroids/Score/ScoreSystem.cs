using System;
using Asteroids.Movables;
namespace Asteroids.Score
{
    public class ScoreSystem
    {
        private int _score = 0;
        public int Score { get { return _score; } set { Score = value; } }
        public EventHandler OnScoreChange;
        public void AddScore(object o, EventArgs args)
        {
            if (o is Destroyable)
            {
                ChangeScore((o as Destroyable).Score);
            }
        }
        void ChangeScore(int val, bool dif = true)
        {
            if (dif)
                _score += val;
            else
                _score = val;
            OnScoreChange?.Invoke(this, EventArgs.Empty);
        }
    }
}

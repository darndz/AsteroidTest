using System;
using System.Numerics;

namespace Asteroids.Core
{
    public class Movable
    {
        protected int _viewId = 0;
        public int ViewId { get { return _viewId; } set { _viewId = value; } }
        protected Vector2 _position = new Vector2(0,0);
        public Vector2 Position { get { return _position; } set { _position = value; } }
        public float X { get { return _position.X; } set { _position.X = value; } }
        public float Y { get { return _position.Y; } set { _position.Y = value; } }
        protected float _angle = 0;
        public float Angle { get {return _angle; } set { _angle = value; } }
        protected Vector2 _velocity = new Vector2(0,0);
        public Vector2 Velocity { get { return _velocity; } set { _velocity = value; } }
        public static Vector4 ScreenBorders = new Vector4(0,0,0,0);
        public event EventHandler<CollisionArgs> OnCollision;
        public event EventHandler OnDeath;
        public Movable() {}
        public Movable(int viewId, Vector2 velocity)
        {
            _viewId = viewId;
            _velocity = velocity;
        }
        public static void SetScreenBorders(float x, float y, float z, float w)
        {
            ScreenBorders = new Vector4(x,y,z,w);
        }
        public virtual void Rotate(float newAngle)
        {
            _angle = newAngle;
        }
        public virtual void Update(float deltaTime)
        {
            _position += _velocity * deltaTime;

            if (X < ScreenBorders.X)
                X = ScreenBorders.Z;
            else if (X > ScreenBorders.Z)
                X = ScreenBorders.X;
            if (Y < ScreenBorders.Y)
                Y = ScreenBorders.W;
            else if (Y > ScreenBorders.W)
                Y = ScreenBorders.Y;
        }
        public virtual void Collision()
        {
            OnCollision?.Invoke(this, new CollisionArgs());
        }
        public virtual void Die()
        {
            OnDeath?.Invoke(this, EventArgs.Empty);
        } 
    }
}
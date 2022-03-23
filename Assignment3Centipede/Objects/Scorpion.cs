using Microsoft.Xna.Framework;

namespace Assignment3Centipede.Objects
{
    class Scorpion : AnimatedSprite
    {
        private readonly double m_moveRate;
        Rectangle m_scorpionRec;
        GraphicsDeviceManager m_graphics;

        public Scorpion(Vector2 size, Vector2 center, double moveRate, Rectangle scorpionRec, GraphicsDeviceManager m_graphics) : base(size, center)
        {
            m_moveRate = moveRate;
            m_scorpionRec = scorpionRec;
            m_graphics = m_graphics;
        }

        public double xPos
        {
            get { return m_center.X; }
            //set { m_center.Y = value; }
        }

        public double yPos
        {
            get { return m_center.Y; }
            //set { m_center.Y = value; }
        }

        public void moveLeft(GameTime gameTime)
        {
            m_center.X -= (float)(m_moveRate * gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public void moveRight(GameTime gameTime)
        {
            m_center.X += (float)(m_moveRate * gameTime.ElapsedGameTime.TotalMilliseconds);
        }
    }
}

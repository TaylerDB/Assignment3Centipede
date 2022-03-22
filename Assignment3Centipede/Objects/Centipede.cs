using Microsoft.Xna.Framework;


namespace Assignment3Centipede.Objects
{
    class Centipede : AnimatedSprite
    {
        private readonly double m_moveRate;
        Rectangle m_CentipedeRec;
        GraphicsDeviceManager m_graphics;

        int hitCentipede = 0;

        public Centipede(Vector2 size, Vector2 center, double moveRate, Rectangle fleeRec, GraphicsDeviceManager m_graphics) : base(size, center)
        {
            m_moveRate = moveRate;
            m_CentipedeRec = fleeRec;
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

        public int HitCentipede
        {
            get { return hitCentipede; }
            set { hitCentipede = value; }
        }

        public void moveRight(GameTime gameTime)
        {
            m_center.X += (float)(m_moveRate * gameTime.ElapsedGameTime.TotalMilliseconds);
            //m_center.X += 2; // (int)m_moveRate;
        }

        public void moveLeft(GameTime gameTime)
        {
            m_center.X -= (float)(m_moveRate * gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public void moveDown(GameTime gameTime)
        {
            m_center.Y += (float)(m_moveRate * gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public void moveUp(GameTime gameTime)
        {
            m_center.Y -= (float)(m_moveRate * gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public void hit()
        {
            hitCentipede++;
        }
    }
}

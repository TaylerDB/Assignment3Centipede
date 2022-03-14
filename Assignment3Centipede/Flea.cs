using Microsoft.Xna.Framework;

namespace Assignment3Centipede.Objects
{
    class Flea : AnimatedSprite
    {
        private readonly double m_moveRate;
        Rectangle m_fleeRec;
        GraphicsDeviceManager m_graphics;
        
        int hitFlee = 0;


        public Flea(Vector2 size, Vector2 center, double moveRate, Rectangle fleeRec, GraphicsDeviceManager m_graphics) : base(size, center)
        {
            m_moveRate = moveRate;
            m_fleeRec = fleeRec;
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

        public int HitFlee
        {
            get { return hitFlee; }
            set { hitFlee = value; }
        }

        public void moveDown(GameTime gameTime)
        {
            m_center.Y += (float)(m_moveRate * gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public void hit()
        {
            hitFlee++;
        }
    }
}

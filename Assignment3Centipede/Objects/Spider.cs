using Microsoft.Xna.Framework;

namespace Assignment3Centipede.Objects
{
    class Spider : AnimatedSprite
    {
        private readonly double m_moveRate;
        Rectangle m_spiderRec;
        GraphicsDeviceManager m_graphics;

        int hitSpider = 0;

        public Spider(Vector2 size, Vector2 center, double moveRate, Rectangle spiderRec, GraphicsDeviceManager m_graphics) : base(size, center)
        {
            m_moveRate = moveRate;
            m_spiderRec = spiderRec;
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
            get { return hitSpider; }
            set { hitSpider = value; }
        }

        public void moveDown(GameTime gameTime)
        {
            m_center.Y += (float)(m_moveRate * gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public void hit()
        {
            hitSpider++;
        }
    }
}

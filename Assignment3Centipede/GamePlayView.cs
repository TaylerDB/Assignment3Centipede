using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Assignment3Centipede
{
    public class GamePlayView : GameStateView
    {
        private SpriteFont m_font;
        private const string MESSAGE = "Isn't this game fun!";

        // Rectangles
        private Rectangle m_NormMushBox;
        private Rectangle m_ShipBox;
        private Rectangle m_BulletBox;

        // Textures
        private Texture2D m_NormMush1Texture;
        private Texture2D m_NormMush2Texture;
        private Texture2D m_NormMush3Texture;
        private Texture2D m_NormMush4Texture;

        private Texture2D m_BulletTexture;
        private Texture2D m_ShipTexture;
        
        private int shipxPos = 0;

        // Call Mushroom class
        HelpView helpView = new HelpView();
        Mushroom mushroom = new Mushroom();
        Ship ship = new Ship();

        // Data structures
        Dictionary<int,Rectangle> mushroomList = new Dictionary<int, Rectangle>();
        List<Rectangle> bulletList = new List<Rectangle>();

        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");

            // Setup textures
            m_NormMush1Texture = contentManager.Load<Texture2D>("Images/MushNorm/NormMush0");
            m_NormMush2Texture = contentManager.Load<Texture2D>("Images/MushNorm/NormMush1");
            m_NormMush3Texture = contentManager.Load<Texture2D>("Images/MushNorm/NormMush2");
            m_NormMush4Texture = contentManager.Load<Texture2D>("Images/MushNorm/NormMush3");

            m_BulletTexture = contentManager.Load<Texture2D>("Images/Ship/Bullet");
            m_ShipTexture = contentManager.Load<Texture2D>("Images/Ship/Ship");
            
            //m_NormMushBox = new Rectangle(50, 50, 25, 25);
            Random xPos = new Random();
            Random yPos = new Random();

            // Populate Mushroom list - use a dictionary
            for (int i = 0; i <75; i++)
            {
                mushroomList.Add(i, m_NormMushBox = new Rectangle(xPos.Next(m_graphics.GraphicsDevice.Viewport.Width - 25),
                    yPos.Next(25, m_graphics.GraphicsDevice.Viewport.Height - 100), 25, 25));
            }

            m_ShipBox = new Rectangle(m_graphics.GraphicsDevice.Viewport.Width / 2, m_graphics.GraphicsDevice.Viewport.Height - 25, 25, 25);
            
            ship = new Ship(m_ShipBox, m_graphics);

        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }

            // Move ship         
            ship.moveX();
            m_ShipBox.X = ship.xPos;
            ship.moveY();
            m_ShipBox.Y = ship.yPos;


            // I could just use this and not my ship class for movement

            //if (Keyboard.GetState().IsKeyDown(Keys.Up) && m_ShipBox.Y != (m_graphics.GraphicsDevice.Viewport.Height * .7))
            //{
            //    m_ShipBox.Y -= 5;
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.Down) && m_ShipBox.Y != m_graphics.GraphicsDevice.Viewport.Height - 25)
            //{
            //    m_ShipBox.Y += 5;
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.Left) && m_ShipBox.X != 0)
            //{
            //    m_ShipBox.X -= 5;
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.Right) && m_ShipBox.X != m_graphics.GraphicsDevice.Viewport.Width - 25)
            //{
            //    m_ShipBox.X += 5;
            //}

            // Shoot bullets
            if (Keyboard.GetState().IsKeyDown(helpView.Shoot))
            {
                //Thread.Sleep(100);
                bulletList.Add(new Rectangle(m_ShipBox.X + 5, m_ShipBox.Y - 25, 15, 30));
            }

            return GameStateEnum.GamePlay;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            Vector2 stringSize = m_font.MeasureString(MESSAGE);
            m_spriteBatch.DrawString(m_font, MESSAGE,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, m_graphics.PreferredBackBufferHeight / 2 - stringSize.Y), Color.Yellow);

            // Draw mushrooms - call a mushroomRender class, pass m_spriteBatch to constructer
            foreach (var value in mushroomList)
            {
                if (mushroom.Hit == 0)
                {
                    m_spriteBatch.Draw(m_NormMush1Texture, value.Value, Color.White);
                }
                if (mushroom.Hit == 1)
                {
                    m_spriteBatch.Draw(m_NormMush2Texture, value.Value, Color.White);
                }
                if (mushroom.Hit == 2)
                {
                    m_spriteBatch.Draw(m_NormMush3Texture, value.Value, Color.White);
                }
                if (mushroom.Hit == 3)
                {
                    m_spriteBatch.Draw(m_NormMush4Texture, value.Value, Color.White);
                }
            }

            // Draw ship
            m_spriteBatch.Draw(m_ShipTexture, m_ShipBox, Color.White);

            // Draw bullet
            if (bulletList.Count > 0)
            {
                foreach (var value in bulletList)
                {              
                    m_spriteBatch.Draw(m_BulletTexture, value, Color.White);
                }
            }

            m_spriteBatch.End();            
        }

        public override void update(GameTime gameTime)
        {
            // TODO : Get bullets to move up screen

            if (bulletList.Count > 0)
            {
                for (int i = 0; i < bulletList.Count; i++)
                {
                    var bullet = bulletList[i];
                    bullet.Y -= 5;

                    if (bullet.Y < 0)
                        bulletList.RemoveAt(i);
                    else
                        bulletList[i] = bullet;
                    //bulletList[i].Y -= 5;
                }
            }
            //m_BulletBox.Y -= 5;
        }
    }
}

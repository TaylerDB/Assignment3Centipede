using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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
        Mushroom mushroom = new Mushroom();

        List<Rectangle> mushroomList = new List<Rectangle>();

        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");

            // Setup textures
            m_NormMush1Texture = contentManager.Load<Texture2D>("Images/MushNorm/MushNorm1");
            m_NormMush2Texture = contentManager.Load<Texture2D>("Images/MushNorm/MushNorm2");
            m_NormMush3Texture = contentManager.Load<Texture2D>("Images/MushNorm/MushNorm3");
            m_NormMush4Texture = contentManager.Load<Texture2D>("Images/MushNorm/MushNorm4");

            m_BulletTexture = contentManager.Load<Texture2D>("Images/Ship/Bullet");
            m_ShipTexture = contentManager.Load<Texture2D>("Images/Ship/Ship");
            

            //m_NormMushBox = new Rectangle(50, 50, 25, 25);
            Random xPos = new Random();
            Random yPos = new Random();

            // Populate Mushroom list
            for (int i = 0; i <75; i++)
            {
                mushroomList.Add(m_NormMushBox = new Rectangle(xPos.Next(m_graphics.GraphicsDevice.Viewport.Width - 25),
                    yPos.Next(25, m_graphics.GraphicsDevice.Viewport.Height - 100), 25, 25));
            }

            m_ShipBox = new Rectangle(m_graphics.GraphicsDevice.Viewport.Width / 2, m_graphics.GraphicsDevice.Viewport.Height - 25, 25, 25);
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && m_ShipBox.Y != (m_graphics.GraphicsDevice.Viewport.Height * .7))
            {
                m_ShipBox.Y -= 5;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && m_ShipBox.Y != m_graphics.GraphicsDevice.Viewport.Height - 25)
            {
                m_ShipBox.Y += 5;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && m_ShipBox.X != 0)
            {
                m_ShipBox.X -= 5;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && m_ShipBox.X != m_graphics.GraphicsDevice.Viewport.Width - 25)
            {
                m_ShipBox.X += 5;
            }

            return GameStateEnum.GamePlay;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            Vector2 stringSize = m_font.MeasureString(MESSAGE);
            m_spriteBatch.DrawString(m_font, MESSAGE,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, m_graphics.PreferredBackBufferHeight / 2 - stringSize.Y), Color.Yellow);

            // Draw mushrooms
            foreach (var value in mushroomList)
            {
                if (mushroom.Hit == 0)
                {
                    m_spriteBatch.Draw(m_NormMush1Texture, value, Color.White);
                }
                if (mushroom.Hit == 1)
                {
                    m_spriteBatch.Draw(m_NormMush2Texture, value, Color.White);
                }
                if (mushroom.Hit == 2)
                {
                    m_spriteBatch.Draw(m_NormMush3Texture, value, Color.White);
                }
                if (mushroom.Hit == 3)
                {
                    m_spriteBatch.Draw(m_NormMush4Texture, value, Color.White);
                }
            }

            // Draw ship
            m_spriteBatch.Draw(m_ShipTexture, m_ShipBox, Color.White);
            

            m_spriteBatch.End();            
        }

        public override void update(GameTime gameTime)
        {
            
        }
    }
}

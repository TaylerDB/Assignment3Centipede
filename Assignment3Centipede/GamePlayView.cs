using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment3Centipede
{
    public class GamePlayView : GameStateView
    {
        // Consts
        const double fireRate = 0.15f;
        const int bulletSpeed = 10;

        private SpriteFont m_font;
        private const string MESSAGE = "Isn't this game fun!";

        // Rectangles
        private Rectangle m_MushroomBox;
        private Rectangle m_ShipBox;

        // Textures
        private Texture2D m_NormMush1Texture;
        private Texture2D m_NormMush2Texture;
        private Texture2D m_NormMush3Texture;
        private Texture2D m_NormMush4Texture;

        private Texture2D m_BulletTexture;
        private Texture2D m_ShipTexture;
        
        // Call Mushroom class
        HelpView helpView = new HelpView();
        Mushroom mushroom = new Mushroom();
        Ship ship = new Ship();

        TimeSpan fireRateTimer = TimeSpan.FromSeconds(0);

        // Data structures
        List<Rectangle> mushroomList = new List<Rectangle>();

        List<object> mushroomClassList = new List<object>();

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

            // Populate Mushroom list and class
            for (int i = 0; i < 75; i++)
            {
                // Add to mushroom class
                mushroomClassList.Add(new Mushroom());

                // Add to mushroom dictionary
                mushroomList.Add(new Rectangle(xPos.Next(m_graphics.GraphicsDevice.Viewport.Width - 25),
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

            // Shoot bullets
            if (Keyboard.GetState().IsKeyDown(helpView.Shoot))
            {
                // Check game time for fire rate
                if (fireRateTimer.TotalSeconds > fireRate)
                {
                    // Add bullet to list
                    bulletList.Add(new Rectangle(m_ShipBox.X + 5, m_ShipBox.Y - 25, 15, 30));
                    
                    // Reset fire rate timer to zero
                    fireRateTimer = TimeSpan.FromSeconds(0);
                }
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
            for (int i = 0; i < mushroomList.Count; i++)
            {
                var mush = (Mushroom)mushroomClassList.ElementAt(i);
                if (mush.Hit == 0)
                {
                    m_spriteBatch.Draw(m_NormMush1Texture, mushroomList[i], Color.White);
                    mushroomClassList[i] = mush;
                }
                if (mush.Hit == 1)
                {
                    m_spriteBatch.Draw(m_NormMush2Texture, mushroomList[i], Color.White);
                    mushroomClassList[i] = mush;
                }
                if (mush.Hit == 2)
                {
                    m_spriteBatch.Draw(m_NormMush3Texture, mushroomList[i], Color.White);
                    mushroomClassList[i] = mush;
                }
                if (mush.Hit == 3)
                {
                    m_spriteBatch.Draw(m_NormMush4Texture, mushroomList[i], Color.White);
                    mushroomClassList[i] = mush;
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
            // Increase fire rate timer
            fireRateTimer += gameTime.ElapsedGameTime;
            
            // Update bullet position
            if (bulletList.Count > 0)
            {
                for (int i = 0; i < bulletList.Count; i++)
                {
                    bool bulletAlive = true;

                    var bullet = bulletList[i];
                    bullet.Y -= bulletSpeed;

                    // Check for collison on mushrooms
                    for (int m = 0; m < mushroomList.Count; m++)
                    {
                        // Get a copy
                        var mushList = mushroomList[m];
                        var mush = (Mushroom)mushroomClassList.ElementAt(m);

                        // Check if bullet is within parameters of mushroom
                        if (((bullet.X >= mushList.X - 25 ) && (bullet.X <= mushList.X + 25)) && ((bullet.Y >= mushList.Y - 25) && (bullet.Y <= mushList.Y + 25)))
                        {
                            // Tell mushroom
                            mush.hitMushroom();

                            // Remove bullet
                            bulletList.RemoveAt(i);
                            bulletAlive = false;
                        }

                        // Check if mushroom needs to be removed
                        if (mush.Hit > 3)
                        {
                            mushroomClassList.RemoveAt(m);
                            mushroomList.RemoveAt(m);
                        }
                        else
                        {
                            // Set copy back to original
                            mushroomClassList[m] = mush;
                            mushroomList[m] = mushList;
                        }
                    }

                    // Check if bullet is still alive
                    if (bulletAlive)
                    {
                        // Kill bullet if it reaches top of the screen
                        if (bullet.Y < 0)
                            bulletList.RemoveAt(i);

                        // Set copy back to original
                        else
                            bulletList[i] = bullet;
                    }
                }
            }
        }
    }
}

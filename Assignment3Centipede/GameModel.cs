﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3Centipede
{
    class GameModel
    {
        private GraphicsDeviceManager graphics;


        // Consts
        const double fireRate = 0.15f;
        private const int shipMovementSpeed = 5;
        const int bulletSpeed = 10;


        private SpriteFont m_font;
        private const string MESSAGE = "Isn't this game fun!";
        int score = 0;
        int highScore = 0;

        // Rectangles
        //private Rectangle m_MushroomBox;
        private Rectangle m_ShipBox;
        private Rectangle m_ShipBoxLife1;
        private Rectangle m_ShipBoxLife2;
        private Rectangle m_ShipBoxLife3;

        // Textures
        private Texture2D m_NormMush1Texture;
        private Texture2D m_NormMush2Texture;
        private Texture2D m_NormMush3Texture;
        private Texture2D m_NormMush4Texture;

        private Texture2D m_BulletTexture;
        private Texture2D m_ShipTexture;

        HelpView helpView = new HelpView();
        //Mushroom mushroom = new Mushroom();
        Ship ship = new Ship();

        

        TimeSpan fireRateTimer = TimeSpan.FromSeconds(0);

        // Data structures
        List<Rectangle> mushroomList = new List<Rectangle>();

        public List<Rectangle> MushroomList
        {
            get { return mushroomList; }
            set { mushroomList = value; }
        }

        List<object> mushroomClassList = new List<object>();

        List<Rectangle> bulletList = new List<Rectangle>();

        //private Player m_player = new Player();
        //private Centipede m_centipede = new Centipede();
        // private List<Bullet> m_bullets = new ...

        //private KeyboardInput m_input = new KeyboardInput();

        //public Shi Player
        //{
        //    get { return m_player; }
        //}

        //public Centipede Centipede
        //{
        //    get { return m_centipede; }
        //}


        public void loadContent(ContentManager contentManager, GraphicsDeviceManager m_graphics)
        {
            graphics = m_graphics;

            m_font = contentManager.Load<SpriteFont>("Fonts/game");

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
            
            int randX = 0;
            int randY = 0;

            // Populate Mushroom list and class
            for (int i = 0; i < 75; i++)
            {
                // Add to mushroom class
                mushroomClassList.Add(new Mushroom());

                // Make mushrooms in a grid size
                randX = xPos.Next(m_graphics.GraphicsDevice.Viewport.Width - 25);
                while (randX % 30 != 0)
                {
                    randX = xPos.Next(m_graphics.GraphicsDevice.Viewport.Width - 25);
                }

                randY = yPos.Next(50, m_graphics.GraphicsDevice.Viewport.Height - 100);
                while (randY % 30 != 0)
                {
                    randY = yPos.Next(50, m_graphics.GraphicsDevice.Viewport.Height - 100);
                }

                // Add to mushroom list
                mushroomList.Add(new Rectangle(randX, randY, 25, 25));
            }

            // Set up player
            m_ShipBox = new Rectangle(m_graphics.GraphicsDevice.Viewport.Width / 2, m_graphics.GraphicsDevice.Viewport.Height - 25, 25, 25);
            ship = new Ship(m_ShipBox, m_graphics);

            // Draw lives
            m_ShipBoxLife1 = new Rectangle(m_graphics.GraphicsDevice.Viewport.Width / 5, 10, 25, 25);
            m_ShipBoxLife2 = new Rectangle((m_graphics.GraphicsDevice.Viewport.Width / 5) + 30, 10, 25, 25);
            m_ShipBoxLife3 = new Rectangle((m_graphics.GraphicsDevice.Viewport.Width / 5) + 60, 10, 25, 25);
        }

        public void initialize()
        {
            // Read the controls from persistence
            //m_input.registerHandle(Keys.Left, moveLeft);
        }


        public void processInput(GameTime gameTime)
        {

            moveShip();

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
        }

        private void moveShip()
        {
            // Move right
            if ((Keyboard.GetState().IsKeyDown(helpView.MoveRight)) && (m_ShipBox.X < graphics.GraphicsDevice.Viewport.Width - 30))
            {
                //int a = gameModel.MushroomList.Count;
                int canMoveRight = 0;

                for (int i = 0; i < mushroomList.Count; i++)
                {
                    var mushroom = mushroomList[i];
                    //                                                                                                                          - 30
                    if ((m_ShipBox.X < mushroom.X - 30) || (m_ShipBox.X >= mushroom.X + 30) || (m_ShipBox.Y >= mushroom.Y + 25) || (m_ShipBox.Y + 30 <= mushroom.Y))
                    {
                        // Increment if it can move
                        canMoveRight++;                        
                    }
                    mushroomList[i] = mushroom;
                }

                // Only move if all mushrooms are out of the way
                if (canMoveRight == mushroomList.Count)
                {
                    //m_ShipBox.Y -= shipMovementSpeed;
                    ship.moveRight();
                    m_ShipBox.X = ship.xPos;
                }
            }

            // Move left
            if ((Keyboard.GetState().IsKeyDown(helpView.MoveLeft)) && (m_ShipBox.X >= 5))
            {
                int canMoveLeft = 0;

                for (int i = 0; i < mushroomList.Count; i++)
                {
                    var mushroom = mushroomList[i];
                    //                                                                                              + 30                            -30
                    if ((m_ShipBox.X > mushroom.X + 30) || (m_ShipBox.X <= mushroom.X - 30) || (m_ShipBox.Y >= mushroom.Y + 25) || (m_ShipBox.Y + 30 <= mushroom.Y))
                    {
                        // Increment if it can move
                        canMoveLeft++;
                    }
                    mushroomList[i] = mushroom;
                }

                // Only move if all mushrooms are out of the way
                if (canMoveLeft == mushroomList.Count)
                {
                    //m_ShipBox.Y -= shipMovementSpeed;
                    ship.moveLeft();
                    m_ShipBox.X = ship.xPos;
                }
            }
            
            // Move up
            if ((Keyboard.GetState().IsKeyDown(helpView.MoveUp)) && (m_ShipBox.Y >= (graphics.GraphicsDevice.Viewport.Height * .7)))
            {
                int canMoveUp = 0;

                for (int i = 0; i < mushroomList.Count; i++)
                {
                    var mushroom = mushroomList[i];
                    //   Left  +30                                     Right - 30                               + 30                                    - 30
                    if ((m_ShipBox.X >= mushroom.X + 30) || (m_ShipBox.X <= mushroom.X - 30) || (m_ShipBox.Y >= mushroom.Y + 30) || (m_ShipBox.Y < mushroom.Y))
                    {
                        // Increment if it can move
                        canMoveUp++;

                    }
                    mushroomList[i] = mushroom;
                }

                // Only move if all mushrooms are out of the way
                if (canMoveUp == mushroomList.Count)
                {
                    //m_ShipBox.Y -= shipMovementSpeed;
                    ship.moveUp();
                    m_ShipBox.Y = ship.yPos;
                }
            }

            // Move down
            if ((Keyboard.GetState().IsKeyDown(helpView.MoveDown)) && (m_ShipBox.Y != graphics.GraphicsDevice.Viewport.Height - 25))
            {
                int canMoveDown = 0;

                for (int i = 0; i < mushroomList.Count; i++)
                {
                    var mushroom = mushroomList[i];
                    //                         + 30                               -30                                 + 30                         - 30
                    if ((m_ShipBox.X >= mushroom.X + 30) || (m_ShipBox.X <= mushroom.X -30) || (m_ShipBox.Y >= mushroom.Y) || (m_ShipBox.Y < mushroom.Y - 30))
                    {
                        // Increment if it can move
                        canMoveDown++;
                    }
                    mushroomList[i] = mushroom;
                }

                // Only move if all mushrooms are out of the way
                if (canMoveDown == mushroomList.Count)
                {
                    //m_ShipBox.Y -= shipMovementSpeed;
                    ship.moveDown();
                    m_ShipBox.Y = ship.yPos;
                }
            }
        }
   

        public void update(GameTime gameTime)
        {
            //m_player.update(gameTime);
            //m_centipede.update(gameTime);
            updateBullets(gameTime);
        }

        private void updateBullets(GameTime gameTime)
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
                        if ((bullet.X >= mushList.X - 25) && (bullet.X <= mushList.X + 25) && (bullet.Y >= mushList.Y - 25) && (bullet.Y <= mushList.Y + 25))
                        {
                            // Tell mushroom
                            mush.hitMushroom();

                            // Update score
                            score += 4;                            

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

        public void render(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager m_graphics)
        {
            spriteBatch.Begin();

            // Draw mushrooms - call a mushroomRender class, pass m_spriteBatch to constructer
            for (int i = 0; i < mushroomList.Count; i++)
            {
                var mush = (Mushroom)mushroomClassList.ElementAt(i);
                if (mush.Hit == 0)
                {
                    spriteBatch.Draw(m_NormMush1Texture, mushroomList[i], Color.White);
                    mushroomClassList[i] = mush;
                }
                if (mush.Hit == 1)
                {
                    spriteBatch.Draw(m_NormMush2Texture, mushroomList[i], Color.White);
                    mushroomClassList[i] = mush;
                }
                if (mush.Hit == 2)
                {
                    spriteBatch.Draw(m_NormMush3Texture, mushroomList[i], Color.White);
                    mushroomClassList[i] = mush;
                }
                if (mush.Hit == 3)
                {
                    spriteBatch.Draw(m_NormMush4Texture, mushroomList[i], Color.White);
                    mushroomClassList[i] = mush;
                }

            }

            // Draw ship
            spriteBatch.Draw(m_ShipTexture, m_ShipBox, Color.White);

            // Draw bullet
            if (bulletList.Count > 0)
            {
                foreach (var value in bulletList)
                {
                    spriteBatch.Draw(m_BulletTexture, value, Color.White);
                }
            }

            drawUi(spriteBatch, m_graphics);

            spriteBatch.End();
        }

        private void drawUi(SpriteBatch spriteBatch, GraphicsDeviceManager m_graphics)
        {
            // Draw score
            spriteBatch.DrawString(m_font, score.ToString(), new Vector2(m_graphics.GraphicsDevice.Viewport.Width / 8, 5), Color.Red);

            // Draw high score
            spriteBatch.DrawString(m_font, highScore.ToString(), new Vector2(m_graphics.GraphicsDevice.Viewport.Width / 2 , 5), Color.Red);

            // Draw remaining ship lives
            if (ship.Lives == 3)
            {             
                spriteBatch.Draw(m_ShipTexture, m_ShipBoxLife1, Color.White);
                spriteBatch.Draw(m_ShipTexture, m_ShipBoxLife2, Color.White);
                spriteBatch.Draw(m_ShipTexture, m_ShipBoxLife3, Color.White);
            }
            if (ship.Lives == 2)
            {
                spriteBatch.Draw(m_ShipTexture, m_ShipBoxLife1, Color.White);
                spriteBatch.Draw(m_ShipTexture, m_ShipBoxLife2, Color.White);
            }
            if (ship.Lives == 1)
            {
                spriteBatch.Draw(m_ShipTexture, m_ShipBoxLife1, Color.White);
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Assignment3Centipede
{
    class Ship
    {
        Rectangle shipRec;
        GraphicsDeviceManager m_graphics;

        HelpView helpView = new HelpView();

        //GameModel gameModel = new GameModel();

        private const int shipMovementSpeed = 10;

        int lives = 3;

        // Default constructor
        public Ship() { }

        public Ship(Rectangle shipRec, GraphicsDeviceManager m_graphics)
        {
            this.shipRec = shipRec;
            this.m_graphics = m_graphics;
        }

        public int xPos
        {
            get { return shipRec.X; }
            set { shipRec.X = value; }
        }

        public int yPos
        {
            get { return shipRec.Y; }
            set { shipRec.Y = value; }
        }

        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        public void moveRight()
        {
            // Move right
            if (Keyboard.GetState().IsKeyDown(helpView.MoveRight) && shipRec.X != m_graphics.GraphicsDevice.Viewport.Width - 25)
            {
                shipRec.X += shipMovementSpeed;
            }
        }

        public void moveLeft()
        {
            // Move left
            if (Keyboard.GetState().IsKeyDown(helpView.MoveLeft) && shipRec.X != 0)
            {
                shipRec.X -= shipMovementSpeed;
            }
        }

        public void moveUp()
        {
            // Move up
            if (Keyboard.GetState().IsKeyDown(helpView.MoveUp) && shipRec.Y >= (m_graphics.GraphicsDevice.Viewport.Height * .7))
            {
                shipRec.Y -= shipMovementSpeed;
            }
        }

        public void moveDown()
        { 
            // Move down
            if (Keyboard.GetState().IsKeyDown(helpView.MoveDown) && shipRec.Y != m_graphics.GraphicsDevice.Viewport.Height - 25)
            {
                shipRec.Y += shipMovementSpeed;
            }
        }

        public void takeLives()
        {
            lives--;
        }

    }
}

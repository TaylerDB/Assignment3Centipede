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

        private const int shipMovementSpeed = 10;

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

        public void moveX()
        {
            if (Keyboard.GetState().IsKeyDown(helpView.MoveLeft) && shipRec.X != 0)
            {
                shipRec.X -= shipMovementSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(helpView.MoveRight) && shipRec.X != m_graphics.GraphicsDevice.Viewport.Width - 25)
            {
                shipRec.X += shipMovementSpeed;
            }
        }

        public void moveY()
        {
            if (Keyboard.GetState().IsKeyDown(helpView.MoveUp) && shipRec.Y >= (m_graphics.GraphicsDevice.Viewport.Height * .7))
            {
                shipRec.Y -= shipMovementSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(helpView.MoveDown) && shipRec.Y != m_graphics.GraphicsDevice.Viewport.Height - 25)
            {
                shipRec.Y += shipMovementSpeed;
            }
        }
    }
}

using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment3Centipede
{
    public class ControlSaveState
    {
        /// <summary>
        /// Have to have a default constructor for the XmlSerializer.Deserialize method
        /// </summary>
        public ControlSaveState() { }

        /// <summary>
        /// Overloaded constructor used to create an object for long term storage
        /// </summary>
        /// <param name="up"></param>
        /// <param name="down"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="shoot"></param>
        public ControlSaveState(Keys up, Keys down, Keys left, Keys right, Keys shoot)
        {
            this.Up = up;
            this.Down = down;
            this.Left = left;
            this.Right = right;
            this.Shoot = shoot;
        }

        public Keys Up { get; set; }
        public Keys Down { get; set; }
        public Keys Left { get; set; }
        public Keys Right { get; set; }
        public Keys Shoot { get; set; }
    }
}

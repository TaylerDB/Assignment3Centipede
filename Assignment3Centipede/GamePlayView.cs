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
        private GameModel m_model = new GameModel();
        private GameRenderer m_renderer = new GameRenderer();


        private SpriteFont m_font;
        private const string MESSAGE = "Isn't this game fun!";

        public override void loadContent(ContentManager contentManager)
        {
            m_model = new GameModel(m_graphics);
            m_model.loadContent(contentManager);

        }

        public override GameStateEnum processInput(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }

            m_model.processInput(gameTime);

            return GameStateEnum.GamePlay;
        }

        public override void render(GameTime gameTime)
        {
            m_model.render(m_spriteBatch, gameTime);

            //Vector2 stringSize = m_font.MeasureString(MESSAGE);
            //m_spriteBatch.DrawString(m_font, MESSAGE,
            //    new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, m_graphics.PreferredBackBufferHeight / 2 - stringSize.Y), Color.Yellow);
            
        }

        public override void update(GameTime gameTime)
        {
            m_model.update(gameTime);
        }
    }
}

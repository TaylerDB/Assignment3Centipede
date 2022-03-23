using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Assignment3Centipede
{
    public class AboutView : GameStateView
    {
        private SpriteFont m_font;
        private SpriteFont m_font1;
        private const string MESSAGE = "Tayler Baker with the help of Dr. Mathias wrote this amazing game!";
        private const string MESSAGE1 = "Assets used from:";
        private const string MESSAGE2 = "https://www.pngkit.com/view/u2w7r5u2e6u2a9r5_general-sprites-centipede-arcade-game-sprites/";

        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");
            m_font1 = contentManager.Load<SpriteFont>("Fonts/game");
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }

            return GameStateEnum.About;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            Vector2 stringSize = m_font.MeasureString(MESSAGE);
            m_spriteBatch.DrawString(m_font, MESSAGE,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, 200 - stringSize.Y), Color.Yellow);

            Vector2 stringSize1 = m_font.MeasureString(MESSAGE1);
            m_spriteBatch.DrawString(m_font, MESSAGE1,
                new Vector2((m_graphics.PreferredBackBufferWidth / 2) + 500 - stringSize.X / 2, (m_graphics.PreferredBackBufferHeight / 2) + 100 - stringSize.Y), Color.Yellow);


            Vector2 stringSize2 = m_font.MeasureString(MESSAGE2);
            m_spriteBatch.DrawString(m_font1, MESSAGE2,
                new Vector2((m_graphics.PreferredBackBufferWidth / 2) - stringSize.X / 2, (m_graphics.PreferredBackBufferHeight / 2) + 200 - stringSize.Y), Color.Yellow);


            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
        }
    }
}

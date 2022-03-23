using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Assignment3Centipede
{
    public class HighScoresView : GameStateView
    {
        private SpriteFont m_font;
        private const string MESSAGE = "These are the high scores";
        private const string HS0 = "0";
        private const string HS1 = "1";
        private const string HS2 = "2";
        private const string HS3 = "3";
        private const string HS4 = "4";

        GameModel gameModel = new GameModel();

        List<int> highScoreList = new List<int>();

        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");
            
            for (int i = 0; i < 5; i++)
            {

                highScoreList.Add(0);
                            
            }
            
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }

            return GameStateEnum.HighScores;
        }

        public override void render(GameTime gameTime)
        {
            int score = gameModel.getHighScore;

            for (int i = 0; i < 5; i++)
            {
                if (highScoreList[i] < score)
                {
                    highScoreList[i] = score;
                }
            }

            highScoreList.Sort();

            

            m_spriteBatch.Begin();

            Vector2 stringSize = m_font.MeasureString(MESSAGE);
            m_spriteBatch.DrawString(m_font, MESSAGE,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, 200 - stringSize.Y), Color.Yellow);

            Vector2 stringSize1 = m_font.MeasureString(highScoreList[0].ToString());
            m_spriteBatch.DrawString(m_font, highScoreList[0].ToString(),
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 + 300 - stringSize.X / 2, 300 - stringSize.Y), Color.Yellow);

            Vector2 stringSize2 = m_font.MeasureString(highScoreList[1].ToString());
            m_spriteBatch.DrawString(m_font, highScoreList[1].ToString(),
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 + 300 - stringSize.X / 2, 400 - stringSize.Y), Color.Yellow);
            
            Vector2 stringSize3 = m_font.MeasureString(highScoreList[2].ToString());
            m_spriteBatch.DrawString(m_font, highScoreList[2].ToString(),
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 + 300 - stringSize.X / 2, 500 - stringSize.Y), Color.Yellow);

            Vector2 stringSize4 = m_font.MeasureString(highScoreList[3].ToString());
            m_spriteBatch.DrawString(m_font, highScoreList[3].ToString(),
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 + 300 - stringSize.X / 2, 600 - stringSize.Y), Color.Yellow);

            Vector2 stringSize5 = m_font.MeasureString(highScoreList[4].ToString());
            m_spriteBatch.DrawString(m_font, highScoreList[4].ToString(),
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 + 300 - stringSize.X / 2, 700 - stringSize.Y), Color.Yellow);

            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
        }
    }
}

using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLTetrisKlon.src.Core;
using SFMLTetrisKlon.src.Scenes;
using SFMLTetrisKlon.src.UI;
using SFMLTetrisKlon.src.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src.GameMenus
{
    public class GameMenu : GameObject
    {
        #region Properties
        UserInterface m_Interface;
        PlayerController m_PlayerController;
        Arena m_Arena;

        Label labelScore;
        Label labelTime;
        Label labelLevel;
        Label labelLines;
        Label labelHighScore;
        UI.Components.Image imageNextBlock;
        #endregion

        public GameMenu(string name, Window w, PlayerController playerController, Arena arena) : base(name)
        {
            labelScore = new Label("LabelScore");
            labelTime = new Label("LabelTime");
            labelLevel = new Label("LabelLevel");
            labelLines = new Label("LabelLines");
            labelHighScore = new Label("LabelHighscore");
            imageNextBlock = new UI.Components.Image("ImageNextBlock", string.Empty);

            m_Interface = new UserInterface("GameMenu", w);

            // Objects from scene
            m_PlayerController = playerController;
            m_Arena = arena;

            InitializeComponents();
        }
        public void Dispose(bool disposing)
        {
            m_Interface.Destroy(disposing, Application.Game.Window);
            base.Dispose();
        }

        private void InitializeComponents()
        {
            //
            // Score
            //
            labelScore.Content = $"Score: {m_PlayerController.CurrentScore}";
            labelScore.Size = new Vector2f(100, 30);
            labelScore.Location = new Vector2f(Application.Game.Window.Size.X / 2 + labelScore.Size.X / 2, 20);
            labelScore.ForegroundColor = Color.White;
            labelScore.BackgroundColor = Color.Transparent;
            labelScore.FontSize = 15;
            labelScore.Alignment = Alignment.Left;

            //
            // Time
            //
            labelTime.Content = $"Time: {m_PlayerController.ArenaTime.ElapsedTime.AsSeconds()}s";
            labelTime.Size = new Vector2f(100, 30);
            labelTime.Location = new Vector2f(Application.Game.Window.Size.X / 2 + 50, 60);
            labelTime.ForegroundColor = Color.White;
            labelTime.BackgroundColor = Color.Transparent;
            labelTime.FontSize = 15;
            labelTime.Alignment = Alignment.Left;

            //
            // Level
            //
            labelLevel.Content = $"Level: {m_PlayerController.Level}";
            labelLevel.Size = new Vector2f(100, 30);
            labelLevel.Location = new Vector2f(Application.Game.Window.Size.X / 2 + 50, 100);
            labelLevel.ForegroundColor = Color.White;
            labelLevel.BackgroundColor = Color.Transparent;
            labelLevel.FontSize = 15;
            labelLevel.Alignment = Alignment.Left;

            //
            // Lines
            //
            labelLines.Content = $"Lines: {m_PlayerController.Lines}";
            labelLines.Size = new Vector2f(100, 30);
            labelLines.Location = new Vector2f(Application.Game.Window.Size.X / 2 + 50, 140);
            labelLines.ForegroundColor = Color.White;
            labelLines.BackgroundColor = Color.Transparent;
            labelLines.FontSize = 15;
            labelLines.Alignment = Alignment.Left;

            //
            // Highscore
            //
            labelHighScore.Content = $"Highscore: {m_PlayerController.Highscore}";
            labelHighScore.Size = new Vector2f(100, 30);
            labelHighScore.Location = new Vector2f(Application.Game.Window.Size.X / 2 + 50, 180);
            labelHighScore.ForegroundColor = Color.White;
            labelHighScore.BackgroundColor = Color.Transparent;
            labelHighScore.FontSize = 15;
            labelHighScore.Alignment = Alignment.Left;


            //
            // Next Block
            //
            //imageNextBlock.Size = new Vector2f(100, 200);
            //imageNextBlock.Location = new Vector2f(Application.Game.Window.Size.X / 2 + imageNextBlock.Size.X / 2, 220);
            //SetNextBlock();



            //
            // Add Components
            //
            m_Interface.AddChild(new UIElement[]
            {
                labelScore,
                labelTime,
                labelLevel,
                labelLines,
                labelHighScore,
                //imageNextBlock,
            });
        }

        public override void Update(Time elapsed)
        {
            if (IsUpdateDisabled) { return; }

            labelScore.Content = $"Score: {m_PlayerController.CurrentScore}";
            labelTime.Content = $"Time: {Math.Round(m_PlayerController.ArenaTime.ElapsedTime.AsSeconds(), 2)}s";
            labelLevel.Content = $"Level: {m_PlayerController.Level}";
            labelLines.Content = $"Lines: {m_PlayerController.Lines}";
            labelHighScore.Content = $"Highscore: {m_PlayerController.Highscore}";
            SetNextBlock();
        }


        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(m_Interface);
        }


        private void SetNextBlock()
        {
            Color col = BlockInformation.BlockColors.Where(c => c.Key == m_Arena.CurrentPiece).FirstOrDefault().Value;
            int[,] currentBlock = BlockInformation.BlockArrays.Where(a => a.Key == m_Arena.CurrentPiece).FirstOrDefault().Value;
            if (currentBlock != null && col != null)
            {

                SFML.Graphics.Image img = new SFML.Graphics.Image(100, 100, Color.White);
                for (int y = 0; y < currentBlock.GetLength(0); y++)
                {
                    for (int x = 0; x < currentBlock.GetLength(1); x++)
                    {
                        if (currentBlock[y, x] <= 0) { continue; }
                        uint posY = (img.Size.Y / 2) - ((uint)y / 2);
                        uint posX = (img.Size.X / 2) + ((uint)x / 2);

                        for (uint i = 0; i < BlockInformation.Size.Y; i++)
                        {
                            for (uint j = 0; j < BlockInformation.Size.X; j++)
                            {
                                img.SetPixel(posX + j, posY + i, col);
                            }
                        }
                    }
                }

                imageNextBlock.SetImage(img);
            }
        }

        #region Events
        #endregion
    }
}

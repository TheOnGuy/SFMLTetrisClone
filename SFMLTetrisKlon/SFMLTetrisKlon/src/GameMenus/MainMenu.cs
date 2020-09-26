using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLTetrisKlon.src.Core;
using SFMLTetrisKlon.src.GameScenes;
using SFMLTetrisKlon.src.Scenes;
using SFMLTetrisKlon.src.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src.GameMenus
{
    public class MainMenu : GameObject
    {
        #region Properties
        UserInterface m_Interface;
        OptionsMenu m_OptionsMenu;

        UI.Components.Button buttonStart;
        UI.Components.Button buttonOptions;
        UI.Components.Button buttonQuit;
        UI.Components.Label labelTutorialText;

        public bool IsOptionsMenu;

        RectangleShape m_DrawBlock;
        Block[] m_Blocks;
        #endregion

        public MainMenu(string name, Window w) : base(name)
        {
            m_DrawBlock = new RectangleShape(BlockInformation.Size * Settings.Scale);
            m_Blocks = new Block[5];
            SetBlockArray();

            m_Interface = new UserInterface("MainMenu", w);
            InitializeComponents();
        }
        public void Dispose(bool disposing)
        {
            buttonStart.OnClicked -= ButtonStart_Clicked;
            buttonOptions.OnClicked -= ButtonOptions_Clicked;
            buttonQuit.OnClicked -= ButtonQuit_Clicked;

            m_Interface.Destroy(disposing, Application.Game.Window);
            base.Dispose();
        }

        private void InitializeComponents()
        {
            // ButtonStart
            buttonStart = new UI.Components.Button("ButtonStart");
            buttonStart.Content = "Start Game";
            buttonStart.Size = new Vector2f(110, 50);
            buttonStart.Location = new Vector2f(Application.Game.Window.Size.X - 125, Application.Game.Window.Size.Y * 0.10f);
            buttonStart.ForegroundColor = Color.White;
            buttonStart.FontSize = 13;
            buttonStart.BackgroundImage = new Image(@"assets/images/block_icon.png");
            buttonStart.OnClicked += ButtonStart_Clicked;
            buttonStart.SetLayer(1);

            // ButtonOptions
            buttonOptions = new UI.Components.Button("ButtonOptions");
            buttonOptions.Content = "Options";
            buttonOptions.Size = new Vector2f(110, 50);
            buttonOptions.Location = new Vector2f(Application.Game.Window.Size.X - 125, Application.Game.Window.Size.Y * 0.25f);
            buttonOptions.ForegroundColor = Color.White;
            buttonOptions.FontSize = 13;
            buttonOptions.BackgroundImage = new Image(@"assets/images/block_icon.png");
            buttonOptions.OnClicked += ButtonOptions_Clicked;
            buttonOptions.SetLayer(1);

            // ButtonQuit
            buttonQuit = new UI.Components.Button("ButtonQuit");
            buttonQuit.Content = "Quit Game";
            buttonQuit.Size = new Vector2f(110, 50);
            buttonQuit.Location = new Vector2f(Application.Game.Window.Size.X - 125, Application.Game.Window.Size.Y * 0.40f);
            buttonQuit.ForegroundColor = Color.White;
            buttonQuit.FontSize = 13;
            buttonQuit.BackgroundImage = new Image(@"assets/images/block_icon.png");
            buttonQuit.OnClicked += ButtonQuit_Clicked;
            buttonQuit.SetLayer(1);

            // labelTutorialText
            labelTutorialText = new UI.Components.Label("labelTutorialText");
            labelTutorialText.Size = new Vector2f(Application.Game.Window.Size.X * 0.50f, 250);
            labelTutorialText.Location = new Vector2f(Application.Game.Window.Size.X * 0.05f, -30);
            labelTutorialText.Content = "Tutorial:\n" +
                                        "Bewegen des Tetrisblock \n" +
                                        "mit den Pfeiltasten \n" +
                                        "oder ASD\n\n" +
                                        "Rotieren des Tetrisblocks \n" +
                                        "mit R";
            labelTutorialText.ForegroundColor = Color.White;
            labelTutorialText.BackgroundColor = Color.Transparent;
            labelTutorialText.Alignment = UI.Components.Alignment.Left;
            labelTutorialText.FontSize = 18;

            // Add Components
            m_Interface.AddChild(new UIElement[]
            {
                buttonStart,
                buttonOptions,
                buttonQuit,
                labelTutorialText,
            });
        }


        public override void Draw(RenderTarget target, RenderStates states)
        {
            DrawFallingTetrisPieces(target, states);

            if (IsOptionsMenu)
                target.Draw(m_OptionsMenu);
            else
                target.Draw(m_Interface);

        }


        private void SetBlockArray()
        {
            Random rnd = new Random();

            Block b = new Block()
            {
                BlockType = 'I',
                BlockArray = BlockInformation.BlockArrays.Where(ba => ba.Key == 'I').First().Value,
                Position = new Vector2f(rnd.Next(0, Convert.ToInt32(Application.Game.Window.Size.X - (BlockInformation.Size.X * Settings.Scale))), rnd.Next(-50, 0)),
            };                                                                                                                                              
            m_Blocks[0] = b;                                                                                                                                
                                                                                                                                                            
            b = new Block()                                                                                                                                 
            {                                                                                                                                               
                BlockType = 'J',                                                                                                                            
                BlockArray = BlockInformation.BlockArrays.Where(ba => ba.Key == 'J').First().Value,                                                         
                Position = new Vector2f(rnd.Next(0, Convert.ToInt32(Application.Game.Window.Size.X - (BlockInformation.Size.X * Settings.Scale))), rnd.Next(-50, 0)),
            };                                                                                                                                              
            m_Blocks[1] = b;                                                                                                                                
                                                                                                                                                            
            b = new Block()                                                                                                                                 
            {                                                                                                                                               
                BlockType = 'L',                                                                                                                            
                BlockArray = BlockInformation.BlockArrays.Where(ba => ba.Key == 'L').First().Value,                                                         
                Position = new Vector2f(rnd.Next(0, Convert.ToInt32(Application.Game.Window.Size.X - (BlockInformation.Size.X * Settings.Scale))), rnd.Next(-50, 0)),
            };                                                                                                                                              
            m_Blocks[2] = b;                                                                                                                                
                                                                                                                                                            
            b = new Block()                                                                                                                                 
            {                                                                                                                                               
                BlockType = 'O',                                                                                                                            
                BlockArray = BlockInformation.BlockArrays.Where(ba => ba.Key == 'O').First().Value,                                                         
                Position = new Vector2f(rnd.Next(0, Convert.ToInt32(Application.Game.Window.Size.X - (BlockInformation.Size.X * Settings.Scale))), rnd.Next(-50, 0)),
            };                                                                                                                                              
            m_Blocks[3] = b;                                                                                                                                
                                                                                                                                                            
            b = new Block()                                                                                                                                 
            {                                                                                                                                               
                BlockType = 'S',                                                                                                                            
                BlockArray = BlockInformation.BlockArrays.Where(ba => ba.Key == 'S').First().Value,                                                         
                Position = new Vector2f(rnd.Next(0, Convert.ToInt32(Application.Game.Window.Size.X - (BlockInformation.Size.X * Settings.Scale))), rnd.Next(-50, 0)),
            };                                                                                                                                              
            m_Blocks[4] = b;
        }
        private void DrawFallingTetrisPieces(RenderTarget target, RenderStates states)
        {
            // Reset all blocks
            if (m_Blocks[0].Position.Y > Application.Game.Window.Size.Y &&
                m_Blocks[1].Position.Y > Application.Game.Window.Size.Y &&
                m_Blocks[2].Position.Y > Application.Game.Window.Size.Y &&
                m_Blocks[3].Position.Y > Application.Game.Window.Size.Y &&
                m_Blocks[4].Position.Y > Application.Game.Window.Size.Y)
            {
                SetBlockArray();
            }


            int idx = 0;
            foreach (var block in m_Blocks)
            {
                // Draw Block
                for (int y = 0; y < block.BlockArray.GetLength(0); y++)
                {
                    for (int x = 0; x < block.BlockArray.GetLength(1); x++)
                    {
                        m_DrawBlock.Position = block.Position + new Vector2f(x * BlockInformation.Size.X * Settings.Scale, y * BlockInformation.Size.Y * Settings.Scale);
                        if (block.BlockArray[y, x] > 0)
                        {
                            m_DrawBlock.FillColor = BlockInformation.BlockColors[block.BlockType];
                            target.Draw(m_DrawBlock);
                        }
                    }
                }

                idx++;
            }
        }

        public override void Update(Time elapsed)
        {
            if (m_OptionsMenu != null)
            {
                if (!m_OptionsMenu.IsVisible)
                    IsOptionsMenu = false;
                else
                    IsOptionsMenu = true;
            }

            if (IsOptionsMenu == false)
                m_Interface.Update(elapsed);


            for (int i=0; i<m_Blocks.Length; i++)
            {
                m_Blocks[i].Position += new Vector2f(0, BlockInformation.Size.Y * Settings.Scale) * elapsed.AsSeconds();
            }
        }


        #region Events
        private void ButtonStart_Clicked(MouseButtonEventArgs e)
        {
            SceneManager.Load(new GameScene());
        }

        private void ButtonOptions_Clicked(MouseButtonEventArgs e)
        {
            IsOptionsMenu = true;
            m_OptionsMenu = new OptionsMenu("OptionsMenu", Application.Game.Window);
            m_OptionsMenu.IsVisible = true;
            m_OptionsMenu.Parent = this;
        }

        private void ButtonQuit_Clicked(MouseButtonEventArgs e)
        {
            Application.Game.Shutdown();
        }
        #endregion
    }
}

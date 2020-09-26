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
    public class GameOverMenu : GameObject
    {
        #region Properties
        UserInterface m_Interface;
        GameScene m_GameScene;

        UI.Components.Button ButtonRestart;
        UI.Components.Button ButtonQuit;
        UI.Components.Label LabelGameOver;
        UI.Components.Image ImageBlur;
        #endregion

        public GameOverMenu(string name, Window w, GameScene scene) : base(name)
        {
            m_Interface = new UserInterface("GameOverMenu", w);
            m_GameScene = scene;
            InitializeComponents();
        }
        public void Dispose(bool disposing)
        {
            ButtonRestart.OnClicked -= ButtonRestart_Clicked;
            ButtonQuit.OnClicked -= ButtonQuit_Clicked;
            m_Interface.Destroy(disposing, Application.Game.Window);
            base.Dispose();
        }

        public override void Update(Time elapsed)
        {
            if (IsUpdateDisabled) { return; }

            if (IsVisible)
            {
                m_GameScene.DisableUpdate();
                m_Interface.Enable();
            }
            else
            {
                m_GameScene.EnableUpdate();
                m_Interface.Disable();
            }
        }


        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (IsVisible)
                target.Draw(m_Interface);
        }


        private void InitializeComponents()
        {
            //
            // ButtonStart
            //
            ButtonRestart = new UI.Components.Button("ButtonRestart");
            ButtonRestart.Content = "Restart";
            ButtonRestart.FontSize = 12;
            ButtonRestart.ForegroundColor = Color.White;
            ButtonRestart.Size = new Vector2f(150, 75);
            ButtonRestart.Location = new Vector2f(Application.Game.Window.Size.X * 0.10f, 250);
            ButtonRestart.BackgroundImage = new Image(@"assets/images/block_icon.png");
            ButtonRestart.OnClicked += ButtonRestart_Clicked;
            ButtonRestart.OnMouseEntered += ButtonRestart_MouseEntered;
            ButtonRestart.OnMouseLeft += ButtonRestart_MouseLeft;
            ButtonRestart.SetLayer(1);

            //
            // ButtonQuit
            //
            ButtonQuit = new UI.Components.Button("ButtonQuitToMainMenu");
            ButtonQuit.Content = "Quit To Main Menu";
            ButtonQuit.FontSize = 12;
            ButtonQuit.ForegroundColor = Color.White;
            ButtonQuit.Size = new Vector2f(150, 75);
            ButtonQuit.Location = new Vector2f(Application.Game.Window.Size.X - Application.Game.Window.Size.X * 0.10f - ButtonQuit.Size.X, 250);
            ButtonQuit.BackgroundImage = new Image(@"assets/images/block_icon.png");
            ButtonQuit.OnClicked += (ButtonQuit_Clicked);
            ButtonQuit.OnMouseEntered += ButtonQuit_MouseEntered;
            ButtonQuit.OnMouseLeft += ButtonQuit_MouseLeft;
            ButtonQuit.SetLayer(1);

            //
            // LabelGameOver
            //
            LabelGameOver = new UI.Components.Label("LabelGameOver");
            LabelGameOver.Content = $"Game Over";
            LabelGameOver.Size = new Vector2f(100, 50);
            LabelGameOver.Location = new Vector2f(Application.Game.Window.Size.X / 2 - LabelGameOver.Size.X * 1.5f, 100);
            LabelGameOver.ForegroundColor = Color.Red;
            LabelGameOver.BackgroundColor = Color.Transparent;
            LabelGameOver.FontSize = 50;
            LabelGameOver.SetLayer(1);

            //
            // ImageBlur
            //
            Image img = new Image(Application.Game.Window.Size.X, Application.Game.Window.Size.Y, new Color(54, 50, 41));
            ImageBlur = new UI.Components.Image("ImageBlur", img);
            ImageBlur.Size = new Vector2f(Application.Game.Window.Size.X, Application.Game.Window.Size.Y);
            ImageBlur.Transparency = 150;
            ImageBlur.SetLayer(0);

            //
            // Add Components
            //
            m_Interface.AddChild(new UIElement[]
            {
                ButtonRestart,
                ButtonQuit,
                LabelGameOver,
                ImageBlur,
            });
        }




        #region Events
        private void ButtonQuit_Clicked(MouseButtonEventArgs e)
        {
            SceneManager.Load(new MainMenuScene());
        }

        private void ButtonRestart_Clicked(MouseButtonEventArgs e)
        {
            SceneManager.Load(new GameScene());
        }

        private void ButtonQuit_MouseEntered(MouseMoveEventArgs e)
        {
            ButtonQuit.BackgroundImage = new Image(@"assets/images/block_icon_dark.png");
        }
        private void ButtonQuit_MouseLeft(MouseMoveEventArgs e)
        {
            ButtonQuit.BackgroundImage = new Image(@"assets/images/block_icon.png");
        }
        private void ButtonRestart_MouseEntered(MouseMoveEventArgs e)
        {
            ButtonRestart.BackgroundImage = new Image(@"assets/images/block_icon_dark.png");
        }
        private void ButtonRestart_MouseLeft(MouseMoveEventArgs e)
        {
            ButtonRestart.BackgroundImage = new Image(@"assets/images/block_icon.png");
        }
        #endregion
    }
}

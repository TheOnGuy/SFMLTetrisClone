using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLTetrisKlon.src.Core;
using SFMLTetrisKlon.src.Scenes;
using SFMLTetrisKlon.src.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src.UI
{
    public class MainMenu : GameObject
    {
        #region Properties
        UserInterface m_Interface;

        Components.Button ButtonStart;
        Components.Button ButtonQuit;
        Components.Image ImageBackground;
        #endregion

        public MainMenu(string name, Window w) : base(name)
        {
            m_Interface = new UserInterface("MainMenu", w);
            InitializeComponents();
        }
        public void Dispose(bool disposing)
        {
            ButtonStart.OnClicked -= ButtonStart_Clicked;
            ButtonStart.OnMouseEntered -= ButtonStart_MouseEntered;
            ButtonStart.OnMouseLeft -= ButtonStart_MouseLeft;

            ButtonQuit.OnClicked -= ButtonQuit_Clicked;
            ButtonQuit.OnMouseEntered -= ButtonQuit_MouseEntered;
            ButtonQuit.OnMouseLeft -= ButtonQuit_MouseLeft;

            m_Interface.Destroy(disposing, Application.Game.Window);
            base.Dispose();
        }

        private void InitializeComponents()
        {
            //
            // ButtonStart
            //
            ButtonStart = new Components.Button("ButtonStart");
            ButtonStart.Content = "Start Game";
            ButtonStart.Size = new Vector2f(100, 50);
            ButtonStart.Location = new Vector2f(Application.Game.Window.Size.X - 125, Application.Game.Window.Size.Y * 0.10f);
            ButtonStart.BackgroundColor = Color.Transparent;
            ButtonStart.ForegroundColor = Color.White;
            ButtonStart.FontSize = 15;
            ButtonStart.OnClicked += ButtonStart_Clicked;
            ButtonStart.OnMouseEntered += ButtonStart_MouseEntered;
            ButtonStart.OnMouseLeft += ButtonStart_MouseLeft;
            ButtonStart.SetLayer(1);

            //
            // ButtonQuit
            //
            ButtonQuit = new Components.Button("ButtonQuit");
            ButtonQuit.Content = "Quit Game";
            ButtonQuit.Size = new Vector2f(100, 50);
            ButtonQuit.Location = new Vector2f(Application.Game.Window.Size.X - 125, Application.Game.Window.Size.Y * 0.25f);
            ButtonQuit.BackgroundColor = Color.Transparent;
            ButtonQuit.ForegroundColor = Color.White;
            ButtonQuit.FontSize = 15;
            ButtonQuit.OnClicked += ButtonQuit_Clicked;
            ButtonQuit.OnMouseEntered += ButtonQuit_MouseEntered;
            ButtonQuit.OnMouseLeft += ButtonQuit_MouseLeft;
            ButtonQuit.SetLayer(1);

            //
            // ImageBackground
            //
            ImageBackground = new Components.Image("ImageBackground", @"assets/images/MainMenu_Background.jpg");
            ImageBackground.Size = new Vector2f(Application.Game.Window.Size.X, Application.Game.Window.Size.Y);
            ImageBackground.Transparency = 100;
            ImageBackground.SetLayer(0);

            //
            // Add Components
            //
            m_Interface.AddChild(new UIElement[]
            {
                ButtonStart,
                ButtonQuit,
                ImageBackground,
            });
        }


        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(m_Interface);
        }


        #region Events
        public void ButtonStart_Clicked(MouseButtonEventArgs e)
        {
            SceneManager.Load(new GameScene());
        }
        public void ButtonStart_MouseEntered(MouseMoveEventArgs e)
        {
            ButtonStart.ForegroundColor = Color.Red;
        }
        public void ButtonStart_MouseLeft(MouseMoveEventArgs e)
        {
            ButtonStart.ForegroundColor = Color.White;
        }


        public void ButtonQuit_Clicked(MouseButtonEventArgs e)
        {
            Application.Game.Shutdown();
        }
        public void ButtonQuit_MouseEntered(MouseMoveEventArgs e)
        {
            ButtonQuit.ForegroundColor = Color.Red;
        }
        public void ButtonQuit_MouseLeft(MouseMoveEventArgs e)
        {
            ButtonQuit.ForegroundColor = Color.White;
        }
        #endregion
    }
}

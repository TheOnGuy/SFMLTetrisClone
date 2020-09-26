using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLTetrisKlon.src.Core;
using SFMLTetrisKlon.src.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src.UI
{
    public class PauseMenu : GameObject
    {
        #region Properties
        UserInterface m_Interface;
        GameScene m_GameScene;

        Components.Button ButtonResume;
        Components.Button ButtonQuit;
        Components.Image ImageBackgroundBlur;
        #endregion

        public PauseMenu(string name, Window w, GameScene scene) : base(name)
        {
            m_Interface = new UserInterface("PauseMenu", w);
            m_GameScene = scene;
            InitializeComponents();
        }
        public void Dispose(bool disposing)
        {
            ButtonResume.OnClicked -= ButtonResume_Clicked;
            ButtonQuit.OnClicked -= ButtonQuit_Clicked;
            m_Interface.Destroy(disposing, Application.Game.Window);
            base.Dispose();
        }

        public override void Update(Time elapsed)
        {
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
            ButtonResume = new Components.Button("ButtonResume");
            ButtonResume.Content = "Resume Game";
            ButtonResume.Size = new Vector2f(100, 50);
            ButtonResume.Location = new Vector2f(Application.Game.Window.Size.X / 2 - ButtonResume.Size.X / 2, 50);
            ButtonResume.OnClicked += ButtonResume_Clicked;
            ButtonResume.SetLayer(9999);

            //
            // ButtonQuit
            //
            ButtonQuit = new Components.Button("ButtonQuitToMainMenu");
            ButtonQuit.Content = "Quit To Main Menu";
            ButtonQuit.Size = new Vector2f(100, 50);
            ButtonQuit.Location = new Vector2f(Application.Game.Window.Size.X / 2 - ButtonQuit.Size.X / 2, 105);
            ButtonQuit.OnClicked += (ButtonQuit_Clicked);
            ButtonQuit.SetLayer(9999);

            //
            // ImageBackgroundBlur
            //
            ImageBackgroundBlur = new Components.Image("ImageBackgroundBlur", @"assets/images/background_blur.jpg");
            ImageBackgroundBlur.Size = new Vector2f(Application.Game.Window.Size.X, Application.Game.Window.Size.Y);
            ImageBackgroundBlur.Transparency = 100;
            ImageBackgroundBlur.SetLayer(9998);


            //
            // Add Components
            //
            m_Interface.AddChild(new UIElement[]
            {
                ButtonResume,
                ButtonQuit,
                ImageBackgroundBlur,
            });
        }


        #region Events
        private void ButtonQuit_Clicked(MouseButtonEventArgs e)
        {
            SceneManager.Load(new MainMenuScene());
        }

        private void ButtonResume_Clicked(MouseButtonEventArgs e)
        {
            IsVisible = !IsVisible;

            if (IsVisible)
            {
                m_GameScene.DisableUpdate();
            }
            else
            {
                m_GameScene.EnableUpdate();
            }
        }
        #endregion
    }
}

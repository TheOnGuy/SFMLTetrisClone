using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLTetrisKlon.src;
using SFMLTetrisKlon.src.GameScenes;
using SFMLTetrisKlon.src.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src.Core
{
    public class Game
    {
        private RenderWindow m_Window;
        private Clock m_GameTimer;

        public RenderWindow Window { get => m_Window; }


        public Game(string name, Vector2f size)
        {
            m_Window = new RenderWindow(new VideoMode((uint)size.X, (uint)size.Y), name);
            m_Window.Closed += (_, __) => m_Window.Close();
            m_Window.SetFramerateLimit(60);

            m_GameTimer = new Clock();
        }

        public void Run()
        {
            SceneManager.Load(new MainMenuScene());

            while (m_Window.IsOpen)
            {
                // Eventloop
                m_Window.DispatchEvents();

                // Update
                var elapsed = m_GameTimer.Restart();
                OnGameUpdate?.Invoke(elapsed);

                // Draw
                //m_Window.Clear(new Color(222, 184, 135));
                m_Window.Clear(Color.Black);
                OnGameDraw?.Invoke(m_Window, RenderStates.Default);
                m_Window.Display();
            }

            Shutdown();
        }


        public void Shutdown()
        {
            if (!m_Window.IsOpen) { return; }
            m_Window.Close();
        }


        #region Events
        public event Action<Time> OnGameUpdate;
        public event Action<RenderTarget, RenderStates> OnGameDraw;
        #endregion
    }
}

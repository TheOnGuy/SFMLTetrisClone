using SFML.Graphics;
using SFMLTetrisKlon.src.Core;
using SFMLTetrisKlon.src.GameMenus;
using SFMLTetrisKlon.src.Scenes;
using SFMLTetrisKlon.src.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src.GameScenes
{
    public class GameScene : Scene
    {
        #region Fields
        public Arena m_Arena { get; private set; }
        public PlayerController m_PlayerController { get; private set; }

        public GameMenu m_GameMenu { get; private set; }
        public PauseMenu m_PauseMenu { get; private set; }
        public GameOverMenu m_GameOverMenu { get; private set; }
        #endregion

        public GameScene()
        {
            SoundManager.PlaySound("GameSound");
            InitializeGameObjects();
        }
        public override void Destroy()
        {
            SoundManager.StopSound("GameSound");
            m_PauseMenu.Dispose(true);
            m_GameMenu.Dispose(true);
            m_GameOverMenu.Dispose(true);
            base.Destroy();
        }

        private void InitializeGameObjects()
        {
            //
            // Arena
            //
            m_Arena = new Arena("Arena", new SFML.System.Vector2f(Settings.Scale, Settings.Scale));
            m_Arena.Parent = this;
            //m_Arena.ArenaScale = new SFML.System.Vector2f(2f, 2f);

            //
            // PlayerController
            // 
            m_PlayerController = new PlayerController("PlayerController", m_Arena);
            m_PlayerController.Parent = this;

            // 
            // GameMenu
            //
            m_GameMenu = new GameMenu("GameMenu", Application.Game.Window, m_PlayerController, m_Arena);
            m_GameMenu.SetLayer(9998);
            m_GameMenu.Parent = this;

            //
            // PauseMenu
            //
            m_PauseMenu = new PauseMenu("PauseMenu", Application.Game.Window, this);
            m_PauseMenu.SetLayer(9999);
            m_PauseMenu.Parent = this;
            m_PauseMenu.IsVisible = false;
            m_PauseMenu.IsUpdateDisabled = true;

            //
            // GameOverMenu
            //
            m_GameOverMenu = new GameOverMenu("GameOverMenu", Application.Game.Window, this);
            m_GameOverMenu.SetLayer(9999);
            m_GameOverMenu.Parent = this;
            m_GameOverMenu.IsVisible = false;
            m_GameOverMenu.IsUpdateDisabled = true;

            //
            // Add all gameobjects to the scene
            //
            AddGameObject(m_Arena);
            AddGameObject(m_PlayerController);
            AddGameObject(m_GameMenu);
            AddGameObject(m_PauseMenu);
            AddGameObject(m_GameOverMenu);
        }

        public void DisableUpdate()
        {
            m_Arena.IsUpdateDisabled = true;
            m_PlayerController.IsUpdateDisabled = true;
        }
        public void EnableUpdate()
        {
            m_Arena.IsUpdateDisabled = false;
            m_PlayerController.IsUpdateDisabled = false;
        }
    }
}

using SFML.Graphics;
using SFMLTetrisKlon.src.Core;
using SFMLTetrisKlon.src.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src.Scenes
{
    public class GameScene : Scene
    {
        #region Fields
        public Arena m_Arena { get; private set; }
        public PlayerController m_PlayerController { get; private set; }

        public GameMenu m_GameMenu { get; private set; }
        public PauseMenu m_PauseMenu { get; private set; }
        #endregion

        public GameScene()
        {
            InitializeGameObjects();
        }
        public override void Destroy()
        {
            m_PauseMenu.Dispose(true);
            m_GameMenu.Dispose(true);
            base.Destroy();
        }

        private void InitializeGameObjects()
        {
            //
            // Arena
            //
            m_Arena = new Arena("Arena");
            m_Arena.Parent = this;
            m_Arena.ArenaScale = new SFML.System.Vector2f(2f, 2f);

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

            //
            // Add all gameobjects to the scene
            //
            AddGameObject(m_Arena);
            AddGameObject(m_PlayerController);
            AddGameObject(m_GameMenu);
            AddGameObject(m_PauseMenu);
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

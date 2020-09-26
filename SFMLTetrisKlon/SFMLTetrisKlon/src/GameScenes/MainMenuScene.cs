using SFML.Audio;
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
    public class MainMenuScene : Scene
    {
        #region Fields
        private MainMenu m_MainMenu;
        #endregion

        public MainMenuScene()
        {
            SoundManager.PlaySound("MenuSound");

            InitializeGameObjects();
        }

        public override void Destroy()
        {
            SoundManager.StopSound("MenuSound");
            m_MainMenu.Dispose(true);
            base.Destroy();
        }

        private void InitializeGameObjects()
        {
            //
            // MainMenu Interface
            //
            m_MainMenu = new MainMenu("MainMenu", Application.Game.Window);

            //
            // Add GameObjects
            //
            AddGameObject(m_MainMenu);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src.Scenes
{
    public static class SceneManager
    {
        private static Scene m_CurrentScene;
        public static Scene CurrentScene { get => m_CurrentScene; }



        public static void Load(Scene s)
        {
            Unload();
            m_CurrentScene = s;
        }

        private static void Unload()
        {
            m_CurrentScene?.Destroy();
            m_CurrentScene = null;
        }
    }
}

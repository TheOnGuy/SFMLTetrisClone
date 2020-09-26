using SFML.Graphics;
using SFML.System;
using SFMLTetrisKlon.src.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src.Scenes
{
    public class Scene : Transformable, Drawable
    {
        private List<GameObject> m_Children;
        public List<GameObject> Children { get => m_Children; set => m_Children = value; }


        public Scene()
        {
            Children = new List<GameObject>();
            Application.Game.OnGameUpdate += Update;
            Application.Game.OnGameDraw += Draw;
        }
        public virtual void Destroy()
        {
            Application.Game.OnGameUpdate -= Update;
            Application.Game.OnGameDraw -= Draw;
        }

        private void Update(Time elapsed)
        {
            // Update all child elements
            Children.ForEach(c => c.Update(elapsed));
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            m_Children = m_Children.OrderBy(c => c.Layer).ToList();

            foreach (var child in m_Children)
            {
                target.Draw(child);
            }
        }



        public void AddGameObject(GameObject obj)
        {
            Children.Add(obj);
        }
        public void RemoveGameObject(GameObject obj)
        {
            Children.Remove(obj);
        }
    }
}

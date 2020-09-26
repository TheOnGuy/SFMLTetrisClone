using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src.Core
{
    public class GameObject : Transformable, Drawable
    {
        #region Fields
        private object m_Parent;
        private string m_Name;
        private int m_Layer;
        private bool m_IsVisible;
        private bool m_IsUpdateDisabled;
        #endregion

        #region Properties
        public object Parent { get => m_Parent; set => m_Parent = value; }
        public string Name { get => m_Name; set => m_Name = value; }
        /// <summary>
        /// Layer for Z-Ordering. The smaller the value the further the game object
        /// is drawn in the background.
        /// </summary>
        public int Layer { get => m_Layer; }
        public bool IsVisible { get => m_IsVisible; set => m_IsVisible = value; }
        public bool IsUpdateDisabled { get => m_IsUpdateDisabled; set => m_IsUpdateDisabled = value; }
        #endregion

        public GameObject(string name)
        {
            Name = name;
            SetLayer(0);
        }

        public virtual void Update(Time elapsed) { }
        public virtual void Draw(RenderTarget target, RenderStates states) { }


        public void SetLayer(int layer)
        {
            m_Layer = layer;
        }
    }
}

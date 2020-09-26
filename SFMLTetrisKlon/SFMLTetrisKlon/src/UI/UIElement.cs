using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src.UI
{
    public class UIElement : Transformable, Drawable
    {
        #region Fields
        private string m_Name;
        private object m_Tag;
        private bool m_IsVisible;
        private bool m_IsFocused;
        private int m_Layer;

        private List<UIElement> m_Children;

        private RectangleShape m_Shape;
        private Vector2f m_Size = new Vector2f(100, 50);
        private Vector2f m_Location = new Vector2f(0, 0);
        #endregion

        #region Properties
        public string Name { get => m_Name; set => m_Name = value; }
        public object Tag { get => m_Tag; set => m_Tag = value; }
        public bool IsVisible { get => m_IsVisible; set => m_IsVisible = value; }
        public bool IsFocused { get => m_IsFocused; set => m_IsFocused = value; }

        public List<UIElement> Children { get => m_Children; set => m_Children = value; }

        public int Layer { get => m_Layer; }

        protected RectangleShape Shape { get => m_Shape; set => m_Shape = value; }
        public Vector2f Size
        {
            get => m_Size;
            set { m_Size = value; Shape.Size = m_Size; OnSizeChanged?.Invoke(); Update(); }
        }
        public Vector2f Location
        {
            get => m_Location;
            set { m_Location = value; Shape.Position = m_Location; OnLocationChanged?.Invoke(); Update(); }
        }
        #endregion

        #region UIElement API
        public UIElement(string name)
        {
            Name = name;
            Tag = null;
            IsVisible = true;

            m_Children = new List<UIElement>();
        }

        public void SetLayer(int layer)
        {
            m_Layer = layer;
        }

        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            if (Shape != null)
            {
                if (IsVisible)
                    target.Draw(Shape);
            }

            foreach (var child in Children)
            {
                if (child.IsVisible)
                {
                    target.Draw(child);
                }
            }
        }

        public void AddChild(UIElement element)
        {
            m_Children.Add(element);
        }
        public void AddChild(UIElement[] element)
        {
            m_Children.AddRange(element);
        }
        public void RemoveChild(UIElement element) {
            m_Children.Remove(element);
        }


        protected virtual void Update() { }


        public void OnClick(MouseButtonEventArgs e)
        {
            IsFocused = true;
            OnClicked?.Invoke(e);
        }
        public void OnMouseEnter(MouseMoveEventArgs e)
        {
            var bounds = Shape.GetGlobalBounds();

            if ((e.X > bounds.Left ||e.X < bounds.Left + bounds.Width) && (e.Y > bounds.Top || e.Y < bounds.Top + bounds.Height))
            {
                OnMouseEntered?.Invoke(e);
            }
        }
        public void OnMouseLeave(MouseMoveEventArgs e)
        {
            var bounds = Shape.GetGlobalBounds();

            if ((e.X < bounds.Left || e.X > bounds.Left + bounds.Width) && (e.Y < bounds.Top || e.Y > bounds.Top + bounds.Height))
            {
                OnMouseLeft?.Invoke(e);
            }
        }
        #endregion

        #region Events
        public event Action<MouseMoveEventArgs> OnMouseEntered;
        public event Action<MouseMoveEventArgs> OnMouseLeft;
        public event Action<MouseButtonEventArgs> OnClicked;
        public event Action OnSizeChanged;
        public event Action OnLocationChanged;
        #endregion
    }
}

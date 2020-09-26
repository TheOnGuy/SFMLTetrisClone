using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLTetrisKlon.src.Core;
using SFMLTetrisKlon.src.GameScenes;
using SFMLTetrisKlon.src.Scenes;
using SFMLTetrisKlon.src.UI.Components;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src.UI
{
    public class UserInterface : GameObject
    {
        #region Fields
        private UIElement m_Root;
        private bool m_IsDisabled = false;
        #endregion


        #region User Interface API
        public UserInterface(string name, Window w) : base(name)
        {
            w.MouseButtonPressed += HandleMouseButtonClickEvent;
            w.MouseMoved += MouseEnter;
            w.Resized += HandleResizeEvent;
            w.TextEntered += HandleTextInput;
            ConstructRootElement();
        }

        public void AddChild(UIElement element)
        {
            m_Root.AddChild(element);
        }
        public void AddChild(UIElement[] elements)
        {
            m_Root.AddChild(elements);
        }
        public void RemoveChild(UIElement element)
        {
            m_Root.RemoveChild(element);
        }


        public void Enable()
        {
            m_IsDisabled = false;
        }
        public void Disable()
        {
            m_IsDisabled = true;
        }

        public void Destroy(bool disposing, Window w)
        {
            w.MouseButtonPressed -= HandleMouseButtonClickEvent;
            w.MouseMoved -= MouseEnter;
            w.Resized -= HandleResizeEvent;
            w.TextEntered -= HandleTextInput;
            base.Destroy(disposing);
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            m_Root.Children = m_Root.Children.OrderBy(c => c.Layer).ToList();
            m_Root.Draw(target, states);
            m_Root.Children = m_Root.Children.OrderByDescending(c => c.Layer).ToList();
        }
        #endregion


        #region Event Function
        private void HandleTextInput(object sender, TextEventArgs e)
        {
            var element = GetFocusedElement(m_Root);

            if (element == null) { return; }
            TextBox tb = null;
            try
            {
                tb = (TextBox)element;
            }
            catch (Exception ex)
            {
                return;
            }
            tb.SetText(e);
        }
        private void HandleMouseButtonClickEvent(object sender, MouseButtonEventArgs e)
        {
            if (m_IsDisabled) { return; }

            UnfocusElements(m_Root);
            var element = GetAffectedElement(m_Root, e.X, e.Y);

            if (element != null)
            {
                element.OnClick(e);
            }
        }
        private void MouseEnter(object sender, MouseMoveEventArgs e)
        {
            if (m_IsDisabled) { return; }
            MouseLeave(sender, e);
            var element = GetAffectedElement(m_Root, e.X, e.Y);
            if (element != null)
            {
                element.OnMouseEnter(e);
            }
        }
        private void MouseLeave(object sender, MouseMoveEventArgs e)
        {
            if (m_IsDisabled) { return; }
            GetMouseEnteredElements(m_Root, e);
        }
        private void HandleResizeEvent(object sender, SizeEventArgs e)
        {
            Application.Game.Window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));

            var t = SceneManager.CurrentScene.GetType();
            Scene s = null;
            if (t == typeof(MainMenuScene))
            {
                s = new MainMenuScene();
            }
            else if (t == typeof(GameScene))
            {
                s = new GameScene();
            }
            SceneManager.Load(s);
        }
        #endregion


        #region Helper Functions
        private void GetMouseEnteredElements(UIElement element, MouseMoveEventArgs e)
        {
            foreach (var child in element.Children)
            {
                child.OnMouseLeave(e);
                GetMouseEnteredElements(child, e);
            }
        }
        private UIElement GetFocusedElement(UIElement element)
        {
            foreach (var child in element.Children)
            {
                if (child.IsFocused)
                {
                    return child;
                }
                GetFocusedElement(child);
            }

            return null;
        }
        private void UnfocusElements(UIElement element)
        {
            foreach (var child in element.Children)
            {
                if (child.IsFocused)
                {
                    Vector2f mousePos = new Vector2f(Mouse.GetPosition().X, Mouse.GetPosition().Y);
                    if ((mousePos.X < child.Location.X || mousePos.X > child.Location.X + child.Size.X) && 
                        (mousePos.Y < child.Location.Y ||mousePos.Y > child.Location.Y + child.Size.Y)) 
                    {
                        child.IsFocused = false;
                    }
                }
                UnfocusElements(child);
            }
        }

        private void ConstructRootElement()
        {
            m_Root = new UIElement("root");
            m_Root.IsVisible = false;
        }

        private UIElement GetAffectedElement(UIElement element, float x, float y)
        {
            // Search element that should receive the event
            foreach (var child in element.Children)
            {
                if ((x >= child.Location.X && x <= child.Location.X + child.Size.X) &&
                    (y >= child.Location.Y && y <= child.Location.Y + child.Size.Y))
                {
                    if (child.GetType() != typeof(Components.Image))
                    {
                        return child;
                    }
                }
                GetAffectedElement(child, x, y);
            }

            // Nothing found return root
            return null;
        }
        #endregion
    }
}

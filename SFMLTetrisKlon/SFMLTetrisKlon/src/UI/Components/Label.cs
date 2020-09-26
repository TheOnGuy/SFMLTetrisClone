using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src.UI.Components
{
    public enum Alignment
    {
        Left,
        Center,
        Right
    }

    public class Label : UIElement
    {
        #region Fields
        private Color m_BackgroundColor = Color.White;
        private Color m_ForegroundColor = Color.Black;

        private Font m_Font = new Font(@"assets/fonts/arial.ttf");
        private int m_FontSize = 10;
        private Text m_Text;
        private string m_Content = "Default Label Text";
        private Alignment m_Alignment = Alignment.Center;
        #endregion

        #region Properties
        public Color BackgroundColor
        {
            get => m_BackgroundColor;
            set { m_BackgroundColor = value; Shape.FillColor = m_BackgroundColor; }
        }
        public Color ForegroundColor
        {
            get => m_ForegroundColor;
            set { m_ForegroundColor = value; m_Text.FillColor = m_ForegroundColor; }
        }

        public Font Font { get => m_Font; set => m_Font = value; }
        public int FontSize
        {
            get => m_FontSize;
            set { m_FontSize = value; m_Text.CharacterSize = (uint)m_FontSize; }
        }
        public string Content
        {
            get => m_Content;
            set { m_Content = value; m_Text.DisplayedString = m_Content; }
        }
        public Alignment Alignment
        {
            get => m_Alignment;
            set { m_Alignment = value; Update(); }
        }
        #endregion

        #region Label API
        public Label(string name) : base(name)
        {
            Name = name;

            // Shape
            Shape = new RectangleShape()
            {
                Size = Size,
                FillColor = BackgroundColor,
            };

            // Text
            m_Text = new Text(Content, Font);
            m_Text.FillColor = m_ForegroundColor;
            m_Text.CharacterSize = (uint)FontSize;
            m_Text.Position = new Vector2f(
                x: Shape.Position.X + (Size.X / 2) - (m_Text.GetGlobalBounds().Width / 2),
                y: Shape.Position.Y + (Size.Y / 2) - (m_Text.CharacterSize / 2)
            );
        }


        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            target.Draw(m_Text);
        }
        #endregion


        protected override void Update()
        {
            Vector2f pos = new Vector2f(0, 0);

            switch (Alignment)
            {
                case Alignment.Left:
                    pos = new Vector2f(
                        x: Shape.Position.X + 5,
                        y: Shape.Position.Y + (Size.Y / 2) - (m_Text.CharacterSize / 2)
                    );
                    break;
                case Alignment.Center:
                    pos = new Vector2f(
                        x: Shape.Position.X + (Size.X / 2) - (m_Text.GetGlobalBounds().Width / 2),
                        y: Shape.Position.Y + (Size.Y / 2) - (m_Text.CharacterSize / 2)
                    );
                    break;
                case Alignment.Right:
                    pos = new Vector2f(
                        x: Shape.Position.X + (Size.X / 2) - (m_Text.GetGlobalBounds().Width / 2),
                        y: Shape.Position.Y + (Size.Y / 2) - (m_Text.CharacterSize / 2)
                    );
                    break;
            }

            m_Text.Position = pos;
        }

        #region Events
        #endregion
    }
}

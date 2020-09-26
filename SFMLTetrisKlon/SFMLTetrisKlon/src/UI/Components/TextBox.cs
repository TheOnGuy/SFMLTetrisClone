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
    public class TextBox : UIElement
    {
        #region Fields
        private Color m_BackgroundColor = Color.White;
        private Color m_ForegroundColor = Color.Black;

        private Font m_Font = new Font(@"assets/fonts/arial.ttf");
        private int m_FontSize = 10;
        private Text m_Text;
        private string m_Content = "Default Text";
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
        #endregion

        public TextBox(string name) : base(name)
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


        protected override void Update()
        {
            m_Text.Position = new Vector2f(
                x: Shape.Position.X + (Size.X / 2) - (m_Text.GetGlobalBounds().Width / 2),
                y: Shape.Position.Y + (Size.Y / 2) - (m_Text.CharacterSize / 2)
            );
        }


        public void SetText(TextEventArgs e)
        {
            if (e.Unicode == "\b")
            {
                if (Content.Length <= 0) { return; }
                Content = Content.Substring(0, Content.Length - 1);
            }
            else
            {
                Content += e.Unicode;
            }
            OnTextChanged?.Invoke();
        }

        #region Events
        public event Action OnTextChanged;
        #endregion
    }
}

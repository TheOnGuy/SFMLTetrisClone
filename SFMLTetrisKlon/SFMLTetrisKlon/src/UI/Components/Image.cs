using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src.UI.Components
{
    public class Image : UIElement
    {
        #region Fields
        private string m_ImagePath;
        private Texture m_Image;
        private byte m_Transparency;
        private bool m_Repeat;
        private Color m_Color = Color.White;
        #endregion

        #region Properties
        public Color Color
        {
            get => m_Color;
            set { m_Color = value; Update(); }
        }
        public byte Transparency
        {
            get => m_Transparency;
            set { m_Transparency = value; Update(); }
        }
        public bool Repeat
        {
            get => m_Repeat;
            set { m_Repeat = value; Update(); }
        }
        #endregion

        #region API
        public Image(string name, string image) : base(name)
        {
            m_ImagePath = image;
            Shape = new RectangleShape()
            {
                Size = Size
            };

            Shape.FillColor = Color.White;
            SetImage(m_ImagePath);
        }
        public Image(string name, SFML.Graphics.Image image) : base(name)
        {
            Shape = new RectangleShape()
            {
                Size = Size
            };

            Shape.FillColor = Color.White;
            SetImage(image);
        }


        public void SetImage(string image)
        {
            if (image == string.Empty)
                m_Image = new Texture(new SFML.Graphics.Image(100, 100, Color.Magenta));
            else
                m_Image = new Texture(image);

            Update();
        }
        public void SetImage(SFML.Graphics.Image image)
        {
            m_Image = new Texture(image);
            Update();
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Shape);
            base.Draw(target, states);
        }
        #endregion

        protected override void Update()
        {
            m_Image.Repeated = Repeat;
            m_Image.Smooth = true;
            Shape.Texture = m_Image;
            Shape.FillColor = new Color(Color.R, Color.G, Color.B, Transparency);
        }

        #region Events
        #endregion
    }
}

using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLTetrisKlon.src.Core;
using SFMLTetrisKlon.src.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src.GameMenus
{
    public class OptionsMenu : GameObject
    {
        #region Properties
        UserInterface m_Interface;

        UI.Components.Button buttonBack;

        UI.Components.Label labelVolumeText;
        UI.Components.TextBox textboxVolumeInput;

        UI.Components.Label labelScaleText;
        UI.Components.TextBox textboxScaleInput;

        UI.Components.Button buttonCancel;
        UI.Components.Button buttonApply;

        Block block;
        #endregion

        public OptionsMenu(string name, Window w) : base(name)
        {
            m_Interface = new UserInterface("OptionsMenu", w);
            InitializeComponents();
        }


        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(m_Interface);
            DrawPreviewPiece(target, states);
        }
        private void DrawPreviewPiece(RenderTarget t, RenderStates s)
        {
            var m_Block = new RectangleShape(BlockInformation.Size * Settings.Scale);
            for (int y = 0; y < block.BlockArray.GetLength(0); y++)
            {
                for (int x = 0; x < block.BlockArray.GetLength(1); x++)
                {
                    m_Block.Position = block.Position + (new Vector2f(x * BlockInformation.Size.X * Settings.Scale, y * BlockInformation.Size.Y * Settings.Scale));
                    if (block.BlockArray[y, x] > 0)
                    {
                        m_Block.FillColor = BlockInformation.BlockColors[block.BlockType];
                        t.Draw(m_Block);
                    }
                }
            }
        }
        public override void Update(Time elapsed)
        {
            m_Interface.Update(elapsed);
        }


        private void InitializeComponents()
        {
            // buttonBack
            buttonBack = new UI.Components.Button("buttonBack");
            buttonBack.Size = new Vector2f(120, 50);
            buttonBack.Location = new Vector2f(Application.Game.Window.Size.X * 0.05f, 5);
            buttonBack.Content = "Back";
            buttonBack.FontSize = 15;
            buttonBack.ForegroundColor = Color.White;
            buttonBack.BackgroundImage = new Image(@"assets/images/block_icon.png");
            buttonBack.OnClicked += ButtonBack_Clicked;
            buttonBack.SetLayer(1);


            // labelVolumeText
            labelVolumeText = new UI.Components.Label("labelVolumeText");
            labelVolumeText.Location = new Vector2f(Application.Game.Window.Size.X * 0.05f, Application.Game.Window.Size.Y * 0.15f);
            labelVolumeText.Size = new Vector2f(120, 50);
            labelVolumeText.Content = "Volume:";
            labelVolumeText.FontSize = 18;
            labelVolumeText.Alignment = UI.Components.Alignment.Center;
            labelVolumeText.BackgroundColor = Color.Transparent;
            labelVolumeText.ForegroundColor = Color.White;
            labelVolumeText.SetLayer(1);

            // textboxVolumeInput
            textboxVolumeInput = new UI.Components.TextBox("textboxVolumeInput");
            textboxVolumeInput.Location = new Vector2f(Application.Game.Window.Size.X * 0.35f, Application.Game.Window.Size.Y * 0.145f);
            textboxVolumeInput.Size = new Vector2f(120, 50);
            textboxVolumeInput.Content = Settings.Volume.ToString();
            textboxVolumeInput.FontSize = 18;
            textboxVolumeInput.BackgroundColor = Color.Transparent;
            textboxVolumeInput.ForegroundColor = Color.White;
            textboxVolumeInput.SetLayer(1);

            // labelScaleText
            labelScaleText = new UI.Components.Label("labelScaleText");
            labelScaleText.Location = new Vector2f(Application.Game.Window.Size.X * 0.05f, Application.Game.Window.Size.Y * 0.3f);
            labelScaleText.Size = new Vector2f(120, 50);
            labelScaleText.Content = "Scale:";
            labelScaleText.FontSize = 18;
            labelScaleText.Alignment = UI.Components.Alignment.Center;
            labelScaleText.BackgroundColor = Color.Transparent;
            labelScaleText.ForegroundColor = Color.White;
            labelScaleText.SetLayer(1);

            // textboxScaleInput
            textboxScaleInput = new UI.Components.TextBox("textboxScaleInput");
            textboxScaleInput.Location = new Vector2f(Application.Game.Window.Size.X * 0.35f, Application.Game.Window.Size.Y * 0.295f);
            textboxScaleInput.Size = new Vector2f(120, 50);
            textboxScaleInput.Content = Settings.Scale.ToString();
            textboxScaleInput.FontSize = 18;
            textboxScaleInput.BackgroundColor = Color.Transparent;
            textboxScaleInput.ForegroundColor = Color.White;
            textboxScaleInput.SetLayer(1);

            // block
            block.BlockType = 'T';
            block.BlockArray = BlockInformation.BlockArrays.Where(b => b.Key == block.BlockType).First().Value;
            block.Position = new Vector2f(Application.Game.Window.Size.X * 0.35f, Application.Game.Window.Size.Y * 0.4f);

            // buttonCancel
            buttonCancel = new UI.Components.Button("buttonCancel");
            buttonCancel.Location = new Vector2f(Application.Game.Window.Size.X * 0.05f, Application.Game.Window.Size.Y * 0.90f);
            buttonCancel.Size = new Vector2f(150, 50);
            buttonCancel.Content = "Cancel";
            buttonCancel.FontSize = 15;
            buttonCancel.ForegroundColor = Color.White;
            buttonCancel.BackgroundImage = new Image(@"assets/images/block_icon.png");
            buttonCancel.OnClicked += ButtonCancel_Click;
            buttonCancel.SetLayer(1);

            // buttonApply
            buttonApply = new UI.Components.Button("buttonCancel");
            buttonApply.Size = new Vector2f(150, 50);
            buttonApply.Location = new Vector2f(Application.Game.Window.Size.X * 0.95f - buttonApply.Size.X, Application.Game.Window.Size.Y * 0.90f);
            buttonApply.Content = "Apply Changes";
            buttonApply.FontSize = 15;
            buttonApply.ForegroundColor = Color.White;
            buttonApply.BackgroundImage = new Image(@"assets/images/block_icon.png");
            buttonApply.OnClicked += ButtonApply_Click;
            buttonApply.SetLayer(1);

            // buttonApply

            m_Interface.AddChild(new UIElement[]
            {
                labelVolumeText,
                textboxVolumeInput,
                labelScaleText,
                textboxScaleInput,
                buttonCancel,
                buttonApply,
                buttonBack,
            });
        }


        #region Event Functions
        private void ButtonCancel_Click(MouseButtonEventArgs e)
        {
            this.IsVisible = false;
        }
        private void ButtonApply_Click(MouseButtonEventArgs e)
        {
            Settings.Volume = Convert.ToInt32(textboxVolumeInput.Content);
            Settings.Scale = Convert.ToInt32(textboxScaleInput.Content);
            SoundManager.ChangeVolume(Settings.Volume);
            Settings.Save();
        }

        private void ButtonBack_Clicked(MouseButtonEventArgs e)
        {
            this.IsVisible = false;
        }
        #endregion
    }
}

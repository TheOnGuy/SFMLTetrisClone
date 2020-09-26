using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLTetrisKlon.src.Core;
using SFMLTetrisKlon.src.GameScenes;
using SFMLTetrisKlon.src.Scenes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src
{
    public class Arena : GameObject
    {
        #region Fields
        private char[,] m_PlayField; // 1st/2nd Dimension contain the block type
        private Clock m_ForceDownDelayTimer;
        private double m_ForceDownDelay = 1;

        private bool m_GameOver;

        private Queue<char> m_RandomBlocks;
        private Block m_CurrentBlock;
        private Block m_PreviewBlock;
        private RectangleShape m_Block = new RectangleShape(BlockInformation.Size);
        #endregion

        #region Properties
        public char CurrentPiece { get => m_CurrentBlock.BlockType; }
        public char PreviewBlock { get => m_PreviewBlock.BlockType; }

        public Vector2f ArenaSize { get; set; }
        public Vector2f ArenaScale { get; set; } = new Vector2f(1.0f, 1.0f);
        public bool GameOver { get => m_GameOver; }
        #endregion


        public Arena(string name) : base(name)
        {
            m_RandomBlocks = new Queue<char>(7);
            m_CurrentBlock = new Block();

            // Play field
            m_PlayField = InitializeArray(22, 12);
            SetCurrentPiece();

            // Drawing block
            m_Block.Texture = new Texture(@"assets/images/block_icon.png");
            m_Block.Texture.Smooth = true;
        }
        public Arena(string name, Vector2f scale) : base(name)
        {
            ArenaScale = scale;

            m_RandomBlocks = new Queue<char>(7);
            m_CurrentBlock = new Block();

            // Play field
            m_PlayField = InitializeArray(22, 12);
            SetCurrentPiece();

            // Drawing block
            m_Block.Texture = new Texture(@"assets/images/block_icon.png");
            m_Block.Texture.Smooth = true;
        }


        public void SetNewForceDelay(double seconds)
        {
            m_ForceDownDelay = seconds;
        }

        public override void Update(Time elapsed)
        {
            if (IsUpdateDisabled || m_GameOver) { return; }


            bool forceDown;
            if (m_ForceDownDelayTimer == null)
            {
                m_ForceDownDelayTimer = new Clock();
                forceDown = false;
            }
            else
                forceDown = m_ForceDownDelayTimer.ElapsedTime.AsSeconds() >= m_ForceDownDelay;

            if (forceDown)
            {
                MoveCurrentPieceDown();
                m_ForceDownDelayTimer.Restart();

            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            DrawArena(target, states);
            DrawCurrentPreviewPiece(target, states);
            DrawCurrentPiece(target, states);
            if (m_GameOver) { return; }
            CheckLine(target, states);
        }

        #region Movements
        public void MoveCurrentPieceDown()
        {
            Vector2f newPos;
            bool canMove;

            newPos = new Vector2f(m_CurrentBlock.Position.X, m_CurrentBlock.Position.Y + (BlockInformation.Size.Y * ArenaScale.Y));

            canMove = CheckMove(m_CurrentBlock, 0, 1);


            if (canMove)
            {
                m_CurrentBlock.Position = newPos;
                MovePreviewPiece();
            }


            m_ForceDownDelayTimer.Restart(); // Restart timer so the piece doesn't get moved twice in one update
        }
        public void MovePreviewPiece()
        {
            m_PreviewBlock.Position = m_CurrentBlock.Position;

            bool canMove = CheckMove(m_PreviewBlock, 0, 1);
            while (canMove)
            {
                //if (canMove)
                m_PreviewBlock.Position = new Vector2f(m_PreviewBlock.Position.X, m_PreviewBlock.Position.Y + (BlockInformation.Size.Y * ArenaScale.Y)); ;
                canMove = CheckMove(m_PreviewBlock, 0, 1);
            }
            //m_ForceDownDelayTimer.Restart(); // Restart timer so the piece doesn't get moved twice in one update
        }

        public void MoveCurrentPieceHorizontally(bool rightDir)
        {
            Vector2f newPos;
            bool canMove;

            if (rightDir)
                newPos = new Vector2f(m_CurrentBlock.Position.X + (BlockInformation.Size.X * ArenaScale.X), m_CurrentBlock.Position.Y);
            else
                newPos = new Vector2f(m_CurrentBlock.Position.X - (BlockInformation.Size.X * ArenaScale.X), m_CurrentBlock.Position.Y);

            if (rightDir)
                canMove = CheckMove(m_CurrentBlock, 1, 0);
            else
                canMove = CheckMove(m_CurrentBlock, -1, 0);

            if (canMove)
            {
                m_CurrentBlock.Position = newPos;
                MovePreviewPiece();
            }
        }

        public void RotateCurrentPiece()
        {
            m_CurrentBlock.BlockArray = m_CurrentBlock.Rotate();
            if (!CheckMove(m_CurrentBlock, 0, 0))
            {
                m_CurrentBlock.BlockArray = m_CurrentBlock.Rotate();
                m_CurrentBlock.BlockArray = m_CurrentBlock.Rotate();
                m_CurrentBlock.BlockArray = m_CurrentBlock.Rotate();
            }
            else
            {
                m_PreviewBlock.BlockArray = m_PreviewBlock.Rotate();
                MovePreviewPiece();
            }
            m_ForceDownDelayTimer.Restart(); // Restart timer so the piece gets locked after rotation
        }
        #endregion





        #region Block 
        private void InitializeBlock(char type)
        {
            m_CurrentBlock.BlockType = type;
            m_CurrentBlock.BlockArray = BlockInformation.BlockArrays.Where(b => b.Key == m_CurrentBlock.BlockType).First().Value;
            m_CurrentBlock.Position = new Vector2f(
                x: m_PlayField.GetLength(1) * BlockInformation.Size.X * ArenaScale.X / 2,
                y: 0
            );
            var canSpawn = CheckMove(m_CurrentBlock, 0, 0);
            if (canSpawn) { return; }

            // Game over here
            m_GameOver = true;
            Console.WriteLine("game over");
            var gameScene = (GameScene)SceneManager.CurrentScene;
            gameScene.m_PlayerController.Save(); // Save highscore to file
            gameScene.m_GameOverMenu.IsVisible = true;
            gameScene.m_GameOverMenu.IsUpdateDisabled = false;
            gameScene.m_PauseMenu.IsUpdateDisabled = true;
            gameScene.m_GameMenu.IsUpdateDisabled = true;
        }

        /// <summary>
        /// Select a random block from a queue. If queue is empty renew it with a new random selection of blocks.
        /// </summary>
        public void SetCurrentPiece()
        {
            const string types = "IJLOSTZ";
            if (m_RandomBlocks.Count == 0)
            {
                Random rnd = new Random();
                for (int i = 0; i < 7; i++)
                {
                    var c = types[rnd.Next(0, types.Length)];
                    if (!m_RandomBlocks.Contains(c))
                        m_RandomBlocks.Enqueue(c);
                    else
                        i--; // Loop until one is found
                }
            }

            InitializeBlock(m_RandomBlocks.Dequeue());
            m_PreviewBlock = new Block() { BlockArray = m_CurrentBlock.BlockArray, Position = m_CurrentBlock.Position, BlockType = m_CurrentBlock.BlockType };
            MovePreviewPiece();
        }

        private void DrawCurrentPiece(RenderTarget t, RenderStates s)
        {
            for (int y = 0; y < m_CurrentBlock.BlockArray.GetLength(0); y++)
            {
                for (int x = 0; x < m_CurrentBlock.BlockArray.GetLength(1); x++)
                {
                    m_Block.Position = m_CurrentBlock.Position + (new Vector2f(x * BlockInformation.Size.X * ArenaScale.X, y * BlockInformation.Size.Y * ArenaScale.Y));
                    if (m_CurrentBlock.BlockArray[y, x] > 0)
                    {
                        m_Block.FillColor = BlockInformation.BlockColors[m_CurrentBlock.BlockType];
                        m_Block.OutlineColor = m_Block.FillColor;
                        t.Draw(m_Block);
                    }
                }
            }
        }

        private void DrawCurrentPreviewPiece(RenderTarget t, RenderStates s)
        {
            var oldOutlineColor = m_Block.OutlineColor;
            var oldOutlineThickness = m_Block.OutlineThickness;

            for (int y = 0; y < m_PreviewBlock.BlockArray.GetLength(0); y++)
            {
                for (int x = 0; x < m_PreviewBlock.BlockArray.GetLength(1); x++)
                {
                    m_Block.Position = m_PreviewBlock.Position + (new Vector2f(x * BlockInformation.Size.X * ArenaScale.X, y * BlockInformation.Size.Y * ArenaScale.Y));
                    if (m_PreviewBlock.BlockArray[y, x] > 0)
                    {
                        Color col;
                        BlockInformation.BlockColors.TryGetValue(m_PreviewBlock.BlockType, out col);

                        m_Block.FillColor = new Color(col.R, col.G, col.B, 100);
                        m_Block.OutlineColor = col;
                        m_Block.OutlineThickness = 1.25f;
                        t.Draw(m_Block);
                    }
                }
            }

            m_Block.OutlineColor = oldOutlineColor;
            m_Block.OutlineThickness = oldOutlineThickness;
        }
        #endregion




        #region Playfield functions
        private char[,] InitializeArray(int h, int w)
        {
            char[,] ret = new char[h, w];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (y == (h - 1))
                        ret[y, x] = '#';
                    else
                    {
                        if (x == 0 || x == w - 1)
                            ret[y, x] = '#';
                        else
                            ret[y, x] = ' ';
                    }
                }
            }

            return ret;
        }


        /// <summary>
        /// Writes the piece to the playfield and selects a new one.
        /// </summary>
        private void WritePieceToPlayfield()
        {
            int posX = (int)(m_CurrentBlock.Position.X / (ArenaScale.X * 10));
            int posY = (int)(m_CurrentBlock.Position.Y / (ArenaScale.Y * 10));

            for (int y = 0; y < m_CurrentBlock.BlockArray.GetLength(0); y++)
            {
                for (int x = 0; x < m_CurrentBlock.BlockArray.GetLength(1); x++)
                {
                    if (m_CurrentBlock.BlockArray[y, x] == 0) { continue; }

                    m_PlayField[posY, posX + x] = m_CurrentBlock.BlockType;
                }
                posY++;
            }
        }


        /// <summary>
        /// Check if a line was formed. If line was formed, destroy it and move everything done one block.
        /// </summary>
        private void CheckLine(RenderTarget t, RenderStates s)
        {
            int y = m_PlayField.GetLength(0) - 2;
            var playerController = ((GameScene)Parent).m_PlayerController;

            int linesCleared = 0;
            // Additional for loop to count lines, because 
            // Playfield gets modified each time MovePlayFieldDown()
            // is called
            for (int i = 0; i < m_PlayField.GetLength(0); i++)
            {
                string st = string.Empty;
                for (int x = 1; x < m_PlayField.GetLength(1) - 1; x++)
                {
                    st += m_PlayField[i, x];
                }
                if (st.Contains(' ') || st.Contains('#')) { continue; }

                linesCleared++;
            }
            playerController.UpdateScore(linesCleared);


            for (; y >= 0; y--)
            {
                string st = string.Empty;
                for (int x = 0; x < m_PlayField.GetLength(1); x++)
                {
                    st += m_PlayField[y, x];
                }
                if (st.Contains(' ')) { continue; }

                MovePlayfieldDown(y);
                DrawArena(t, s);
                // Increment y again, because lines get skipped
                // after the playfield is moved down one.
                y++;
            }
        }

        private void MovePlayfieldDown(int row)
        {
            char[,] newArray = m_PlayField;

            for (int y = row; y > 0; y--)
            {
                for (int x = 0; x < m_PlayField.GetLength(1); x++)
                {
                    newArray[y, x] = m_PlayField[y - 1, x];
                }
            }

            m_PlayField = newArray;
        }

        private bool CheckMove(Block m_CurrentBlock, int X, int Y)
        {
            char[,] overlay = new char[m_PlayField.GetLength(0), m_PlayField.GetLength(1)];
            int BlockHeightInArena = (int)(m_CurrentBlock.Position.Y / (ArenaScale.Y * 10)) + Y;
            int BlockWidthInArena = (int)(m_CurrentBlock.Position.X / (ArenaScale.Y * 10)) + X;

            if (BlockHeightInArena + m_CurrentBlock.BlockArray.GetLength(0) > m_PlayField.GetLength(0) ||
                BlockHeightInArena < 0 ||
                BlockWidthInArena + m_CurrentBlock.BlockArray.GetLength(1) > m_PlayField.GetLength(1) ||
                BlockWidthInArena < 0)
            {
                return false;
            }

            for (int i = BlockHeightInArena; i < BlockHeightInArena + m_CurrentBlock.BlockArray.GetLength(0); i++)
            {
                for (int j = BlockWidthInArena; j < BlockWidthInArena + m_CurrentBlock.BlockArray.GetLength(1); j++)
                {
                    if (m_CurrentBlock.BlockArray[i - BlockHeightInArena, j - BlockWidthInArena] == 1)
                    {
                        overlay[i, j] = '1';
                    }
                }
            }

            for (int i = 0; i < m_PlayField.GetLength(0); i++)
            {
                for (int j = 0; j < m_PlayField.GetLength(1); j++)
                {
                    if (m_PlayField[i, j] != ' ' && overlay[i, j] == '1')
                    {
                        if (Y != 0 && m_CurrentBlock.Equals(this.m_CurrentBlock) && !GameOver)
                        {
                            WritePieceToPlayfield();
                            SetCurrentPiece();
                        }
                        return false;
                    }
                }
            }
            return true;
        }



        private void DrawArena(RenderTarget t, RenderStates s)
        {
            m_Block.Size = new Vector2f(
                x: BlockInformation.Size.X * ArenaScale.X,
                y: BlockInformation.Size.Y * ArenaScale.Y
            );
            m_Block.FillColor = Color.White;

            for (int y = 0; y < m_PlayField.GetLength(0); y++)
            {
                for (int x = 0; x < m_PlayField.GetLength(1); x++)
                {
                    m_Block.Position = new Vector2f(x * BlockInformation.Size.X * ArenaScale.X, y * BlockInformation.Size.Y * ArenaScale.Y);
                    if (m_PlayField[y, x] == '#')
                    {
                        m_Block.FillColor = Color.White;
                    }
                    else
                    {
                        Color col = BlockInformation.BlockColors.Where(c => c.Key == m_PlayField[y, x]).FirstOrDefault().Value;
                        m_Block.FillColor = col != null ? col : Color.Magenta;
                    }
                    t.Draw(m_Block);
                }
            }
        }



        private void DisplayArena()
        {
            Console.Clear();

            for (int y = 0; y < m_PlayField.GetLength(0); y++)
            {
                for (int x = 0; x < m_PlayField.GetLength(1); x++)
                {
                    Console.Write(m_PlayField[y, x]);
                }
                Console.WriteLine();
            }
        }
        #endregion
    }
}

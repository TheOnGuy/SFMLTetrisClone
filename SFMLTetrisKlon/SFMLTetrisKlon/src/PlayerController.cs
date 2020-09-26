using SFML.Audio;
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
    public class PlayerController : GameObject
    {
        #region Properties
        private Clock m_DelayTimer;
        private int m_Delay = 150;
        private Arena Arena { get; set; }
        private int m_LevelLines;

        private const string m_SaveFile = @"assets/save/tetris.save";


        public Clock ArenaTime;
        public int CurrentScore { get; private set; }
        public int Level { get; set; } = 1;
        public int Lines { get; set; }
        public int Highscore { get; private set; }
        #endregion

        public PlayerController(string name, Arena arena) : base(name)
        {
            Arena = arena;
            ArenaTime = new Clock();
            Load();
        }


        public override void Update(Time elapsed)
        {
            if (IsUpdateDisabled || Arena.GameOver) { return; }

            bool delayOver;
            if (m_DelayTimer == null)
            {
                delayOver = true; m_DelayTimer = new Clock();
            }
            else
            {
                delayOver = m_DelayTimer.ElapsedTime.AsMilliseconds() >= m_Delay;
            }


            if ((Keyboard.IsKeyPressed(Keyboard.Key.Left) || Keyboard.IsKeyPressed(Keyboard.Key.A)) && delayOver)
            {
                Arena.MoveCurrentPieceHorizontally(false);
                m_DelayTimer.Restart();
            }
            if ((Keyboard.IsKeyPressed(Keyboard.Key.Right) || Keyboard.IsKeyPressed(Keyboard.Key.D)) && delayOver)
            {
                Arena.MoveCurrentPieceHorizontally(true);
                m_DelayTimer.Restart();
            }
            if ((Keyboard.IsKeyPressed(Keyboard.Key.Down) || Keyboard.IsKeyPressed(Keyboard.Key.S)) && delayOver)
            {
                Arena.MoveCurrentPieceDown();
                m_DelayTimer.Restart();
            }
#if DEBUG
            if (Keyboard.IsKeyPressed(Keyboard.Key.C) && delayOver)
            {
                Arena.SetCurrentPiece();
                m_DelayTimer.Restart();
            }
#endif


            if (Keyboard.IsKeyPressed(Keyboard.Key.R) && delayOver)
            {
                Arena.RotateCurrentPiece();
                m_DelayTimer.Restart();
            }
            
            if ((Keyboard.IsKeyPressed(Keyboard.Key.Escape) || Keyboard.IsKeyPressed(Keyboard.Key.P)) && delayOver)
            {
                var s = (GameScene)SceneManager.CurrentScene;
                s.m_PauseMenu.IsVisible = true;
                s.m_PauseMenu.IsUpdateDisabled = false;
                m_DelayTimer.Restart();
            }
        }
   
        public void UpdateScore(int amountLines)
        {
            int oldCurrentScore = CurrentScore;
            Lines += amountLines;
            m_LevelLines += amountLines;

            if (m_LevelLines >= 10)
            {
                m_LevelLines = 0;
                Level += 1;
                //Time = (0.8-((Level-1)*0.007))(Level-1)
                double newTime = Math.Pow((double)(0.8 - ((Level - 1) * 0.007)), (double)(Level - 1));
                ((GameScene)SceneManager.CurrentScene).m_Arena.SetNewForceDelay(newTime);
                // Make sound faster
                Sound s;
                SoundManager.Sounds.TryGetValue("GameSound", out s);
                s.Pitch = (float)newTime;
            }

            switch (amountLines)
            {
                case 1:
                    CurrentScore += 40;
                    break;
                case 2:
                    CurrentScore += 100;
                    break;
                case 3:
                    CurrentScore += 300;
                    break;
                case 4:
                    CurrentScore += 1200;
                    break;
            }

            if (CurrentScore > Highscore)
            {
                Highscore = CurrentScore;
            }

            if (CurrentScore > oldCurrentScore)
            {
                Console.Clear();
                Console.WriteLine($"Highscore: {Highscore}");
                Console.WriteLine($"Score: {CurrentScore}");
                Console.WriteLine($"Lines: {Lines}");
            }
        }

        public void Save()
        {
            if (!File.Exists(m_SaveFile))
                File.Create(m_SaveFile).Close();

            using (StreamWriter sw = new StreamWriter(m_SaveFile))
            {
                sw.Write($"highscore={Highscore}");
            }
        }
        public void Load()
        {
            if (!File.Exists(m_SaveFile)) { return; }

            using (StreamReader sw = new StreamReader(m_SaveFile))
            {
                var line = sw.ReadLine();
                var key = line.Split('=')[0];
                var val = line.Split('=')[1];

                if (key == "highscore")
                {
                    Highscore = Convert.ToInt32(val);
                }
            }
        }
    }
}

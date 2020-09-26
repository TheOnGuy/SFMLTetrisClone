using SFML.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src.Core
{
    static class Application
    {
        public static Game Game;

        public static void Run()
        {
            // Settingss
            Settings.Load();

            // Add the music
            var menuMusic = new Sound(new SoundBuffer("assets/music/menu.ogg"));
            menuMusic.Loop = true;
            menuMusic.Volume = Settings.Volume;
            SoundManager.AddSound("MenuSound", menuMusic);
            var gameMusic = new Sound(new SoundBuffer("assets/music/game.ogg"));
            gameMusic.Loop = true;
            gameMusic.Volume = Settings.Volume;
            SoundManager.AddSound("GameSound", gameMusic);

            Game = new Game("Tetris", new SFML.System.Vector2f(450, 600));
            
            Game.Run();
        }
    }
}

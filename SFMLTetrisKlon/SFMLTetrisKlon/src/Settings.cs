using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src
{
    public static class Settings
    {
        private const string m_SettingsFile = @"assets/settings/tetris.settings";
        private static int volume = 100;
        private static float scale = 1;

        public static int Volume 
        { 
            get => volume;
            set 
            {
                if (value > 100)
                    volume = 100;
                else if (value < 0)
                    volume = 0;
                else
                    volume = value;
            } 
        }

        public static float Scale
        {
            get => scale;
            set
            {
                if (value > 100)
                    scale = 100;
                else if (value < 0)
                    scale = 0;
                else
                    scale = value;
            }
        }

        public static void Save()
        {
            if (!File.Exists(m_SettingsFile))
                File.Create(m_SettingsFile).Close();

            using (StreamWriter sw = new StreamWriter(m_SettingsFile))
            {
                sw.WriteLine($"volume={Volume}");
                sw.WriteLine($"scale={Scale}");
            }
        }

        public static void Load()
        {
            if (!File.Exists(m_SettingsFile)) { return; }

            using (StreamReader sw = new StreamReader(m_SettingsFile))
            {
                while (!sw.EndOfStream)
                {
                    var line = sw.ReadLine();
                    var key = line.Split('=')[0];
                    var val = line.Split('=')[1];

                    switch (key)
                    {
                        case "volume":
                            Volume = Convert.ToInt32(val);
                            break;
                        case "scale":
                            Scale = (float)Convert.ToDouble(val);
                            break;
                        default:
                            break;
                    }
                }
                
                
            }
        }
    }
}

using SFML.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src
{
    public static class SoundManager
    {
        public static Dictionary<string, Sound> Sounds = new Dictionary<string, Sound>();


        public static void PlaySound(string name)
        {
            Sound s;
            Sounds.TryGetValue(name, out s);
            s?.Play();
        }
        public static void StopSound(string name)
        {
            Sound s;
            Sounds.TryGetValue(name, out s);
            s?.Stop();
        }

        public static void AddSound(string name, Sound s)
        {
            Sounds.Add(name, s);
        }
        public static void RemoveSound(string name)
        {
            Sounds.Remove(name);
        }

        public static void ChangeVolume(int newVolume)
        {
            foreach (var s in Sounds.Values)
            {
                s.Volume = newVolume;
            }
        }
    }
}

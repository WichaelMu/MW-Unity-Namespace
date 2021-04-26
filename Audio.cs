using System;
using UnityEngine;

namespace MW.Audio {


    [Serializable]
    public class Sound {
        public AudioClip Sounds;
        public string Name;

        [Range(0f, 1f)]
        public float Volume = 1, Pitch = 1;

        public bool Loop;

        [HideInInspector]
        public AudioSource sound;
    }

    /// <summary>The Audio controller for in-game sounds.</summary>
    public class Audio : MonoBehaviour {
        public static Audio AudioLogic;

        public bool MuteAll;
        public Sound[] Sounds;

        const string kErr1 = "Sound of name: ";
        const string kErr2 = " could not be ";

        /// <summary>Populates the Sounds array to match the settings.</summary>
        public void Initialise(Sound[] Sounds) {
            if (AudioLogic == null) {
                AudioLogic = this;
                gameObject.name = "Audio";
            } else {
                Debug.LogWarning("Ensure there is only one Audio object in the scene and that only one is being initialised");
                Destroy(gameObject);
            }

            this.Sounds = Sounds;

            if (!MuteAll)
                foreach (Sound s in Sounds) {
                    s.sound = gameObject.AddComponent<AudioSource>();
                    s.sound.clip = s.Sounds;
                    s.sound.volume = s.Volume;
                    s.sound.pitch = s.Pitch;
                    s.sound.loop = s.Loop;
                }
        }

        /// <summary>Plays sound of name n.</summary>
        /// <param name="n">The name of the requested sound to play in capital casing.</param>

        public void Play(string n) {
            if (MuteAll)
                return;
            Sound s = Find(n);
            if (s != null && !IsPlaying(s))
                s.sound.Play();
            if (s == null)
                Debug.LogWarning(kErr1 + n + kErr2 + "played!");
        }

        /// <summary>Stops sound of name n.</summary>
        /// <param name="n">The name of the requested sound to stop playing in capital casing.</param>

        public void Stop(string n) {
            if (MuteAll)
                return;
            Sound s = Find(n);
            if (s != null && IsPlaying(s))
                s.sound.Stop();
            if (s == null)
                Debug.LogWarning(kErr1 + n + kErr2 + "stopped!");
        }

        /// <summary>Stop every sound in the game.</summary>

        public void StopAll() {
            if (MuteAll)
                return;
            foreach (Sound s in Sounds)
                s.sound.Stop();
        }

        /// <summary>Returns a sound in the Sounds array.</summary>
        /// <param name="n">The name of the requested sound.</param>
        /// <returns>The sound clip of the requested sound.</returns>

        Sound Find(string n) {
            return Array.Find(Sounds, sound => sound.Name == n);
        }

        /// <summary>Whether or not sound of name n is playing.</summary>
        /// <param name="n">The name of the sound to query in capital casing.</param>
        public bool IsPlaying(string n) {
            if (MuteAll)
                return false;
            return Find(n).sound.isPlaying;
        }

        bool IsPlaying(Sound s) {
            return s.sound.isPlaying;
        }
    }
}

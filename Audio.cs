using System;
using UnityEngine;

namespace MW.Audio {


    [Serializable]
    public class Sound {
        public AudioClip ACSounds;
        public string sName;

        [Range(0f, 1f)]
        public float fVVolume = 1, fPitch = 1;

        public bool bLoop;

        [HideInInspector]
        public AudioSource ASSound;
    }

    /// <summary>The Audio controller for in-game sounds.</summary>
    public class AudioControl : MonoBehaviour {
        public static AudioControl AAudioLogic;

        public bool bMuteAll;
        public Sound[] SSounds;

        const string kErr1 = "Sound of name: ";
        const string kErr2 = " could not be ";

        /// <summary>Populates the Sounds array to match the settings.</summary>
        public void Initialise(Sound[] SSounds) {
            if (AAudioLogic == null) {
                AAudioLogic = this;
                gameObject.name = "Audio";
            } else {
                Debug.LogWarning("Ensure there is only one Audio object in the scene and that only one is being initialised");
                Destroy(gameObject);
            }

            this.SSounds = SSounds;

            if (!bMuteAll)
                foreach (Sound s in SSounds) {
                    s.ASSound = gameObject.AddComponent<AudioSource>();
                    s.ASSound.clip = s.ACSounds;
                    s.ASSound.volume = s.fVVolume;
                    s.ASSound.pitch = s.fPitch;
                    s.ASSound.loop = s.bLoop;
                }
        }

        /// <summary>Plays sound of name n.</summary>
        /// <param name="sName">The name of the requested sound to play in capital casing.</param>

        public void Play(string sName) {
            if (bMuteAll)
                return;
            Sound s = Find(sName);
            if (s != null && !IsPlaying(s))
                s.ASSound.Play();
            if (s == null)
                Debug.LogWarning(kErr1 + sName + kErr2 + "played!");
        }

        /// <summary>Stops sound of name n.</summary>
        /// <param name="sName">The name of the requested sound to stop playing in capital casing.</param>

        public void Stop(string sName) {
            if (bMuteAll)
                return;
            Sound s = Find(sName);
            if (s != null && IsPlaying(s))
                s.ASSound.Stop();
            if (s == null)
                Debug.LogWarning(kErr1 + sName + kErr2 + "stopped!");
        }

        /// <summary>Stop every sound in the game.</summary>

        public void StopAll() {
            if (bMuteAll)
                return;
            foreach (Sound s in SSounds)
                s.ASSound.Stop();
        }

        /// <summary>Returns a sound in the Sounds array.</summary>
        /// <param name="n">The name of the requested sound.</param>
        /// <returns>The sound clip of the requested sound.</returns>

        Sound Find(string n) {
            return Array.Find(SSounds, sound => sound.sName == n);
        }

        /// <summary>Whether or not sound of name n is playing.</summary>
        /// <param name="sName">The name of the sound to query in capital casing.</param>
        public bool IsPlaying(string sName) {
            if (bMuteAll)
                return false;
            return Find(sName).ASSound.isPlaying;
        }

        bool IsPlaying(Sound s) {
            return s.ASSound.isPlaying;
        }
    }
}

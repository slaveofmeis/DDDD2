using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
namespace DDDD2.GameComponents
{
    public class AudioManager
    {
        private const float SOUND_CREMENT_VALUE = 0.0125f;
        private const float CREMENT_VALUE = 0.01f;
        private const float TRANSITION_CREMENT_VALUE = 0.05f;
        // Start Menu Screen?
        
        private const string NAVIGATION = "Sounds/sf3_sfx_menu_select";
        private const string SELECT = "Sounds/29929__DJ_Chronos__Menu_Nav_3-2";
        private const string BACK = "Sounds/sf3_sfx_menu_back";
        private const string BUZZ = "Sounds/107871__awfulTHEsample__noizz7";
        private const string POSITIVE = "Sounds/sf3_sfx_menu_validate";
        private const string EQUIP = "Sounds/21693__ice9ine__light_switch";
        private const string CLICK = "Sounds/113218__satrebor__click";
        private const string CACHING = "Sounds/91924__Benboncan__Till_With_Bell";
       

        private List<SoundEffectInstance> soundList;
        private Song mySong;
        // Battle Related
        private SoundEffectInstance myBattleSong, randEncSound, battleWonSound,
            bossBattleSong, regularBattleSong;

        Game myGame;
        string currentSong, nextMapSong;
        private bool isTransitioning, isFadeOut, isPausing, isSwitchingMap;
        private float fakeVolume;
        private Dictionary<SoundEffectInstance, bool> fadeOutSounds;
        //private Dictionary<string, SoundEffectInstance> eventSoundsDict;

        public AudioManager(Game game)
        {
            myGame = game;
            currentSong = "";
            nextMapSong = "";
            isTransitioning = false;
            isFadeOut = false;
            isPausing = false;
            isSwitchingMap = false;
            fakeVolume = 0;
            fadeOutSounds = new Dictionary<SoundEffectInstance, bool>();
            soundList = new List<SoundEffectInstance>();
        }

        public void LoadContent()
        {
            /*
            // BATTLE
            randEncSound = myGame.Content.Load<SoundEffect>(RANDOM_ENCOUNTER).CreateInstance();
            battleWonSound = myGame.Content.Load<SoundEffect>(BATTLE_WON).CreateInstance();
            bossBattleSong = myGame.Content.Load<SoundEffect>(BOSS_BATTLE).CreateInstance();
            regularBattleSong = myGame.Content.Load<SoundEffect>(REGULAR_BATTLE).CreateInstance();
            // EVENT
            sadSong = myGame.Content.Load<SoundEffect>(SAD_SONG).CreateInstance();
            eventSoundsDict.Add("sadsong", sadSong);
            fadeOutSounds.Add(battleWonSound, false);
            fadeOutSounds.Add(bossBattleSong, false);
            fadeOutSounds.Add(regularBattleSong, false);
            */

        }

        public bool IsPlaying
        {
            get { return MediaPlayer.State == MediaState.Stopped; }
        }

        public void Play(string songToPlay)
        {
            //currentSong.Equals("Music/" + songToPlay)
            if (currentSong.Equals("Music/" + songToPlay) && MediaPlayer.State != MediaState.Stopped && MediaPlayer.State != MediaState.Paused)
            {
            }
            else
            {
                //MediaPlayer.Pause();
                MediaPlayer.Stop();
                mySong = myGame.Content.Load<Song>("Music/" + songToPlay);
                MediaPlayer.Volume = 0f;
                //MediaPlayer.Resume();
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(mySong);
                fakeVolume = 0f;
                isTransitioning = true;
            }
            currentSong = "Music/" + songToPlay;
        }

        public void PlayMapSwitch(string songToPlay)
        {
            //mySong = myGame.Content.Load<Song>("Music\\" + songToPlay);
            /*if (currentSong.Equals("Music/" + songToPlay) && MediaPlayer.State != MediaState.Stopped && MediaPlayer.State != MediaState.Paused)
            {
            }
            */
            //else
            //{
                isSwitchingMap = true;
                /*MediaPlayer.Resume();
                MediaPlayer.Play(mySong);
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = 0f;
                fakeVolume = 0f;
                isTransitioning = true;*/
            //}
            nextMapSong = songToPlay;
        }

        public void PlayEventSound(string sound, bool loop)
        {
            //PlaySound(eventSoundsDict[sound], loop);
        }
        public void StopEventSound(string sound)
        {
            //StopSound(eventSoundsDict[sound]);
        }

        public void Pause()
        {
            MediaPlayer.Pause();
        }

        public void PauseFromEvent()
        {
            isPausing = true;
        }

        public void Resume()
        {
            MediaPlayer.Volume = 0f;
            fakeVolume = 0f;
            isTransitioning = true;
            MediaPlayer.Resume();
        }

        public void Stop()
        {
            MediaPlayer.Stop();
            //isFadeOut = true;
        }

        private void PlaySound(SoundEffectInstance mySound, bool looped)
        {
            fadeOutSounds[mySound] = false;
            if (looped && !mySound.IsLooped)
                mySound.IsLooped = true;
            mySound.Volume = 0.5f;
            mySound.Play();
        }
        private void StopSound(SoundEffectInstance mySound)
        {
            fadeOutSounds[mySound] = true;
            //mySound.Stop();
        }

        private SoundEffectInstance LoadSoundInstance(string sound)
        {
            SoundEffectInstance aSound = myGame.Content.Load<SoundEffect>(sound).CreateInstance();
            aSound.IsLooped = false;
            return aSound;
        }

        #region MenuSounds

        private void PlayStaticSound(string sound)
        {
            SoundEffectInstance aSound = myGame.Content.Load<SoundEffect>(sound).CreateInstance();
            aSound.IsLooped = false;
            //aSound.Pitch = 1;
            soundList.Add(aSound);
            aSound.Play();
        }
        public void PlayNavSound()
        {
            PlayStaticSound(NAVIGATION);
        }
        public void PlaySelectSound()
        {
            PlayStaticSound(SELECT);
        }
        public void PlayBackSound()
        {
            PlayStaticSound(BACK);
        }
        public void PlayEquipSound()
        {
            PlayStaticSound(EQUIP);
        }
        public void PlayPositiveSound()
        {
            PlayStaticSound(POSITIVE);
        }
        public void PlayBuzzSound()
        {
            PlayStaticSound(BUZZ);
        }
        public void PlayClickSound()
        {
            PlayStaticSound(CLICK);
        }
        public void PlayCaChingSound()
        {
            PlayStaticSound(CACHING);
        }

        #endregion

        /* region battle action sounds
        public void PlaySpecifiedSound(string sound)
        {
            SoundEffectInstance aSound = myGame.Content.Load<SoundEffect>(battleSoundsDict[sound]).CreateInstance();
            //aSound.Pitch = 1;
            soundList.Add(aSound);
            aSound.Play();
        }
        public void PlayGotHitSound()
        {
            PlayStaticSound(TACKLE_1);
        }
        public void PlayUseItemSound()
        {
            PlayStaticSound(POSITIVE);
        }
        public void PlayUseSkillSound()
        {
            PlayStaticSound(SKILL_START);
        }
        public void PlayMonsterDie()
        {
            PlayStaticSound(DIE_1);
        }
        public void PlaySwipeSound()
        {
            PlayStaticSound(SWIPE_1);
        }
        */

        private void CheckFadeOutSongs()
        {
            foreach (KeyValuePair<SoundEffectInstance, bool> kvp in fadeOutSounds)
            {
                if (kvp.Value)
                {
                    if ((kvp.Key.Volume - SOUND_CREMENT_VALUE) < 0)
                    {
                        kvp.Key.Volume = 0;
                        if (kvp.Key.State == SoundState.Playing)
                            kvp.Key.Stop();
                    }
                    else
                    {
                        kvp.Key.Volume -= SOUND_CREMENT_VALUE;
                    }
                }
            }
        }

        public List<SoundEffectInstance> getSoundList()
        {
            return soundList;
        }

        private void DisposeSounds()
        {
            int indexOfSound = int.MaxValue;
            foreach (SoundEffectInstance s in soundList)
            {
                if (s.State == SoundState.Stopped || s.State == SoundState.Paused)
                {
                    indexOfSound = soundList.IndexOf(s);
                }
            }
            SoundEffectInstance disposeMe;
            if (indexOfSound != int.MaxValue)
            {
                disposeMe = soundList[indexOfSound];
                soundList.RemoveAt(indexOfSound);
                disposeMe.Dispose();
            }
        }

        public void Update()
        {
            //Console.WriteLine("Volume: " + MediaPlayer.Volume + " Current Song:" + currentSong + " Status: " + MediaPlayer.State + " IsMuted: " + MediaPlayer.IsMuted);
            //Console.WriteLine("istransitioning: " + isTransitioning + " isMapSwitch: " + isSwitchingMap);
            CheckFadeOutSongs();
            DisposeSounds();
            if (isTransitioning && !isSwitchingMap)
            {
                if ((fakeVolume + CREMENT_VALUE) < 1)
                {
                    MediaPlayer.Volume += CREMENT_VALUE;
                    fakeVolume += CREMENT_VALUE;
                }
                else
                {
                    MediaPlayer.Volume = 1;
                    fakeVolume = 1;
                    isTransitioning = false;
                }
            }
            else if (isSwitchingMap)
            {
                if ((fakeVolume - TRANSITION_CREMENT_VALUE) > 0)
                {
                    MediaPlayer.Volume -= TRANSITION_CREMENT_VALUE;
                    fakeVolume -= TRANSITION_CREMENT_VALUE;
                }
                else
                {
                    isSwitchingMap = false;
                    Play(nextMapSong);
                }
            }
            else if (isFadeOut)
            {
                if ((fakeVolume - CREMENT_VALUE) > 0)
                {
                    MediaPlayer.Volume -= CREMENT_VALUE;
                    fakeVolume -= CREMENT_VALUE;
                }
                else
                {
                    MediaPlayer.Volume = 0;
                    MediaPlayer.Stop();
                    isFadeOut = false;
                }
            }
            else if (isPausing)
            {
                if ((fakeVolume - CREMENT_VALUE) > 0)
                {
                    MediaPlayer.Volume -= CREMENT_VALUE;
                    fakeVolume -= CREMENT_VALUE;
                }
                else
                {
                    MediaPlayer.Volume = 0;
                    MediaPlayer.Pause();
                    isPausing = false;
                }
            }
        }
    }
}

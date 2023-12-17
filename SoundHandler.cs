using System;
using NAudio.Wave;

namespace Galaxy_Life_Tool {
    public class SoundHandler {
        public static void Play(Sounds name)
        {
            var path = FormHandler.GetCorrectPath("./Sounds/" + Enum.GetName(typeof(Sounds), name) + ".mp3");
            var audioFileReader = new AudioFileReader(path);
            //wait till sound is actually available
            try{
                // Create an instance of WaveOutEvent to play the audio
                var waveOutEvent = new WaveOutEvent();

                // Connect the AudioFileReader to the WaveOutEvent
                waveOutEvent.Init(audioFileReader);

                // Start playing the audio
                waveOutEvent.Play();
            }
            catch (Exception ex){
                return;
            }
        }

        public enum Sounds {
            Bomb_explosion, Bomb_explosion_2, Bomb_explosion_3,
            Bomb_explosion_4, Bomb_explosion_5, Bones,
            Build, Click, ClickBass,
            Collecting_coins, Collecting_minerals, Collecting_minerals_2,
            Firework, Freeze_turret_shot, Freeze_turret_shot_2,
            Hurray, LevelUp, Nuke_beep,
            Nuke_explosion, Nuke_falling, Nuke_mashroom,
            Photon_1, Photon_2, Repair,
            Saw, ShortShot, ShortShotLow,
            Shot, Shot_2, Something,
            Something2, Thicking, Voice_1,
            Voice_2, Voice_3, Voice_4,
            Voice_5, Voice_6, Voice_7,
            Voice_8, Voice_9, Voice_10,
            Voice_11, Voice_12, Voice_13,
            Vomit, Warp, Warp_dimmed
        }
    }
}

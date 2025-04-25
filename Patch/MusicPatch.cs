using System;
using HarmonyLib;
using UnityEngine;



namespace NOMusicReplacer.Patch
{
    [HarmonyPatch(typeof(MusicManager))]
    internal class MusicPatch
    {
        [HarmonyPatch("PlayMusic")]
        [HarmonyPrefix]
        static void SwapTheme(ref AudioClip audioClip, ref bool repeat)
        {
            repeat = MusicReplacerBase.LoopSetting;
            string song_title = audioClip.ToString();

            AudioClip new_clip = GetNewSong(song_title);

            
            if (new_clip != null)
            {
                audioClip = new_clip;
                MusicReplacerBase.mls.LogInfo("Replaced: " + song_title + " with: " + new_clip.ToString());
            }
            else
            {
                MusicReplacerBase.mls.LogError(song_title + " resulted in a failed pull from an asset bundle");
            }
        }

        [HarmonyPatch("CrossFadeMusic")]
        [HarmonyPrefix]
        static void SwapCrossTheme(ref AudioClip audioClip, ref bool repeat)
        {
            repeat = MusicReplacerBase.LoopSetting;

            string song_title = audioClip.ToString();

            AudioClip new_clip = GetNewSong(song_title);


            if (new_clip != null)
            {
                audioClip = new_clip;
                MusicReplacerBase.mls.LogInfo("Replaced: " + song_title + " with: " + new_clip.ToString());
            }

        }

        static AudioClip GetNewSong(string song_title) 
        {
            if (!MusicReplacerBase.ConversionDict.ContainsKey(song_title)){
                MusicReplacerBase.mls.LogError(song_title + " resulted in a failed pull.");
                return null;
            }
            string target_key = MusicReplacerBase.ConversionDict[song_title];
            if (MusicReplacerBase.BundleDict[target_key] == false)
            {
                MusicReplacerBase.mls.LogInfo("Asset Bundle not found, playing " + song_title);
                return null;
            }

            return MusicReplacerBase.GetReplacement(target_key);
        }
    }
}

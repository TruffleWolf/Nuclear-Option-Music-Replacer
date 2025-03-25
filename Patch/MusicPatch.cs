using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Windows;


namespace NOMusicReplacer.Patch
{
    [HarmonyPatch(typeof(MusicManager))]
    internal class MusicPatch
    {
        [HarmonyPatch("PlayMusic")]
        [HarmonyPrefix]
        static void SwapTheme(ref AudioClip audioClip)
        {
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
        static void SwapCrossTheme(ref AudioClip audioClip)
        {

            string song_title = audioClip.ToString();

            AudioClip new_clip = GetNewSong(song_title);


            if (new_clip != null)
            {
                audioClip = new_clip;
                MusicReplacerBase.mls.LogInfo("Replaced: " + song_title + " with: " + new_clip.ToString());
            }
            else
            {
                MusicReplacerBase.mls.LogError(song_title + " resulted in a failed pull. Likely either the song is not recognized by the mod or the asset bundle is broken");
            }

        }

        static AudioClip GetNewSong(string song_title) 
        {
            if (!MusicReplacerBase.ConversionDict.ContainsKey(song_title)){
                return null;
            }
            string target_key = MusicReplacerBase.ConversionDict[song_title];
            if (MusicReplacerBase.BundleDict[target_key] == null)
            {
                MusicReplacerBase.mls.LogInfo("Asset Bundle not found, playing " + song_title);
                return null;
            }

            return MusicReplacerBase.GetReplacement(target_key);
        }
    }
}

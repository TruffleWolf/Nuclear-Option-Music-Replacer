using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using NOMusicReplacer.Patch;
using UnityEngine;

namespace NOMusicReplacer
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class MusicReplacerBase : BaseUnityPlugin
    {
        private const string modGUID = "Truffle.NOMusicReplacer";
        private const string modName = "Nuclear Option Music Replacer";
        private const string modVersion = "0.29.5.1";

        private readonly Harmony harmony = new Harmony(modGUID);

        internal static Dictionary<string,AssetBundle> BundleDict = new Dictionary<string,AssetBundle>();
        internal static Dictionary<string,List<AudioClip>> AudioDict = new Dictionary<string,List<AudioClip>>();
        internal static List<string> PackNames = new List<string>() { 
            "title","pala","bdf","cricket","compass","chicane","revoker","tarantula",
            "ifrit","medusa","darkreach","win","loss","kill","tactical","strategic"
        };
        
        internal static Dictionary<string,string> ConversionDict = new Dictionary<string,string>();


        internal static MusicReplacerBase Instance;
        internal static ManualLogSource mls;
        internal static System.Random rng = new System.Random();



        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            

            harmony.PatchAll(typeof(MusicReplacerBase));
            harmony.PatchAll(typeof(MusicPatch));
            mls.LogInfo("Music Replacer Started");
            string FolderPath = Instance.Info.Location;
            
            FolderPath = FolderPath.TrimEnd("NOMusicReplacer.dll".ToCharArray());
            
            //The songs the replacer is looking for and the pack to replace with
            ConversionDict.Add("Ignition (UnityEngine.AudioClip)", "title");
            ConversionDict.Add("9. PALA (UnityEngine.AudioClip)", "pala");
            ConversionDict.Add("2. BDF (UnityEngine.AudioClip)", "bdf");
            ConversionDict.Add("CI-22 Cricket (UnityEngine.AudioClip)", "cricket");
            ConversionDict.Add("Compass_song1 (UnityEngine.AudioClip)", "compass");
            ConversionDict.Add("SAH46_Chicane (UnityEngine.AudioClip)", "chicane");
            ConversionDict.Add("Furball (UnityEngine.AudioClip)", "revoker");
            ConversionDict.Add("18. VL-49 Tarantula (UnityEngine.AudioClip)", "tarantula");
            ConversionDict.Add("KR-67_Ifrit_2 (UnityEngine.AudioClip)", "ifrit");
            ConversionDict.Add("EW-25_Medusa (UnityEngine.AudioClip)", "medusa");
            ConversionDict.Add("SFB81_Darkreach (UnityEngine.AudioClip)", "darkreach");
            ConversionDict.Add("Stratosphere_-_fadeout_ending (UnityEngine.AudioClip)", "win");
            ConversionDict.Add("Mission_Failed_extended (1) (UnityEngine.AudioClip)", "loss");
            ConversionDict.Add("12. Nuclear Escalation (UnityEngine.AudioClip)", "tactical");
            ConversionDict.Add("10. Agrapol (UnityEngine.AudioClip)", "strategic");
            ConversionDict.Add("Kill_song (UnityEngine.AudioClip)", "kill");

            for (int i = 0; i < PackNames.Count; i++)
            {
                BundleDict.Add(PackNames[i],AssetBundle.LoadFromFile(FolderPath+ PackNames[i]));
                if (BundleDict[PackNames[i]] != null)
                {
                    AudioDict.Add(PackNames[i], BundleDict[PackNames[i]].LoadAllAssets<AudioClip>().ToList());
                    mls.LogInfo(PackNames[i] + " asset bundle loaded");
                }
                else
                {
                    mls.LogError("Failed to load bundle " + PackNames[i]);
                }
            }
        }

        internal static AudioClip GetReplacement(string input)
        {

            List<AudioClip> clip_list = AudioDict[input];
            int index = rng.Next(clip_list.Count);
            return clip_list[index];
        }

    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using NOMusicReplacer.Patch;
using UnityEngine;
using UnityEngine.Networking;

namespace NOMusicReplacer
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class MusicReplacerBase : BaseUnityPlugin
    {
        private const string modGUID = "Truffle.NOMusicReplacer";
        private const string modName = "Nuclear Option Music Replacer";
        private const string modVersion = "0.30.2.1";

        private readonly Harmony harmony = new Harmony(modGUID);

        internal static Dictionary<string,bool> BundleDict = new Dictionary<string,bool>();
        internal static Dictionary<string,List<AudioClip>> AudioDict = new Dictionary<string,List<AudioClip>>();
        internal static List<string> PackNames = new List<string>() { 
            "cricket","compass","chicane","revoker","tarantula","ifrit","medusa","darkreach","vortex",
            "win","loss","kill","tactical","strategic","title","pala","bdf",
        };
        internal static List<string> PlanePacks = new List<string>()
        {
            "cricket","compass","chicane","revoker","tarantula","ifrit","medusa","darkreach","vortex"
        };
        internal static List<string> EventPacks = new List<string>()
        {
            "win","loss","kill","tactical","strategic","title","pala","bdf"
        };
        
        internal static Dictionary<string,string> ConversionDict = new Dictionary<string,string>();


        internal static MusicReplacerBase Instance;
        internal static ManualLogSource mls;
        internal static System.Random rng = new System.Random();
        internal static bool LoopSetting = false;
        internal static string FolderPath;
        XmlDocument settings = new XmlDocument();


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
            FolderPath = Instance.Info.Location;
            
            FolderPath = FolderPath.TrimEnd("NOMusicReplacer.dll".ToCharArray());

            settings.Load(FolderPath + "settings.xml");
            XmlNode node = settings.DocumentElement.SelectSingleNode("/Settings");

            if (node != null) {
                string sLoop = node.SelectSingleNode("LoopMusic").InnerText;
                bool.TryParse(sLoop, out bool loop_result);
                LoopSetting = loop_result;
            }
            else
            {
                mls.LogError("settings.xml corrupted");
            }

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
            ConversionDict.Add("FS-20 Vortex (UnityEngine.AudioClip)", "vortex");
            ConversionDict.Add("Stratosphere_-_fadeout_ending (UnityEngine.AudioClip)", "win");
            ConversionDict.Add("Mission_Failed_extended (1) (UnityEngine.AudioClip)", "loss");
            ConversionDict.Add("12. Nuclear Escalation (UnityEngine.AudioClip)", "tactical");
            ConversionDict.Add("Ignus Balls (UnityEngine.AudioClip)", "tactical");
            ConversionDict.Add("3. Port Maris (UnityEngine.AudioClip)", "strategic");
            ConversionDict.Add("10. Agrapol (UnityEngine.AudioClip)", "strategic");
            ConversionDict.Add("BDF Island Theme 3 (UnityEngine.AudioClip)", "strategic");
            ConversionDict.Add("PALA Island theme 2 (UnityEngine.AudioClip)", "strategic");
            ConversionDict.Add("Kill_song (UnityEngine.AudioClip)", "kill");

            PreloadAudio(FolderPath);
            //for (int i = 0; i < PackNames.Count; i++)
            //{
            //    BundleDict.Add(PackNames[i], AssetBundle.LoadFromFile(FolderPath + PackNames[i]));
            //    if (BundleDict[PackNames[i]] != null)
            //    {
            //        AudioDict.Add(PackNames[i], BundleDict[PackNames[i]].LoadAllAssets<AudioClip>().ToList());
            //        mls.LogInfo(PackNames[i] + " asset bundle loaded");
            //    }
            //    else
            //    {
            //        mls.LogError("Failed to load bundle " + PackNames[i]);
            //    }
            //}
        }

        internal static AudioClip GetReplacement(string input)
        {

            List<AudioClip> clip_list = AudioDict[input];
            int index = rng.Next(clip_list.Count);
            return clip_list[index];
        }

        void PreloadAudio(string path)
        {
            mls.LogInfo("BASEPATH: " + path);
            string PlanePath = path + "Aircraft" +Path.DirectorySeparatorChar;
            string EventPath = path + "Events" + Path.DirectorySeparatorChar;
            
            for (int i = 0; i < PlanePacks.Count; i++)
            {
                string[] songPaths = Directory.GetFiles(PlanePath + PlanePacks[i]);
                if (songPaths.Length == 0)
                {
                    mls.LogInfo(PlanePacks[i]+" music not found.");
                    BundleDict.Add(PlanePacks[i], false);
                    continue;
                }
                List<AudioClip> musicList = new List<AudioClip>();
                foreach(string song in songPaths)
                {
                    AudioClip result = LoadSong(song);
                    if (result != null)
                    {
                        musicList.Add(result);
                    }
                }
                if (musicList.Count == 0){
                    mls.LogInfo(PlanePacks[i] + " music not found.");
                    BundleDict.Add(PlanePacks[i], false);
                    continue;
                }
                BundleDict.Add(PlanePacks[i], true);
                AudioDict.Add(PlanePacks[i], musicList);
                mls.LogInfo(PlanePacks[i] + " asset bundle loaded");
            }

            for (int i = 0; i < EventPacks.Count; i++)
            {
                string[] songPaths = Directory.GetFiles(EventPath + EventPacks[i]);
                if (songPaths.Length == 0)
                {
                    mls.LogInfo(EventPacks[i] + " music not found.");
                    BundleDict.Add(EventPacks[i], false);
                    continue;
                }
                List<AudioClip> musicList = new List<AudioClip>();
                foreach (string song in songPaths)
                {
                    AudioClip result = LoadSong(song);
                    if (result != null)
                    {
                        musicList.Add(result);
                    }
                }
                if (musicList.Count == 0)
                {
                    mls.LogInfo(EventPacks[i] + " music not found.");
                    BundleDict.Add(EventPacks[i], false);
                    continue;
                }
                BundleDict.Add(EventPacks[i], true);
                AudioDict.Add(EventPacks[i], musicList);
                mls.LogInfo(EventPacks[i] + " asset bundle loaded");
            }
            
        }

        AudioClip LoadSong(string path)
        {
            var musicType = GetAudioType(path);
            if (musicType  == AudioType.UNKNOWN)
            {
                return null;
            }
            var loader = UnityWebRequestMultimedia.GetAudioClip(path,musicType);

            loader.SendWebRequest();

            while (true)
            {
                if (loader.isDone) break;
            }

            if (loader.error != null) {
                mls.LogError(loader.error);
                return null;
            }
            var clip = DownloadHandlerAudioClip.GetContent(loader);
            if(clip && clip.loadState == AudioDataLoadState.Loaded)
            {
                clip.name = path.TrimStart(FolderPath.ToCharArray());
                return clip;
            }
            mls.LogError($"Failed to load clip:{path}");
            return null;
        }

        private static AudioType GetAudioType(string path)
        {
            var extension = Path.GetExtension(path).ToLower();

            if (extension == ".wav")
                return AudioType.WAV;
            if (extension == ".ogg")
                return AudioType.OGGVORBIS;
            if (extension == ".mp3")
                return AudioType.MPEG;

            mls.LogError($"Unsupported extension: {path}");
            return AudioType.UNKNOWN;
        }
    }
}

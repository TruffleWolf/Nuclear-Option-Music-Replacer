# Creating Music Packs

**This tutorial assumes you've already installed BepInEx and the music replacer plugin.**

First and formost you're going to need:
- **[Unity 2022.3.6f1](https://unity.com/releases/editor/archive)**
- **[This Script](https://github.com/TruffleWolf/Nuclear-Option-Music-Replacer/blob/main/Tools/CreateAssetBundle.cs)** for exporting AssetBundles
- Some form of audio software like **[Audacity](https://audacityteam.org/download/)**
- Some music

---

Create a new Unity 2022.3.6f1 project and open it.
﻿<p align="center">
    <img src="https://github.com/TruffleWolf/Nuclear-Option-Music-Replacer/blob/main/Documents/Images/blank_project.png">
</p>


Create a new folder "Editor" in the project and add **[The above script](https://github.com/TruffleWolf/Nuclear-Option-Music-Replacer/blob/main/Tools/CreateAssetBundle.cs)** to it. 
<p align="center">
    <img src="https://github.com/TruffleWolf/Nuclear-Option-Music-Replacer/blob/main/Documents/Images/edit_folder.png">
</p>


**Restart Unity.** You should now have the option "Build AssetBundles" under the Assets dropdown.
<p align="center">
    <img src="https://github.com/TruffleWolf/Nuclear-Option-Music-Replacer/blob/main/Documents/Images/build_option.png">
</p>

Drag in drop your music into the project. As a rule music should be under 10mb and in .wav format. Larger filesizes and .ogg will still work but can cause the game to stutter when the song is loaded. The 10mb will likely be higher or lower depending on your hardware (SSDs will likely be able to go much higher). In my experience .ogg causes stuttering even at very low (<2mb) filesizes.
<p align="center">
    <img src="https://github.com/TruffleWolf/Nuclear-Option-Music-Replacer/blob/main/Documents/Images/music_added.png">
</p>

Now that your music added, select each one and add it to a new AssetBundle in the bottom right. 
<p align="center">
    <img src="https://github.com/TruffleWolf/Nuclear-Option-Music-Replacer/blob/main/Documents/Images/bundle_name.png">
</p>

The name of the bundle will determine when the music is played in game. See the below chart for the correct bundle names. Names are case sensitive.

| Bundle Name | Situation |
|--------------|-------------|
| title   | Title Screen      |
| pala | PALA Team Join      |
| bdf   | BDF Team Join      |
| cricket   | Cricket Takeoff      |
| compass   | Compass Takeoff      |
| chicane   | Chicane Takeoff      |
| revoker   | Revoker Takeoff      |
| tarantula   | Tarantula Takeoff      |
| ifrit   | Ifrit Takeoff     |
| medusa   | Medusa Takeoff      |
| darkreach   | Darkreach Takeoff      |
| win   | Mission Success      |
| loss   | Mission Failure      |
| tactical   | Tactical Nukes Unlocked      |
| strategic   | Strategic Nukes Unlocked      |
| kill   | High Value Kill      |

If multiple songs are added to the same bundle, the mod will pick one at random each time it's played. If no bundle is created, the mod will play the game's default music.

Once you've finished adding music to whichever bundle you like, use the "Build AssetBundles" option under the Assets dropdown that was created earlier in this tutorial. This will create a new folder in the project called "StreamingAssets". Navigate to that folder in your filesystem.
<p align="center">
    <img src="https://github.com/TruffleWolf/Nuclear-Option-Music-Replacer/blob/main/Documents/Images/bundle_folder.png">
</p>

Copy the relevant bundles (the files without a filetype) to the mod folder in your game's BepInEx install.
<p align="center">
    <img src="https://github.com/TruffleWolf/Nuclear-Option-Music-Replacer/blob/main/Documents/Images/mod_folder.png">
</p>

And you're done. Start up the game and enjoy the new music.





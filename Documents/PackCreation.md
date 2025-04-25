# Creating Music Packs

**This tutorial assumes you've already installed BepInEx and the music replacer plugin.**

---
Once the mod is installed you should have a folder that looks like this. 
<p align="center">
    <img src="https://github.com/TruffleWolf/Nuclear-Option-Music-Replacer/blob/main/Documents/Images/music1.png">
</p>

The Aircraft folder is for each aircraft's takeoff music. The event folder is for miscellaneous music like the title screen or when various nukes become unlocked.

<p align="center">
    <img src="https://github.com/TruffleWolf/Nuclear-Option-Music-Replacer/blob/main/Documents/Images/music2.png">
</p>

The names of each folder determine when the music is played in game. See the below charts for the correct folder. Folder names are case sensitive. Music placed outside the below listed folders will not be read by the mod.

**Aircraft**
| Folder Name | Situation |
|--------------|-------------|
| cricket   | Cricket Takeoff      |
| compass   | Compass Takeoff      |
| chicane   | Chicane Takeoff      |
| revoker   | Revoker Takeoff      |
| vortex | Vortex Takeoff    |
| tarantula   | Tarantula Takeoff      |
| ifrit   | Ifrit Takeoff     |
| medusa   | Medusa Takeoff      |
| darkreach   | Darkreach Takeoff      |

**Events**
| Folder Name | Situation |
|--------------|-------------|
| title   | Title Screen      |
| pala | PALA Team Join      |
| bdf   | BDF Team Join      |
| win   | Mission Success      |
| loss   | Mission Failure      |
| tactical   | Tactical Nukes Unlocked      |
| strategic   | Strategic Nukes Unlocked      |
| kill   | High Value Kill      |

If multiple songs are added to the same folder, the mod will pick one at random each time it's played. If no music is found, the mod will play the game's default music.
Currently supported filetypes are .wav, .ogg, and .mp3.







# Husky
Husky is a BSP Extractor for Call of Duty. It can rip the raw vertex/face data to a file that can be opened in software such as Autodesk Maya (currently only OBJ). It also includes UVs and material names, and in the future, will export other data such as static models, dynamic models, etc.

### Supported Games

* Call of Duty: World at War
* Call of Duty: Modern Warfare 3
* Call of Duty: Modern Warfare 2
* Call of Duty: Advanced Warfare
* Call of Duty: Ghosts
* Call of Duty: Modern Warfare Remastered

**Call of Duty: Black Ops 3 will never be supported, for obvious reasons**

### Downloading/Using Husky

To download Husky, go to the [Releases](https://github.com/Scobalula/Husky/releases) and download the latest build.

To use Husky, simply run the game, load the map you want to extract, and run Husky.

Once the map is exported, you will have 3 files for it:

* **mapname**.obj - Main 3D Obj File
* **mapname**.mtl - Material Info
* **mapname**.txt - A search string for Wraith/Greyhound (only contains color maps)

For Ghosts, AW, and MWR:

* **mapname**.map - Map file with **static** model locations and rotations

If you wish to use textures (be warned they can result in high RAM usage) then make sure to have the _images folder in the same location as the obj/mtl file and export PNGs (do not ask for other formats, it's staying as PNG, do a find/replace if you want to use other formats).

### License/Disclaimers

Husky is licensed under the GPL license and it and its source code is free to use and modify under the. Husky comes with NO warranty, any damages caused are solely the responsibility of the user. See the LICENSE file for more information.

All BSP data extracted using Husky is property of the developers, etc. Husky simply parses the data out, what you do with the data is your reponsibility.

Some of the exported models can get pretty big. While all have loaded in Maya with no issue, make sure to have available resources available to load and view them.

**Husky is currently in alpha, and with that in mind, bugs, errors, you know, the bad stuff.**

## FAQ

* Q: Husky says it cannot find my game?

* A: Check the above list for a supported game, when searching for a supported game, Husky loops over all processes and stops at the first match, if it's not finding your game and it is supported, your exe is not the same name as what Husky expects or something is blocking .NET from returning its process.

* Q: Husky says my game is not supported?

* A: I verify the addresses on legitimate up to date copies of the supported games in the English locale. If you're using a modified executable (Pirated, etc.) I will not support it.

* Q: The exported OBJ is corrupt when imported?

* A: Tons of BSPs across all supported games have been verified, if you have find a legitimate instance of a corrupt export, please open an issue with the name of the map, etc. as much info as you can.

* Q: Why is there a bunch of geo at the origin?

* A: It appears in all games, script brushmodels are at the origin, and I assume the map_ents assets or some other data is used to tell the game where to move them to on load.

## Credits

* DTZxPorter - Normal Unpacking Code from Wraith + Other misc info from Wraith, SELib
* Anna Baker - [Icon](https://thenounproject.com/term/husky/1121992/) ([https://thenounproject.com/anna.baker194/](https://thenounproject.com/anna.baker194/))

## Support Me

If you use Husky in any of your projects, it would be appreciated if you provide credit for its use, a lot of time and work went into developing it and a simple credit isn't too much to ask for.

If you'd like to support me even more, consider donating, I develop a lot of apps including Husky and majority are available free of charge with source code included:

[![Donate](https://img.shields.io/badge/Donate-PayPal-yellowgreen.svg)](https://www.paypal.me/scobalula)

# Husky
Husky is a BSP Extractor for Call of Duty. It can rip the raw vertex/face data to a file that can be opened in software such as Autodesk Maya (currently only OBJ). It also includes UVs and material names, and in the future, will export other data such as static models, dynamic models, etc.

### Supported Games

* Call of Duty: Modern Warfare
* Call of Duty: Modern Warfare 2
* Call of Duty: Modern Warfare 3
* Call of Duty: Ghosts
* Call of Duty: Infinite Warfare
* Call of Duty: World at War
* Call of Duty: Black Ops
* Call of Duty: Black Ops 2
* Call of Duty: Black Ops 3
* Call of Duty: Advanced Warfare
* Call of DUty: World War 2
* Call of Duty: Modern Warfare Remastered

### Downloading/Using Husky

To download Husky, go to the [Releases](https://github.com/Scobalula/Husky/releases) and download the latest build.

To use Husky, simply run the game, load the map you want to extract, and run Husky, then click the paper airplane to export the loaded map. In some cases you may need to run Husky as an administator.

Once the map is exported, you will have 3 files for it:

* **mapname**.obj - Main 3D Obj File
* **mapname**.mtl - Material Info
* **mapname**.txt - A search string for Wraith/Greyhound (only contains color maps)
* **mapname**.map - Map file with **static** model locations and rotations

If you wish to use textures (be warned they can result in high RAM usage) then make sure to have the _images folder (use Wraith/Greyhound to export the required images) in the same location as the obj/mtl file and export PNGs (do not ask for other formats, it's staying as PNG, do a find/replace if you want to use other formats).

### License/Disclaimers

Husky is licensed under the GPL license and it and its source code is free to use and modify under the terms of the GPL. Husky comes with NO warranty, any damages caused are solely the responsibility of the user. See the LICENSE file for more information.

**All** BSP data extracted using Husky is property of the developers, etc. and with this in mind you need to understand the limitation of what you can do with the data. Husky simply parses it out, what you do with it, is your responsibility.

Some of the exported models can get pretty big. While all have loaded in Maya with no issue, make sure to have available resources available to load and view them.

Husky is provided ***AS IS***, do not ask how to do XYZ, how to import it into X program, etc. You need to know what you're doing, we can't hold your hand through it.

Also I've noticed several people asking how to import it into Unity/Unreal, there is a fine line between using the Geo to remake a map in another Call of Duty Game using its own SDK **vs** using the Geo as it is in your game/IP, for example, so ensure you know what you're doing, copyright wise.

## FAQ

* Q: Husky says it cannot find my game?

* A: Check the above list for a supported game, when searching for a supported game, Husky loops over all processes and stops at the first match, if it's not finding your game and it is supported, your exe is not the same name as what Husky expects or something is blocking .NET from returning its process.

* Q: Husky says my game is not supported?

* A: I verify the addresses on legitimate up to date copies of the supported games in the English locale. If you're using a modified executable (Pirated, etc.) I will not support it.

* Q: The exported OBJ is corrupt when imported?

* A: Tons of BSPs across all supported games have been verified, if you have find a legitimate instance of a corrupt export, please open an issue with the name of the map, etc. as much info as you can.

* Q: Why is there a bunch of geo at the origin?

* A: It appears in all games, script brushmodels are at the origin, and I assume the map_ents assets or some other data is used to tell the game where to move them to on load. This will remain as is.

## Credits

* DTZxPorter - Normal Unpacking Code from Wraith, Half Floats code, Other misc info from Wraith.
* Anna Baker - [Icon](https://thenounproject.com/term/husky/1121992/) ([https://thenounproject.com/anna.baker194/](https://thenounproject.com/anna.baker194/))

## Support Me

If you use Husky in any of your projects, it would be appreciated if you provide credit for its use, a lot of time and work went into developing it and a simple credit isn't too much to ask for.

If you'd like to support me even more, consider donating, I develop a lot of apps including Husky and majority are available free of charge with source code included:

[![Donate](https://img.shields.io/badge/Donate-PayPal-yellowgreen.svg)](https://www.paypal.me/scobalula)

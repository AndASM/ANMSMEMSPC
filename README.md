# ANMSMEMSPC: Andy's NMS Mod Enabler for the Microsoft Store / Xbox GamePass PC edition
This is a utility that will mark DISABLEMODS.TXT as deleted for the Windows Store version of No Man's Sky.
Finally gamepass users can have nearly a full experience without being kneecapped by the terrible Windows Store platform.

# Instructions
Run this app. It should only need to be used once. The file should remain deleted unless you reinstall the game.

To find the MODS folder, navigate to `%localappdata%\Packages` in the URI bar of the Windows file Explorer. Then navigate into
`HelloGames.NoMansSky_bs190hzg1sesy\LocalCache\Local\Microsoft\WritablePackageRoot\GAMEDATA\PCBANKS\MODS`. The random-looking
characters at the end of `HelloGames.NoMansSky_bs190hzg1sesy` may be different.

Mods can be installed by coping their .pak files into that MODS folder the same as they are on other editions of the game,
such as the Steam version. The only roadblock was the `DISABLEMODS.TXT` file, which this utility is designed to remove.
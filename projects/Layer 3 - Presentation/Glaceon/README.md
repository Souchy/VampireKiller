# Glaceon project

Serves as the client UI.
Only project using Godot?
May also serve on server-side as headless?

## Lapis

Is the main client part.
Holds the main menu, lobbies, settings, login...

## Sapphire

Is the game client part.
Holds the game, entities, game ui...

## Setup

1. Make sure to have the following submodules:
   1. projects\Layer 2 - Application\Godot
   2. projects\Layer 1 - Data\VampireAssets

2. Build Godot fork
   1. > cd "projects\Layer 2 - Application\Godot"
   2. Edit ./build_net.bat location for nuget source
   3. > ./build_net.bat
   4. Now use this build as your godot executable: "projects\Layer 2 - Application\Godot\bin\godot.windows.editor.dev.x86_64.mono.exe"
   5. It is important to import characters with the right bonemap. Otherwise Godot will try to reimport scenes sometimes and it will break characters bones.

3. Create the following symlinks in that order. The res:// paths do need to be exactly the same and can be case-sensitive, just use your own absolute target path. Godot supports symlinks outside of res:// only with absolute path, not relative path.
   1. Unidot importer addon:
      1. > cd Glaceon/addons/
      2. > mklink /D unidot_importer "C:\VampireKiller\projects\Layer 1 - Data\VampireAssets\addons\unidot_importer"
   2. Custom character import plugin addon:
      1. > cd Glaceon/addons/
      2. > mklink /D myplugin "C:\VampireKiller\projects\Layer 1 - Data\VampireAssets\addons\myplugin"
   3. Assets sources:
      1. > cd Glaceon/
      2. > mklink /D Assets "C:\VampireKiller\projects\Layer 1 - Data\VampireAssets\Assets"
   4. Assets custom:
      1. > cd Glaceon/
      2. > mklink /D vampireassets "C:\VampireKiller\projects\Layer 1 - Data\VampireAssets\vampireassets"

4. Symlink explanation
   1. Now all those symlinks point at the VampireAssets submodule.
   2. This means when you add an asset or scene, it goes into that submodule.
   3. Meaning, you commit to the separate VampireAssets repo and it doesn't clutter the main VampireKiller repository.
   4. It's like a shortcut, except it's seamless. So paths like "Glaceon/Assets/mything" will work even though it actually points to "VampireAssets/Assets/mything".
   5. After you commit to the submodule repo, you need to commit the submodule change in VampireKiller, so it knows what version to use.

5. Folders
   1. Assets is like our raw/source assets (raws from mixamo/synty/unity).
   2. vampireassets (lowercase) is like our "utilized" or "modified" assets AND scenes (old db).
      1. Contains creatures, maps, spells...
      2. Most decors will probably come directly from Assets
   3. This stuff is still debatable.
      1. Like the characters .glbs in vampireassets might go to Assets.
      2. But the corresponding folders in Assets are generated by Unidot so it's touchy to modify them.
   4. Assets and vampireassets should not contain .cs scripts. That will crash the VampireAssets project.
      1. Skills & status scripts are in TestGems.
      2. Maps should probably move there too or be gdscript.

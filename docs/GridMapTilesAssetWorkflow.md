# GridMap Tile Asset Workflow

## 0.0. Prereqs
- Godot
- If extracting from Synty `.unitypackage`
    - Unidot Importer addon for godot (second option of this article: https://gamefromscratch.com/move-from-unity-to-godot-engine-in-seconds/)

## 1.0. Creating a MeshLibrary
1. Acquire the base ressources (or scenes) for the wanted tiles
    - We will need the `.mesh` files & any relevant textures (`.mat`, `.png`, `.tres`, `.res`)
    - Can also be scenes (`.scn`, `.tscn`)
2. Create a scene that will contain the `MeshLibrary`
3. Add all the wanted tiles (as `MeshInstance3D` or plain `Node3D` if dealing with scenes)
4. (Optional) If using `MeshInstance3D` from base ressources, collision should be added:
    - Automatic:
        - Select all the `MeshInstance3D` to which we want to add collision
        - In the middle part of the editor, above the scene preview, click `Mesh > Create Trimesh Body`
    - Manual:
        - Create `StaticBody3D` under the `MeshInstance3D` we are adding collision to
        - Add a `CollisionShape3D` as a child to the `StaticBody3D` with the required collision
5. Save the scene and export it as a `MeshLibrary` (`Scene > Export As >  MeshLibrary`)

## 2.0. Creating a GridMap with a MeshLibrary
1. Create a new `Node3D` scene which will contain the `GridMaps`
2. (Optional) If navigation is required, and the tiles from the `MeshLibrary` have collisions
    - Create a `NavigationRegion3D` to hold the `GridMaps`
3. Create 3 `GridMaps` under the `Node3D` (or `NavigationRegion3D`)
    - Floors
    - Walls
    - Props (misc objects)
4. Load the `MeshLibrary` created from **1.0.**
5. Place tiles in appropriate `GridMaps`
6. Profit?
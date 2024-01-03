# Creature Asset Workflow

## 0.0. Prereqs
- Godot
- Blender
- Adobe account
- If extracting from Synty `.unitypackage`
	- Unidot Importer addon for godot (second option of this article: https://gamefromscratch.com/move-from-unity-to-godot-engine-in-seconds/)

## 1.0. Character animations workflow
### 1.1. Acquiring the initial skeleton & animations .blend file (only needs to be done once per skeleton)
1. Acquire a character scene (`.fbx`) 
	- If using `.unitypackage`:
		- extract into a blender project
		- export from blender project into `.fbx`
2. Upload to mixamo
3. Go through autorigging (or keep the initial bones)
4. Get an initial set of animations
	- Export as `FBX Binary(.fbx)`
	- `Without Skin`
	- `No keyframe reduction`
	- Make sure to add to the `docs/MixamoAnimations.md` file with the new animations
5. Import them all into blender
6. Remove all except one instance of armature/skeleton
7. Save blender file

#### 1.1.1. Generating reusable animation library (.res)
1. Export blender file from `1.1.`: `Export > gtLF 2.0 (.glb/.glbf)`
   - Make sure animations included in export
2. Add to the godot assets
3. In godot, select newly added `.glb` scene, and click the `Import` tab over the scene tree
4. Change `Import As:` from `Scene` to `Animation Library`
5. Click `Reimport` at the bottom
6. Open an `AnimationPlayer`
7. In the animation tab of the `AnimationPlayer`, click `Animation > Manage Animations...`
8. Click `Load Library`, then select the `Animation Library` scene that was imported and configured at steps 3 and 4
9. Once loaded, click the save icon next to the library and then `Make Unique`
10. Save the library `.res` file
11. The `.glb` file from step 1 to 5 may now be deleted 

### 1.2. Adding characters making use of an existing animation library
If all the characters in a pack share the same skeleton (ie: PolygonFantasyCharacters or PolygonFantasyRivals), they may all be exported in a single `.gltf` file. The step **1.1.** may then be done once with that skeleton. Afterwards a scene may simply instantiate the `.gltf` scene as a child and an `AnimationPlayer` (with an animation library from **1.1.1.**) as another child
for the animations to work accross the characters. If for some reason the characters of a pack where partially exported, more can be added later to the `.gltf` scene, and then reimported into Godot for a relatively seemless update.

Sharing animations between packs can be a challenge since Synty is not constant with the naming of the bones in their skeletons. Typically redoing **1.1.** should be done for every different skeleton. But there is a method that can be applied to adapt a character model to another skeleton. It can be a bit buggy, but if the models are similar enough to each other it should work fine:
1. Acquire a character scene (`.fbx`) 
	- If using `.unitypackage`:
		- extract into a blender project
		- export from blender project into `.fbx`
2. Import the character model into the blender file from **1.1.** (the one with the skeleton we want to adapt to)
3. Select character mesh, `SHIFT+CLICK` the armature/skeleton (not in the object tree, `SHIFT+CLICK` an actual bone on the screen)
4. Hit `CTRL+P`, select `With Automatic Weights`
	- If an error/warning occurs above and it doesn't work:
		- Change to `Edit Mode`
		- Select the character mesh from the object tree
		- Find the `Mesh` dropdown menu (over the perspective)
		- `Mesh > Clean Up > Merge by Distance`
		- Retry steps 3-4
5. `Export > gtLF 2.0 (.glb/.glbf)`
	- Do not include animations in the export
	- If material shared between assets: 
		- `Material > Materials > No export`
		- Create a `StandardMaterial3D` in Godot, adding the texture image to `Albedo > Texture`
6. Import into godot, character scene ready to be used and animated by an animation player with the library

### 1.3. Adding animations to an existing animation library
1. Open the blender file from **1.1.** for the library we want to add to
2. Export the armature / skeleton from the blender file as `.fbx` 
3. Upload to mixamo
4. Get additional animations
	 - Export as `FBX Binary(.fbx)`
	 - `Without Skin`
	 - `No keyframe reduction`
	 - Make sure to update the `docs/MixamoAnimations.md` file with the new animations
5. Import them all into the blender file from **1.1.**
6. Remove all except one instance of armature/skeleton
7. Save blender file
8. Regenerate animation library (**1.1.1.**)

### 1.5. Adding weapons to the characters
TODO
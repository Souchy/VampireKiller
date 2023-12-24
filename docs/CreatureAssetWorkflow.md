## 1.0. Character animations workflow
### 1.1. Acquiring the initial armature/skeleton & animations .blend file (only needs to be done once)
1. Acquire a character scene (`.fbx`) 
	- If using `.unitypackage`:
		- extract into a blender project
		- export from blender project into `.fbx`
2. Upload @ mixamo
3. Go through autorigging
4. Get an initial set of animations
	- Export as `FBX Binary(.fbx)`
	- `Without Skin`
	- `No keyframe reduction`
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
11. The `.glb` file from step 1 to 5 may be deleted 

### 1.2. Adding characters
1. Acquire a character scene (`.fbx`) 
	- If using `.unitypackage`:
		- extract into a blender project
		- export from blender project into `.fbx`
2. Import the character model into the blender file from **1.1.**
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


#### 1.2.1. Adding characters with non humanoid (or large) bodies
1. Follow **1.1.** from steps 1 to 3
2. Once model on mixamo, download T-Pose animation (selected when no animations have been chosen)
	- Export as `FBX Binary(.fbx)`
	- One `With Skin`, rest `Without Skin`
	- `No keyframe reduction`
3. Import into blender
4. Remove all except one instance of armature/skeleton and mesh
5. (Optional) Save blender file
6. Import into godot
   - depending on how different character scene is from character used in **1.1.1.** for the animation library, animations might need to be redone/redownloaded from mixamo

### 1.3. Adding animations to the library
1. Acquire a humanoid character scene (`.fbx`) on mixamo
	- If **1.1.** was done and the old character hasn't been replaced:
		- Simply login to your account and reuse the old character
	- If using `.unitypackage`:
		- extract into a blender project
		- export from blender project into `.fbx`
		- Upload @ mixamo and go thourhg autorigin
2. Get additional animations
	 - Export as `FBX Binary(.fbx)`
	 - `Without Skin`
	 - `No keyframe reduction`
5. Import them all into the blender file from **1.1.**
6. Remove all except one instance of armature/skeleton
7. Save blender file
8. Regenerate animation library (**1.1.1.**)

### 1.5. Adding weapons to the characters
TODO
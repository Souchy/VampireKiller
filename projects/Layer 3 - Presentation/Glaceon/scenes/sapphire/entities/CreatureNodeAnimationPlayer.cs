using Godot;
using GodotSharpKit.Generator;
using System;
using System.Collections.Generic;
using vampirekiller.glaceon.util;
using vampirekiller.eevee.util;
using System.Linq;
using static Godot.Animation;
using vampirekiller.logia;
using Godot.Collections;


//public partial class MyMaterial : ParticleProcessMaterial
//{
//    public void adasd()
//    {

//    }
//}


public enum AnimationState
{
    idle,
    idle_gesture,
    moving,
    casting,
    receiveHit,
    death
}

public partial class CreatureNodeAnimationPlayer : AnimationPlayer
{
    private Action currentCallback;
    private string previousAnimation;
    private AnimationState state = AnimationState.idle;
    private double gestureTimer = 5;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        foreach(var lib in this.GetAnimationLibraryList().Select(n => this.GetAnimationLibrary(n)))
            initLibrary(lib);
    }

    /// <summary>
    /// TODO: fix Random idle "looking" animations. AnimationPlayer.process doesn't process.
    /// </summary>
    public override void _Process(double delta)
    {
        if(state == AnimationState.idle)
        {
            gestureTimer -= delta;
            if(gestureTimer < 0)
            {
                if(playAnimationOneShot(AnimationState.idle_gesture, "action_adventure/idle"))
                    gestureTimer = 5;
            }
        }
    }

    public void loadLibrary(string libraryPath)
    {
        var name = libraryPath.trimPathExt();
        if(this.HasAnimationLibrary(name))
            return;
        if(!libraryPath.Contains("res://"))
            libraryPath = Paths.animations + libraryPath;
        var lib = AssetCache.Load<AnimationLibrary>(libraryPath, ".glb");
        this.AddAnimationLibrary(name, lib);
        initLibrary(lib);
    }

    private void initLibrary(AnimationLibrary lib)
    {
        foreach(var anim in lib.GetAnimationList().Select(n => lib.GetAnimation(n)))
        {
            // Disable root translation
            int root = anim.FindTrack("%GeneralSkeleton:Root", TrackType.Position3D);
            if (root != -1)
                anim.TrackSetEnabled(root, false);
            // Add method callback
            if(!hasMethodTrack(anim) && anim.LoopMode == LoopModeEnum.None)
            {
                var dic = new Godot.Collections.Dictionary<string, Variant>
                {
                    //{ "name", nameof(executeWindupCallback) },
                    { "method", nameof(executeWindupCallback) },
                    { "args", new Godot.Collections.Array() }
                };
                var newTrack = anim.AddTrack(TrackType.Method);
                anim.TrackSetPath(newTrack, "AnimationPlayer");
                var a = anim.TrackInsertKey(newTrack, anim.Length - 0.01f, dic);
                GD.Print("new track: " + newTrack + ", " + a);
            }
        }
    }

    public bool playAnimationLoop(AnimationState state, string animation, double increasedSpeedPercent = 100)
    {
        if(!canPlayAnimation(state, animation))
            return false;
        this.state = state;
        this.currentCallback = null;
        // TODO: transitions? ex: if(this.state = running && newState == idle) -> play("run_to_stop") + on finish play("idle") ou animationTree...
        this.Play(animation, customSpeed: (float) increasedSpeedPercent / 100f);
        return true;
    }

    public bool playAnimationOneShot(AnimationState state, string animation, Action callback = null, double animationTime = 0)
    {
        if (!canPlayAnimation(state, animation))
            return false;
        this.state = state;
        this.currentCallback = callback;
        if (animationTime != 0)
        {
            //this.GetAnimation(animation).Length = (float) animationTime;
            var customSpeed = this.GetAnimation(animation).Length / (float) animationTime;
            this.Play(animation, customSpeed: customSpeed);
        } else
        {
            this.Play(animation);
        }
        var asd = this.CurrentAnimation;
        return true;
    }

    private bool canPlayAnimation(AnimationState newState, string animation) //SupportedAnimation animation)
    {
        if(newState < this.state && this.state > AnimationState.moving)
            return false;

        var libs = this.GetAnimationLibraryList();

        // Make sure animation exists
        if (!this.HasAnimation(animation)) //this.animationToAnimationName.ContainsKey(animation)) 
        {
            loadLibrary("pro_magic_pack");
            return false;
        }

        if (!this.IsPlaying())
            return true;

        // If animation playing, make sure we are in a state that allows us to override it

        // If the animation is the same as current one, do not override
        if (this.CurrentAnimation == animation)
            return false;

        // TODO: check animation cancelling timer

        //// If the animation has lower priority than the current one, and the current animation is not looping, do not override
        //// (looping animations need to be able to override eachother to avoid needing to wait until the end of the loop to change animation)
        //if (this.currentAnimation > animation && !isLoopingAnimation(this.currentAnimation))
        //    return false;

        return true;
    }

    // Method overload is to allow it to be connected to the AnimationFinished signal
    public void executeWindupCallback() //string onWindupEnd)
    {
		if (this.currentCallback != null) 
		{
            var call = currentCallback;
            // Clean avant de call pour la concurrency.
            // si le player envoie une autre commande avant que ca finisse, vu que c'est null, il peut deja l'appliquer
            this.currentCallback = null;
            this.state = AnimationState.idle;
	        call();
		}
    }

    private static bool hasMethodTrack(Animation animation)
    {
        for (var i = 0; i < animation.GetTrackCount(); i++)
        {
            if (animation.TrackGetType(i) == TrackType.Method)
            {
                return true;
            }
        }
        return false;
    }

}

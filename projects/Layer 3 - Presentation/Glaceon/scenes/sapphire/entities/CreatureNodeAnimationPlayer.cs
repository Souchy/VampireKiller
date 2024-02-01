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
using vampirekiller.eevee.creature;

//public enum AnimationState
//{
//    idle,
//    idle_gesture,
//    moving,

//    casting,
//    receiveHit,
//    death
//}
public record AnimationState(int idx, bool loop, params int[] restrictedStatesSource)
{
    public static readonly AnimationState idle = new(0, true);
    public static readonly AnimationState idle_gesture = new(1, false, 0);
    public static readonly AnimationState moving = new(2, true);

    public static readonly AnimationState casting = new(3, false);
    public static readonly AnimationState receiveHit = new(4, false);
    public static readonly AnimationState death = new(5, false);
    public static readonly AnimationState dance = new(6, false);
}

public partial class CreatureNodeAnimationPlayer : AnimationPlayer
{
    public CreatureSkin skin { get; set; }
    private Action currentCallback;
    private string previousAnimation;
    private AnimationState state = AnimationState.idle;
    private double gestureTimer = 10;
    private double gestureDelta = 0;
    private bool isLooping = false;
    private Random random = new Random();
    // idle -> moving
    // idle -> gesture
    // moving -> idle

    // gesture -> no idle
    // gesture -> moving instant
    private AnimationNodeStateMachine m;
    private AnimationNodeBlendSpace2D b;
    private AnimationNodeAnimation walking;
    private AnimationNodeAnimation run;
    private AnimationNodeAnimation idle;
    private List<AnimationNodeAnimation> gestures;
    private double animationTimer = 0;
    private double animationLength = 0;

    public override void _EnterTree()
    {
        base._EnterTree();
    }

    public void loadSkin()
    {
        m = new AnimationNodeStateMachine();
        m.AddNode("idle", new AnimationNodeAnimation()
        {
            Animation = skin.animations.idle
        });
        m.AddTransition("", "", new AnimationNodeStateMachineTransition()
        {
            AdvanceCondition = "idle"
        });
        int i = 0;
        foreach (var gesture in skin.animations.idleOneShots)
        {
            m.AddNode("idle_gesture" + (i++), new AnimationNodeAnimation()
            {
                Animation = gesture
            });
        }


        b = new AnimationNodeBlendSpace2D();
        walking = new AnimationNodeAnimation()
        {
            Animation = skin.animations.walk
        };
        run = new AnimationNodeAnimation()
        {
            Animation = skin.animations.run
        };
        b.AddBlendPoint(walking, new Vector2(0, 1));
        b.AddBlendPoint(run, new Vector2(0, 1));

    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        // Clear preset anim libraries
        foreach (var lib in this.GetAnimationLibraryList().ToList())
            RemoveAnimationLibrary(lib);
    }

    /// <summary>
    /// TODO: add Random idle "looking" animations. AnimationPlayer.process doesn't process.
    /// </summary>
    public override void _Process(double delta)
    {
        animationTimer += delta;
        if (state == AnimationState.idle)
        {
            gestureDelta += delta;
            if (gestureDelta >= gestureTimer)
            {
                var i = random.Next(skin.animations.idleOneShots.Length);
                var gesture = skin.animations.idleOneShots[i];
                if (playAnimationOneShot(AnimationState.idle_gesture, gesture))
                    gestureDelta = 0;
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
        if(lib == null)
            throw new Exception("Null animation library");
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
                    { "method", nameof(executeWindupCallback) },
                    { "args", new Godot.Collections.Array() }
                };
                var newTrack = anim.AddTrack(TrackType.Method);
                anim.TrackSetPath(newTrack, "AnimationPlayer");
                var a = anim.TrackInsertKey(newTrack, anim.Length - 0.01f, dic);
            }
        }
    }

    public bool playAnimationLoop(AnimationState state, string animation, double increasedSpeedPercent = 0)
    {
        if(!canPlayAnimation(state, animation, true))
            return false;
        this.state = state;
        this.isLooping = true;
        this.currentCallback = null;
        this.animationTimer = 0;
        this.animationLength = this.GetAnimation(animation).Length;
        // TODO: transitions? ex: if(this.state = running && newState == idle) -> play("run_to_stop") + on finish play("idle") ou animationTree...
        this.Play(animation, customSpeed: (float) (increasedSpeedPercent + 100f) / 100f);
        return true;
    }

    public bool playAnimationOneShot(AnimationState state, string animation, Action callback = null, double animationTime = 0)
    {
        if (!canPlayAnimation(state, animation))
            return false;
        this.state = state;
        this.isLooping = false;
        this.currentCallback = callback;
        this.animationTimer = 0;
        this.animationLength = this.GetAnimation(animation).Length;
        if (animationTime != 0)
        {
            //this.GetAnimation(animation).Length = (float) animationTime;
            var customSpeed = this.GetAnimation(animation).Length / (float) animationTime;
            this.Play(animation, customSpeed: customSpeed);
        } else
        {
            this.Play(animation);
        }
        return true;
    }

    private bool canPlayAnimation(AnimationState newState, string animation, bool isLoop = false)
    {
        if (!this.IsPlaying())
            return true;

        // Dont override self if looping
        if(state == newState && isLooping)
            return false;

        // Allow to cancel the rest of an animation after the callback is called
        // Should have a timeBegin and timeEnd so you can cancel before and cancel after but not during.
        //  ~~Maybe add an animationTrack at 20% its timelength to call "setAnimationLocked(true)" then "setAnimationLocked(false)" at the end.~~
        //  Would be a problem if the false never gets called + this prob adds overhead.
        //  
        //if (state == newState && currentCallback != null)
        //    return false;

        // can cancel first 20% and last 80%
        if(!isLooping && this.animationTimer > animationLength * 0.2d && this.animationTimer < animationLength * 0.8d)
            return false;

        if(newState.restrictedStatesSource.Length > 0 && !newState.restrictedStatesSource.Contains(state.idx))
            return false;

        //if (this.state.idx > newState.idx)
        //    return false;

        // Make sure animation exists
        if (!this.HasAnimation(animation))
            return false;

        // If the animation is the same as current one, do not override
        //if (this.CurrentAnimation == animation)
        //    return false;

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

using Godot;
using GodotSharpKit.Generator;
using System;
using System.Collections.Generic;
using vampirekiller.glaceon.util;
using vampirekiller.eevee.util;
using System.Linq;
using static Godot.Animation;


//public partial class MyMaterial : ParticleProcessMaterial
//{
//    public void adasd()
//    {
        
//    }
//}

public partial class CreatureNodeAnimationPlayer : AnimationPlayer
{
    // Defined in the order of priorities
    // if the player is attacking, keep animating attack if walk input is received
    // if the player is walking, cancel the walk animation and start attacking if attack input is received
    // public enum SupportedAnimation
    // {
    //     Idle,   // Loop animation
    //     Walk,   // Loop animation
    //     Attack, // Action animation
    //     Death,  // Action animation
    //     Unknown // Used as a fallback
    // }

    //private Dictionary<SupportedAnimation, string> animationToAnimationName;
    //private Dictionary<SupportedAnimation, bool> animationToHasCallback;
    //private SupportedAnimation currentAnimation = SupportedAnimation.Idle;
    //private Action animationCallback;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        //(this.animationToAnimationName, this.animationToHasCallback) = initAnimations(this);
        //this.playAnimation(SupportedAnimation.Idle);
        //foreach(var anim in this.GetAnimationList())
        //{
        //    // If animation does not contain a callback, setup callback when animation ends
        //    if (!this.animationToHasCallback.GetValueOrDefault(anim, false))
        //    {
        //        this.Connect(SignalName.AnimationFinished, Callable.From<string>(this.executeWindupCallback));
        //    }
        //}
        foreach(var lib in this.GetAnimationLibraryList().Select(n => this.GetAnimationLibrary(n)))
            initLibrary(lib);
    }

    public void loadLibrary(string libraryPath)
    {
        var name = libraryPath.trimPathExt();
        if(this.HasAnimationLibrary(name))
            return;
        var lib = AssetCache.Load<AnimationLibrary>(libraryPath, ".glb");
        this.AddAnimationLibrary(name, lib);
        initLibrary(lib);
    }

    private static void initLibrary(AnimationLibrary lib)
    {
        foreach(var anim in lib.GetAnimationList().Select(n => lib.GetAnimation(n)))
        {
            // Disable root translation
            int root = anim.FindTrack("Root", TrackType.Position3D);
            anim.TrackSetEnabled(root, false);
            // Add method callback
            if(!hasMethodTrack(anim))
            {
                var newTrack = anim.AddTrack(TrackType.Method);
                anim.TrackInsertKey(newTrack, anim.Length, nameof(executeWindupCallback));
            }
        }
    }

    private bool canPlayAnimation(string animation) //SupportedAnimation animation)
    {
        // Make sure animation exists
        if (!this.HasAnimation(animation)) //this.animationToAnimationName.ContainsKey(animation))
            return false;

        if(!this.IsPlaying())
            return true;

        // If animation playing, make sure we are in a state that allows us to override it

        // If the animation is the same as current one, do not override
        if (this.currentAnimation == animation)
            return false;

        // TODO: check animation cancelling timer

        //// If the animation has lower priority than the current one, and the current animation is not looping, do not override
        //// (looping animations need to be able to override eachother to avoid needing to wait until the end of the loop to change animation)
        //if (this.currentAnimation > animation && !isLoopingAnimation(this.currentAnimation))
        //    return false;

        return true;
    }

    private string currentAnimation;
    private Action currentCallback;

    public void playAnimationLoop(string animation, double increasedSpeedPercent = 100)
    {
        if(canPlayAnimation(animation))
            return;
        // TODO: transitions? ex: if(this.state = running && newState == idle) -> play("run_to_stop") + on finish play("idle") ou animationTree...
        this.Play(animation, customSpeed: (float) increasedSpeedPercent / 100f);
    }
    public void playAnimationOneShot(string animation, Action callback = null, double animationTime = 0)
    {
        if (canPlayAnimation(animation))
            return;
        if(callback != null)
        {
            currentCallback = callback;
        }
        if(animationTime != 0)
        {
            //this.GetAnimation(animation).Length = (float) animationTime;
            var customSpeed = this.GetAnimation(animation).Length / (float) animationTime;
            this.Play(animation, customSpeed: customSpeed);
        }
    }
    //public void playAnimation(SupportedAnimation animation, Action onWindupEnd = null, double animationTime = 0)
    //{
    //    if(!canPlayAnimation(animation)) 
    //        return;

    //    var animationName = animationToAnimationName.GetValue(animation);
    //    this.currentAnimation = animation;
    //    //p r o b l e m :
    //    this.animationCallback = null; // Cleanup old callback
        
    //    this.GetAnimation(animationName).Length = (float) animationTime;
    //    this.Play(animationName);
    //}

    //public void playAnimation(SupportedAnimation animation, Action onWindupEnd)
    //{
    //    if (!canPlayAnimation(animation))
    //        return;

    //    // Setup windup callback
    //    this.animationCallback = onWindupEnd;
    //    // Play
    //    this.playAnimation(animation);
    //}

    // Method overload is to allow it to be connected to the AnimationFinished signal
    public void executeWindupCallback() //string onWindupEnd)
    {
		if (this.currentCallback != null) 
		{
            var call = currentCallback;
            // Clean avant de call pour la concurrency.
            // si le player envoie une autre commande avant que ca finisse, vu que c'est nul, il peut deja l'appliquer
            this.currentCallback = null;
	        call();

	        //if (this.IsConnected(SignalName.AnimationFinished, Callable.From<string>(this.executeWindupCallback)))
	        //{
	        //    this.Disconnect(SignalName.AnimationFinished, Callable.From<string>(this.executeWindupCallback));
	        //}
		}
    }

    //private static bool isLoopingAnimation(SupportedAnimation animation)
    //{
    //    return animation <= SupportedAnimation.Walk;
    //}

    //private static (Dictionary<SupportedAnimation, string>, Dictionary<SupportedAnimation, bool>) initAnimations(AnimationPlayer player)
    //{
    //    var animationNames = new Dictionary<SupportedAnimation, string>();
    //    var hasCallbacks = new Dictionary<SupportedAnimation, bool>();
    //    foreach (var animationName in player.GetAnimationList())
    //    {
    //        SupportedAnimation matchedAnimation = matchAnimation(animationName);
    //        if (!animationNames.ContainsKey(matchedAnimation))
    //        {
    //            animationNames.Add(matchedAnimation, animationName);
    //            hasCallbacks.Add(matchedAnimation, checkForMethodTrack(player.GetAnimation(animationName)));
    //        }
    //    }
    //    return (animationNames, hasCallbacks);
    //}

    //private static SupportedAnimation matchAnimation(String animationName)
    //{
    //    var lowerCaseAnimationName = animationName.ToLower();
    //    foreach (var supportedAnimation in Enum.GetValues<SupportedAnimation>())
    //    {
    //        if (lowerCaseAnimationName.Contains(supportedAnimation.ToString().ToLower()))
    //        {
    //            return supportedAnimation;
    //        }
    //    }
    //    return SupportedAnimation.Unknown;
    //}

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

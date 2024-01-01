using Godot;
using GodotSharpKit.Generator;
using System;
using System.Collections.Generic;

public partial class CreatureNodeAnimationPlayer : AnimationPlayer
{
    // Defined in the order of priorities
    // if the player is attacking, keep animating attack if walk input is received
    // if the player is walking, cancel the walk animation and start attacking if attack input is received
    public enum SupportedAnimation
    {
        Idle,   // Loop animation
        Walk,   // Loop animation
        Attack, // Action animation
        Death,  // Action animation
        Unknown // Used as a fallback
    }

    private Dictionary<SupportedAnimation, string> animationToAnimationName;
    private Dictionary<SupportedAnimation, bool> animationToHasCallback;
    private SupportedAnimation currentAnimation = SupportedAnimation.Idle;
    private Action animationCallback;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        (this.animationToAnimationName, this.animationToHasCallback) = initAnimations(this);
        this.playAnimation(SupportedAnimation.Idle);
    }

    public bool playAnimation(SupportedAnimation animation)
    {
        // Make sure animation exists
        if (!this.animationToAnimationName.ContainsKey(animation))
        {
            return false;
        }

        // If animation playing, make sure we are in a state that allows us to override it
        if (this.IsPlaying())
        {
            // If the animation is the same as current one, do not override
            if (this.currentAnimation == animation)
            {
                return false;
            }
            // If the animation has lower priority than the current one, and the current animation is not looping, do not override
            // (looping animations need to be able to override eachother to avoid needing to wait until the end of the loop to change animation)
            if (this.currentAnimation > animation && !isLoopingAnimation(this.currentAnimation))
            {
                return false;
            }
        }

        var animationName = animationToAnimationName.GetValue(animation);
        this.currentAnimation = animation;
        this.animationCallback = null; // Cleanup old callback
        this.Play(animationName);
        return true;
    }

    public bool playAnimation(SupportedAnimation animation, Action onWindupEnd)
    {
        bool animationPlayed = this.playAnimation(animation);
        if (animationPlayed)
        {
            // Setup windup callback
            this.animationCallback = onWindupEnd;
            // If animation does not contain a callback, setup callback when animation ends
            if (!this.animationToHasCallback.GetValueOrDefault(animation, false))
            {
                this.AnimationFinished += this.executeWindupCallback;
            }
        }
        return animationPlayed;
    }

    public void executeWindupCallback()
    {
        this.animationCallback();

        // Cleanup
        this.animationCallback = null;
        if (this.IsConnected(SignalName.AnimationFinished, Callable.From(this.executeWindupCallback)))
        {
            this.AnimationFinished -= this.executeWindupCallback;
        }
    }

    // Method overload is to allow it to be connected to the AnimationFinished signal
    public void executeWindupCallback(StringName onWindupEnd)
    {
        this.executeWindupCallback();
    }

    private static bool isLoopingAnimation(SupportedAnimation animation)
    {
        return animation <= SupportedAnimation.Walk;
    }

    private static (Dictionary<SupportedAnimation, string>, Dictionary<SupportedAnimation, bool>) initAnimations(AnimationPlayer player)
    {
        var animationNames = new Dictionary<SupportedAnimation, string>();
        var hasCallbacks = new Dictionary<SupportedAnimation, bool>();
        foreach (var animationName in player.GetAnimationList())
        {
            SupportedAnimation matchedAnimation = matchAnimation(animationName);
            if (!animationNames.ContainsKey(matchedAnimation))
            {
                animationNames.Add(matchedAnimation, animationName);
                hasCallbacks.Add(matchedAnimation, checkForMethodTrack(player.GetAnimation(animationName)));
            }
        }
        return (animationNames, hasCallbacks);
    }

    private static SupportedAnimation matchAnimation(String animationName)
    {
        var lowerCaseAnimationName = animationName.ToLower();
        foreach (var supportedAnimation in Enum.GetValues<SupportedAnimation>())
        {
            if (lowerCaseAnimationName.Contains(supportedAnimation.ToString().ToLower()))
            {
                return supportedAnimation;
            }
        }
        return SupportedAnimation.Unknown;
    }

    private static bool checkForMethodTrack(Animation animation)
    {
        for (var i = 0; i <  animation.GetTrackCount(); i++)
        {
            if (animation.TrackGetType(i) == Animation.TrackType.Method)
            {
                return true;
            }
        }
        return false;
    }
}

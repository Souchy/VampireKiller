using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vampirekiller.glaceon.sapphire.entities;

public class CreatureStateMachine
{
    //public AnimationState2 previousState { get; set; }
    public AnimationState2 state { get; set; }

    public bool canSetState(AnimationState2 s)
    {
        //if(isPlaying()) 
        // maybe we dont even need to set the idle if the player does it naturally (auto-play animation)
        // bc idle is only when nothing else is playing
        if (state.id > s.id || (state.id == s.id && state.looping))
            return false;

        
        return true;
    }
    public bool setState(AnimationState2 s)
    {
        if (!canSetState(s))
            return false;
        state = s;
        // playAnimation, ou p-e l'animationPlayer observe la state machine et lui-même play son animation basé sur ça, c'est mieux
        return true;
    }
}


public record AnimationState2(int id, bool looping = false)
{

    public static readonly AnimationState2 idle = new AnimationState2(0, true);
    public static readonly AnimationState2 idle_gesture = new AnimationState2(1);
    /// <summary>
    /// Would use a blend2Dspace pour blender root motion walking/running stuff
    /// </summary>
    public static readonly AnimationState2 moving = new AnimationState2(2, true);

    public static readonly AnimationState2 receiveHit = new AnimationState2(3);
    public static readonly AnimationState2 casting = new AnimationState2(4);
    public static readonly AnimationState2 dying = new AnimationState2(5);
    public static readonly AnimationState2 dead = new AnimationState2(6);

}

//public class State { }

//public class StateIdle
//{
//    public List<Type> from => new() { typeof(StateMoving) };
//    public List<Type> to => new() { };
//}
//public class StateIdleGesture
//{
//    public List<Type> from => new() { typeof(StateIdle) };
//    public List<Type> to => new() { };
//}
//public class StateMoving
//{

//}
//public class StateCasting
//{

//}
//public class StateReceivingHit
//{

//}
//public class StateDead
//{

//}


using Util.ecs;
using Util.entity;
using vampirekiller.eevee.triggers;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.equipment;
using VampireKiller.eevee.vampirekiller.eevee.spells;

namespace vampirekiller.eevee.actions;

// public interface IPlayerAction : IAction
// {
//     public CreatureInstance getSourceCreature() => fight.creatures.values.FirstOrDefault(c => c.entityUid == sourceEntity);
//     public CreatureInstance getRaycastCreature() => fight.creatures.values.FirstOrDefault(c => c.entityUid == targetEntity);
// }

public class ActionCastActive : Action //, IActionTrigger
{
    public TriggerType triggerType => TriggerType.onCastActive;
    //public ID activeItem { get; set; }
    //public int slot { get; set; }
    public ID skillInstanceId { get; set; }

    public ActionCastActive() { }
    public ActionCastActive(ID skillInstanceId) => this.skillInstanceId = skillInstanceId;
    //public ActionCastActive(int slot) => this.slot = slot;
    //public ActionCastActive(ID activeItem) => this.activeItem = activeItem;

    //public Item getItem() => fight.items.get(i => i.entityUid == activeItem);
    public SpellInstance? getActive() => getSourceCreature().activeSkills.get(s => s.entityUid == skillInstanceId); //getSourceCreature().activeSkills.getAt(slot);
    public CreatureInstance getSourceCreature() => fight.creatures.values.First(c => c.entityUid == sourceEntity);
    protected override IAction copyImplementation()
        => new ActionCastActive(skillInstanceId); //activeItem);
}

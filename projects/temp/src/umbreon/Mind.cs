


using Godot;
using VampireKiller;

public static class Mind {
    
    public static void castWeapon(CreatureInstance caster, Vector3 cursor, Weapon active) {
        if(active.currentAmmo <= 0) return;
        active.currentAmmo--;

    }

    public static void castSpell(CreatureInstance caster, Vector3 cursor, SpellScroll active) {
        var spellStats = active.spellScrollModel.spellModel.modelStats;
        var resourceType = spellStats.GetSpellResourceCostType().value;
        var casterResource = (StatInt) caster.resources.get(resourceType);
        var resourceCost = spellStats.GetSpellResourceCost();
        if(casterResource.value < resourceCost.value) {
            return;
        }
    }

}

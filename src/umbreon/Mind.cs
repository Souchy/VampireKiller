


using Godot;
using VampireKiller;

public static class Mind {
    
    public static void castWeapon(CreatureInstance caster, Vector3 cursor, Weapon active) {
        if(active.currentAmmo <= 0) return;
        active.currentAmmo--;

    }

    public static void castSpell(CreatureInstance caster, Vector3 cursor, SpellScroll active) {
        if(caster.resources.GetMana() < active.spellScrollModel.spellModel.)
    }

}
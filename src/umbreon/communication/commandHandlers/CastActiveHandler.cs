

using aaaaaaaa;
using VampireKiller;

public class CastActiveHandler : CommandHandler<CastActiveCommand>
{

    public void handle(CastActiveCommand command) {
        Fight fight = new();
        Player player = fight.players[command.playerId];
        ItemInstance item = player.creatureInstance.inventory.equipedActives[command.slot];
        if(item is Weapon w) {
            Mind.castWeapon(player.creatureInstance, command.cursor, w);
        } else
        if(item is SpellScroll s) {
            Mind.castSpell(player.creatureInstance, command.cursor,s);
        }
    }

}
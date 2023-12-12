# DB project or database

Long-lived implementations of Eevee models:
- SpellModels
- ItemModels
- CreatureModels

Storage options:
- Hardcoded
  - Engine-agnostic
  - Type-check safety
  - Doesn't need UI, quick iterations
- Json
  - Engine-agnostic
  - No restrictions / static-typing 
  - May need a UI
- Mongodb, sql
  - Engine-agnostic
  - May need a UI
- Godot resources 
  - Adds a dependancy
  - partial class must extend Resource
  - Has the Godot's resource editor


Short-lived objects:
- Fight
- SpellInstance
- ItemInstance
- CreatureInstance

Storage Options:
- Redis
- Mongodb

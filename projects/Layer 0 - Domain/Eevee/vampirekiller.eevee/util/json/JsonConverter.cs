using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.structures;
using VampireKiller.eevee.vampirekiller.eevee.statements;

namespace vampirekiller.eevee.util.json;

/// <summary>
/// Not finalized. Deserializer doesn't work yet. Serializer does.
/// </summary>
public class SmartListStatementConverter : JsonConverter<SmartList<IStatement>>
{
    public override SmartList<IStatement>? ReadJson(JsonReader reader, Type objectType, SmartList<IStatement>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        //var jobj = JObject.Load(reader);
        //serializer.Populate(reader, null);

        //reader.Read(); // read start array

        var ja = JArray.Load(reader);

        var statements = ja.Select(token =>
        {

            return token.ToObject<Statement>();
        });

        var value = SmartList<IStatement>.Create(); //Activator.CreateInstance<SmartList<IStatement>>();
        foreach (var statement in statements)
            value.add(statement);

        return value;
    }

    public override void WriteJson(JsonWriter writer, SmartList<IStatement>? value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        foreach (var s in value.values)
        {
            serializer.Serialize(writer, s);
        }
        writer.WriteEndArray();
    }
}

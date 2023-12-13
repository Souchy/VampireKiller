using Moq;
using Util.communication.events;
using Util.entity;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace TestProject1;

public class TestRegister
{
    private class EventBusAdapter : EventBus
    {
        public IEnumerable<Subscription> subs => base.subs;
    }
    public class StatsDicAdapter : StatsDic
    {
        public int i = 0;
        public override void initialize()
        {
            i++;
        }
    }


    [Fact]
    public void TestInitializeOnCreate1()
    {
        // Act
        StatsDicAdapter dic = Register.instance.CreateEntity<StatsDicAdapter>();

        // Assert
        Assert.Equal(1, dic.i);
    }
    [Fact]
    public void TestInitializeOnCreate2()
    {
        // Act
        EventBus.factory = () => new EventBusAdapter();
        StatsDic dic = Register.instance.CreateEntity<StatsDic>();
        var dicbus = (EventBusAdapter) dic.GetEntityBus();

        // Assert
        Assert.Equal(3, dicbus.subs.Count());
        foreach(var sub in dicbus.subs)
        {
            Assert.Equal(dic, sub.subscriber);
        }
    }

}
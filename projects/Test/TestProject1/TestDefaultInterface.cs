using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;

namespace TestProject1;
public class TestDefaultInterface
{
    public interface TestInterface
    {
        public int value { get; set; }
        public int meth() => 0;
        public void initialize()
        {
        }
    }
    public class TestImplementation : TestInterface
    {
        public int value { get; set; } = 0;
        public int meth() => 1;
        public void initialize()
        {
            value++;
        }
    }
    public class TestIdentifiable : Identifiable
    {
        public ID entityUid { get; set; }
        public int value { get; set; }
        public void initialize()
        {
            value++;
        }
        public void Dispose()
        {
        }
    }
    public class Factory
    {
        public static T create<T>() where T : TestInterface, new()
        {
            T t = new T();
            t.initialize();
            return t;
        }
        public static T createIdentifiable<T>() where T : Identifiable, new()
        {
            T t = new T();
            t.initialize();
            return t;
        }
    }


    [Fact]
    public void testMethod()
    {
        var test = new TestImplementation();
        Assert.Equal(1, test.meth());
    }
    [Fact]
    public void testMethodFactory()
    {
        var test = Factory.create<TestImplementation>();
        Assert.Equal(1, test.meth());
    }

    [Fact]
    public void testInitialize()
    {
        var test = new TestImplementation();
        test.initialize();
        Assert.Equal(1, test.value);
    }
    [Fact]
    public void testInitializeFactory()
    {
        var test = Factory.create<TestImplementation>();
        Assert.Equal(1, test.value);
    }

    [Fact]
    public void testIdentifiable()
    {
        var test = Factory.createIdentifiable<TestIdentifiable>();
        Assert.Equal(1, test.value);
    }

}

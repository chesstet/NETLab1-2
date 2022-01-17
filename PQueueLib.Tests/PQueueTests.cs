using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using Moq;
using Xunit;

namespace PQueueLib.Tests
{
    public class PQueueTests
    {
        public PQueueTests()
        {
        }
        [Fact]
        public void Can_Create_With_Capacity_Test()
        {
            var pQueue = PQueueSetHelper.CreatePQueueObject<short>(8);

            Assert.IsType<PQueue<short>>(pQueue);
        }

        [Fact]
        public void Cannot_Create_With_Capacity_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(()=>PQueueSetHelper.CreatePQueueObject<short>(-3));
        }

        [Theory]
        [InlineData(5, 7)]
        [InlineData(-45, 5337)]
        [InlineData("test1", "test2")]
        [InlineData("DJMn", "UDG*#B#*HD")]
        public void Can_Enqueue_Test<T>(T item1, T item2)
        {
            var pQueue = PQueueSetHelper.CreatePQueueObject<T>();
            var count = pQueue.Count;

            pQueue.Enqueue(item1);
            pQueue.Enqueue(item2);
            var countResult = pQueue.Count;
            var firstElementResult = pQueue.Peek();

            Assert.Equal(count + 2, countResult);
            Assert.Equal(item1, firstElementResult);
        }

        [Fact]
        public void Can_Peek_Test()
        {
            var pQueue = PQueueSetHelper.CreatePQueueObjectWithElements(3,4,5346,6,32,1);

            var result = pQueue.Peek();

            Assert.Equal(3, result);
        }

        [Fact]
        public void Cannot_Peek_Test()
        {
            var pQueue = PQueueSetHelper.CreatePQueueObject<int>();

            Assert.Throws<InvalidOperationException>(()=> pQueue.Peek());
        }

        [Theory]
#pragma warning disable xUnit1010
        [InlineData(new[]{5, 7, 4, 6, 2, 2})]
        [InlineData(new[]{ -45, 5337, 3, 4, 5346, 6, 32, 1})]
        [InlineData(new[]{ -45.5, 5337.7, 3.8, 4.7, 5346.78, 6.7, 32.78, 1.363})]
#pragma warning restore xUnit1010
        public void Can_Dequeue_Test<T>(T[] elements)
        {
            var pQueue = PQueueSetHelper.CreatePQueueObjectWithElements(elements);
            var count = pQueue.Count;

            var firstElementResult = pQueue.Dequeue();
            var countResult = pQueue.Count;

            Assert.Equal(count-1,countResult);
            Assert.Equal(elements[0],firstElementResult);
        }

        [Fact]
        public void Cannot_Dequeue_Test()
        {
            var pQueue = PQueueSetHelper.CreatePQueueObject<int>();

            Assert.Throws<InvalidOperationException>(()=>pQueue.Dequeue());
        }

        [Fact]
        public void Contains_Element_Test()
        {
            var pQueue = PQueueSetHelper.CreatePQueueObjectWithElements(1,4,5,7,-8,4,-23,2,-5,67,7,8);

            var result1 = pQueue.Contains(8);
            var result2 = pQueue.Contains(-23);

            Assert.True(result1);
            Assert.True(result2);
        }

        [Fact]
        public void Not_Contains_Element_Test()
        {
            var pQueue = PQueueSetHelper.CreatePQueueObjectWithElements(1, 4, 5, 7, -8, 4, -23, 2, -5, 67, 7, 8);

            var result1 = pQueue.Contains(10);
            var result2 = pQueue.Contains(-234);

            Assert.False(result1);
            Assert.False(result2);
        }

        [Fact]
        public void Can_Clear_Test()
        {
            var pQueue = PQueueSetHelper.CreatePQueueObjectWithElements(1, 4, 5, 7, -8, 4, -23, 2, -5, 67, 7, 8);

            pQueue.Clear();

            Assert.Empty(pQueue);
        }
        [Fact]
        public void Count_Test()
        {
            var pQueue = PQueueSetHelper.CreatePQueueObjectWithElements(1, 4, 5, 7, -8, 4, -23, 2, -5, 67, 7, 8);

            var result = pQueue.Count;

            Assert.Equal(12, result);
        }
    }
}

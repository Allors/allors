namespace Allors.Workspace.Adapters.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Workspace;
    using Xunit;

    public static class AssertExtensions
    {
        public static void ShouldContainSingle(this IEnumerable<IObject> @this, IObject expected)
        {
            var objects = @this.ToArray();

            Assert.Single(objects);
            Assert.Contains(expected, objects);
        }

        public static void ShouldHaveSameElements<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            T[] expectedObjects = expected as T[] ?? expected?.ToArray() ?? Array.Empty<T>();
            T[] actualObjects = actual as T[] ?? actual?.ToArray() ?? Array.Empty<T>();

            Assert.Equal(expectedObjects.Length, actualObjects.Length);
            foreach (var actualObject in actualObjects)
            {
                Assert.Contains(actualObject, expectedObjects);
            }
        }
    }
}

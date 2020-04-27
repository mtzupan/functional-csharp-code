using System;
using System.Collections.Generic;
using System.Linq;
using NUnit;
using NUnit.Framework;

namespace Exercises.Chapter1
{
    public static class Exercises
    {
        // 2. Write a function that negates a given predicate: whenvever the given predicate
        // evaluates to `true`, the resulting function evaluates to `false`, and vice versa.
        public static IEnumerable<bool> Negate<T> ( this IEnumerable<T> ts, Func<T, bool> predicate )
        {
            foreach ( T t in ts )
                yield return !predicate ( t );
        }

        // 3. Write a method that uses quicksort to sort a `List<int>` (return a new list,
        // rather than sorting it in place).
        static List<int> QuickSort ( this List<int> list )
        {
            if ( list.Count == 0 )
                return new List<int> ( );

            var pivot = list [ 0 ];
            var rest = list.Skip ( 1 );

            // It finds the element called Pivot which divides the array into two halves
            // In such a way that elements in the left half are smaller than pivot and
            // Elements in the right are greater than pivot.

            var smaller = rest.Where ( o => o <= pivot );
            var larger = rest.Where ( o => o > pivot );

            // We recursively perform three steps:
            // a. Bring the pivot to its appropriate position, such that left of the pivot is smaller and right is greater
            // b. Quick sort the left part
            // c. Quick sort the right part

            return smaller.ToList ( ).QuickSort ( )
                .Append ( pivot )
                .Concat ( ( larger.ToList ( ).QuickSort ( ) ) )
                .ToList ( );
        }

        // 4. Generalize your implementation to take a `List<T>`, and additionally a 
        // `Comparison<T>` delegate.
        static List<T> QuickSort<T> ( this List<T> list, Comparison<T> comparison )
        {
            if ( list.Count == 0 )
                return new List<T> ( );

            var pivot = list [ 0 ];
            var rest = list.Skip ( 1 );

            var smaller = rest.Where ( o => comparison ( o, pivot ) <= 0 );
            var larger = rest.Where ( o => comparison ( o, pivot ) > 0 );

            return smaller.ToList ( ).QuickSort ( comparison )
                .Concat ( new List<T> { pivot } )
                .Concat ( larger.ToList ( ).QuickSort ( comparison ) )
                .ToList ( );
        }

        // 5. In this chapter, you've seen a `Using` function that takes an `IDisposable`
        // and a function of type `Func<TDisp, R>`. Write an overload of `Using` that
        // takes a `Func<IDisposable>` as first
        // parameter, instead of the `IDisposable`. (This can be used to fix warnings
        // given by some code analysis tools about instantiating an `IDisposable` and
        // not disposing it.)
        static R Using<T, R> ( Func<T> createDisposable, Func<T, R> func ) where T : IDisposable
        {
            using ( var disposable = createDisposable ( ) ) return func ( disposable );
        }
    }

    [TestFixture]
    public class Chapter_1_Exercises
    {
        static Func<int, bool> func = x => x % 2 == 0;

        [TestCase ( new int [ 4 ] { 0, 1, 2, 3 }, new bool [ 4 ] { false, true, false, true } )]
        [Test]
        public void Chapter_1_Question_2(int[] inputArray, bool[] expectedOutput)
        {
            var inputList = inputArray.ToList ( );
            var outputList = inputList.Negate ( func ).ToList();

            Assert.AreEqual ( outputList, expectedOutput.ToList ( ) );
        }
        
        [TestCase () ]
        [Test]
        public void Chapter_1_Question_3()
        {

        }
    }

}

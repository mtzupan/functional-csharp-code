using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Exercises.Chapter2
{

    // 1. Write a console app that calculates a user's Body-Mass Index:
    //   - prompt the user for her height in metres and weight in kg
    //   - calculate the BMI as weight/height^2
    //   - output a message: underweight(bmi<18.5), overweight(bmi>=25) or healthy weight
    // 2. Structure your code so that structure it so that pure and impure parts are separate
    // 3. Unit test the pure parts
    // 4. Unit test the impure parts using the HOF-based approach

    public static class Bmi
    {
        public static void Run ( )
        {
            Run ( Read, Write );
        }

        internal static void Run ( Func<Action, string> read, Action<Action> write )
        {
            string height = read ( () => Console.WriteLine( " Enter height in m:" ) );
            string weight = read ( () => Console.WriteLine( " Enter weight in kg: "));

            string message = GetBmiMessage ( DetermineBmi ( height, weight ) );

            write ( ( ) => Console.WriteLine ( message ) );
        }

        // Impure -- test with HOF methods
        public static string Read ( Action writeCommand )
        {
            writeCommand.Invoke ( );
            return Console.ReadLine ( );
        }

        // Impure -- test with HOF methods
        public static void Write ( Action writeCommand )
        {
            writeCommand.Invoke ( ) ;
        }

        // Pure
        public static double DetermineBmi ( string heightStr, string weightStr )
        {
            double height = Convert.ToDouble ( heightStr );
            double weight = Convert.ToDouble ( weightStr );

            return weight / Math.Pow ( height, 2 );
        }

        // Pure
        public static string GetBmiMessage ( double bmi )
        {
            switch ( bmi )
            {
                case double n when ( n < 18.5 ):
                    return "Underweight";
                case double n when ( n >= 18.5 && n <= 25 ):
                    return "Healthy weight";
                case double n when ( n > 25 ):
                    return "Overweight";
                default:
                    throw new Exception ( "Error in switch statement ranges!" );
            }
        }
    }

    [TestFixture]
    public class Chapter_2_Exercises
    {
        // Unit test the 'pure' parts
        [TestCase ( 10, "Underweight" )]
        [TestCase ( 20, "Healthy weight" )]
        [TestCase ( 30, "Overweight" )]
        [Test]
        public void GetBmiMessage_ReturnsExpectedMessage(double bmi, string expectedMessage)
        {
            Assert.AreEqual(Bmi.GetBmiMessage ( bmi ), expectedMessage);
        }

        [TestCase ( "90", "1.8", 90/3.24 ) ]
        [Test]
        public void DetermineBmi_ReturnsCorrectResult(string weight, string height, double bmi)
        {
            Assert.AreEqual ( Bmi.DetermineBmi ( height, weight ), bmi);
        }

        // Unit test the 'impure' parts
        //static string weightPrompt = " Enter height in m: ";
        //static string weightResponse = "90";
        //static string heightPrompt = " Enter weight in kg: ";
        //static string heightResponse = "1.8";
        [TestCase(" Enter height in m: ", "1.8")]
        [Test]
        public void Read_ReturnsStringResult(string heightPrompt, string heightResponse)
        {
            // Assert.AreEqual ( Bmi.Read ( ( ) => { } ) );
        }

        // TODO: Do not have Console.ReadLine in the Read method. Pass in as we do Console.WriteLine.
    }
}

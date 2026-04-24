using System;
using System.Collections;
using System.Collections.Generic;

using CarFactory;
using CarFactory.Core;
using CarFactory.Core.Engines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CarFactory.Core.Engines.UnitTests
{
    [TestClass]
    public class GasolineEngineTests
    {
        /// <summary>
        /// Verifies that calling Increase increments the Speed property by one for a variety of starting speeds,
        /// including boundary values. Inputs tested: 0 (default), positive, negative, int.MaxValue - 1, and int.MaxValue.
        /// Expected behavior: Speed becomes start + 1 for normal values; when starting at int.MaxValue an overflow wrap
        /// occurs and Speed becomes int.MinValue.
        /// </summary>
        [TestMethod]
        public void Increase_StartsWithVariousSpeeds_IncrementsByOne_WrappingOnOverflow()
        {
            // Arrange
            var testCases = new (int start, int expected)[]
            {
                (0, 1),
                (1, 2),
                (-1, 0),
                (100, 101),
                (int.MaxValue - 1, int.MaxValue),
                // overflow case: int.MaxValue + 1 wraps to int.MinValue in unchecked context
                (int.MaxValue, int.MinValue)
            };

            foreach (var (start, expected) in testCases)
            {
                // Arrange per case
                var engine = new GasolineEngine();
                engine.UpdateSpeed(start);

                // Act
                engine.Increase();

                // Assert
                Assert.AreEqual(expected, engine.Speed, $"Start: {start} expected Speed {expected} after Increase.");
            }
        }

        /// <summary>
        /// Ensures multiple sequential calls to Increase accumulate correctly and that wrapping occurs across multiple increments.
        /// Scenario 1: start at 0 and call Increase 5 times -> expected Speed = 5.
        /// Scenario 2 (wrap across calls): start at int.MaxValue - 2 and call Increase 3 times -> expected wrap to int.MinValue.
        /// </summary>
        [TestMethod]
        public void Increase_MultipleSequentialCalls_AccumulatesCorrectly()
        {
            // Scenario 1
            {
                // Arrange
                var engine = new GasolineEngine();
                engine.UpdateSpeed(0);

                // Act
                for (int i = 0; i < 5; i++)
                {
                    engine.Increase();
                }

                // Assert
                Assert.AreEqual(5, engine.Speed, "After 5 increments from 0 the Speed should be 5.");
            }

            // Scenario 2 - overflow across multiple increments
            {
                // Arrange
                var engine = new GasolineEngine();
                engine.UpdateSpeed(int.MaxValue - 2);

                // Act
                for (int i = 0; i < 3; i++)
                {
                    engine.Increase();
                }

                // Assert
                // Calculation: (int.MaxValue - 2) + 3 => int.MaxValue + 1 => wraps to int.MinValue
                Assert.AreEqual(int.MinValue, engine.Speed, "Incrementing past int.MaxValue should wrap to int.MinValue.");
            }
        }

        /// <summary>
        /// Verifies that UpdateSpeed assigns the provided integer value to the Speed property.
        /// Tests multiple representative integer values including int.MinValue, negative, zero, positive, and int.MaxValue.
        /// Expected: After calling UpdateSpeed(value), Speed equals value and no exception is thrown.
        /// </summary>
        [TestMethod]
        public void UpdateSpeed_VariousIntegers_SetsSpeedToProvidedValue()
        {
            // Arrange & Act & Assert for a set of representative integer values.
            int[] testValues = new[] { int.MinValue, -1, 0, 1, 42, int.MaxValue };

            foreach (int value in testValues)
            {
                // Arrange
                var engine = new GasolineEngine();

                // Act
                engine.UpdateSpeed(value);

                // Assert
                Assert.AreEqual(value, engine.Speed, $"Speed should be set to the provided value {value}.");
            }
        }

        /// <summary>
        /// Verifies that successive calls to UpdateSpeed override previous values and the last provided value is retained.
        /// Input conditions: first call with a positive value, second call with a negative value.
        /// Expected: Speed equals the last value passed to UpdateSpeed.
        /// </summary>
        [TestMethod]
        public void UpdateSpeed_CalledMultipleTimes_LastValueWins()
        {
            // Arrange
            var engine = new GasolineEngine();
            int firstValue = 10;
            int secondValue = -5;

            // Act
            engine.UpdateSpeed(firstValue);
            engine.UpdateSpeed(secondValue);

            // Assert
            Assert.AreEqual(secondValue, engine.Speed, "Speed should reflect the last value passed to UpdateSpeed.");
        }

        /// <summary>
        /// Verifies Decrease reduces Speed by exactly one when Speed is positive,
        /// and leaves Speed unchanged when Speed is zero or negative.
        /// Inputs tested: int.MinValue (negative), -1 (negative), 0, 1, 2, int.MaxValue.
        /// Expected: negative and zero remain unchanged; positive decrease by 1.
        /// </summary>
        [TestMethod]
        public void Decrease_WhenVariousInitialSpeeds_UpdatesSpeedAccordingly()
        {
            // Arrange
            var cases = new List<(int initial, int expected)>
            {
                (int.MinValue, int.MinValue), // negative extreme stays the same
                (-1, -1),                     // negative stays the same
                (0, 0),                       // zero stays the same
                (1, 0),                       // positive decrements to 0
                (2, 1),                       // positive decrements by 1
                (int.MaxValue, int.MaxValue - 1) // large positive decrements by 1
            };

            foreach (var (initial, expected) in cases)
            {
                // Arrange per-case
                var engine = new GasolineEngine();
                // Use UpdateSpeed to set the internal Speed to the desired starting value
                engine.UpdateSpeed(initial);

                // Act
                engine.Decrease();

                // Assert
                Assert.AreEqual(expected, engine.Speed, $"Initial Speed={initial} produced unexpected Speed after Decrease.");
            }
        }

        /// <summary>
        /// Verifies that multiple consecutive Decrease calls never reduce Speed below zero when starting from a small positive value.
        /// Input: start = 1, two Decrease calls.
        /// Expected: first call reduces to 0, second call leaves at 0 (no negative wrap).
        /// </summary>
        [TestMethod]
        public void Decrease_TwiceFromOne_DoesNotBecomeNegative()
        {
            // Arrange
            var engine = new GasolineEngine();
            engine.UpdateSpeed(1);

            // Act - first decrease
            engine.Decrease();

            // Assert after first decrease
            Assert.AreEqual(0, engine.Speed, "After first Decrease from 1 expected Speed to be 0.");

            // Act - second decrease should not change (remain 0)
            engine.Decrease();

            // Assert after second decrease
            Assert.AreEqual(0, engine.Speed, "After second Decrease from 0 expected Speed to remain 0 and not become negative.");
        }
    }
}
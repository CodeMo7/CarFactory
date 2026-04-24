using System;
using System.Collections;

using CarFactory;
using CarFactory.Core;
using CarFactory.Core.Engines;
using CarFactory.Core.Interfaces;
using CarFactory.Core.Models;
using CarFactory.Factory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CarFactory.Factory.UnitTests
{
    [TestClass]
    public class CarFactoryTests
    {
        /// <summary>
        /// Verifies that ChangeEngine replaces the car's engine for each valid EngineType.
        /// Input conditions: car initially built with a mock IEngine, then ChangeEngine is called with each defined EngineType.
        /// Expected result: the original (mocked) engine should not receive UpdateSpeed calls after replacement and Car.Start works (Speed == 0).
        /// </summary>
        [TestMethod]
        public void ChangeEngine_ValidEngineTypes_ReplacesEngineAndDoesNotInvokePreviousEngine()
        {
            // Arrange
            var validTypes = new[] { EngineType.Gasoline, EngineType.Electronic, EngineType.Hybrid };

            foreach (var engineType in validTypes)
            {
                var engineMock = new Mock<IEngine>(MockBehavior.Loose);
                // create car with mock engine
                var car = new Car(engineMock.Object);

                // Act
                CarFactory.ChangeEngine(car, engineType);

                // After changing the engine, invoking a car behavior that calls engine.UpdateSpeed
                // ensures the active engine receives the call. If the engine was not replaced, the mock would receive it.
                car.Start();

                // Assert
                engineMock.Verify(m => m.UpdateSpeed(It.IsAny<int>()), Times.Never,
                    $"Original engine should not be invoked after ChangeEngine for EngineType {engineType}.");

                Assert.AreEqual(0, car.Speed, "After Start, car.Speed should be 0 regardless of engine implementation.");
            }
        }

        /// <summary>
        /// Verifies that ChangeEngine throws ArgumentOutOfRangeException when an invalid EngineType value is provided.
        /// Input conditions: car is valid, engineType is an out-of-range enum value (cast from an int).
        /// Expected result: ArgumentOutOfRangeException is thrown.
        /// </summary>
        [TestMethod]
        public void ChangeEngine_InvalidEngineValue_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var initialEngine = new Mock<IEngine>().Object;
            var car = new Car(initialEngine);
            var invalidEngineValue = (EngineType)999;

            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                CarFactory.ChangeEngine(car, invalidEngineValue));
        }

        /// <summary>
        /// Verifies that ChangeEngine invoked with a null Car reference results in a NullReferenceException.
        /// Input conditions: car is null (nullable variable), engineType is valid.
        /// Expected result: NullReferenceException is thrown because method dereferences the car parameter.
        /// </summary>
        [TestMethod]
        public void ChangeEngine_NullCar_ThrowsNullReferenceException()
        {
            // Arrange
            Car? car = null;

            // Act & Assert
            Assert.ThrowsException<NullReferenceException>(() =>
                CarFactory.ChangeEngine(car!, EngineType.Gasoline));
        }

        /// <summary>
        /// Verifies that CreateCar returns a non-null Car for each defined EngineType.
        /// Input conditions: iterates all values of the EngineType enum (Gasoline, Electronic, Hybrid).
        /// Expected result: a non-null Car is returned for each EngineType and the Car instance behaves as expected
        /// for basic operations (Start sets Speed to 0, Accelerate increases speed by 20 from 0, Brake brings speed back to 0).
        /// This validates that CreateCar successfully constructs a Car with a usable engine for defined enum values.
        /// </summary>
        [TestMethod]
        public void CreateCar_ValidEngineTypes_ReturnsCarWithFunctionalControls()
        {
            // Arrange
            var engineTypes = (EngineType[])Enum.GetValues(typeof(EngineType));

            // Act & Assert: iterate through all defined enum values to ensure CreateCar succeeds and the Car works.
            foreach (var type in engineTypes)
            {
                // Act
                Car car = CarFactory.CreateCar(type);

                // Assert - created instance
                Assert.IsNotNull(car, $"CreateCar returned null for EngineType '{type}'.");

                // Act - start the car which should set Speed to 0 and call engine.UpdateSpeed(0)
                car.Start();

                // Assert - Speed after start should be 0
                Assert.AreEqual(0, car.Speed, $"After Start(), expected Speed 0 for EngineType '{type}'.");

                // Act - accelerate once: from 0 -> 20 (as per Car.Accelerate logic)
                car.Accelerate();

                // Assert - Speed should be 20 after single accelerate (since initial was 0)
                Assert.AreEqual(20, car.Speed, $"After Accelerate(), expected Speed 20 for EngineType '{type}'.");

                // Act - brake once: should bring Speed back to 0
                car.Brake();

                // Assert - Speed should be 0 after braking from 20
                Assert.AreEqual(0, car.Speed, $"After Brake(), expected Speed 0 for EngineType '{type}'.");
            }
        }

        /// <summary>
        /// Verifies that CreateCar throws ArgumentOutOfRangeException when provided with an out-of-range EngineType value.
        /// Input conditions: an integer value cast to EngineType that is outside the defined enum range.
        /// Expected result: ArgumentOutOfRangeException is thrown by CreateCar (via internal CreateEngine).
        /// </summary>
        [TestMethod]
        public void CreateCar_InvalidEngineType_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var invalidValue = (EngineType)int.MaxValue;

            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => CarFactory.CreateCar(invalidValue),
                "CreateCar should throw ArgumentOutOfRangeException for an invalid EngineType value.");
        }
    }
}
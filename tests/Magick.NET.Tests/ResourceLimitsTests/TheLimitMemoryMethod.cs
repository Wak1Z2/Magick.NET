﻿// Copyright Dirk Lemstra https://github.com/dlemstra/Magick.NET.
// Licensed under the Apache License, Version 2.0.

using System;
using ImageMagick;
using Xunit;

namespace Magick.NET.Tests;

public partial class ResourceLimitsTests
{
    [Collection(nameof(IsolatedUnitTest))]
    public class TheLimitMemoryMethod
    {
        [Fact]
        public void ShouldThrowExceptionWhenValueIsNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>("percentage", () => ResourceLimits.LimitMemory(new Percentage(-0.99)));
        }

        [Fact]
        public void ShouldThrowExceptionWhenValueIsTooHigh()
        {
            Assert.Throws<ArgumentOutOfRangeException>("percentage", () => ResourceLimits.LimitMemory(new Percentage(100.1)));
        }

        [Fact]
        public void ShouldChangeAreaAndMemory()
        {
            IsolatedUnitTest.Execute(() =>
            {
                var area = ResourceLimits.Area;
                var memory = ResourceLimits.Memory;

                ResourceLimits.LimitMemory((Percentage)80);

                Assert.NotEqual(area, ResourceLimits.Area);
                Assert.NotEqual(memory, ResourceLimits.Memory);

                ResourceLimits.Area = area;
                ResourceLimits.Memory = memory;
            });
        }

#if WINDOWS_BUILD
        [Fact]
        public void ShouldSetMemoryAndAreaToTheCorrectValues()
        {
            IsolatedUnitTest.Execute(() =>
            {
                var distance = 16384UL;
                var area = ResourceLimits.Area;
                var memory = ResourceLimits.Memory;

                Assert.InRange(area, (memory * 4) - distance, (memory * 4) + distance);

                ResourceLimits.LimitMemory((Percentage)100);

                Assert.InRange(ResourceLimits.Area, (area * 2) - distance, (area * 2) + distance);
                Assert.InRange(ResourceLimits.Memory, (memory * 2) - distance, (memory * 2) + distance);

                ResourceLimits.Area = area;
                Assert.Equal(area, ResourceLimits.Area);

                ResourceLimits.Memory = memory;
                Assert.Equal(memory, ResourceLimits.Memory);
            });
        }
#endif
    }
}

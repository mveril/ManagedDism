﻿// Copyright (c). All rights reserved.
//
// Licensed under the MIT license.

using Shouldly;
using System;
using System.Collections.Generic;

namespace Microsoft.Dism.Tests
{
    public class DismDriverCollectionTest : DismCollectionTest<DismDriverCollection, DismDriver>
    {
        public DismDriverCollectionTest(TestWimTemplate template)
            : base(template)
        {
        }

        protected override DismDriverCollection CreateCollection(List<DismDriver> expectedCollection)
        {
            return new DismDriverCollection(expectedCollection);
        }

        protected override DismDriverCollection CreateCollection()
        {
            return new DismDriverCollection();
        }

        protected override List<DismDriver> GetCollection()
        {
            return new List<DismDriver>
            {
                new DismDriver(new DismApi.DismDriver_
                {
                   Architecture = (ushort)DismProcessorArchitecture.AMD64,
                   CompatibleIds = "CompatibleIds",
                   ExcludeIds = "ExcludeIds",
                   HardwareDescription = "HardwareDescription",
                   HardwareId = "HardwareId",
                   ManufacturerName = "ManufacturerName",
                   ServerName = "ServerName",
                }),
            };
        }
    }

    public class DismDriverTest : DismStructTest<DismDriver>
    {
        private readonly DismApi.DismDriver_ _driver = new DismApi.DismDriver_
        {
            Architecture = 9,
            CompatibleIds = "CompatibleIds",
            ExcludeIds = "ExcludeIds",
            HardwareDescription = "HardwareDescription",
            HardwareId = "HardwareId",
            ManufacturerName = "ManufacturerName",
            ServerName = "ServerName",
        };

        public DismDriverTest(TestWimTemplate template)
            : base(template)
        {
        }

        protected override DismDriver Item => ItemPtr != IntPtr.Zero ? new DismDriver(ItemPtr) : new DismDriver(_driver);

        protected override object Struct => _driver;

        protected override void VerifyProperties(DismDriver item)
        {
            item.Architecture.ShouldBe((DismProcessorArchitecture)_driver.Architecture);
            item.CompatibleIds.ShouldBe(_driver.CompatibleIds);
            item.ExcludeIds.ShouldBe(_driver.ExcludeIds);
            item.HardwareDescription.ShouldBe(_driver.HardwareDescription);
            item.HardwareId.ShouldBe(_driver.HardwareId);
            item.ManufacturerName.ShouldBe(_driver.ManufacturerName);
            item.ServerName.ShouldBe(_driver.ServerName);
        }
    }
}
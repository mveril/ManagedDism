﻿// Copyright (c). All rights reserved.
//
// Licensed under the MIT license.

using Shouldly;
using System;
using System.Collections.Generic;

namespace Microsoft.Dism.Tests
{
    public class DismPackageCollectionTest : DismCollectionTest<DismPackageCollection, DismPackage>
    {
        public DismPackageCollectionTest(TestWimTemplate template)
            : base(template)
        {
        }

        protected override DismPackageCollection CreateCollection(List<DismPackage> expectedCollection)
        {
            return new DismPackageCollection(expectedCollection);
        }

        protected override DismPackageCollection CreateCollection()
        {
            return new DismPackageCollection();
        }

        protected override List<DismPackage> GetCollection()
        {
            return new List<DismPackage>
            {
                new DismPackage(new DismApi.DismPackage_
                {
                   InstallTime = DateTime.Today,
                   PackageName = "PackageName1",
                   PackageState = DismPackageFeatureState.Installed,
                   ReleaseType = DismReleaseType.LanguagePack,
                }),
                new DismPackage(new DismApi.DismPackage_
                {
                   InstallTime = DateTime.Today.AddDays(-17),
                   PackageName = "PackageName2",
                   PackageState = DismPackageFeatureState.Removed,
                   ReleaseType = DismReleaseType.LanguagePack,
                }),
            };
        }
    }

    public class DismPackageTest : DismStructTest<DismPackage>
    {
        private readonly DismApi.DismPackage_ _package = new DismApi.DismPackage_
        {
            InstallTime = DateTime.Today.AddDays(-1),
            PackageName = "88FFD05E-1523-45D8-B3AF-DD41EA81B984",
            PackageState = DismPackageFeatureState.Removed,
            ReleaseType = DismReleaseType.SecurityUpdate,
        };

        public DismPackageTest(TestWimTemplate template)
            : base(template)
        {
        }

        protected override DismPackage Item => new DismPackage(_package);

        protected override object Struct => _package;

        protected override void VerifyProperties(DismPackage item)
        {
            item.InstallTime.ShouldBe((DateTime)_package.InstallTime);
            item.PackageName.ShouldBe(_package.PackageName);
            item.PackageState.ShouldBe(_package.PackageState);
            item.ReleaseType.ShouldBe(_package.ReleaseType);
        }
    }
}
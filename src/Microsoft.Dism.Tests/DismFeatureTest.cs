﻿// Copyright (c). All rights reserved.
//
// Licensed under the MIT license.

using Shouldly;
using System;
using System.Collections.Generic;

namespace Microsoft.Dism.Tests
{
    public class DismFeatureCollectionTest : DismCollectionTest<DismFeatureCollection, DismFeature>
    {
        public DismFeatureCollectionTest(TestWimTemplate template)
            : base(template)
        {
        }

        protected override DismFeatureCollection CreateCollection(List<DismFeature> expectedCollection)
        {
            return new DismFeatureCollection(expectedCollection);
        }

        protected override DismFeatureCollection CreateCollection()
        {
            return new DismFeatureCollection();
        }

        protected override List<DismFeature> GetCollection()
        {
            return new List<DismFeature>
            {
                new DismFeature(new DismApi.DismFeature_
                {
                    FeatureName = "FeatureName1",
                    State = DismPackageFeatureState.PartiallyInstalled,
                }),
                new DismFeature(new DismApi.DismFeature_
                {
                    FeatureName = "FeatureName2",
                    State = DismPackageFeatureState.Superseded,
                }),
            };
        }
    }

    public class DismFeatureTest : DismStructTest<DismFeature>
    {
        private readonly DismApi.DismFeature_ _feature = new DismApi.DismFeature_
        {
            FeatureName = "ED1E66B9-7234-4D2E-A31F-39F6AB0559D9",
            State = DismPackageFeatureState.PartiallyInstalled,
        };

        public DismFeatureTest(TestWimTemplate template)
            : base(template)
        {
        }

        protected override DismFeature Item => ItemPtr != IntPtr.Zero ? new DismFeature(ItemPtr) : new DismFeature(_feature);

        protected override object Struct => _feature;

        protected override void VerifyProperties(DismFeature item)
        {
            item.FeatureName.ShouldBe(_feature.FeatureName);
            item.State.ShouldBe(_feature.State);
        }
    }
}
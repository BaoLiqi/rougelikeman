﻿namespace Spine.Unity
{
    using System;
    using System.Runtime.InteropServices;

    public class SpineTransformConstraint : SpineAttributeBase
    {
        public SpineTransformConstraint(string startsWith = "", string dataField = "", bool includeNone = true, bool fallbackToTextField = false)
        {
            base.startsWith = startsWith;
            base.dataField = dataField;
            base.includeNone = includeNone;
            base.fallbackToTextField = fallbackToTextField;
        }
    }
}


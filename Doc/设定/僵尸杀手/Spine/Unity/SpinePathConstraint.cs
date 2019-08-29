﻿namespace Spine.Unity
{
    using System;
    using System.Runtime.InteropServices;

    public class SpinePathConstraint : SpineAttributeBase
    {
        public SpinePathConstraint(string startsWith = "", string dataField = "", bool includeNone = true, bool fallbackToTextField = false)
        {
            base.startsWith = startsWith;
            base.dataField = dataField;
            base.includeNone = includeNone;
            base.fallbackToTextField = fallbackToTextField;
        }
    }
}


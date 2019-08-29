﻿namespace TableTool
{
    using System;

    public class Stage_Level_chapter2Model : LocalModel<Stage_Level_chapter2, string>
    {
        private const string _Filename = "Stage_Level_chapter2";

        protected override string GetBeanKey(Stage_Level_chapter2 bean) => 
            bean.RoomID;

        protected override string Filename =>
            "Stage_Level_chapter2";
    }
}


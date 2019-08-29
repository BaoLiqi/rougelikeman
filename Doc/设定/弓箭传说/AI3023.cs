﻿using System;

public class AI3023 : AIBase
{
    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        AIBase.ActionSequence action = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        action.AddAction(base.GetActionRemoveMove());
        action.AddAction(base.GetActionWait("actionwaitrandom", 0x44c));
        action.AddAction(new AIMove1011(base.m_Entity, 600, 0x3e8));
        int num = 1;
        if (base.m_Entity.IsElite)
        {
            num = 3;
        }
        for (int i = 0; i < num; i++)
        {
            action.AddAction(new AIMove1021(base.m_Entity, 600));
            action.AddAction(base.GetActionWait("actionwaitrandom", 400));
        }
        base.AddAction(action);
    }
}


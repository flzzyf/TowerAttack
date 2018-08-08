using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchTower : OccupiableBuilding
{
    public override void BuildSetting(GameObject _node)
    {
        base.BuildSetting(_node);

    }

    public override void OccupiedEffect()
    {
        base.OccupiedEffect();

        FogOfWarManager.Instance().AddNodesWithinRangeToPlayerVision(player, node, 3);
    }

    public override void LiberatedEffect()
    {
        base.LiberatedEffect();

        FogOfWarManager.Instance().RemoveNodesWithinRangeToPlayerVision(player, node, 3);

    }
}

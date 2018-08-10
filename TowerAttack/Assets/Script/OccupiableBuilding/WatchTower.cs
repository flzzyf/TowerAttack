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
        FogOfWarManager.Instance().AddNodesWithinRangeToPlayerVision(player, node, 3);
        base.OccupiedEffect();
    }

    public override void LiberatedEffect()
    {
        FogOfWarManager.Instance().RemoveNodesWithinRangeToPlayerVision(player, node, 3);

        base.LiberatedEffect();

    }
}

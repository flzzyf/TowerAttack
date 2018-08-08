using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchTower : OccupiableBuilding
{
    public override void BuildSetting(GameObject _node)
    {
        base.BuildSetting(_node);

        foreach (var item in nearbyNodes)
        {
            //将周围节点变为农场土地
            //item.GetComponent<NodeItem>().ChangeNodeAppearence("Farm");
        }
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

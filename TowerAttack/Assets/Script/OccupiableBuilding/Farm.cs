using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : OccupiableBuilding
{
    public override void BuildSetting(GameObject _node)
    {
        base.BuildSetting(_node);

        foreach (var item in nearbyNodes)
        {
            //将周围节点变为农场土地
            item.GetComponent<NodeItem>().ChangeNodeAppearence("Farm");
        }
    }

    public override void OccupiedEffect()
    {
        base.OccupiedEffect();

        ScoreManager.Instance().ModifyFarmer(player, 5);
    }

    public override void LiberatedEffect()
    {
        base.LiberatedEffect();

        ScoreManager.Instance().ModifyFarmer(player, -5);
    }
}

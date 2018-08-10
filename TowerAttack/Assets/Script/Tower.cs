using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour 
{
    public int range = 1;
    public float hp = 3;
    float currentHp;
    public int damage = 1;
    public int vision = 2;

    public GameObject prefab_bullet;

    public GameObject flag;
    public GameObject roof;

    public float searchTargetCD = 0.5f;

    public float attackCD = 0.5f;
    float currentAttackCD;

    public Transform launchPos;

    [HideInInspector]
    public int player;

    [HideInInspector]
    public GameObject target;

    [HideInInspector]
    public GameObject node;

    public Vector2 impactAreaCenter;
    public Vector2 impactAreaSize = new Vector2(1 ,1);

    public GameObject gfx_tower;
    public GameObject gfx_building;
    public GameObject gfx_upgrading;

    [HideInInspector]
    public bool building;

    bool isDead;

    public float upgradeTime = 3;
    public bool[] upgraded = new bool[2];

    void Start () 
	{
        currentHp = hp;

        flag.GetComponent<SpriteRenderer>().color = PlayerManager.Instance().players[player].color;
        roof.GetComponent<SpriteRenderer>().color = PlayerManager.Instance().players[player].color;

    }
    void Update () 
	{
        if (currentAttackCD > 0)
            currentAttackCD -= Time.deltaTime;
        else
        {
            if (target != null)
            {
                currentAttackCD = attackCD;
                Attack(target);
            }

        }
    }

    public void Building()
    {
        //所有塔开始搜索目标
        BuildManager.Instance().AllTowerStartSearching();

        ToggleParticles(gfx_building, true);

        gfx_tower.SetActive(false);

        building = true;
    }

    public void BuildingFinish()
    {
        ToggleParticles(gfx_building, false);

        gfx_tower.SetActive(true);

        building = false;

        SearchTarget();
    }

    //塔搜索目标
    public void SearchTarget()
    {
        if (building || target != null)
            return;

        foreach (var item in MapManager.Instance().GetNearbyNodesWithinRange(node, range))
        {
            if (item.GetComponent<NodeItem>().tower != null)
                if (PlayerManager.Instance().isEnemy(item.GetComponent<NodeItem>().tower.GetComponent<Tower>().player, player))
                    target = item.GetComponent<NodeItem>().tower;
        }
    }

    public void Attack(GameObject _target)
    {
        SoundManager.Instance().Play("Arrow_Launch");

        GameObject go = Instantiate(prefab_bullet, launchPos.position, Quaternion.identity, ParentManager.Instance().GetParent("Missile"));

        float fixedDamage = damage * attackCD;

        go.GetComponent<Bullet>().Launch(_target, fixedDamage);
    }


    public void TakeDamage(float _amount)
    {
        currentHp -= _amount;
        if(!isDead && currentHp <= 0)
        {
            isDead = true;
            Death();
        }
    }

    //塔被摧毁
    public void Death()
    {
        if (player == GameManager.Instance().player)
            SoundManager.Instance().Play("Tower_Death");

        node.GetComponent<NodeItem>().tower = null;

        BuildManager.Instance().towers.Remove(gameObject);

        if(!building)
            node.GetComponent<NodeItem>().TowerDestoryed(player);

        DestroyImmediate(gameObject);

        //所有塔开始搜索目标
        BuildManager.Instance().AllTowerStartSearching();
    }

    public void SetOrderInLayer(int _order)
    {
        foreach (var item in GetComponentsInChildren<SpriteRenderer>(true))
        {
            item.sortingOrder = _order;
        }
    }

    //获取轰击点
    public Vector2 GetImpactPoint()
    {
        Vector2 point;
        point.x = impactAreaCenter.x + Random.Range(-1, 1) * impactAreaSize.x / 2;
        point.y = impactAreaCenter.y + Random.Range(-1, 1) * impactAreaSize.y / 2;
        point += (Vector2)transform.position;

        return point;
    }

    //绘制线框
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube((Vector2)transform.position + impactAreaCenter, impactAreaSize);
    }

    //升级射程
    public void Upgrade_Range()
    {
        range++;

        //减少周围节点战力
        foreach (var item in MapManager.Instance().GetNodesWithinRange(node, range - 1))
        {
            item.GetComponent<NodeItem>().GetComponent<NodeItem>().playerForce[player]--;
        }
        //增加周围节点战力
        foreach (var item in MapManager.Instance().GetNodesWithinRange(node, range))
        {
            item.GetComponent<NodeItem>().GetComponent<NodeItem>().playerForce[player]++;
        }
        //更新边界和战力数字
        foreach (var item in MapManager.Instance().GetNodesWithinRange(node, range + 1))
        {
            item.GetComponent<NodeItem>().UpdateBorders();
            item.GetComponent<NodeItem>().UpdateForceText(player);
        }

    }

    //升级视野
    public void Upgrade_Vision()
    {
        vision++;
        FogOfWarManager.Instance().AddNodesWithinRangeToPlayerVision(player, node, vision);
        FogOfWarManager.Instance().RemoveNodesWithinRangeToPlayerVision(player, node, vision - 1);
    }

    public void Upgrade(int _index)
    {
        target = null;

        StartCoroutine(Upgrading(_index));
    }

    IEnumerator Upgrading(int _index)
    {
        float upgradeProgress = 0;

        //建造前设置
        ToggleParticles(gfx_upgrading, true);

        building = true;

        ScoreManager.Instance().ModifyWorker(player, 5);

        AudioSource source = null;
        if (player == GameManager.Instance().player)
        {
            SoundManager.Instance().Play("Construct_Start");
            source = SoundManager.Instance().Play("Construct_Loop");
        }

        while (!isDead && upgradeProgress < upgradeTime)
        {
            upgradeProgress += Time.deltaTime;

            yield return null;
        }

        //建造后设置
        ToggleParticles(gfx_upgrading, false);
        building = false;
        ScoreManager.Instance().ModifyWorker(player, -5);
        if (source != null)
        {
            source.Stop();
            Destroy(source);
        }
        SoundManager.Instance().Play("UpgradeComplete");

        if (!isDead)
        {
            upgraded[_index] = true;

            SearchTarget();

            if (_index == 0)
                Upgrade_Range();
            else
                Upgrade_Vision();
        }
    }

    void ToggleParticles(GameObject _go, bool _on)
    {
        _go.SetActive(_on);
        if(_on)
        {
            foreach (var item in _go.GetComponentsInChildren<ParticleSystem>(true))
            {
                item.Play();
#pragma warning disable CS0618 // Type or member is obsolete
                item.enableEmission = true;
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }
        else
        {
            foreach (var item in _go.GetComponentsInChildren<ParticleSystem>(true))
            {
                item.Stop();
#pragma warning disable CS0618 // Type or member is obsolete
                item.enableEmission = false;
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }
    }
}

public enum EnemyAttackType
{
    /// <summary>
    /// 近战
    /// </summary>
    Melee=1,
    /// <summary>
    /// 远程
    /// </summary>
    Remote,
    /// <summary>
    /// 自爆
    /// </summary>
    Explosion,
    /// <summary>
    /// 分裂
    /// </summary>
    Split,
    /// <summary>
    /// boss
    /// </summary>
    Boss
}

public enum CharacterType
{
    Player,
    Enemy,
    Boss,
    Wall,
    Arrow,
    Effect,
    SmallWall,
    Drop,
    Terrain,
    AttackWall,
    Trigger
}
public enum ElementAttributeType
{
    None=0,
    Fire=1,
    Soil=2,
    Wood=3,
    Wind=4
}

public enum PoolType
{
    ExplosionEnemy=0,
    MeleeEnemy,
    RemoteEnemy,
    SplitEnemy,
    MesBox,
    burning_1,
    explosion_1,
    explosion_2,
    explosion_3,
    explosion_4,

}

public enum AnimType
{
    Idle,
    Run,
    Sprint,
    Death,
    Rotate,
    Attack,
    UseSkill,
    UseSprint
}

public enum PlayerDataType
{
    /// <summary>
    /// 愤怒
    /// </summary>
    Anger,
    Hp,
    /// <summary>
    /// 碎片
    /// </summary>
    Fragment,
    All
}

public enum GameFinishType
{
    Fail,
    Win
}

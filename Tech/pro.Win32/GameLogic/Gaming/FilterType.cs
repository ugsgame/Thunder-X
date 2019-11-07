
using MatrixEngine.CocoStudio.Armature;

namespace Thunder.GameLogic.Gaming
{
    /// <summary>
    /// 碰撞筛选类型
    /// </summary>
    public class FilterType
    {
        public static readonly CCColliderFilter FilterEnemy = new CCColliderFilter(0x0002, 0x0004, 0);
        public static readonly CCColliderFilter FilterPlayer = new CCColliderFilter(0x0004, 0x0002, 0);
        public static readonly CCColliderFilter AllFilter = new CCColliderFilter(0xffff, 0xffff, 0);
        public static readonly CCColliderFilter NullFilter = new CCColliderFilter(0x0000, 0x0000, 0);
    }

    /// <summary>
    /// 和上面一一对应
    /// </summary>
    public enum FilterEnum
    {
        Enemy,
        Player,
        All,
        Null,
    }
}

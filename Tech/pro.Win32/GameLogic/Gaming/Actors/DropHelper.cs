using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Thunder.GameLogic.Gaming.Actors.Drops;

namespace Thunder.GameLogic.Gaming.Actors
{
    public class DropHelper:IDisposable
    {
        public struct DropTable
        {
            public DropType dropType;
            public int dropNum;
            public int dropNumVar;

            public DropTable(DropType type, int num, int numVar)
            {
                dropType = type;
                dropNum = num;
                dropNumVar = numVar;
            }
        }

        List<DropTable> dropHelper = new List<DropTable>();

        public bool IsWithProp;

        public DropHelper()
        {

        }
        public void Dispose()
        {
            dropHelper.Clear();
        }
    
        public void Add(DropTable table)
        {
            dropHelper.Add(table);
        }
        public void Add(DropType type, int num, int numVar)
        {
            dropHelper.Add(new DropTable(type, num, numVar));
            if (type == DropType.Drop_Bomb || type == DropType.Drop_Power || type == DropType.Drop_Shield || type == DropType.Drop_Skill)
            {
                IsWithProp = true;
            }
        }

        public void Play(Actor owner,DropConfig config)
        {
            foreach (var item in dropHelper)
            {
                DropManager.Instance.Drap(config,owner, item.dropType, item.dropNum, item.dropNumVar);
            }
        }
        public void PlayEnemy(Actor owner)
        {
            foreach (var item in dropHelper)
            {
                DropManager.Instance.DrapEnemy(owner, item.dropType, item.dropNum, item.dropNumVar);
            }
        }
        public void PlayBoss(Actor owner)
        {
            foreach (var item in dropHelper)
            {
                DropManager.Instance.DrapBoss(owner, item.dropType, item.dropNum, item.dropNumVar);
            }
        }
    }
}

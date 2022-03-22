using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistEditor
{
    public interface INodeDataCollection
    {
        /// <summary>
        /// Перемещает указанный в startPos объект в коллекцию, указанную в endPos
        /// </summary>
        /// <param name="startPos">Координаты перемещаемого объекта</param>
        /// <param name="endPos">Координаты новой коллекции</param>
        /// <param name="removeOld">Удалять ли перемещаемый объект в его исходном положении?</param>

        public void MoveChild(int[] startPos, int[] endPos, bool removeOld)
        {
            // получаем объект для перемещения/копирования
            INodeData? movingObject = PlaylistManager.GetInstance().GetObject(startPos);

            if (movingObject == null)
                return;

            // добавляем в конец указанной группы перемещаемый объект
            INodeDataCollection? newGroup = PlaylistManager.GetInstance().GetObject(endPos) as INodeDataCollection;
            newGroup?.AddChild(movingObject);

            // находим родительский объект для перемещаемого объекта и удаляем по указанному индексу перемещаемый объект, если нужно
            if (removeOld)
                PlaylistManager.GetInstance().GetParent(startPos)?.RemoveChild(startPos[^1]);
        }

        /// <summary>
        /// Внедряет объект из startPos на место объекта endPos, который смещается вниз
        /// </summary>
        /// <param name="startPos">Координаты перемещаемого объекта</param>
        /// <param name="endPos">Координаты нового места перемещаемого объекта</param>
        /// <param name="removeOld">Удалять ли перемещаемый объект в его исходном положении?</param>

        public string SwapChild(int[] startPos, int[] endPos, bool removeOld) {
            // получаем объект для перемещения/копирования
            INodeData? movingObject = PlaylistManager.GetInstance().GetObject(startPos);

            if (movingObject == null)
                return "NoObject";

            // находим родительский объект для перемещаемого объекта и удаляем по указанному индексу перемещаемый объект, если нужно
            if (removeOld)
                PlaylistManager.GetInstance().GetParent(startPos)?.RemoveChild(startPos[^1]);

            // добавляем в указанное место необходимый объект
            INodeDataCollection? newGroup = PlaylistManager.GetInstance().GetParent(endPos);
            newGroup?.AddChild(movingObject, endPos[^1]);
                     

            return "SuccessfullySwapped";
        }

        public void RemoveChild(int index);
        public void AddChild(INodeData element, int index = -1);
        public string AddChild();

    }
}

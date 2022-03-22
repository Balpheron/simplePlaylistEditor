using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistEditor
{
    internal static class TreeManager
    {
        // получение массива из индексов узла и всех его родительских узлов
        internal static int[] GetCoordinates(TreeNode targetNode)
        {
            // создаем массив длинной, равной глубине выбранного узла
            int[] coordinates = new int[targetNode.Level + 1];
            // записываем последовательно индексы узла и его родительских узлов
            int curStep = targetNode.Level;
            while (curStep >= 0)
            {
                coordinates[curStep] = targetNode.Index;
                try
                {
                    targetNode = targetNode.Parent;
                }
                catch (Exception)
                {
                    break;
                }
                curStep--;
            }

            return coordinates;
        }

        internal static string CoordinatesPrinter(int[] coords)
        {
            string result = "";
            foreach (var item in coords)
            {
                result += item.ToString() + " : ";
            }
            return result;
        }

        // пытаемся произвести манипуляции с перемещением элементов внутри коллекций
        // в случае успеха возвращается true
        internal static string PasteNode(ref TreeNode draggedNode, ref TreeNode targetNode, bool removeDragged)
        {
            TreeView treeView = draggedNode.TreeView;
            // получаем координаты переносимого и целевого объектов
            int[] draggedCoords = GetCoordinates(draggedNode);
            int[] targetCoords =  GetCoordinates(targetNode);
            // в зависимости от уровня глубины целевого узла:
            int levelDiff = draggedNode.Level - targetNode.Level;

            string statusMessage = "";
            
            // или добавляем в конец коллекции, если уровень на один выше
            if (levelDiff == 1)
            {
                INodeDataCollection? nodeDataCollection = (INodeDataCollection)PlaylistManager.GetInstance().GetObject(targetCoords);

                if (nodeDataCollection == null)
                    return $"NoObjectFound";

                nodeDataCollection.MoveChild(draggedCoords, targetCoords, true);
                MoveNode(ref draggedNode, ref targetNode, removeDragged);
                                
            }
            // или внедряем переносимый элемент вместо целевого элемента, а всю остальную коллекцию смещаем вниз
            else if (levelDiff == 0)
            {
                INodeDataCollection? nodeDataCollection = (INodeDataCollection)PlaylistManager.GetInstance().GetParent(targetCoords);

                if (nodeDataCollection == null)
                    return "NoObjectFound";

                statusMessage += nodeDataCollection.SwapChild(draggedCoords, targetCoords, true);
                SwapNodes(ref draggedNode, ref targetNode, removeDragged);

                return statusMessage;
            }
            return statusMessage;
        }

        // ставим один узел на место другого
        internal static void SwapNodes(ref TreeNode draggedNode, ref TreeNode targetNode, bool removeDragged)
        {
            // клонируем перемещаемый узел и вставляем его на место целевого
            TreeNode clondeNode = (TreeNode)draggedNode.Clone();
            int insertionIndex = targetNode.Index;
            // если переносим узел сверху вниз, то переносимый узел становится ниже целевого узла
            if (draggedNode.Index < insertionIndex && draggedNode.Parent.Index == targetNode.Parent.Index)
                insertionIndex++;
            targetNode.Parent.Nodes.Insert(insertionIndex, clondeNode);
            // удаляем перетаскиваемый узел
            if (removeDragged)
                draggedNode.Remove();
            // если нужно, то выделяем вставленную копию
            clondeNode.TreeView.SelectedNode = clondeNode;
        }

        // перемещаем узел в конец коллекции
        internal static void MoveNode(ref TreeNode draggedNode, ref TreeNode targetNode, bool removeDragged)
        {
            // перемещение узлов
            // если нужно, то удаляем исходный узел для перемещения
            if (removeDragged)
            {
                draggedNode.Remove();
                targetNode.Nodes.Add(draggedNode);
                draggedNode.TreeView.SelectedNode = draggedNode;
            }
            else
            {
                TreeNode clone = (TreeNode)draggedNode.Clone();
                targetNode.Nodes.Add(clone);
                clone.TreeView.SelectedNode = clone;
            }
            // если нужно, то выделяем вставленную копию
            
        }

        // удаляем информацию об узле
        internal static void DeleteNodeData(TreeNode targetNode)
        {
            int[] coords = GetCoordinates(targetNode);
            INodeDataCollection? nodeDataCollection = PlaylistManager.GetInstance().GetParent(coords);
            nodeDataCollection?.RemoveChild(coords[^1]);
            targetNode.Remove();
        }

        // добавление нового элемента в выделенную коллекцию
        internal static void AddElement(TreeNode? selected)
        {
            // если не выбран ни один элемент, добавляем новый плейлист
            string displayedName = "";
            if (selected == null)
            {
                displayedName = PlaylistManager.GetInstance().AddChild();
                Form1.instance.MainTree.Nodes.Add(displayedName);
            }
            // добавляем новый элемент в выбранную коллекцию
            else
            {
                int[] coords = GetCoordinates(selected);
                INodeDataCollection? nodeDataCollection = (INodeDataCollection)PlaylistManager.GetInstance().GetObject(coords);

                if (nodeDataCollection == null)
                    return;

                displayedName = nodeDataCollection.AddChild();
            }
            // добавляем узел в дерево
            selected?.Nodes.Add(displayedName);
        }

        internal static void RenameElement(TreeNode selected, string newName)
        {
            int[] coords = GetCoordinates(selected);
            PlaylistManager.GetInstance().GetObject(coords)?.Rename(newName);
        }
        // получаем узел дерева по указанным координатам
        internal static TreeNode GetNode(int[] coords)
        {
            TreeView tree = Form1.instance.MainTree;
            TreeNode result = tree.Nodes[coords[0]];
            int depth = 1;
            while(depth < coords.Length)
            {
                result = result.Nodes[coords[depth]];
                depth++;
            }
            return result;
        }
    }
}

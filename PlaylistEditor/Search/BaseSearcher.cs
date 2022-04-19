using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PlaylistEditor.Search
{
    internal class BaseSearcher : ISearcher
    {
        List<TreeNode> currentList = new List<TreeNode>();

        int currentIndex = -1;
        readonly int endLevel = 2;

        // перемещаем индекс вперед и возвращаем полученный элемент
        TreeNode? ISearcher.Next 
        { 
            get 
            { 
                if (currentList.Count == 0) return null;
                if (currentIndex < currentList.Count - 1) currentIndex++;
                else currentIndex = 0;
                return currentList[currentIndex]; 
            } 
        }

        // аналогично, но назад
        TreeNode? ISearcher.Previous
        {
            get
            {
                if (currentList.Count == 0) return null;
                if (currentIndex > 0) currentIndex--;
                else currentIndex = currentList.Count - 1;
                return currentList[currentIndex];
            }
        }

        int ISearcher.CurrentIndex
        {
            get { return currentIndex + 1; }
        }

        // получаем изначальную коллекцию для поиска
        async Task ISearcher.InitializeSearch(TreeNodeCollection initialNodes)
        {
            // сбрасываем индекс текущего элемента
            currentIndex = -1;

            await Task.Run(() =>
            {
                // очищаем пометку результатов поиска на предыдущем шаге
                foreach (var node in currentList)
                {
                    node.BackColor = Color.White;
                }
            });

            currentList = new List<TreeNode>();
            ComposeList(initialNodes);
        }

        // проходим по всему дереву вниз до уровня endlevel и добавляем элементы
        // с этого уровня в один список
        private void ComposeList(TreeNodeCollection node)
        {
            for (int childNode = 0; childNode < node.Count; childNode++)
            {
                if (node[childNode].Level < endLevel)
                    ComposeList(node[childNode].Nodes);
                else currentList.Add(node[childNode]);
                
            }
        }

        // находим в коллекции все элементы, содержащие заданную строку,
        // и возвращаем число найденных результатов
        async Task<int> ISearcher.Search(string searchText)
        {
           
            
            if (searchText == null || searchText.Length < 1)
                return 0;

            await Task.Run(() =>
            {
                // получаем результаты
                var searched = from nodes in currentList
                               where nodes.Text.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                               select nodes;


                currentList = searched.ToList();
                // помечаем полученные результаты
                foreach (var node in currentList)
                {
                    node.BackColor = Color.Yellow;
                }
            });

            // возвращаем количество найденных результатов
            return currentList.Count;
            
        }
    }
}

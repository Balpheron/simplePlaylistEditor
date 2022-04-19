using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistEditor.Search
{
    internal interface ISearcher
    {
        // интерфейс для поиска элементов в дереве

        // получение начального списка элементов для поиска
        internal Task InitializeSearch(TreeNodeCollection initialNodes);

        // возвращает индекс текущего элемента
        internal int CurrentIndex { get; }

        // асинхронно производит поиск внутри начального списка по заданному критерию
        // и возвращает число найденных элементов
        internal Task<int> Search(string searchText);

        // возвращает следующий элемент из отсеенного списки
        internal TreeNode? Next { get; }
        // ...предыдущий
        internal TreeNode? Previous { get; }
    }
}

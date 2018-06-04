using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UKPO2
{
    public class DNAGraph
    {
        //Класс вершины графа
        public class Verticle
        {
            String fragment;//Значение
            List<Verticle> neighbours;//Смежные вершины
            int indexInMolecule;

            public Verticle(String fragment) //Свойство для списка вершин
            {
                this.fragment = fragment;
                this.neighbours = new List<Verticle>();
            }

            public String Fragment //Свойство для значения фрагмента
            {
                get { return this.fragment; }
                set { this.fragment = value; }
            }

            public int Index
            {
                get { return this.indexInMolecule; }
                set { this.indexInMolecule = value; }
            }
            //public List<Verticle> Neighbours
            //{
            //    get { return this.neighbours; }
            //}

            public List<Verticle> GetNeighbours() //Получение ссылки на список
            {
                return this.neighbours;
            }

            public void AddNeighbour(List<Verticle> list, int index)//Запись в список ссылки на значение из списка list
            {
                this.neighbours.Add(list[index]);
            }
        }

        String originMolecule; //Изначальная молекула
        List<Verticle> verticleList;//Фрагменты молекулы

        public List<Verticle> VerticleList //Свойсто для получения списка фрагментов/вершин графа
        {
            get
            {
                return verticleList;
            }

        }

        public DNAGraph(String originMolecule, String[] fragments)//Принимает молекулу и фрагменты
        {
            this.originMolecule = originMolecule;
            // BUG INJECTION
            //if(fragments.Length != 0)
            verticleList = new List<Verticle>();
            //Добавление фрагментов
            foreach (var fragment in fragments)
            {
                // BUG INJECTION
                if(IsUnique(fragment))
                    verticleList.Add(new Verticle(fragment));
            }
            //Расставление связей в графе
            SetVerticlesIndexes();
            SetVerticleRelations();
        }

        public void SetVerticlesIndexes()
        {
            foreach (var verticle in verticleList)
            {
                verticle.Index = originMolecule.IndexOf(verticle.Fragment);
            }
        }

        public List<List<String>> GetPaths()//Получение списка путей в графе, по которым можно составить молекулу
        {
            var beginningIndexes = BegginingIndexes();//Получение индексов вершин, из которых имеет смысл
            //начинать поиск пути - фрагменты стоят в начале молекулы
            var possiblePaths = new List<List<String>>();
            //Добавление в список путей
            foreach (var index in beginningIndexes)
            {
                AddPath(new List<string>(), verticleList[index], possiblePaths);
            }

            return possiblePaths;
        }

        private bool IsUnique(String fragment)//Проверяет уникальность фрагмента
        {
            if (verticleList.Count != 0)
            {
                foreach (var verticle in verticleList)
                {
                    if (verticle.Fragment == fragment)
                        return false;
                }
            }
            return true;
        }

        //Принимает фрагменты и проверяет, возможна ли между ними связь. Фрагменты могут накладываться
        private bool IsPostfix(String thisFragment, String neighbourFragment)
        {
            //Получает индекс текущего фрагмента в молекуле
            var indexInOrigin = originMolecule.IndexOf(thisFragment);//Берет индекс первого вхождения фрагмента
            if (indexInOrigin == -1) return false;

            //Сравнивает данный фрагмент с вероятным соседом начиная со второго символа по порядку
            //Например ААЦГ начальный фрагмент. Смежные фрагмент должны стоять справа. Молекула ААЦГУУААЦЦ.
            //Подходят: АЦГГУ, ЦГУУА, ГУУ. Не подходит ААЦГУ, т.к. он не следует за данным фрагментом, а начинается на той же позиции
            for (var i = indexInOrigin + 1; 
                ((i + neighbourFragment.Length) <= (originMolecule.Length)) 
                && (i <= indexInOrigin + thisFragment.Length); ++i)
            {
                if (originMolecule[i] == neighbourFragment[0])
                {
                    int j;
                    for (j = 1; j < neighbourFragment.Length; ++j)
                    {
                            if (originMolecule[i + j] != neighbourFragment[j])
                                break;
                    }
                    //Если прошли по всему фрагменту и не возвратили фолс - фрагмент является смежным
                    if (j == neighbourFragment.Length)
                        return true;
                }
            }
            return false;
        }
        //Расставляет отношения между вершинами
        private void SetVerticleRelations()
        {
            for (int i = 0; i < verticleList.Count; ++i)
            {
                for (int j = 0; j < verticleList.Count; ++j)
                {
                    if (i != j)
                    {
                        if (verticleList[j].Index > verticleList[i].Index)
                        {
                            if (IsPostfix(verticleList[i].Fragment, verticleList[j].Fragment))
                            {
                                verticleList[i].AddNeighbour(verticleList, j);
                            }
                        }
                    }
                }
            }
        }

        private List<int> BegginingIndexes()
        {
            List<int> beginningIndexes = new List<int>();
            for (var i = 0; i < verticleList.Count; ++i)
            {
                //Если индекс фрагмента в молекуле = 0, фрагмент может начинать путь - записываем индекс фрагмента в список
                if (originMolecule.IndexOf(verticleList[i].Fragment) == 0)
                    beginningIndexes.Add(i);
            }

            if (beginningIndexes.Count != 0)
                return beginningIndexes;
            else
                return null;
        }
        //currentPath путь при текущей итерации. При вызове метода берется пустой список
        //verticle вершина, которая может начинать путь
        //paths список, в который будут добавляться пути
        private void AddPath(List<String> currentPath, Verticle verticle, List<List<String>> paths)
        {
            //Добавляем фрагмент в путь
            currentPath.Add(verticle.Fragment);
            //Получаем смежные фрагменты в виде списка
            var neighbours = verticle.GetNeighbours();

            if (neighbours.Count == 0) //Если смежных нет
            {
                //Если при этом фрагмент не стоит на завершающей позиции в молекуле
                if ((originMolecule.IndexOf(verticle.Fragment) + verticle.Fragment.Length) != (originMolecule.Length))
                    return;
                else
                {
                    // BUG INJECTION
                    //Если фрагмент завершает молекулу - путь найден и добавляется в список путей
                    paths.Add(currentPath);
                    return;
                }
            }

            for (var i = 0; i < neighbours.Count; ++i)
            {
                //Рекурсивно вызывается для всех смежных вершин
                //Создается копия currentPath
                AddPath(new List<String>(currentPath), neighbours[i], paths);
                // BUG INJECTION
                //AddPath(currentPath, neighbours[i], paths);
            }
        }
    }
}
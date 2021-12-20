using UnityEngine;
/*
 *  @brief: Класс генерации массива лабиринта
 */
public class MazeDataGenerator
{
    public float placementThreshold;

    public MazeDataGenerator()
    {
        placementThreshold = .1f;
    }

    int CELL = 0;
    int WALL = 1;
    int VISITED = 2;
    class Cell : System.ICloneable
    {
        public int x;
        public int y;
        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Cell()
        {
            this.x = 0;
            this.y = 0;
        }

        public object Clone()
        {
            return new Cell { x = this.x, y = this.y };
        }
    };

    class Stack
    {
        Cell[] cells;
        int countStack = 0;
        public Stack(int size)
        {
            cells = new Cell[size];
        }
        public int GetStackSize()
        {
            return countStack;
        }

        public bool push(Cell cell)
        {
            if (countStack == cells.Length)
                return false;
            cells[countStack] = cell;
            countStack++;
            return true;
        }
        public Cell pop()
        {
            if (countStack == 0)
                return new Cell(0, 0);
            Cell temp = (Cell)cells[countStack - 1].Clone();
            cells[countStack - 1] = null;
            countStack--;
            return temp;
        }
    };

    class CellString
    {
        public Cell[] cells;
        public int size;
    };

    public int[,] FromDimensions(int sizeRows, int sizeCols)
    {
        Stack stack = new Stack(sizeCols * sizeRows);

        int[,] maze = new int[sizeRows, sizeCols]; //создаем матрицу - двумерный массив
        for (int i = 0; i < sizeRows; i++)
        {
            for (int j = 0; j < sizeCols; j++)
            {
                if ((i % 2 != 0 && j % 2 != 0) && //если ячейка нечетная по x и y, 
                   (i < sizeRows - 1 && j < sizeCols - 1))   //и при этом находится в пределах стен лабиринта
                    maze[i, j] = CELL;       //то это КЛЕТКА
                else maze[i, j] = WALL;           //в остальных случаях это СТЕНА.
            }
        }

        Cell currentCell = new Cell(1, 1);
        maze[currentCell.x, currentCell.y] = VISITED;
        Cell neighbourCell;
        do
        {
            CellString Neighbours = GetNeigh(sizeCols, sizeRows, maze, currentCell);

            if (Neighbours.size != 0)
            { //если у клетки есть непосещенные соседи
                int randNum = Random.Range(0, Neighbours.size);
                neighbourCell = Neighbours.cells[randNum]; //выбираем случайного соседа
                stack.push(currentCell); //заносим текущую точку в стек
                maze = removeWall(currentCell, neighbourCell, maze); //убираем стену между текущей и сосендней точками
                currentCell = neighbourCell; //делаем соседнюю точку текущей и отмечаем ее посещенной
                maze[currentCell.x, currentCell.y] = VISITED;
            }
            else if (stack.GetStackSize() > 0)
            { //если нет соседей, возвращаемся на предыдущую точку
                currentCell = stack.pop();
            }
            else
            { //если нет соседей и точек в стеке, но не все точки посещены, выбираем случайную из непосещенных
                currentCell = getUnvisitedCells(sizeCols, sizeRows, maze);
                if (currentCell == null)
                    break;
            }
        }
        while (unvisitedCount(sizeRows, sizeCols, maze) > 0);
        mazeRevers(maze, sizeRows, sizeCols);
        return maze;

    }


    int unvisitedCount(int sizeRows, int sizeCols, int[,] maze)
    {
        int cnt = 0;
        for (int i = 1; i < sizeCols; i++)
            for (int j = 1; j < sizeRows; j++)
            {
                if (maze[j, i] == CELL)
                    cnt++;
            }
        return cnt;
    }
    CellString GetNeigh(int width, int height, int[,] maze, Cell c)
    {
        int x = c.x;
        int y = c.y;
        Cell up = new Cell(x + 2, y);
        Cell rt = new Cell(x, y + 2);
        Cell dw = new Cell(x - 2, y);
        Cell lt = new Cell(x, y - 2);
        Cell[] d = { dw, rt, up, lt };
        int size = 0;

        CellString cells = new CellString();
        cells.cells = new Cell[4];


        for (int i = 0; i < 4; i++)
        { //для каждого направдения
            if (d[i].x > 0 && d[i].x < height && d[i].y > 0 && d[i].y < width)
            { //если не выходит за границы лабиринта
                int mazeCellCurrent = maze[d[i].x, d[i].y];
                Cell cellCurrent = d[i];
                if (mazeCellCurrent != WALL && mazeCellCurrent != VISITED)
                { //и не посещена\является стеной
                    cells.cells[size] = cellCurrent; //записать в массив;
                    size++;
                }
            }
        }
        cells.size = size;
        return cells;
    }

    int[,] removeWall(Cell first, Cell second, int[,] maze)
    {
        int xDiff = second.x - first.x;
        int yDiff = second.y - first.y;
        int addX, addY;
        Cell target = new Cell();

        addX = (xDiff != 0) ? (xDiff / Mathf.Abs(xDiff)) : 0;
        addY = (yDiff != 0) ? (yDiff / Mathf.Abs(yDiff)) : 0;

        target.x = first.x + addX; //координаты стенки
        target.y = first.y + addY;

        maze[target.x, target.y] = VISITED;
        return maze;
    }

    Cell getUnvisitedCells(int sizeCols, int sizeRows, int[,] maze)
    {
        for (int i = 1; i < sizeCols; i++)
            for (int j = 1; j < sizeRows; j++)
            {
                if (maze[j, i] == CELL)
                {
                    return new Cell(i, j);

                }
            }
        return null;
    }

    void mazeRevers(int[,] maze, int row, int col)
    { 
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (maze[i, j] == 2)
                {
                    maze[i, j] = 0;
                }
            }
        }
    }

}

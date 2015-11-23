using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

/// <summary>
/// Класс игрового поля
/// </summary>
public class Board : MonoBehaviour
{
    #region properties

    /// <summary>
    /// Ширина игрового поля
    /// </summary>
    public int width = 8;
    /// <summary>
    /// Высота игрового поля
    /// </summary>
    public int height = 8;

    public Piece piecePrefab;
    /// <summary>
    /// Главная камера
    /// </summary>
    public Transform cam;

    Piece[,] _grid;

    Transform _transform;

    #endregion

    /// <summary>
    /// Возвращает массив со всеми фишками
    /// </summary>
    /// <returns></returns>
    public Piece[,] GetPieces()
    {
        return _grid;
    }

    #region Swapping

    /// <summary>
    /// Совершить ход
    /// </summary>
    /// <param name="first">Первая фишка</param>
    /// <param name="second">Вторая фишка</param>
    public IEnumerator Swap(Piece first, Piece second)
    {
        MakeSwap(first, second);
        AnimationController.instance.AddSwaped(first);
        AnimationController.instance.AddSwaped(second);
        yield return StartCoroutine(AnimationController.instance.SwapAnimation());

        List<List<Piece>> firstMatches = FindMatches((int)first.coor.x, (int)first.coor.y);
        List<List<Piece>> secondMatches = FindMatches((int)second.coor.x, (int)second.coor.y);

        if (firstMatches.Count == 0 && secondMatches.Count == 0)
        {
            MakeSwap(second, first);
            AnimationController.instance.AddSwaped(first);
            AnimationController.instance.AddSwaped(second);
            yield return StartCoroutine(AnimationController.instance.SwapAnimation());
        }
        else
        {
            List<Piece> firstRemoved = RemoveMatches(firstMatches);
            List<Piece> secondRemoved = RemoveMatches(secondMatches);
            yield return StartCoroutine(AnimationController.instance.RemoveAnimation());

            LetDownPieces(firstRemoved);
            LetDownPieces(secondRemoved);
            yield return StartCoroutine(AnimationController.instance.LetDownAnimation());

            AddNewPieces(firstRemoved);
            AddNewPieces(secondRemoved);
            yield return StartCoroutine(AnimationController.instance.FillingAnimation());

            yield return StartCoroutine(StartCyrcle());
        }
    }

    /// <summary>
    /// Меняет местами фишки
    /// </summary>
    /// <param name="first">первая фишка</param>
    /// <param name="second">вторая фишка</param>
    void MakeSwap(Piece first, Piece second)
    {
        _grid[(int)first.coor.x, (int)first.coor.y] = second;
        _grid[(int)second.coor.x, (int)second.coor.y] = first;

        Vector2 buffer = first.coor;
        first.coor = second.coor;
        second.coor = buffer;
    }

    /// <summary>
    /// Ищет линии по всему полю
    /// </summary>
    /// <returns>Список линий</returns>
    List<List<Piece>> FindMatches()
    {
        List<List<Piece>> matches = new List<List<Piece>>();

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width - 2; col++)
            {
                List<Piece> match = FindHorizMatch(col, row);
                if (match.Count > 2)
                {
                    matches.Add(match);
                    col += match.Count - 1;
                }
            }
        }

        for (int col = 0; col < width; col++)
        {
            for (int row = 0; row < height - 2; row++)
            {
                List<Piece> match = FindVertMatch(col, row);
                if (match.Count > 2)
                {
                    matches.Add(match);
                    row += match.Count - 1;
                }
            }
        }

        return matches;
    }

    /// <summary>
    /// Ищет линии в заданном ряду и столбце
    /// </summary>
    /// <param name="col">столбец</param>
    /// <param name="row">ряд</param>
    /// <returns>список линий</returns>
    List<List<Piece>> FindMatches(int col, int row)
    {
        List<List<Piece>> matches = new List<List<Piece>>();

        for (int x = 0; x < width - 2; x++)
        {
            List<Piece> match = FindHorizMatch(x, row);
            if (match.Count > 2)
            {
                matches.Add(match);
                x += match.Count - 1;
            }
        }

        for (int y = 0; y < height - 2; y++)
        {
            List<Piece> match = FindVertMatch(col, y);
            if (match.Count > 2)
            {
                matches.Add(match);
                y += match.Count - 1;
            }
        }

        return matches;
    }

    /// <summary>
    /// ищет линии в заданном ряду
    /// </summary>
    /// <param name="col">столбец</param>
    /// <param name="row">ряд</param>
    /// <returns>список линий</returns>
    List<Piece> FindHorizMatch(int col, int row)
    {
        List<Piece> match = new List<Piece>();
        match.Add(_grid[col, row]);

        for (int i = 1; col + i < width; i++)
        {
            if (_grid[col, row].Jewel == _grid[col + i, row].Jewel)
                match.Add(_grid[col + i, row]);
            else
                return match;
        }

        return match;
    }

    /// <summary>
    /// ищет линии в заданном столбце
    /// </summary>
    /// <param name="col">столбец</param>
    /// <param name="row">ряд</param>
    /// <returns>список линий</returns>
    List<Piece> FindVertMatch(int col, int row)
    {
        List<Piece> match = new List<Piece>();
        match.Add(_grid[col, row]);

        for (int i = 1; row + i < height; i++)
        {
            if (_grid[col, row].Jewel == _grid[col, row + i].Jewel)
                match.Add(_grid[col, row + i]);
            else
                return match;
        }

        return match;
    }

    /// <summary>
    /// Цикл поиска и удаления возникающих на поле линий
    /// </summary>
    IEnumerator StartCyrcle()
    {
        while (true)
        {
            List<List<Piece>> matches = FindMatches();
            if (matches.Count != 0)
            {
                List<Piece> removed = RemoveMatches(matches);
                yield return StartCoroutine(AnimationController.instance.RemoveAnimation());

                LetDownPieces(removed);
                yield return StartCoroutine(AnimationController.instance.LetDownAnimation());

                AddNewPieces(removed);
                yield return StartCoroutine(AnimationController.instance.FillingAnimation());
            }
            else
                yield break;
        }
    }

    List<Piece> RemoveMatches(List<List<Piece>> matches)
    {
        List<Piece> ret = new List<Piece>();

        foreach (List<Piece> match in matches)
        {
            foreach (Piece piece in match)
            {
                AnimationController.instance.AddRemoved(piece);
                ret.Add(piece);
            }
        }

        ret.Sort(delegate(Piece x, Piece y)
        {
            if (x.coor.y > y.coor.y) return -1;
            if (x.coor.y < y.coor.y) return 1;
            return 0;
        });

        return ret;
    }

    void LetDownPieces(List<Piece> list)
    {
        foreach (Piece piece in list)
        {
            int x = (int)piece.coor.x;

            for (int y = (int)piece.coor.y; y < height - 1; y++)
            {
                MakeSwap(_grid[x, y], _grid[x, y + 1]);
                AnimationController.instance.AddFalled(_grid[x, y]);
            }
        }
    }

    void AddNewPieces(List<Piece> list)
    {
        foreach (Piece piece in list)
        {
            AnimationController.instance.AddFilled(piece);
        }
    }

    #endregion

    #region StartGame

    void Awake () {
        _transform = GetComponent<Transform>();
        _grid = new Piece[width, height];
        InitBoard();
        CenterCamera();
        AnimationController.instance.SetHeight(height);
	}
	
    void Start()
    {
        SetFirstTurn();
        FillingPieces();
        CheckForMatches();
    }

    /// <summary>
    /// Центрирует камеру на игровом поле.
    /// </summary>
    void CenterCamera()
    {
        Vector3 pos = new Vector3((float)width/2 - 0.5f, (float)height/2 - 0.5f, -10);
        cam.position = pos;
    }

    /// <summary>
    /// Заполняет игровое поле фишками
    /// </summary>
    void InitBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Piece instance = Instantiate(piecePrefab, new Vector3(x, y, 0), Quaternion.identity) as Piece;
                instance.transform.SetParent(_transform);
                instance.coor = new Vector2(x, y);
                _grid[x, y] = instance;
            }
        }
    }

    /// <summary>
    /// Ищет ряды среди всех фишек на поле и меняет фишки так, чтобы удалить ряды.
    /// </summary>
    void CheckForMatches()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height - 2; y++)
            {
                if (_grid[x, y].Jewel == _grid[x, y+1].Jewel &&
                    _grid[x, y].Jewel == _grid[x, y + 2].Jewel)
                {
                    Jewels.RemoveColor(_grid[x, y].Jewel);

                    if(x > 0)
                        Jewels.RemoveColor(_grid[x - 1, y].Jewel);

                    if(x < width - 1)
                        Jewels.RemoveColor(_grid[x + 1, y].Jewel);

                    if (y > 0)
                        Jewels.RemoveColor(_grid[x, y - 1].Jewel);

                    _grid[x, y].Jewel = Jewels.GetRandomColor();
                    Jewels.SetupColors();
                }
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width - 2; x++)
            {
                if (_grid[x, y].Jewel == _grid[x + 1, y].Jewel &&
                    _grid[x, y].Jewel == _grid[x + 2, y].Jewel)
                {
                    Jewels.RemoveColor(_grid[x, y].Jewel);

                    if (x > 0)
                        Jewels.RemoveColor(_grid[x - 1, y].Jewel);

                    if (y < height - 1)
                        Jewels.RemoveColor(_grid[x, y + 1].Jewel);

                    if (y > 0)
                        Jewels.RemoveColor(_grid[x, y - 1].Jewel);

                    _grid[x, y].Jewel = Jewels.GetRandomColor();
                    Jewels.SetupColors();
                }
            }
        }
    }

    /// <summary>
    /// Устанавливает паттерн на игровом поле для возможности первого хода
    /// </summary>
    void SetFirstTurn()
    {
        int x = Random.Range(2, width - 2);
        int y = Random.Range(2, height - 2);
        int pattern = Random.Range(1, 8);

        switch (pattern)
        {
            case 1:
                CreatePattern(new Vector2(x, y), Vector2.left, new Vector2[] { Vector2.right * 2, Vector2.one, new Vector2(1, -1) });
                break;
            case 2:
                CreatePattern(new Vector2(x, y), Vector2.right, new Vector2[] { Vector2.left * 2, new Vector2(-1, -1), new Vector2(-1, 1) });
                break;
            case 3:
                CreatePattern(new Vector2(x, y), Vector2.up, new Vector2[] { Vector2.down * 2, new Vector2(1, -1), new Vector2(-1, -1) });
                break;
            case 4: 
                CreatePattern(new Vector2(x, y), Vector2.down, new Vector2[] { Vector2.up * 2, new Vector2(-1, 1), new Vector2(1, 1) });
                break;
            case 5:
                CreatePattern(new Vector2(x, y), Vector2.left * 2, new Vector2[] { new Vector2(-1, 1), new Vector2(-1, -1) });
                break;
            case 6:
                CreatePattern(new Vector2(x, y), Vector2.right * 2, new Vector2[] { new Vector2(1, 1), new Vector2(1, -1) });
                break;
            case 7:
                CreatePattern(new Vector2(x, y), Vector2.up * 2, new Vector2[] { new Vector2(-1, 1), new Vector2(1, 1) });
                break;
            case 8:
                CreatePattern(new Vector2(x, y), Vector2.down * 2, new Vector2[] { new Vector2(-1, -1), new Vector2(1, -1) });
                break;
        }
    }

    /// <summary>
    /// Создаёт паттерн на игровом поле
    /// </summary>
    /// <param name="origin">Некотарая клетка игрового поля</param>
    /// <param name="necessary">Необходимая в паттерне фишка</param>
    /// <param name="enough">Массив возможных, но не обязательных фишек</param>
    void CreatePattern(Vector2 origin, Vector2 necessary, Vector2[] enough)
    {
        JewelsEnum color = _grid[(int)origin.x, (int)origin.y].SetRandomColor();
        _grid[(int)(origin.x + necessary.x), (int)(origin.y + necessary.y)].Jewel = color;
        int i = Random.Range(0, enough.Length - 1);
        Vector2 concretEnough = enough[i];
        _grid[(int)(origin.x + concretEnough.x), (int)(origin.y + concretEnough.y)].Jewel = color;
    }

    /// <summary>
    /// Сравнивает цвета фишек
    /// </summary>
    /// <param name="origin">первая фишка</param>
    /// <param name="compared">Координаты второй фишки относительно первой</param>
    /// <returns>true если цвета совпадают</returns>
    bool ComparePieces(Piece origin, Vector2 compared)
    {
        if (CheckPieceOnBoard(origin, compared) && 
            origin.Jewel == _grid[(int)(origin.coor.x + compared.x), (int)(origin.coor.y + compared.y)].Jewel)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Заполняет свободные клетки поля
    /// </summary>
    void FillingPieces()
    {
        for (int y = height - 1; y >= 0; y--)
        {
            bool br = true;

            for (int x = 0; x < width; x++)
            {
                if (_grid[x, y].Jewel == JewelsEnum.None)
                {
                    br = false;
                    _grid[x, y].SetRandomColor();
                }
            }

            if (br)
                break;
        }
    }

    #endregion
    
    /// <summary>
    /// Проверяет возможность совершения хода
    /// </summary>
    /// <param name="firstClick">Первая фишка</param>
    /// <param name="piece">Вторая фишка</param>
    /// <returns>true если ход возможен</returns>
    public static bool CheckForSwap(Piece firstClick, Piece piece)
    {
        if (firstClick.coor + Vector2.up == piece.coor ||
            firstClick.coor + Vector2.down == piece.coor ||
            firstClick.coor + Vector2.left == piece.coor ||
            firstClick.coor + Vector2.right == piece.coor)
            return true;

        return false;
    }

    /// <summary>
    /// Проверяет выход за пределы игрового поля
    /// </summary>
    /// <param name="origin">первая фишка</param>
    /// <param name="compared">координаты второй фишки относительно первой</param>
    /// <returns></returns>
    bool CheckPieceOnBoard(Piece origin, Vector2 compared)
    {
        if (origin.coor.x + compared.x < 0 ||
            origin.coor.x + compared.x >= width ||
            origin.coor.y + compared.y < 0 ||
            origin.coor.y + compared.y >= width)
            return false;

        return true;
    }
}

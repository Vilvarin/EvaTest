using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {
    public int width = 8;
    public int height = 8;

    public Piece piecePrefab;
    public Transform cam;

    Piece[,] _pieces;

    Transform _transform;

    public Piece GetPiece(int x, int y){
        return _pieces[x, y];
    }

    public Piece[,] GetPieces()
    {
        return _pieces;
    }

	void Awake () {
        _transform = GetComponent<Transform>();
        _pieces = new Piece[width, height];
        InitBoard();
        CenterCamera();
	}
	
    void InitBoard()
    {
        AddPieces();

        while (true)
        {
            if (!CheckForMatches() && CheckForFirstTurn()) break;
            ReloadPieces();
        }
    }

    void ReloadPieces()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _pieces[x, y].SetRandomPiece();
            }
        }
    }

    void CenterCamera()
    {
        Vector3 pos = new Vector3((float)width/2 - 0.5f, (float)height/2 - 0.5f, -10);
        cam.position = pos;
    }

    void AddPieces()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Piece instance = Instantiate(piecePrefab, new Vector3(x, y, 0), Quaternion.identity) as Piece;
                instance.transform.SetParent(_transform);
                instance.coor = new Vector2(x, y);
                instance.SetRandomPiece();
                _pieces[x, y] = instance;
            }
        }
    }

    bool CheckForMatches()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height - 2; y++)
            {
                if (_pieces[x, y].jewel == _pieces[x, y+1].jewel && 
                    _pieces[x, y].jewel == _pieces[x, y+2].jewel) 
                    return true;
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width - 2; x++)
            {
                if (_pieces[x, y].jewel == _pieces[x + 1, y].jewel && 
                    _pieces[x, y].jewel == _pieces[x + 2, y].jewel) 
                    return true;
            }
        }

        return false;
    }

    bool CheckForFirstTurn()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (
                    FindPattern(_pieces[x, y], Vector2.left, new Vector2[] { Vector2.right * 2, Vector2.one, new Vector2(1, -1) })         ||
                    FindPattern(_pieces[x, y], Vector2.right, new Vector2[] { Vector2.left * 2, new Vector2(-1, -1), new Vector2(-1, 1) }) ||
                    FindPattern(_pieces[x, y], Vector2.up, new Vector2[] { Vector2.down * 2, Vector2.one, new Vector2(-1, 1) })            ||
                    FindPattern(_pieces[x, y], Vector2.down, new Vector2[] { Vector2.up * 2, new Vector2(-1, -1), new Vector2(1, -1) })    ||
                    FindPattern(_pieces[x, y], Vector2.left * 2, new Vector2[] { new Vector2(-1, 1), new Vector2(-1, -1) })                ||
                    FindPattern(_pieces[x, y], Vector2.right * 2, new Vector2[] { new Vector2(1, 1), new Vector2(1, -1) })                 ||
                    FindPattern(_pieces[x, y], Vector2.up * 2, new Vector2[] { new Vector2(-1, 1), new Vector2(1, 1) })                    ||
                    FindPattern(_pieces[x, y], Vector2.down * 2, new Vector2[] { new Vector2(-1, -1), new Vector2(1, -1) })
                  )
                    return true;
            }
        }

        return false;
    }

    bool FindPattern(Piece origin, Vector2 necessary, Vector2[] enough)
    {
        if (ComparePieces(origin, necessary))
        {
            foreach (Vector2 vector in enough)
            {
                if (ComparePieces(origin, vector))
                    return true;
            }
        }

        return false;
    }

    bool ComparePieces(Piece origin, Vector2 compared)
    {
        if (origin.coor.x + compared.x < 0      ||
            origin.coor.x + compared.x >= width ||
            origin.coor.y + compared.y < 0      || 
            origin.coor.y + compared.y >= width)
            return false;

        if (origin.jewel == _pieces[(int)(origin.coor.x + compared.x), (int)(origin.coor.y + compared.y)].jewel)
            return true;

        return false;
    }

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

    List<Piece> FindHorizMatch(int col, int row)
    {
        List<Piece> match = new List<Piece>();
        match.Add(_pieces[col, row]);

        for (int i = 1; col + i < width; i++)
        {
            if (_pieces[col, row].jewel == _pieces[col + i, row].jewel)
                match.Add(_pieces[col + i, row]);
            else
                return match;
        }

        return match;
    }

    List<Piece> FindVertMatch(int col, int row)
    {
        List<Piece> match = new List<Piece>();
        match.Add(_pieces[col, row]);

        for (int i = 1; row + i < height; i++)
        {
            if (_pieces[col, row].jewel == _pieces[col, row + i].jewel)
                match.Add(_pieces[col, row + i]);
            else
                return match;
        }

        return match;
    }

    public static bool CheckForSwap(Piece firstClick, Piece piece)
    {
        if (firstClick.coor + Vector2.up == piece.coor   ||
            firstClick.coor + Vector2.down == piece.coor ||
            firstClick.coor + Vector2.left == piece.coor ||
            firstClick.coor + Vector2.right == piece.coor)
            return true;

        return false;
    }

    public void Swap(Piece first, Piece second)
    {
        MakeSwap(first, second);

        List<List<Piece>> FirstMatches = FindMatches((int)first.coor.x, (int)first.coor.y);
        List<List<Piece>> SecondMatches = FindMatches((int)second.coor.x, (int)second.coor.y);

        if (FirstMatches.Count == 0 && SecondMatches.Count == 0)
        {
            MakeSwap(second, first);
        }
        else
        {
            first.UpdateJewel();
            second.UpdateJewel();
            RemoveMatches(FirstMatches);
            RemoveMatches(SecondMatches);
            FillingPieces();
        }

        StartCyrcle();
    }

    void StartCyrcle()
    {
        while (true)
        {
            List<List<Piece>> matches = FindMatches();
            if (matches.Count != 0)
            {
                RemoveMatches(matches);
                FillingPieces();
            }
            else
                break;
        }
    }

    void MakeSwap(Piece first, Piece second)
    {
        JewelsEnum buffer = first.jewel;
        first.jewel = second.jewel;
        second.jewel = buffer;
    }

    void RemoveMatches(List<List<Piece>> matches)
    {
        foreach (List<Piece> match in matches)
        {
            for (int i = match.Count - 1; i >= 0; i--)
            {
                match[i].jewel = JewelsEnum.None;
                LetDownPieces(match[i]);
            }
        }
    }

    void LetDownPieces(Piece piece)
    {
        int x = (int)piece.coor.x;
        for (int y = (int)piece.coor.y; y < height - 1; y++)
        {
            _pieces[x, y].jewel = _pieces[x, y + 1].jewel;
            _pieces[x, y].UpdateJewel();
        }

        _pieces[x, height - 1].jewel = JewelsEnum.None;
        _pieces[x, height - 1].UpdateJewel();
    }

    void FillingPieces()
    {
        for (int y = height - 1; y >= 0; y--)
        {
            bool br = true;

            for (int x = 0; x < width; x++)
            {
                if (_pieces[x, y].jewel == JewelsEnum.None)
                {
                    br = false;
                    _pieces[x, y].SetRandomPiece();
                    _pieces[x, y].UpdateJewel();
                }
            }

            if (br)
                break;
        }
    }
}

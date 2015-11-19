using UnityEngine;
using System;
using System.Collections;

public class Piece: MonoBehaviour
{
    public Vector2 coor;
    public JewelsEnum jewel;

    public event EventHandler<PiecesEventArgs> clickAction = delegate { };

    SpriteRenderer _render;

    void Start()
    {
        _render = GetComponent<SpriteRenderer>();
        UpdateJewel();
    }

    void OnMouseDown()
    {
        clickAction(this, new PiecesEventArgs(this));
    }

    public void AddSelected()
    {
        _render.color = new Color(_render.color.r, _render.color.g, _render.color.b, 0.5f);
    }

    public void RemoveSelected()
    {
        _render.color = new Color(_render.color.r, _render.color.g, _render.color.b, 1f);
    }

    public void SetRandomPiece()
    {
        jewel = (JewelsEnum)UnityEngine.Random.Range(1, Enum.GetNames(typeof(JewelsEnum)).Length);
    }

    public void UpdateJewel()
    {
        switch (jewel)
        {
            case JewelsEnum.None:
                _render.sprite = null;
                break;

            case JewelsEnum.Black:
                _render.sprite = JewelsStorage.instance.blackJewel;
                break;

            case JewelsEnum.Blue:
                _render.sprite = JewelsStorage.instance.blueJewel;
                break;

            case JewelsEnum.Green:
                _render.sprite = JewelsStorage.instance.greenJewel;
                break;

            case JewelsEnum.Pink:
                _render.sprite = JewelsStorage.instance.pinkJewel;
                break;

            case JewelsEnum.Red:
                _render.sprite = JewelsStorage.instance.redJewel;
                break;

            case JewelsEnum.Silver:
                _render.sprite = JewelsStorage.instance.silverJewel;
                break;

            case JewelsEnum.Yellow:
                _render.sprite = JewelsStorage.instance.yellowJewel;
                break;

            default:
                throw new System.InvalidOperationException("Задано незарегистрированное значение для спрайта кристалла");
        }
    }
}

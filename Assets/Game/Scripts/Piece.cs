using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Класс фишки\тайла игрового поля
/// </summary>
public class Piece: MonoBehaviour
{
    /// <summary>
    /// Координаты фишки на игровом поле
    /// </summary>
    public Vector2 coor;

    private JewelsEnum _jewel;
    /// <summary>
    /// Тип фишки
    /// </summary>
    public JewelsEnum Jewel
    {
        get
        {
            return _jewel;
        }

        set
        {
            _jewel = value;
            UpdateJewel();
        }
    }

    public event EventHandler<PiecesEventArgs> clickAction = delegate { };

    SpriteRenderer _render;
    Transform _transform;

    void Awake()
    {
        _render = GetComponent<SpriteRenderer>();
        _transform = GetComponent<Transform>();
        _render.sprite = null;
    }

    void OnMouseDown()
    {
        clickAction(this, new PiecesEventArgs(this));
    }

    /// <summary>
    /// Выделяет фишку при нажатии
    /// </summary>
    public void AddSelected()
    {
        _render.color = new Color(_render.color.r, _render.color.g, _render.color.b, 0.5f);
    }

    /// <summary>
    /// убирает выделение
    /// </summary>
    public void RemoveSelected()
    {
        _render.color = new Color(_render.color.r, _render.color.g, _render.color.b, 1f);
    }

    /// <summary>
    /// Задает случайный цвет
    /// </summary>
    /// <returns>Заданный цвет</returns>
    public JewelsEnum SetRandomColor()
    {
        Jewel = (JewelsEnum)UnityEngine.Random.Range(1, Enum.GetNames(typeof(JewelsEnum)).Length);
        return Jewel;
    }

    /// <summary>
    /// Обновить текстуру фишки. Вызывается при смене типа фишки
    /// </summary>
    public void UpdateJewel()
    {
        switch (Jewel)
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

    public IEnumerator Move(float time)
    {
        Vector3 start = _transform.position;
        Vector3 end = new Vector3(coor.x, coor.y, 0);

        for (float t = 0; t <= 1; t += Time.deltaTime / time)
        {
            _transform.position = Vector3.Lerp(start, end, t);
            yield return new WaitForEndOfFrame();
        }

        _transform.position = end;
    }

    public IEnumerator Remove(float time)
    {
        Color start = _render.color;
        Color end = new Color(start.r, start.g, start.b, 0);

        for (float t = 0; t <= 1; t += Time.deltaTime / time)
        {
            _render.color = Color.Lerp(start, end, t);
            yield return new WaitForEndOfFrame();
        }

        _render.color = end;
        Jewel = JewelsEnum.None;
    }

    public void OffTransperency()
    {
        _render.color = new Color(_render.color.r, _render.color.g, _render.color.b, 1);
    }
}

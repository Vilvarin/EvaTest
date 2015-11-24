using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Контроллирует порядок анимаций. Одиночка
/// </summary>
public class AnimationController : MonoBehaviour
{
    public static AnimationController instance;

    //Высота игрового поля
    int _height = 8;
    public void SetHeight(int h)
    {
        _height = h;
    }

    //Обмен фишками
    public float swapTime = 1;
    List<Piece> _swapAnimation = new List<Piece>();
    public void AddSwaped(Piece piece)
    {
        _swapAnimation.Add(piece);
    }

    //Удаление
    public float removeTime = 1;
    List<Piece> _removeAnimation = new List<Piece>();
    public void AddRemoved(Piece piece)
    {
        _removeAnimation.Add(piece);
    }

    //Падение
    public float letDownTime = 1;
    List<Piece> _letDownAnimation = new List<Piece>();
    public void AddFalled(Piece piece)
    {
        _letDownAnimation.Add(piece);
    }

    //Появление новых фишек
    public float fillTime = 1;
    List<Piece> _fillingAnimation = new List<Piece>();
    public void AddFilled(Piece piece)
    {
        _fillingAnimation.Add(piece);
    }

    /// <summary>
    /// Запуск анимации обмена
    /// </summary>
    /// <returns></returns>
    public IEnumerator SwapAnimation()
    {
        foreach (Piece piece in _swapAnimation)
        {
            StartCoroutine(piece.Move(swapTime));
        }

        _swapAnimation.Clear();

        yield return new WaitForSeconds(swapTime);
    }

    /// <summary>
    /// запуск анимации удаления
    /// </summary>
    /// <returns></returns>
    public IEnumerator RemoveAnimation()
    {
        foreach (Piece piece in _removeAnimation)
        {
            StartCoroutine(piece.Remove(removeTime));
        }

        _removeAnimation.Clear();

        yield return new WaitForSeconds(removeTime);
    }

    /// <summary>
    /// Запуск анимации падения
    /// </summary>
    /// <returns></returns>
    public IEnumerator LetDownAnimation()
    {
        foreach (Piece piece in _letDownAnimation)
        {
            StartCoroutine(piece.Move(letDownTime));
        }

        _letDownAnimation.Clear();

        yield return new WaitForSeconds(letDownTime);
    }

    /// <summary>
    /// Запуск анимации появления фишек
    /// </summary>
    /// <returns></returns>
    public IEnumerator FillingAnimation()
    {
        foreach (Piece piece in _fillingAnimation)
        {
            piece.transform.Translate(0, _height, 0);
            piece.OffTransperency();
            StartCoroutine(piece.Move(fillTime));
        }

        _fillingAnimation.Clear();

        yield return new WaitForSeconds(fillTime);
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
}

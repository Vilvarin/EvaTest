using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Вспомогательный класс для хранения списка типов фишек
/// </summary>
static class Jewels
{
    /// <summary>
    /// Список цветов из JewelsEnum в числовом представлении
    /// </summary>
    static List<int> colors = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };

    /// <summary>
    /// Получить случайный цвет из списка
    /// </summary>
    /// <returns>Перечисление цвета фишки</returns>
    static public JewelsEnum GetRandomColor()
    {
        int i = Random.Range(0, colors.Count - 1);
        return (JewelsEnum)colors[i];
    }

    /// <summary>
    /// Удалить цвет из списка
    /// </summary>
    /// <param name="color">Цвет, который хотим удалить</param>
    static public void RemoveColor(JewelsEnum color)
    {
        colors.Remove((int)color);
    }

    /// <summary>
    /// Сбросить цвета по умолчанию. В списке будут присутствовать все цвета
    /// </summary>
    static public void SetupColors()
    {
        colors = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
    }
}

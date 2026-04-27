using System;
using UnityEngine;
using Random = System.Random;
using System.Text;

public static class TextAnimationInjector
{
    public static string InjectAnimatedTexts(
        string text,
        string linkTag,
        int minGap,
        int maxGap,
        int minLength,
        int maxLength,
        int? seed = null
    )
    {
        if (string.IsNullOrEmpty(text))
            return text;

        if (minGap < 0 || maxGap < minGap)
            Debug.Log("Nieprawidłowy zakres gap.");

        if (minLength <= 0 || maxLength < minLength)
            Debug.Log("Nieprawidłowy zakres length.");

        Random random = seed.HasValue ? new(seed.Value) : new();
        StringBuilder sb = new(text.Length * 2);

        int nextInsertAt = random.Next(minGap, maxGap + 1);
        Debug.Log(nextInsertAt);
        bool insideExistingTag = false;

        for (int i = 0; i < text.Length;)
        {
            // wykrywanie istniejących tagów
            if (text[i] == '<')
                insideExistingTag = true;

            if (insideExistingTag)
            {
                sb.Append(text[i]);

                if (text[i] == '>')
                    insideExistingTag = false;

                i++;
                continue;
            }

            // moment rozpoczęcia wstawki
            if (i >= nextInsertAt)
            {
                int length = random.Next(minLength, maxLength + 1);
                int end = Math.Min(i + length, text.Length);

                sb.Append("<link=").Append(linkTag).Append(">");

                for (int j = i; j < end; j++)
                    sb.Append(text[j]);

                sb.Append("</link>");

                i = end;
                nextInsertAt = i + random.Next(minGap, maxGap + 1);
                continue;
            }

            sb.Append(text[i]);
            i++;
        }

        return sb.ToString();
    }
}
using System;
using System.Text;

public static class TextAnimationInjector
{
    public static string InjectAnimatedLinks(
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

        var random = seed.HasValue ? new Random(seed.Value) : new Random();
        var sb = new StringBuilder(text.Length * 2);

        int nextInsertAt = random.Next(minGap, maxGap);
        bool insideExistingTag = false;

        for (int i = 0; i < text.Length; i++)
        {
            // wykrywanie istniejących tagów (TMP, rich text)
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
                int length = random.Next(minLength, maxLength);
                int end = Math.Min(i + length, text.Length);

                sb.Append("<link=").Append(linkTag).Append(">");

                for (int j = i; j < end; j++)
                    sb.Append(text[j]);

                sb.Append("</link>");

                i = end;
                nextInsertAt = i + random.Next(minGap, maxGap);
                continue;
            }

            sb.Append(text[i]);
            i++;
        }

        return sb.ToString();
    }
}
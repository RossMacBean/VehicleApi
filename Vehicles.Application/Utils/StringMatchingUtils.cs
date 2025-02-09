namespace Vehicles.Application.Utils;

public static class StringMatchingUtils
{
    /// <summary>
    /// <para>
    /// Calculates the edit distance of two strings using the Levenshtein Distance formula
    /// See <link>https://devopedia.org/levenshtein-distance</link> for a good explanation
    /// </para>
    /// <para>
    /// This implementation was shamelessly stolen with minor changes for readability
    /// from <link>https://rosettacode.org/wiki/Levenshtein_distance#C#</link> it uses a dynamic programming
    /// approach to calculate the 'edit distance'; i.e. the minimum number of edits to transform one string into the other
    /// </para>
    /// </summary>
    /// <param name="s1">The first string to compare</param>
    /// <param name="s2">The second string to compare</param>
    /// <returns>The edit distance between the two input strings</returns>
    public static int LevenshteinDistance(string s1, string s2)
    {
        if (s1.Length == 0)
        {
            return s2.Length;
        }
	
        if (s2.Length == 0)
        {
            return s1.Length;
        }

        // Initialise the dynamic programming array and set the seed values
        var dp = new int[s1.Length + 1, s2.Length + 1];
        for (var i = 0; i <= s1.Length; i++)
        {
            dp[i, 0] = i;
        }

        for (var j = 0; j <= s2.Length; j++)
        {
            dp[0, j] = j;
        }

        // Do the main algorithm!
        for (var j = 1; j <= s2.Length; j++)
        {
            for (var i = 1; i <= s1.Length; i++)
            {
                // The characters at this index match, no operation
                if (s1[i - 1] == s2[j - 1])
                {
                    dp[i, j] = dp[i - 1, j - 1];
                }
                else
                {
                    // Evaluate the 3 possible operations we could do to make the strings 'closer'
                    var deletion = dp[i - 1, j] + 1;
                    var insertion = dp[i, j - 1] + 1;
                    var substitution = dp[i - 1, j - 1] + 1;
                    
                    // We're trying to find the minimum distance between the strings, so pick the smallest one
                    dp[i, j] = Math.Min(Math.Min(deletion, insertion), substitution);
                }
            }
        }

        return dp[s1.Length, s2.Length];
    }
}
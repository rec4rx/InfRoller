using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The <c>NetDef</c> static class.
/// Texts, error codes and links
/// </summary>
public static class NetDef
{
    public const string API_SUBMIT_SCORE = "<link here>";
    /// <summary>
    /// Response int, string Dictionary for response code from server
    /// </summary>
    public static Dictionary<long, string> Response = new Dictionary<long, string>
    {
        {404, "404 - Username not found (user has not registered with the leaderboard service)"},
        {405, "405 - Invalid Username supplied"},
        {200, "200 - Ok"},
    };
}

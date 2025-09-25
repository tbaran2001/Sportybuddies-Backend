#nullable enable
using HotChocolate;

namespace Sportybuddies.API.Modules.Profiles.GraphQL;

/// <summary>
/// Simple test queries for GraphQL
/// </summary>
public class TestQueries
{
    /// <summary>
    /// Simple hello world query for testing
    /// </summary>
    /// <returns>Hello world message</returns>
    public string GetHello() => "Hello, GraphQL World!";
    
    /// <summary>
    /// Simple echo query for testing
    /// </summary>
    /// <param name="message">Message to echo</param>
    /// <returns>Echoed message</returns>
    public string Echo(string message) => $"Echo: {message}";
}
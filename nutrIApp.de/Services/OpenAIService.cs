using Azure;
using Azure.AI.OpenAI;

namespace nutrIApp.de.Services;

public class OpenAIService
{
    OpenAIClient client;
    static readonly char[] trimChars = new char[] { '\n', '?' };
    public void Initialize(string openAIKey)
    {
        client = new OpenAIClient(openAIKey);
    }

    internal async Task<string> CallOpenAIChat(string promptPT2, string prevRecipe)
    {
        string prompt = GeneratePrompt(promptPT2, prevRecipe);
        ChatCompletionsOptions options = new ChatCompletionsOptions();
        options.ChoiceCount = 1;
        options.Messages.Add(new ChatMessage(ChatRole.User, prompt));
        var response = await client.GetChatCompletionsAsync(
            "gpt-4o-mini",
            options);
        StringWriter sw = new StringWriter();
        foreach (ChatChoice choice in response.Value.Choices)
        {
            var text = choice.Message.Content.TrimStart(trimChars);
            sw.WriteLine(text);
        }
        var message = sw.ToString();
        return message;
    }

    private static string GeneratePrompt(string promptPT2, string prevRecipe)
    {
        return $"You are a skilled nutritionist with deep knowledge of balanced nutrition and an exceptional sense of taste. Your task is to analyze an existing recipe called {prevRecipe} and, based on requested changes, create a new version of the recipe that is healthy, delicious, and coherent in terms of macronutrients and flavors. {promptPT2}. Finally, summarize the recipe to the most essential ingredients and steps to carry it out and do not mention anything about the analysis of the previous recipe..";
    }
}

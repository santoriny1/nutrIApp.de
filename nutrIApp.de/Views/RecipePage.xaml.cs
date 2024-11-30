using System.ClientModel;
using nutrIApp.de.Services;
using OpenAI;
using OpenAI.Images;

namespace nutrIApp.de.Views;

public partial class RecipePage : ContentPage
{

	OpenAIService openAIService;
	private OpenAIClient _chatGptClient;
	public RecipePage(OpenAIService svc)
	{
		openAIService = svc;
		InitializeComponent();
		this.Loaded += MainPage_Loaded;

	}

	private void MainPage_Loaded(object sender, EventArgs e)
	{
		var openAiKey = Constants.OpenAIKey;

		_chatGptClient = new(openAiKey);
	}

	private async void GeneratetBtn_Clicked(object sender, EventArgs e)
    {
		await GetNewRecipe(ProteinCheckBox.IsChecked, CarbsCheckBox.IsChecked, HealthyFatsCheckBox.IsChecked, FiberCheckBox.IsChecked, VegetablesCheckBox.IsChecked);
	}

	private async Task GetNewRecipe(bool changeProtein, bool changeCarbs, bool changeHealthyFats, bool changeFiber, bool changeVegetables)
	{
		try
		{
			List<string> changes = new List<string>();

			if (changeProtein) changes.Add("Protein");
			if (changeCarbs) changes.Add("Carbs");
			if (changeHealthyFats) changes.Add("Healthy Fats");
			if (changeFiber) changes.Add("Fiber");
			if (changeVegetables) changes.Add("Vegetables");

			SmallLabel.Text = "Working on it...This can take a little.";
			var promptPT2 = changes.Count > 0
			? $"The patient requested changes in the following categories: {string.Join(", ", changes)}, you should avoid repeating the ingredients of the selected categories."
			: "The patient did not request any changes, improve the current recipe."; ;
			var message = await openAIService.CallOpenAIChat(promptPT2, "Roasted Salmon with Smoky Chickpeas and Greens");
			var imagePrompt = $"Create the dish from the following recipe: {message}";
			GeneratedImage.Source = await GetImageAsync(imagePrompt);

			SmallLabel.Text = message;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
	}

	public async Task<ImageSource> GetImageAsync(string prompt)
	{
		// Use the DALL-E 3 model for image generation.
		ImageClient imageClient = _chatGptClient.GetImageClient("dall-e-3");

		// Generate an image based on the prompt with a 1024x1024 resolution, the default for DALL-E 3.
		ClientResult<GeneratedImage> response = await imageClient.GenerateImageAsync(prompt,
			new ImageGenerationOptions
			{
				Size = GeneratedImageSize.W1024xH1024,
				ResponseFormat = GeneratedImageFormat.Uri
			});

		// Image generation responses provide URLs you can use to retrieve requested image(s).
		Uri imageUri = response.Value.ImageUri;

		return ImageSource.FromUri(imageUri);
	}
}
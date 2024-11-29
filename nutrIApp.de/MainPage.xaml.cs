using System.ClientModel;
using nutrIApp.de.Services;
using OpenAI;
using OpenAI.Images;

namespace nutrIApp.de;

public partial class MainPage : ContentPage
{
	OpenAIService openAIService;
	private OpenAIClient _chatGptClient;

	public MainPage(OpenAIService svc)
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

	private async void OnRestaurantClicked(object sender, EventArgs e)
	{
		await GetRecommendation("restaurant");
	}

	private async void OnHotelClicked(object sender, EventArgs e)
	{
		await GetRecommendation("hotel");
	}

	private async void OnAttractionClicked(object sender, EventArgs e)
	{
		await GetRecommendation("attraction");
	}

	private async Task GetRecommendation(string recommendationType)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(LocationEntry.Text))
			{
				await DisplayAlert("Empty location", "Please enter a location (city or postal code)", "OK");
				return;
			}
			SmallLabel.Text = "Working on it...This can take a little while based on the model selected.";
			var message = await openAIService.CallOpenAI(recommendationType, LocationEntry.Text);
			if (IncludeImageChk.IsChecked)
			{
				var imagePrompt = $"Show some fun things to do in {LocationEntry.Text} when visiting a {recommendationType}.";
				GeneratedImage.Source = await GetImageAsync(imagePrompt);
			}
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


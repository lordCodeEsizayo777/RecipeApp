using RecipeAppFinal.Models;
using RecipeAppFinal.Pages;
using RecipeAppFinal.Service;

namespace RecipeAppFinal;


public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}
	private void ViewRecipeButton_Clicked(System.Object sender, System.EventArgs e)
	{

		// go to add recipe page
		Navigation.PushAsync(new CreateRecipePage());


	}
	private async void AddNewRecipe_Clicked(System.Object sender, System.EventArgs e)
	{
		await Navigation.PushAsync(new CreateRecipePage());
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await RecipeDatabase.Init();
		var recipes = await RecipeDatabase.GetRecipesAsync();
		RecipesCollectionView.ItemsSource = recipes;
	}

	private async void OnAddRecipeClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new CreateRecipePage());
	}

	private async void OnEditRecipeClicked(object sender, EventArgs e)
	{
		var button = (Button)sender;
		var recipe = (Recipe)button.CommandParameter;
		await Navigation.PushAsync(new CreateRecipePage(recipe));
	}

	private async void OnDeleteRecipeClicked(object sender, EventArgs e)
	{
		var button = (Button)sender;
		var recipe = (Recipe)button.CommandParameter;

		bool confirm = await DisplayAlert("Confirm", "Delete this recipe?", "Yes", "No");
		if (confirm)
		{
			await RecipeDatabase.Init();
			await RecipeDatabase.DeleteRecipeAsync(recipe);
			RecipesCollectionView.ItemsSource = await RecipeDatabase.GetRecipesAsync();
		}
	}

	private async void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
	{
		var keyword = e.NewTextValue?.ToLower() ?? "";
		var allRecipes = await RecipeDatabase.GetRecipesAsync();

		var filtered = allRecipes
			.Where(r =>
				(!string.IsNullOrWhiteSpace(r.Name) && r.Name.ToLower().Contains(keyword)) ||
				(!string.IsNullOrWhiteSpace(r.Description) && r.Description.ToLower().Contains(keyword)) ||
				(!string.IsNullOrWhiteSpace(r.Category) && r.Category.ToLower().Contains(keyword)))
			.ToList();

		RecipesCollectionView.ItemsSource = filtered;
	}



}
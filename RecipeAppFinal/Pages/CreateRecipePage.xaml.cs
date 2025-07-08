using RecipeAppFinal.Models;
using RecipeAppFinal.Service;

namespace RecipeAppFinal.Pages;

public partial class CreateRecipePage : ContentPage
{
	private Recipe _currentRecipe;

	public CreateRecipePage(Recipe recipe = null)
	{
		InitializeComponent();
		_currentRecipe = recipe ?? new Recipe();
		BindingContext = _currentRecipe;

		NameEntry.Text = _currentRecipe.Name;
		CategoryEntry.Text = _currentRecipe.Category;
		DescriptionEditor.Text = _currentRecipe.Description;
		InstructionsEditor.Text = _currentRecipe.Instructions; // New

	}

	private async void OnSaveClicked(object sender, EventArgs e)
	{
		_currentRecipe.Name = NameEntry.Text;
		_currentRecipe.Category = CategoryEntry.Text;
		_currentRecipe.Description = DescriptionEditor.Text;
		_currentRecipe.Instructions = InstructionsEditor.Text; // NEW

		await RecipeDatabase.Init();
		await RecipeDatabase.SaveRecipeAsync(_currentRecipe);

		await Navigation.PopAsync(); // Go back to main page
	}

}
using System;
using SQLite;
using RecipeAppFinal.Models;

namespace RecipeAppFinal.Service;

public class RecipeDatabase
{

    // SQLiteAsyncConnection is used for asynchronous database operations
    // This allows the app to remain responsive while performing database operations
    // It is initialized with the path to the database file, which is stored in the app's data directory
    // The database file is created if it does not exist
    private static SQLiteAsyncConnection _database;

    // This method initializes the database connection and creates the Recipe table if it does not exist
    // It is called once at the start of the application to ensure the database is ready for use
    // The database file is stored in the app's data directory, which is platform-specific
    // For example, on Android, it is stored in /data/data/<package_name>/files/recipes.db
    // On iOS, it is stored in the app's sandboxed Documents directory
    // The CreateTableAsync method is used to create the Recipe table if it does

    public static async Task Init()
    {
        if (_database != null)
            return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "recipes.db");
        _database = new SQLiteAsyncConnection(dbPath);
        await _database.CreateTableAsync<Recipe>();
    }

    // The following methods provide CRUD (Create, Read, Update, Delete) operations for the Recipe table
    // GetRecipesAsync retrieves all recipes from the database
    // GetRecipeAsync retrieves a specific recipe by its ID
    // SaveRecipeAsync saves a recipe to the database, inserting it if it is new (

    // Id is 0) or updating it if it already exists (Id is not 0)
    // DeleteRecipeAsync deletes a recipe from the database
    // These methods return Task objects, allowing them to be awaited in asynchronous code

    public static Task<List<Recipe>> GetRecipesAsync()
        => _database.Table<Recipe>().ToListAsync();

    // Retrieves a recipe by its ID
    // Returns the first recipe that matches the given ID or null if no match is found
    // This method is useful for displaying a specific recipe's details in the UI
    // It uses the FirstOrDefaultAsync method to find the recipe asynchronously
    // If no recipe with the specified ID exists, it returns null
    public static Task<Recipe> GetRecipeAsync(int id)
        => _database.Table<Recipe>().FirstOrDefaultAsync(r => r.Id == id);

    // Saves a recipe to the database
    // If the recipe's Id is 0, it is considered a new recipe and is
    // inserted into the database
    // If the Id is not 0, it is considered an existing recipe and is updated
    // The method returns a Task<int> indicating the number of rows affected by the operation
    // This allows the app to handle both creating new recipes and updating existing ones

    public static Task<int> SaveRecipeAsync(Recipe recipe)
        => recipe.Id == 0 ? _database.InsertAsync(recipe) : _database.UpdateAsync(recipe);

    // Deletes a recipe from the database
    // This method takes a Recipe object as a parameter and deletes it from the database
    // It returns a Task<int> indicating the number of rows affected by the operation
    // This allows the app to remove recipes from the database, such as when a user deletes
    // a recipe from the UI
    // The DeleteAsync method is used to perform the deletion asynchronously, ensuring that the app remains
    // responsive while the operation is being performed
    // It is important to note that the recipe must exist in the database for the deletion to
    // be successful; otherwise, it will not affect any rows

    public static Task<int> DeleteRecipeAsync(Recipe recipe)
        => _database.DeleteAsync(recipe);
}



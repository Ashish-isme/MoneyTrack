using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MoneyTrack.Services
{
    public class TagService
    {
        private readonly string customTagsFilePath = Path.Combine(AppContext.BaseDirectory, "user_custom_tags.json");
        private Dictionary<int, List<string>> userCustomTags = new(); // A dictionary that maps userId to a list of custom tags

        public TagService()
        {
            LoadCustomTags(); 
        }

        // Get all custom tags for a specific user
        public List<string> GetCustomTags(int userId)
        {
            return userCustomTags.ContainsKey(userId) ? userCustomTags[userId] : new List<string>();
        }

        // Add a new custom tag for a user
        public void AddCustomTag(int userId, string tag)
        {
            if (!userCustomTags.ContainsKey(userId))
            {
                userCustomTags[userId] = new List<string>();
            }

            // Add the tag if it doesn't already exist for the user
            if (!userCustomTags[userId].Contains(tag))
            {
                userCustomTags[userId].Add(tag);
                SaveCustomTags(); // Save the updated custom tags to the file
            }
        }

        // Remove a custom tag for a user
        public void RemoveCustomTag(int userId, string tag)
        {
            if (userCustomTags.ContainsKey(userId) && userCustomTags[userId].Contains(tag))
            {
                userCustomTags[userId].Remove(tag);
                SaveCustomTags(); // Save the updated custom tags to the file
            }
        }

        // Load custom tags from the JSON file
        private void LoadCustomTags()
        {
            if (File.Exists(customTagsFilePath))
            {
                try
                {
                    var json = File.ReadAllText(customTagsFilePath);
                    userCustomTags = JsonSerializer.Deserialize<Dictionary<int, List<string>>>(json) ?? new Dictionary<int, List<string>>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading custom tags: {ex.Message}");
                    userCustomTags = new Dictionary<int, List<string>>();
                }
            }
            else
            {
                userCustomTags = new Dictionary<int, List<string>>();
                SaveCustomTags(); // If the file doesn't exist, create a new file
            }
        }

        // Save custom tags to the JSON file
        private void SaveCustomTags()
        {
            try
            {
                var json = JsonSerializer.Serialize(userCustomTags, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(customTagsFilePath, json);   
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving custom tags: {ex.Message}");
            }
        }
    }
}

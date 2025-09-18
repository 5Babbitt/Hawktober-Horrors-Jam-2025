using System;
using System.Collections.Generic;
using System.IO;
using _Scripts.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace _Scripts.Notes
{
    public class NoteSystem : Singleton<NoteSystem>
    {
        [SerializeField] private List<string> notekeys = new();
        
        [Header("JSON Settings")] 
        [SerializeField] private string jsonFileName = "notes.json";

        // Simple dictionary: noteKey -> noteText
        private Dictionary<string, string> notes = new Dictionary<string, string>();

        protected override void Awake()
        {
            base.Awake();
            
            LoadNotes();
            notekeys = new List<string>(notes.Keys);
        }

        /// <summary>
        /// Load notes from JSON file
        /// </summary>
        void LoadNotes()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);

            if (!File.Exists(filePath))
            {
                Debug.LogError($"Notes JSON not found: {filePath}");
                return;
            }

            try
            {
                string jsonContent = File.ReadAllText(filePath);
                Dictionary<string, string> notesData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);

                notes = notesData;
                Debug.Log($"Loaded {notes.Count} notes from JSON");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading notes JSON: {e.Message}");
            }
        }

        /// <summary>
        /// Get note text by key
        /// </summary>
        /// <param name="key">Note key</param>
        /// <returns>Note text or empty string if not found</returns>
        public string GetNote(string key)
        {
            return notes.ContainsKey(key) ? notes[key] : "";
        }

        /// <summary>
        /// Check if note exists
        /// </summary>
        /// <param name="key">Note key</param>
        /// <returns>True if note exists</returns>
        public bool HasNote(string key)
        {
            return notes.ContainsKey(key);
        }

        /// <summary>
        /// Get all available note keys (useful for debugging)
        /// </summary>
        /// <returns>List of all note keys</returns>
        public List<string> GetAllKeys()
        {
            return new List<string>(notes.Keys);
        }
    }
    
    /// <summary>
    /// Wrapper class for JSON deserialization
    /// Unity's JsonUtility requires a wrapper for dictionaries
    /// </summary>
    [Serializable]
    public class NotesData
    {
        public Dictionary<string, string> notes = new Dictionary<string, string>();
    }
}
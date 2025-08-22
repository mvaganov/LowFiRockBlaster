using System;
using System.Collections.Generic;
using System.Text;

namespace MrV.CommandLine {
	public class KeyInput : DispatchTable<char> {
		private static KeyInput _instance;
		public static KeyInput Instance {
			get => _instance != null ? _instance : _instance = new KeyInput();
			set => _instance = value;
		}
		public static void Bind(char keyPress, KeyResponse response) => Instance.BindEvent(keyPress, response);
		public static void Read() => Instance.ReadConsoleKeys();
		public static void TriggerEvents() => Instance.TriggerBindings(true);
		public static void Add(char key) => Instance.AddEvent(key);
		public void ReadConsoleKeys() {
			while (Console.KeyAvailable) {
				ConsoleKeyInfo key = Console.ReadKey();
				AddEvent(key.KeyChar);
			}
		}
		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			foreach(var kvp in keyBinding) {
				sb.Append($"'{kvp.Key}': ").Append(string.Join(", ", kvp.Value.ConvertAll(r => r.Method.Name))).Append("\n");
			}
			return sb.ToString();
		}
	}
	public class DispatchTable<KeyType> {
		public delegate void KeyResponse(DispatchTable<KeyType> handler);
		protected Dictionary<KeyType, List<KeyResponse>> keyBinding = new Dictionary<KeyType, List<KeyResponse>>();
		protected List<KeyType> keysToProcess = new List<KeyType>();
		private List<KeyType> _currentlyProcessingKeys = new List<KeyType>();
		private List<List<KeyResponse>> _currentlyProcessingResponses = new List<List<KeyResponse>>();
		private KeyType _currentlyProcessing;
		/// <summary>Identifies <see cref="KeyType"/> that triggered a <see cref="KeyResponse"/> function</summary>
		public KeyType CurrentlyProcessing => _currentlyProcessing;
		public int Count => keysToProcess.Count;
		public KeyType this[int index] => GetEvent(index);
		public KeyType GetEvent(int index) => keysToProcess[index];
		public void AddEvent(KeyType key) => keysToProcess.Add(key);
		public void TriggerBindings(bool consumeEvents) {
			for (int i = 0; i < keysToProcess.Count; i++) {
				KeyType key = keysToProcess[i];
				if (keyBinding.TryGetValue(key, out List<KeyResponse> responses)) {
					_currentlyProcessingKeys.Add(key);
					_currentlyProcessingResponses.Add(responses);
				}
			}
			if (consumeEvents) {
				ClearEvents();
			}
			for (int i = 0; i < _currentlyProcessingResponses.Count; i++) {
				_currentlyProcessing = _currentlyProcessingKeys[i];
				List<KeyResponse> responses = _currentlyProcessingResponses[i];
				responses.ForEach(a => a.Invoke(this));
			}
			_currentlyProcessing = default;
			_currentlyProcessingKeys.Clear();
			_currentlyProcessingResponses.Clear();
		}
		public void ClearEvents() => keysToProcess.Clear();
		public List<KeyResponse> BindEvent(KeyType key, KeyResponse response) {
			if (!keyBinding.TryGetValue(key, out List<KeyResponse> responses)) {
				keyBinding[key] = responses = new List<KeyResponse>();
			}
			responses.Add(response);
			return responses;
		}
	}
}

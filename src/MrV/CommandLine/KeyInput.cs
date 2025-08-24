using System;
using System.Collections.Generic;
using System.Text;

namespace MrV.CommandLine {
	public delegate void KeyResponse();
	public class KeyInput : Dispatcher<char> {
		private static KeyInput _instance;
		public static KeyInput Instance {
			get => _instance != null ? _instance : _instance = new KeyInput();
			set => _instance = value;
		}
		public static void Bind(char keyPress, KeyResponse response) => Instance.BindKeyResponse(keyPress, response);
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
			foreach(var kvp in KeyBinding) {
				sb.Append($"'{kvp.Key}': ").Append(string.Join(", ", kvp.Value.ConvertAll(r => r.Method.Name))).Append("\n");
			}
			return sb.ToString();
		}
	}
	public class Dispatcher<KeyType> {
		protected DispatchTable<KeyType> dispatchTable = new DispatchTable<KeyType>();
		protected List<KeyType> eventsToProcess = new List<KeyType>();
		private List<KeyType> _currentEventCodes = new List<KeyType>();
		private List<List<KeyResponse>> _currentResponses = new List<List<KeyResponse>>();
		private KeyType _currentlyEvent;
		public Dictionary<KeyType, List<KeyResponse>> KeyBinding => dispatchTable.keyBinding;
		/// <summary>Identifies <see cref="KeyType"/> that triggered a <see cref="KeyResponse"/> function</summary>
		public KeyType CurrentlyEvent => _currentlyEvent;
		public int Count => eventsToProcess.Count;
		public KeyType this[int index] => GetEvent(index);
		public KeyType GetEvent(int index) => eventsToProcess[index];
		public void AddEvent(KeyType key) => eventsToProcess.Add(key);
		public void BindKeyResponse(KeyType key, KeyResponse response) => dispatchTable.BindKeyResponse(key, response);
		public void TriggerBindings(bool consumeEvents) {
			for (int i = 0; i < eventsToProcess.Count; i++) {
				KeyType key = eventsToProcess[i];
				if (dispatchTable.keyBinding.TryGetValue(key, out List<KeyResponse> responses)) {
					_currentEventCodes.Add(key);
					_currentResponses.Add(responses);
				}
			}
			if (consumeEvents) {
				ClearEvents();
			}
			for (int i = 0; i < _currentResponses.Count; i++) {
				_currentlyEvent = _currentEventCodes[i];
				List<KeyResponse> responses = _currentResponses[i];
				responses.ForEach(a => a.Invoke());
			}
			_currentlyEvent = default;
			_currentEventCodes.Clear();
			_currentResponses.Clear();
		}
		public void ClearEvents() => eventsToProcess.Clear();
	}
	public class DispatchTable<KeyType> {
		public Dictionary<KeyType, List<KeyResponse>> keyBinding = new Dictionary<KeyType, List<KeyResponse>>();
		public void BindKeyResponse(KeyType key, KeyResponse response) {
			if (!keyBinding.TryGetValue(key, out List<KeyResponse> responses)) {
				keyBinding[key] = responses = new List<KeyResponse>();
			}
			responses.Add(response);
		}
	}
}

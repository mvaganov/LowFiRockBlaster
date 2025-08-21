using System.Collections.Generic;
using System.Diagnostics;

namespace MrV.Task {
	public class Tasks {
		protected class Task {
			public System.Action WhatToDo;
			public long WhenToDoIt;
			public string Source;
			public Task(System.Action whatToDo, long whenToDoIt) {
				WhatToDo = whatToDo; WhenToDoIt = whenToDoIt;
				StackFrame stackFrame = new StackTrace(true).GetFrame(3);
				Source = $"{stackFrame.GetFileName()}:{stackFrame.GetFileLineNumber()}";
			}
			public void Invoke() => WhatToDo?.Invoke();
		}
		protected List<Task> tasks = new List<Task>();
		protected static Tasks _instance;
		public static Tasks Instance => _instance != null ? _instance : _instance = new Tasks();
		public static void Add(System.Action whatToDo, long delay = 0) => Instance.Enqueue(whatToDo, delay);
		public static bool Update() => Instance.RunUpdate();
		public void Enqueue(System.Action whatToDo, long delay = 0) {
			long when = Time.TimeMsCurrentFrame + delay;
			int index = Algorithm.BinarySearchWithInsertionPoint(tasks, when, GetTimeFromTask, LongLessThan);
			long GetTimeFromTask(Task t) => t.WhenToDoIt;
			bool LongLessThan(long a, long b) => a < b;
			if (index < 0) {
				index = ~index;
			}
			tasks.Insert(index, new Task(whatToDo, when));
		}
		public bool RunUpdate() {
			if (tasks.Count == 0 || Time.TimeMsCurrentFrame < tasks[0].WhenToDoIt) {
				return false;
			}
			List<Task> toExecuteNow = new List<Task>();
			while (tasks.Count > 0 && Time.TimeMsCurrentFrame >= tasks[0].WhenToDoIt) {
				toExecuteNow.Add(tasks[0]);
				tasks.RemoveAt(0);
			}
			for (int i = 0; i < toExecuteNow.Count; ++i) {
				toExecuteNow[i].Invoke();
			}
			return true;
		}
	}
}

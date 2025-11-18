using Core.Scriptable_Objects;
using UnityEngine;

namespace Ink.Demos.Basic_Demo.Scripts
{
	public class QuitGameOnKeypress : MonoBehaviour {
		private void OnEnable()
		{
			PlayerControls.OnEscapePressedEvent += QuitApp;
		}

		private void QuitApp()
		{
			Application.Quit();
		}
	}
}
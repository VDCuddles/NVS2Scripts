using System;
using UnityEngine;
using TMPro;

public class HotkeyMenu : MonoBehaviour
{
	[SerializeField] private GameObject gamePersistent = default;
	private bool isAwaitingKey = false;
	private InputController ic;
	private Type ictype = typeof(InputController);

	void Start()
	{
		GameObject gp;
		if (!FindObjectOfType<GameOptions>())
		{
			gp = Instantiate(gamePersistent);
			gp.name = gamePersistent.name;
		}

		//foreach (FieldInfo prop in ictype.GetFields())
		//Debug.Log(prop.Name);
		ic = FindObjectOfType<InputController>();
		foreach (TextMeshProUGUI gui in FindObjectsOfType<TextMeshProUGUI>())
		{
			if (ictype.GetField(gui.gameObject.name) != null)
			{
				if (gui.gameObject.name == ictype.GetField(gui.gameObject.name).Name)
					gui.text = ictype.GetField(gui.gameObject.name).GetValue(ic).ToString();
			}
			if (gui.text.Contains("Alpha"))
				gui.text = gui.text.Remove(0, 5);
		}
	}

	private void Update()
	{
		if (isAwaitingKey && Input.anyKey)
		{
			foreach (TextMeshProUGUI gui in FindObjectsOfType<TextMeshProUGUI>())
				if (gui.text == "...")
					foreach (KeyCode vKey in Enum.GetValues(typeof(KeyCode)))
						if (Input.GetKey(vKey))
						{
							gui.text = vKey.ToString();
							if (gui.text.Contains("Alpha"))
								gui.text = gui.text.Remove(0, 5);
							ictype.GetField(gui.gameObject.name).SetValue(ic, vKey);
						}
			isAwaitingKey = false;
		}
	}

	public void AwaitKeyPress()
	{
		isAwaitingKey = true;

	}

}

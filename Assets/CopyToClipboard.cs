using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CopyToClipboard : MonoBehaviour
{
	public void Copy()
	{
		GUIUtility.systemCopyBuffer = GetComponent<TextMeshProUGUI>().text;
	}
}

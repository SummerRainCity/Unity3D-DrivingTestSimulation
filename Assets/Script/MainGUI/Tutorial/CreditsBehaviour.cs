using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace game_core
{
/// <summary>
/// Credits behaviour class shows credits slowly on credits scene,
/// whether user touches the credits changes the movement flag.
/// </summary>
public class CreditsBehaviour : MonoBehaviour {

	//VARIABLES
	public 	float 			endMarker;
	public 	float 			speed 	= 	1.0f;
	private RectTransform	rectTransform;
	private bool			onMoveFlag	=	true;
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		rectTransform			=	GetComponent<RectTransform> ();
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update() {
		
		if (rectTransform.localPosition.y < endMarker && onMoveFlag)
		{
				rectTransform.localPosition += Vector3.up * TimeManager.deltaTime * speed ;
		}
	}
	
	/// <summary>
	/// Raises the click event.
	/// </summary>
	public void OnClick()
	{
			onMoveFlag=!onMoveFlag;
	}
}

}

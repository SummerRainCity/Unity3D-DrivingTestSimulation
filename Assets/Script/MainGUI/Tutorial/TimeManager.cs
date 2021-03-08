using UnityEngine;
using System.Collections;
namespace game_core{
/// <summary>
/// Time manager class; Controls time flow of the game.
/// </summary>
public static class TimeManager{

	private static bool 	_isPaused 			= 	false;
	private static float	_previousTimeScale	=	1.0f;
	private static float	_time				=  	0.0f;

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="game_core.TimeManager"/> is paused.
	/// </summary>
	/// <value><c>true</c> if is paused; otherwise, <c>false</c>.</value>
	public static bool isPaused
	{
		get{	return _isPaused;}
		set{
			_isPaused		=value;
			}

	}

	/// <summary>
	/// Gets or sets the previous time scale.
	/// </summary>
	/// <value>The previous time scale.</value>
	public static float previousTimeScale
	{
		get{return _previousTimeScale;}
		set{_previousTimeScale=value;}
	}

	/// <summary>
	/// Gets or sets the time scale.
	/// </summary>
	/// <value>The time scale.</value>
	public static float timeScale
	{
		get{return Time.timeScale;}
		set{Time.timeScale=value;}
	}

	/// <summary>
	/// Gets or sets the time.
	/// </summary>
	/// <value>The time.</value>
	public static float time
	{
		get{return _time;}
		set{_time=value;}
	}

	/// <summary>
	/// Gets the delta time.
	/// </summary>
	/// <value>The delta time.</value>
	public static float deltaTime
	{
		get{return (isPaused)?0:Time.deltaTime;}
	}
}
}
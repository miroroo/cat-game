using UnityEngine;

public class WireClickHandler : MonoBehaviour
{
	WiresGame game;
	WiresGame.Wire wire;

	public void Init(WiresGame g, WiresGame.Wire w)
	{
		game = g;
		wire = w;
	}

	void OnMouseDown()
	{
		game?.TryConnect(wire);
	}
}
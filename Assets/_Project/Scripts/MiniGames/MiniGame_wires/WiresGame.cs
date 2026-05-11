using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiresGame : MonoBehaviour
{
	[System.Serializable]
	public class Wire
	{
		public SpriteRenderer sprite;
		public Color color;
		[HideInInspector] public bool connected;
	}

	public List<Wire> wires = new List<Wire>();
	public float showTime = 1f;
	public float delay = 0.3f;
	public float connectTime = 3f;
	public GameObject winPanel, losePanel;

	private List<Wire> order = new List<Wire>();
	private int index = 0;
	private bool playing = false;
	private Coroutine timer;

	void Start()
	{
		foreach (Wire w in wires)
		{
			w.sprite.color = Color.gray;
			if (w.sprite.GetComponent<Collider2D>() == null)
				w.sprite.gameObject.AddComponent<BoxCollider2D>();

			WireClickHandler h = w.sprite.GetComponent<WireClickHandler>();
			if (h == null) h = w.sprite.gameObject.AddComponent<WireClickHandler>();
			h.Init(this, w);
		}
		StartGame();
	}

	void StartGame()
	{
		StopAllCoroutines();
		playing = true;
		index = 0;
		winPanel.SetActive(false);
		losePanel.SetActive(false);
		foreach (Wire w in wires)
		{
			w.connected = false;
			w.sprite.color = Color.gray;
		}
		StartCoroutine(Show());
	}

	IEnumerator Show()
	{
		order = new List<Wire>(wires);
		for (int i = 0; i < order.Count; i++)
		{
			int r = Random.Range(i, order.Count);
			Wire t = order[i];
			order[i] = order[r];
			order[r] = t;
		}

		foreach (Wire w in order)
		{
			w.sprite.color = w.color;
			yield return new WaitForSeconds(showTime);
			w.sprite.color = Color.gray;
			yield return new WaitForSeconds(delay);
		}

		NextWire();
	}

	void NextWire()
	{
		if (!playing) return;
		if (index >= order.Count) { Win(); return; }

		if (timer != null) StopCoroutine(timer);
		timer = StartCoroutine(Timeout());

		order[index].sprite.color = Color.white;
	}

	IEnumerator Timeout()
	{
		float t = 0;
		while (t < connectTime)
		{
			if (order[index].connected) yield break;
			t += Time.deltaTime;
			yield return null;
		}
		Lose();
	}

	public void TryConnect(Wire clicked)
	{
		if (!playing || clicked.connected) return;

		if (clicked == order[index])
		{
			clicked.connected = true;
			clicked.sprite.color = Color.green;
			index++;
			NextWire();
		}
		else
		{
			StartCoroutine(FlashRed(clicked));
			Lose();
		}
	}

	IEnumerator FlashRed(Wire w)
	{
		w.sprite.color = Color.red;
		yield return new WaitForSeconds(0.2f);
		if (!w.connected) w.sprite.color = Color.gray;
	}

	void Win()
	{
		playing = false;
		winPanel.SetActive(true);
	}

	void Lose()
	{
		playing = false;
		losePanel.SetActive(true);
	}
}
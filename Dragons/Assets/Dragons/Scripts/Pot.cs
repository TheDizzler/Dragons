using System;
using UnityEngine;
using UnityEngine.UI;

namespace AtomosZ.Gambal.Poker
{
	public class Pot : MonoBehaviour
	{
		public int total { get; private set; }
		[SerializeField] private Text potTotal = null;
		[SerializeField] private Text currentRaise = null;


		void Start()
		{
			potTotal.text = "$" + $"{total:n0}";
			currentRaise.text = "$" + $"{total:n0}";
		}

		public void AddToPot(int amount)
		{
			if (amount < 0)
				throw new Exception("Cannot add negative amount to pot!");
			total += amount;
			potTotal.text = "$" + $"{total:n0}";
		}

		public void DisplayRaiseAmount(int totalRaiseAmount)
		{
			currentRaise.text = "$" + $"{totalRaiseAmount:n0}";
		}
	}
}
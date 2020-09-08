using System;
using UnityEngine;

namespace AtomosZ.Gambal.Poker
{
	public class Pot : MonoBehaviour
	{
		public int total { get; private set; }


		public void AddToPot(int amount)
		{
			if (amount <= 0)
				throw new Exception("Cannot add 0 or negative amount to pot!");
			total += amount;
		}
	}
}
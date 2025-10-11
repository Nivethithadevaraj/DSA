using CompanyHierarchy.Model;
using System;

namespace CompanyHierarchy.View
{
	public class CompanyView
	{
		public void DisplayTraversals(dynamic root, dynamic tree)
		{
			Console.Write("In-order: ");
			tree.InOrder(root);
			Console.WriteLine();

			Console.Write("Pre-order: ");
			tree.PreOrder(root);
			Console.WriteLine();

			Console.Write("Post-order: ");
			tree.PostOrder(root);
			Console.WriteLine();
		}
	}
}

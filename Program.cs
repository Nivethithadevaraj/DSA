using System;
using System.IO;
using CompanyHierarchy.Controller;
using CompanyHierarchy.View;

namespace CompanyHierarchy
{
    class Program
    {
        static void Main(string[] args)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(basePath, @"..\..\.."));
            string filePath = Path.Combine(projectRoot, "Data", "Company.csv");

            CompanyController controller = new CompanyController();
            CompanyView view = new CompanyView();

            controller.LoadEmployees(filePath);
            controller.BuildTrees();
            controller.DeleteFromBST();

            Console.WriteLine("\n=== BST Traversals ===");
            view.DisplayTraversals(controller.bst.Root, controller.bst);

            Console.WriteLine("\n=== AVL Traversals ===");
            view.DisplayTraversals(controller.avl.Root, controller.avl);

            Console.WriteLine("\n✅ Program finished successfully!");
        }
    }
}

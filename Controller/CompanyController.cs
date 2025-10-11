using CompanyHierarchy.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace CompanyHierarchy.Controller
{
    public class CompanyController
    {
        public BST bst = new();
        public AVL avl = new();
        private List<Employee> employees = new();

        public void LoadEmployees(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"? File not found: {filePath}");
                return;
            }

            var lines = File.ReadAllLines(filePath);
            for (int i = 1; i < Math.Min(lines.Length, 11); i++) // ?? read only first 10 rows
            {
                var parts = lines[i].Split(',');
                if (parts.Length < 3) continue;

                try
                {
                    int id = int.Parse(parts[1]);
                    string name = parts[2];
                    int managerId = 0;
                    int.TryParse(parts[9], out managerId);

                    employees.Add(new Employee { Id = id, Name = name, ManagerId = managerId });
                }
                catch { continue; }
            }

            Console.WriteLine($"? Loaded {employees.Count} employees from CSV!");
        }

        public void BuildTrees()
        {
            foreach (var emp in employees)
            {
                bst.Insert(emp);
                avl.Insert(emp);
            }
            Console.WriteLine("?? BST and AVL Trees built successfully!");
        }

        public void DeleteFromBST()
        {
            if (employees.Count == 0)
            {
                Console.WriteLine("No employees to delete.");
                return;
            }

            int idToDelete = employees[0].Id; // delete first one just for demo
            Console.WriteLine($"\n??? Deleting Employee ID {idToDelete} ({employees[0].Name}) from BST");
            bst.Delete(idToDelete);
        }
    }
}

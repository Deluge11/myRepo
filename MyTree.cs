using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataTest
{

    public class BinaryTree
    {
        public int TreeLevel { get; private set; } = -1;
        public int NodesNum { get; private set; }
        public Node Root { get; set; } = null!;
        public List<Node[]> Levels { get; set; } = null!;

        public void Add(String value)
        {
            if (TreeLevel == -1)
            {
                TreeLevel++;
                NodesNum++;
                Root = new Node(value, TreeLevel, 0, GetSize(value));
                Levels = new List<Node[]>();
                Levels.Add(new Node[] { Root });
            }
            else
            {
                AddNode(value);
            }
        }
        private void AddNode(String value)
        {
            //Levels.RemoveAt(Levels.Count - 1);
            //int LastIndexValue = (Int16)Math.Pow(2, TreeLevel) - 1;


            int lastArray = Levels.Count - 1;
            int LastIndexValue = Levels[lastArray].Length - 1;

            if (Levels[lastArray][LastIndexValue] == null)
            {
                for (int i = 0; i <= LastIndexValue + 1; i++)
                {
                    if (Levels[lastArray][i] == null)
                    {
                        NodesNum++;
                        Levels[lastArray][i] = new Node(value, TreeLevel, i, GetSize(value));
                        if (i % 2 == 0)
                        {
                            Levels[TreeLevel - 1][i / 2].Left = Levels[lastArray][i];
                            HeapSort(value, i);
                        }
                        else
                        {
                            Levels[TreeLevel - 1][i / 2].Right = Levels[lastArray][i];
                            HeapSort(value, i);
                        }
                        return;
                    }

                }

            }
            else
            {
                TreeLevel++;
                int newArrSize = (Int16)Math.Pow(2, TreeLevel);
                Node[] newArr = new Node[newArrSize];
                Levels.Add(newArr);
                lastArray = Levels.Count - 1;
                NodesNum++;
                Levels[lastArray][0] = new Node(value, TreeLevel, 0, GetSize(value));
                Levels[lastArray - 1][0].Left = Levels[lastArray][0];
                HeapSort(value, 0);
                return;

            }
        }

        private void HeapSort(string Value, int nodeIndex)
        {
            int ValueSize = GetSize(Value);
            int CheckLevel = TreeLevel;
            bool flipped = false;

            for (int i = nodeIndex; i >= 0; i--)
            {  
                if (flipped)
                {
                    i++;
                    flipped = false;
                }     
                if (i == 0)
                {
                    int UpperTreeLastIndex = Levels[CheckLevel - 1].Length - 1;
                    if (ValueSize < Levels[CheckLevel - 1][UpperTreeLastIndex].ValueSize)
                    {
                        Levels[CheckLevel][0].SetValue(Levels[CheckLevel - 1][UpperTreeLastIndex].Value);
                        Levels[CheckLevel - 1][UpperTreeLastIndex].SetValue(Value);
                        
                        i = Levels[--CheckLevel].Length - 1;
                        flipped = true;
                        continue;
                    }
                    break;
                }
              
                else if (i > 0)
                {
                    if (ValueSize < Levels[CheckLevel][i - 1].ValueSize)
                    {
                        Levels[CheckLevel][i].SetValue(Levels[CheckLevel][i - 1].Value);
                        Levels[CheckLevel][i - 1].SetValue(Value);
                        continue;
                    }
                    break;
                }
            }
        }
        protected int GetSize(string Value)
        {
            int ValueSize = 0;

            if (int.TryParse(Value, out ValueSize))
            {
                ValueSize = int.Parse(Value);
            }
            else
            {
                int i = 0;
                while (!Value[i].Equals('@'))
                {
                    ValueSize += (int)Value[i];
                    if (Value.Length == ++i)
                        break;
                }
            }
            return ValueSize;
        }
        public void PrintTree()
        {
            if (TreeLevel == -1)
            {
                Console.WriteLine("Tree is Empty");
                return;
            }
            Print(Root);
        }
        private void Print(Node node, string space = " ")
        {
            Console.WriteLine(space + node.Value);

            if (node.Left != null)
                Print(node.Left, space + "  ");
            if (node.Right != null)
                Print(node.Right, space + "  ");
        }
        public void PrintLevels()
        {
            if (TreeLevel == -1)
            {
                Console.WriteLine("Tree is Empty");
                return;
            }
            int level = 1;
            foreach (var Lvl in Levels)
            {
                Console.WriteLine("Level:" + level++);
                foreach (var Node in Lvl)
                {
                    if (Node == null)
                        return;
                    Console.WriteLine("  " + Node.Value);
                }
                Console.WriteLine("==============");
            }
        }


    }

    public class Node:BinaryTree
    {
        //public int LeftIndex { get; set; }
        //public int RightIndex { get; set; }

        public int ValueSize { get; set; }
        public String Value { get; set; }
        public int IndexOfNode { get; private set; }
        public int NodeinArr { get; private set; }
        public int ParentIndex { get; private set; }
        public Node Left { get; set; } = null!;
        public Node Right { get; set; } = null!;
        public int NodeLevel { get; private set; }


        public void SetValue(string val)
        {
            Value=val;
            ValueSize = base.GetSize(val);
        }

        public Node(String value, int Level, int inArr, int ValSize)
        {

            //LeftIndex = 2 * IndexOfNode + 1;
            //RightIndex = 2 * IndexOfNode + 2;
            Value = value;
            NodeLevel = Level;
            NodeinArr = inArr;
            ValueSize = ValSize;

            IndexOfNode = NodeinArr + (int)Math.Pow(2, NodeLevel) - 1;
            ParentIndex = IndexOfNode % 2 == 0 ? IndexOfNode / 2 - 1 : IndexOfNode / 2;



        }
    }
}

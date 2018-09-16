﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GRaff;
using System.Linq;
using System.Collections.Generic;

namespace GRaff.UnitTesting
{
	[TestClass]
	public class RBTreeTest
	{
		class TestElement : GameElement
		{
			public int Tag;

			public TestElement(int depth)
				: this(depth, depth)
			{ }

			public TestElement(int depth, int tag)
			{
				Depth = depth;
				Tag = tag;
			}

			public override string ToString()
			{
				return $"Depth={Depth}; Tag={Tag}";
			}
		}

        [TestMethod]
		public void RBTree_Add_And_Sort()
		{
			var tree = new RedBlackTree();
			var elements = GRandom.Range(0, 100).Select(i => new TestElement(i)).ToArray();

			foreach (var element in elements)
				tree.Add(element);

            Assert.AreEqual(elements.Length, tree.Count);

			var results = tree.ToArray();
			for (int i = 0; i < results.Length; i++)
				Assert.AreEqual(i, results[i].Depth);
		}

        [TestMethod]
        public void RBTree_Contains()
        {
            var tree = new RedBlackTree();
            var elements = GRandom.Range(0, 100).Select(i => new TestElement(i)).ToArray();

            var isAdded = new bool[elements.Length];
            var addCount = 0;
            for (var i = 0; i < elements.Length; i++)
                if (GRandom.Probability(0.2))
                {
                    tree.Add(elements[i]);
                    isAdded[i] = true;
                    addCount++;
                }

            Assert.AreEqual(addCount, tree.Count);

            var results = tree.ToArray();
            for (var i = 0; i < elements.Length; i++)
                Assert.AreEqual(isAdded[i], tree.Contains(elements[i]));
        }

        [TestMethod]
		public void RBTree_Remove()
		{
			var tree = new RedBlackTree();
			var elements = GRandom.Range(0, 100).Select(i => new TestElement(i)).ToArray();
            
            foreach (var element in elements)
                tree.Add(element);

            var isRemoved = new bool[elements.Length];
            var removeCount = 0;
            for (int i = 0; i < elements.Length; i++)
                if (GRandom.Probability(0.2))
                {
                    tree.Remove(elements[i]);
                    isRemoved[i] = true;
                    removeCount++;
                }

            Assert.AreEqual(elements.Length - removeCount, tree.Count);

            var results = tree.ToArray();
            for (int i = 0; i < elements.Length; i++)
				Assert.AreEqual(!isRemoved[i], tree.Contains(elements[i]));
		}


		[TestMethod]
		public void RBTree_Enumerate()
		{
			var collection = new RedBlackTree();
			var indices = GRandom.Range(0, 100).ToArray();
			var elements = indices.Select(i => new TestElement(i)).ToList();

			foreach (var element in elements)
				collection.Add(element);

			indices = indices.OrderBy(i => i).ToArray();
			var index = 0;
			foreach (var element in collection)
				Assert.AreEqual(indices[index++], element.Depth);
		}
	}
}

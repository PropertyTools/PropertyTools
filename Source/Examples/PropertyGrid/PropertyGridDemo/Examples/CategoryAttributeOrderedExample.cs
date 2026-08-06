// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderedCategoryAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using PropertyTools.DataAnnotations;

namespace ExampleLibrary
{
	/// <remarks>
	/// Propertes (fields) represent roman numbers <seealso cref="https://en.wikipedia.org/wiki/Roman_numerals"/>
	/// </remarks>
	[PropertyGridExample]
	public class CategoryAttributeOrderedExample : Example	
    {
		private int numberC = 100;
		private int numberCC = 200;
		private int numberCCC = 300;
		private int numberCD = 400;

		private int numberD = 500;
		private int numberDC = 600;
		private int numberDCC = 700;
		private int numberDCCC = 800;

		private int numberCM = 900;

		private int numberX = 10;
		private int numberL = 50;

		[ReadOnly(true)]
		[PropertyTools.DataAnnotations.Category("Hundreds (Roman)|Category D", tabSortIndex: 1, groupSortIndex: 1)]		
		public int NumberD { get => this.numberD; set { this.numberD = value; this.RaisePropertyChanged(nameof(NumberD)); } }

		[ReadOnly(true)]
		[PropertyTools.DataAnnotations.Category("Hundreds (Roman)|Category D", tabSortIndex: 1, groupSortIndex: 1)]		
		public int NumberDC { get => this.numberDC; set { this.numberDC = value; this.RaisePropertyChanged(nameof(NumberDC)); } }

		[ReadOnly(true)]
		[PropertyTools.DataAnnotations.Category("Hundreds (Roman)|Category D", tabSortIndex: 1, groupSortIndex: 1)]		
		public int NumberDCC { get => this.numberDCC; set { this.numberDCC = value; this.RaisePropertyChanged(nameof(NumberDCC)); } }

		[ReadOnly(true)]
		[PropertyTools.DataAnnotations.Category("Hundreds (Roman)|Category D", tabSortIndex: 1, groupSortIndex: 1)]		
		public int NumberDCCC { get => this.numberDCCC; set { this.numberDCCC = value; this.RaisePropertyChanged(nameof(NumberDCCC)); } }

		[ReadOnly(true)]
		[PropertyTools.DataAnnotations.Category("Hundreds (Roman)|Category C", tabSortIndex: 1)]		
		public int NumberC { get => this.numberC; set { this.numberC = value; this.RaisePropertyChanged(nameof(NumberC)); } }

		[ReadOnly(true)]
		[PropertyTools.DataAnnotations.Category("Hundreds (Roman)|Category C", tabSortIndex: 1)]		
		public int NumberCC { get => this.numberCC; set { this.numberCC = value; this.RaisePropertyChanged(nameof(NumberCC)); } }

		[ReadOnly(true)]
		[PropertyTools.DataAnnotations.Category("Hundreds (Roman)|Category C", tabSortIndex: 1)]		
		public int NumberCCC { get => this.numberCCC; set { this.numberCCC = value; this.RaisePropertyChanged(nameof(NumberCCC)); } }

		[ReadOnly(true)]
		[PropertyTools.DataAnnotations.Category("Hundreds (Roman)|Category C", tabSortIndex: 1)]		
		public int NumberCD { get => this.numberCD; set { this.numberCD = value; this.RaisePropertyChanged(nameof(NumberCD)); } }

		[ReadOnly(true)]
		[PropertyTools.DataAnnotations.Category("Hundreds (Roman)|Category C", tabSortIndex: 1)]
		public int NumberCM { get => this.numberCM; set { this.numberCM = value; this.RaisePropertyChanged(nameof(NumberCM)); } }

		[ReadOnly(true)]
		[PropertyTools.DataAnnotations.Category("Tens (Roman)|Category L", 0, 1)]
		public int NumberL { get => this.numberL; set { this.numberL = value; this.RaisePropertyChanged(nameof(NumberL)); } }

		[ReadOnly(true)]
		[PropertyTools.DataAnnotations.Category("Tens (Roman)|Category X", 0, 0)]
		public int NumberX { get => this.numberX; set { this.numberX = value; this.RaisePropertyChanged(nameof(NumberX)); } }
	}
}
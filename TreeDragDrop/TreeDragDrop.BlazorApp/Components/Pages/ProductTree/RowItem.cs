using TreeDragDrop.BlazorApp.Backend.Products;

namespace TreeDragDrop.BlazorApp.Components.Pages.ProductTree;

internal class RowItem
{
	public ProductNode ProductNode { get; }

	public int NestingLevel { get; }

	public RowItem(ProductNode productNode, int nestingLevel)
	{
		ProductNode = productNode;
		NestingLevel = nestingLevel;
	}
}
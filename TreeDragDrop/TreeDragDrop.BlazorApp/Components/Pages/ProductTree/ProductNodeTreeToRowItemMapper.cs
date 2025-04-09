using System.Collections.Immutable;
using TreeDragDrop.BlazorApp.Backend.Products;

namespace TreeDragDrop.BlazorApp.Components.Pages.ProductTree;

internal class ProductNodeTreeToRowItemMapper
{
	public IImmutableList<RowItem> Flatten(IImmutableList<ProductNode> rootProductNodes)
	{
		return Flatten(rootProductNodes, 0);
	}

	private IImmutableList<RowItem> Flatten(IImmutableList<ProductNode> productNodes, int nestingLevel)
	{
		return productNodes.SelectMany(pn => Flatten(pn, nestingLevel)).ToImmutableList();
	}

	private IImmutableList<RowItem> Flatten(ProductNode productNode, int nestingLevel)
	{
		List<RowItem> rowItem = [new RowItem(productNode, nestingLevel)];
		return rowItem
			.Concat(Flatten(productNode.Children, nestingLevel + 1))
			.ToImmutableList();
	}
}
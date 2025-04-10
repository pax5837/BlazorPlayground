using System.Collections.Immutable;

namespace TreeDragDrop.BlazorApp.Backend.Products;

public class Product
{
	public string Id { get; }

	public string Name { get; set; }

	public decimal Price { get; set; }

	public IImmutableSet<ProductNode> Children => children.ToImmutableHashSet();

	private HashSet<ProductNode> children = new();

	public Product(string id, string name, decimal price)
	{
		Id = id;
		Name = name;
		Price = price;
	}

	public void AddChild(ProductNode productNode)
	{
		children.Add(productNode);
	}

	public ProductNode? RemoveChild(string productId)
	{
		var productNodeOrNull = children.FirstOrDefault(pn => pn.Product.Id .Equals(productId, StringComparison.Ordinal));
		if (productNodeOrNull is not null)
		{
			children.Remove(productNodeOrNull);
		}

		return productNodeOrNull;
	}

	public bool HasChildren() => children.Any();

	public bool HasChild(string productId)
	{
		return children.Any(pn => pn.Product.Id.Equals(productId, StringComparison.Ordinal));
	}

	public bool HasDescendant(List<Product> products)
	{
		var directChildProductIds = children.Select(node => node.Product.Id);
		var productIdsToTest = products.Select(prod => prod.Id);
		if (directChildProductIds.Intersect(productIdsToTest).Any())
		{
			return true;
		}

		foreach (var childProduct in Children.Select(pn => pn.Product))
		{
			if (childProduct.HasDescendant(products))
			{
				return true;
			}
		}

		return false;
	}
}
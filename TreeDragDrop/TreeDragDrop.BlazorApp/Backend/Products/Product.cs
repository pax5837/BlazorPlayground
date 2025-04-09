using System.Collections.Immutable;

namespace TreeDragDrop.BlazorApp.Backend.Products;

public class Product
{
	public int Id { get; }

	public string Name { get; set; }

	public decimal Price { get; set; }

	public IImmutableSet<ProductNode> Children => children.ToImmutableHashSet();

	private HashSet<ProductNode> children = new();

	public Product(int id, string name, decimal price)
	{
		Id = id;
		Name = name;
		Price = price;
	}

	public void AddChild(ProductNode productNode)
	{
		children.Add(productNode);
	}

	public ProductNode? RemoveChild(int productId)
	{
		var productNodeOrNull = children.FirstOrDefault(pn => pn.Product.Id == productId);
		if (productNodeOrNull is not null)
		{
			children.Remove(productNodeOrNull);
		}

		return productNodeOrNull;
	}

	public bool HasChildren() => children.Any();

	public bool HasChild(int productId)
	{
		return children.Any(pn => pn.Product.Id == productId);
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
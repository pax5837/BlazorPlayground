using System.Collections.Immutable;

namespace TreeDragDrop.BlazorApp.Backend.Products;

public class ProductNode : IEquatable<ProductNode>
{
	public Guid NodeId { get; }

	public Product Product { get; }

	public int Quantity { get; set; }

	public string Description => $"{Product.Name} ({Quantity}x)";

	public ProductNode? ParentNode{ get; private set; }

	public bool HasChildren => Product.HasChildren();

	public IImmutableList<ProductNode> Children => Product.Children.ToImmutableList();

	public ProductNode(Product product, int quantity, ProductNode? parentNode = null)
	{
		Product = product;
		NodeId = Guid.NewGuid();
		Quantity = quantity;
		if (parentNode is not null)
		{
			ParentNode = parentNode;
			parentNode.Product.AddChild(this);
		}
	}

	public CanAddChildResult CanAddChild(Product child)
	{
		if(Product.Id == child.Id)
		{
			return CanAddChildResult.ChildAndParentAreSame;
		}

		if (Children.Select(c => c.Product.Id).Contains(child.Id))
		{
			return CanAddChildResult.ChildIsAlreadyAddedToParent;
		}

		if (IsAncestorOrHasSameProduct(child))
		{
			return CanAddChildResult.ChildIsAncestor;
		}

		var ancestors = new List<Product>();
		PopulateAncestors(ancestors);

		if (child.HasDescendant(ancestors))
		{
			return CanAddChildResult.AncestorPresentInDescendants;
		}

		return CanAddChildResult.AddChildPossible;
	}

	private void PopulateAncestors(List<Product> ancestors)
	{
		if (ParentNode is not null)
		{
			ancestors.Add(ParentNode.Product);
			ParentNode.PopulateAncestors(ancestors);
		}
	}

	private bool IsAncestorOrHasSameProduct(Product product)
	{
		if (ParentNode is null)
		{
			return false;
		}

		return ParentNode.Product.Id == product.Id
		       || ParentNode.IsAncestorOrHasSameProduct(product);
	}

	public void AddChild(Product product, int quantity)
	{
		if (Product.HasChild(product.Id))
		{
			return;
		}

		var productNode = new ProductNode(product, quantity, this);

		Product.AddChild(productNode);
	}

	private void AddChild(ProductNode productNode)
	{
		Product.AddChild(productNode);
		productNode.ParentNode = this;
	}

	public void Clone(ProductNode? newParentNode = null)
	{
		new ProductNode(Product, Quantity, newParentNode);
	}

	private void RemoveChild(string childProductId)
	{
		Product.RemoveChild(childProductId);
	}

	public void RemoveFromParent()
	{
		ParentNode?.RemoveChild(Product.Id);
	}

	public void MoveToNewParent(ProductNode to)
	{
		ParentNode?.RemoveChild(Product.Id);
		to.AddChild(this);
	}

	public bool Equals(ProductNode? other)
	{
		if (other is null) return false;
		if (ReferenceEquals(this, other)) return true;
		return NodeId.Equals(other.NodeId);
	}

	public override bool Equals(object? obj)
	{
		if (obj is null) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != GetType()) return false;
		return Equals((ProductNode)obj);
	}

	public override int GetHashCode()
	{
		return NodeId.GetHashCode();
	}
}
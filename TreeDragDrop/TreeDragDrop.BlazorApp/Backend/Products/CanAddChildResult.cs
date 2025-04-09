namespace TreeDragDrop.BlazorApp.Backend.Products;

public enum CanAddChildResult
{
	AddChildPossible = 1,
	ChildAndParentAreSame = 2,
	ChildIsAlreadyAddedToParent = 3,
	ChildIsAncestor = 4,
	AncestorPresentInDescendants = 5,
}
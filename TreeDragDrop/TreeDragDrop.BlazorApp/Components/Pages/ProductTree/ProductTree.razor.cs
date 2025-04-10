using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using TreeDragDrop.BlazorApp.Backend.Products;

namespace TreeDragDrop.BlazorApp.Components.Pages.ProductTree;

public partial class ProductTree : ComponentBase
{
    private object lockyLock = new();

    private object? selection;
    ProductNode? draggedProductNode;

    private ProductNode? selectedProductNode;
    public ProductNode? SelectedProductNode
    {
        get => selectedProductNode;
        set
        {
            if (selectedProductNode == value)
            {
                return;
            }

            selectedProductNode = value;
            ProductNodeSelected.InvokeAsync(value);
        }
    }

    private void OnChange(TreeEventArgs obj)
    {
        if (selection is ProductNode productNode)
        {
            SelectedProductNode = productNode;
            Logger.LogInformation("Selection changed to: {ProductId}", productNode.Product.Id);
        }
    }

    void ItemRender(TreeItemRenderEventArgs args)
    {
        var productNode = (ProductNode)args.Value;

        // Allow drag of all items except the root items.
        if (!RootProductNodes.Contains(productNode))
        {
            // logger.LogInformation("set drag properties on {employeeId}", product.Id);

            args.Attributes.Add("title", "Drag item to reorder");
            args.Attributes.Add("style", "cursor:grab");
            args.Attributes.Add("draggable", "true");
            args.Attributes.Add("ondragstart", EventCallback.Factory.Create<DragEventArgs>(this, (DragEventArgs e) => { HandleDraggedItem(productNode); }));
        }

        // Allow drop over any item including the root item.
        args.Attributes.Add("ondragover", "event.preventDefault()");
        args.Attributes.Add("ondrop", EventCallback.Factory.Create<DragEventArgs>(this, (DragEventArgs e) => { HandleDrop(productNode, e); }));
    }

    void ItemContextMenu(TreeItemContextMenuEventArgs args)
    {
        ContextMenuService.Open(args,
            new List<ContextMenuItem> {
                new ContextMenuItem(){ Text = "Delete", Value = 1, Icon = "delete" },
            },
            (e) =>
            {
                var productNodeToRemove = ((ProductNode)args.Value);
                Logger.LogInformation(
                    "Menu item with Value {Value} clicked. Args {ProductName}",
                    e.Value,
                    productNodeToRemove.Product.Name);
                if ((int)e.Value == 1)
                {
                    productNodeToRemove.RemoveFromParent();
                }
                ContextMenuService.Close();
                InvokeAsync(StateHasChanged);
            }
        );
    }

    private void HandleDraggedItem(ProductNode? productNode)
    {
        Logger.LogInformation("Drag started {productId}", productNode.NodeId);
        if (draggedProductNode == null)
        {
            draggedProductNode = productNode;
        }
        else
        {
            ProductDragged.InvokeAsync();
        }
    }

    private void HandleDrop(ProductNode? targetProductNode, DragEventArgs e)
    {
        lock (lockyLock)
        {
            if ((draggedProductNode is null && DraggedProduct is null) || targetProductNode is null)
            {
                Logger.LogInformation(
                    "No Drop, product node exists: {DraggedProductNodeExists}, dragged product Exists: {DraggedProductExists}, target node exists: {TargetProductNodeExists}",
                    draggedProductNode is not null,
                    DraggedProduct is not null,
                    targetProductNode is not null);
                return;
            }

            if (draggedProductNode is not null)
            {
                DropDraggedProductNode(targetProductNode, e.CtrlKey);
            }

            if (DraggedProduct is not null)
            {
                DropDraggedProduct(targetProductNode);
            }

            draggedProductNode = null;
            DraggedProduct = null;

            Logger.LogInformation("Finish drop on");
        }
    }

    private void DropDraggedProduct(ProductNode targetProductNode)
    {
        Logger.LogInformation(
            "Dropping product with id {ProductId} on product with id {TargetProductID}",
            DraggedProduct.Id,
            targetProductNode.Product.Id);

        var canAddChildResult = targetProductNode.CanAddChild(DraggedProduct);

        if (canAddChildResult == CanAddChildResult.AddChildPossible)
        {
            targetProductNode.AddChild(DraggedProduct, 1);
        }
        else
        {
            EmitDropError(
                canAddChildResult: canAddChildResult,
                parent: targetProductNode.Product,
                candidateChild: DraggedProduct);
        }

        DraggedProduct = null;
    }

    private void DropDraggedProductNode(ProductNode targetProductNode, bool copyNode)
    {
        Logger.LogInformation(
            "Dropping started on {ProductNodeId}, with copying node {DoCopyNode}",
            targetProductNode.NodeId,
            copyNode);
        var canAddChildResult = targetProductNode.CanAddChild(draggedProductNode.Product);
        Logger.LogInformation(
            "Trying adding product with id {CandidateProductId} to product with id {CandidateParentProductId}, can add child: {CanAddChild}",
            draggedProductNode.Product.Id,
            targetProductNode.Product.Id,
            canAddChildResult);

        if (canAddChildResult == CanAddChildResult.AddChildPossible)
        {
            if (copyNode)
            {
                draggedProductNode.Clone(newParentNode: targetProductNode);
            }
            else
            {
                draggedProductNode.MoveToNewParent(targetProductNode);
            }
        }
        else
        {
            EmitDropError(
                canAddChildResult: canAddChildResult,
                parent: targetProductNode.Product,
                candidateChild: draggedProductNode.Product);
        }

        draggedProductNode = null;
    }

    private void EmitDropError(CanAddChildResult canAddChildResult, Product parent, Product candidateChild)
    {

        var message = canAddChildResult switch
        {
            CanAddChildResult.ChildAndParentAreSame => $"Child and parent are the same, product id: {parent.Id}",
            CanAddChildResult.ChildIsAlreadyAddedToParent => $"Child (id: {candidateChild.Id}) is already in the parent (id: {parent.Id}).",
            CanAddChildResult.ChildIsAncestor => $"Child (id: {candidateChild.Id}) is an ancestor of parent (id: {parent.Id}).",
            CanAddChildResult.AncestorPresentInDescendants => $"An ancestor of parent ({parent.Id}) is a descendant of the child (id: {candidateChild.Id}).",
            _ => string.Empty,
        };

        ProductDropFailed.InvokeAsync(message);
    }
}
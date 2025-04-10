using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using TreeDragDrop.BlazorApp.Backend.Products;

namespace TreeDragDrop.BlazorApp.Components.Pages.ProductTree;

public partial class ProductTree : ComponentBase
{
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
        if ((draggedProductNode is null && DraggedProduct is null) || targetProductNode is null)
        {
            Logger.LogInformation(
                "No Drop, product node exists: {DraggedProductNodeExists}, dragged product exists: {DraggedProductExists}, target node exists: {TargetProductNodeExists}",
                draggedProductNode is not null,
                DraggedProduct is not null,
                targetProductNode is not null);
            return;
        }

        if (draggedProductNode is not null)
        {
            Logger.LogInformation(
                "Dropping started on {ProductNodeId}, with ctrl key pressed {ShiftKeyPressed}",
                targetProductNode.NodeId,
                e.CtrlKey);
            var canAddChildResult = targetProductNode.CanAddChild(draggedProductNode.Product);
            Logger.LogInformation(
                "Trying adding product with id {CandidateProductId} to product with id {CandidateParentProductId}, can add child: {CanAddChild}",
                draggedProductNode.Product.Id,
                targetProductNode.Product.Id,
                canAddChildResult);

            if (canAddChildResult == CanAddChildResult.AddChildPossible)
            {
                if (e.CtrlKey)
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
                EmitDropError(canAddChildResult);
            }

            draggedProductNode = null;
        }


        if (DraggedProduct is not null)
        {
            Logger.LogInformation("Dropping product with id {ProductId} from product list", DraggedProduct.Id);

            var canAddChildResult = targetProductNode.CanAddChild(DraggedProduct);

            if (canAddChildResult == CanAddChildResult.AddChildPossible)
            {
                targetProductNode.AddChild(DraggedProduct, 1);
            }
            else
            {
                Logger.LogInformation(
                    "Can not add product with id {ProductId} to a node containing product with id {ToProductId}",
                    DraggedProduct.Id,
                    targetProductNode.Product.Id);
                EmitDropError(canAddChildResult);
            }

            DraggedProduct = null;
        }
    }

    private void EmitDropError(CanAddChildResult canAddChildResult)
    {
        var message = canAddChildResult switch
        {
            CanAddChildResult.ChildAndParentAreSame => "Child and parent are the same",
            CanAddChildResult.ChildIsAlreadyAddedToParent => "Child is already in the parent",
            CanAddChildResult.ChildIsAncestor => "Child is an ancestor",
            CanAddChildResult.AncestorPresentInDescendants => "Ancestor is a descendant",
            _ => string.Empty,
        };

        ProductDropFailed.InvokeAsync(message);
    }
}
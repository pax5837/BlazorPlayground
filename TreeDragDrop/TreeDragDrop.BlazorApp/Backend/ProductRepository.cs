using TreeDragDrop.BlazorApp.Backend.Products;

namespace TreeDragDrop.BlazorApp.Backend;

internal class ProductRepository
{
    public (ProductNode RootNode, HashSet<ProductNode> AllProductNodes) GetProductNodes()
    {
        var productNumber = 1;

        var overallAssemblyProduct = new Product((productNumber++).ToString(), "Component", 1);

        var baseAssemblyProduct = new Product((productNumber++).ToString(), "Base assembly", 1);
        var basePlateProduct = new Product((productNumber++).ToString(), "Base plate", 2);
        var linearAxisAssembly = new Product((productNumber++).ToString(), "Linear axis", 3);
        var motorProduct = new Product((productNumber++).ToString(), "Motor 90W", 4);
        var spindleProduct = new Product((productNumber++).ToString(), "Spindle", 5);
        var sensorProduct = new Product((productNumber++).ToString(), "Sensor", 5);
        var screwM6Product = new Product((productNumber++).ToString(), "Screw M6x30", 1);
        var screwM4Product = new Product((productNumber++).ToString(), "Screw M4x20", 1);
        var armAssembly = new Product((productNumber++).ToString(), "Arm assembly", 20);
        var armProduct = new Product((productNumber++).ToString(), "Arm", 11);
        var gripperProduct = new Product((productNumber++).ToString(), "Gripper", 4);
        var sensorAssembly = new Product((productNumber++).ToString(), "Sensor assy", 3);
        var sensorCableProduct = new Product((productNumber++).ToString(), "Sensor cable", 2);
        var screwM3Product = new Product((productNumber++).ToString(), "Screw M3x10", 1);


        var overallAssemblyNode = new ProductNode(overallAssemblyProduct, 1, null);

        var baseAssemblyNode = new ProductNode(baseAssemblyProduct, 1, overallAssemblyNode);
        var basePlateNode = new ProductNode(basePlateProduct, 1, baseAssemblyNode);
        var baseScrewsNode = new ProductNode(screwM6Product, 6, baseAssemblyNode);

        var linearAssemblyNode = new ProductNode(linearAxisAssembly, 1, overallAssemblyNode);
        var motorNode = new ProductNode(motorProduct, 1, linearAssemblyNode);
        var spindleNode = new ProductNode(spindleProduct, 1, linearAssemblyNode);
        var linearAssyScrewM4 = new ProductNode(screwM4Product, 4, linearAssemblyNode);
        var linearAssyScrewM6 = new ProductNode(screwM6Product, 4, linearAssemblyNode);


        var sensorAssemblyNode = new ProductNode(sensorAssembly, 1, linearAssemblyNode);
        var sensorProductNode = new ProductNode(sensorProduct, 1, sensorAssemblyNode);
        var sensorCableProductNode = new ProductNode(sensorCableProduct, 1, sensorAssemblyNode);
        var sensorScrewsProductNode = new ProductNode(screwM3Product, 2, sensorAssemblyNode);



        var armAssyNode = new ProductNode(armAssembly, 1, linearAssemblyNode);
        var armNode = new ProductNode(armProduct, 1, armAssyNode);
        var gripperNode = new ProductNode(gripperProduct, 1, armAssyNode);
        var armAssyScrewsNode = new ProductNode(screwM4Product, 3, armNode);

        var controlUnitAssyProduct = new Product((productNumber++).ToString(), "Control unit assy", 4);
        var controlUnitProduct = new Product((productNumber++).ToString(), "Control unit", 4);
        var controlUnitCoverProduct = new Product((productNumber++).ToString(), "Control unit cover", 4);

        var controlUnitAssyNode = new ProductNode(controlUnitAssyProduct, 1, overallAssemblyNode);
        var controlUnitNode = new ProductNode(controlUnitProduct, 1, controlUnitAssyNode);
        var controlUnitCoverNode = new ProductNode(controlUnitCoverProduct, 1, controlUnitAssyNode);
        var controlUnitScrewsNode = new ProductNode(screwM4Product, 4, controlUnitAssyNode);

        var allProductsNodes = new HashSet<ProductNode>
        {
            overallAssemblyNode,
            baseAssemblyNode,
            basePlateNode,
            baseScrewsNode,
            linearAssemblyNode,
            motorNode,
            spindleNode,
            linearAssyScrewM4,
            linearAssyScrewM6,
            sensorAssemblyNode,
            sensorProductNode,
            sensorCableProductNode,
            sensorScrewsProductNode,
            armAssyNode,
            armNode,
            gripperNode,
            armAssyScrewsNode,
            controlUnitAssyNode,
            controlUnitNode,
            controlUnitCoverNode,
            controlUnitScrewsNode,
        };

        return (overallAssemblyNode, allProductsNodes);
    }
}
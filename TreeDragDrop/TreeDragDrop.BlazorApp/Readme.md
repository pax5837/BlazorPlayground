# Data structure

````plantuml
@startuml
SkinParam {
    NodeSep 200
    
}

class Product{
    Id: string
    Name: string
    Price: decimal
    
    AddChild(ProductNode)
    RemoveChild(ProductNode)
}


class ProductNode{
    Id: Guid
    Quantity: int 
}

Product -down--> "0..*" ProductNode : Children

ProductNode -up-> "1" Product : Node
ProductNode -> "0..1" ProductNode : Parent

@enduml
````
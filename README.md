# Blazor playground

Testing things out things with Blazor

## Dynamic Update component

Scenario: a backend has parametrizable components, the frontend should know nothing about.
However these parameters need to be set on the frontend.
The frontend and backend might communicate with any given technology, in process (sync communication inside monolithic app),
web api, messaging etc...
To do this ui components supporting specific datatypes are used and populated dynamically from the information sent from the backend.
When changing the value of a parameter, the new value is sent to the backend as a kind of request,
the backend then processes the requested value (taking over or not).
The actual updated value needs to be read from the backend again.

## Tree Drag and Drop with a product structure 

Drag and drop experiments from a tree to itself and from a list to that tree,
using a product structure (structured bill of materials) as a real world example.

Uses Radzen components.

[Details](.\TreeDragDrop\TreeDragDrop.BlazorApp\Readme.md)
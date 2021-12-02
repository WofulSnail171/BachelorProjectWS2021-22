SDF Editor:

-> creating new nodes: right-click on project files -> create -> sdf...

SDF Object:
standart inputs

- circle
- rectangle
- triangle
- line
- bezier curve

SDF Function:
effects for one or more inputs 

- combine
- invert
- lerp
- smooth blend


SDF Output:
- this is the last node in the system
-> set name of the shader (if there is no shader/ hlsl file with this name, it will create a new one in assets/sdfeditor/shader)
-> set input node
-> set material or apply to create new material
-> on apply the shader will be assigned to the material



Known issues, things to avoid:
- putting a node twice into the node system 
- having an infinie loop inside a node

- more nodes to come :)
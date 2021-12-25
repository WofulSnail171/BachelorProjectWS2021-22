SDF Editor:


-> creating new nodes: right-click on project files -> create -> sdf...

SDF Object:
standard inputs (mostly self explanatory):

    - roundness factor (not on textures or circles): uses outer sdf field (can be made visible by outline repetition) 
        -> easiest to see what it does, set the roundness of a line, this will create a capsule
    - roundness factor for rectangle affects each corner individual
        

SDF Function:
effects for one or more inputs 


SDF Output:
- this is the last node in the system
-> set name of the shader (if there is no shader/ hlsl file with this name, it will create a new one in assets/sdfeditor/shader)
-> set input node
-> set material or apply to create new material
-> on apply the shader will be assigned to the material

transforms: this will transform all of your nodes
repetition: finite and infinite option (can be used to create fractals and similar)

color options: most options are self explanatory (transforms affect only textures if assigned)
    outline:
        - smoothness: smooth outlines :3 (can be used to create a cheap anti-aliasing or blur the outlines)
        - in repetition: outline repetition inside the sdf (default: white space)
        - out repetition: outline repetition outside the sdf (default: transparent space)
        - line distance: distance between outlines (if repetition isn't set too low, outlines will repeat themselves)

if you know your way around shader, there's some options on the material as well:
    - zwrite
    - ztest
    - cull
    - blend mode (https://docs.unity3d.com/Manual/SL-Blend.html)

Known issues, things to avoid:
- having an infinite loop inside a node 
  (please dont put a node in a previews nodes input, im not checking for this, so you will get an stack overflow. eg: lerp a inside combine b inside lerp a)
- sometimes, variables will stop setting their shader variables if that happens, try to check 'apply' again
  (can happen when the scene has reloaded or unity is started. If that doesnt help, create a new output or duplicate your old one)

# TypescriptSyntaxPaste
Visual Studio Extension which converts C# SYNTAX to Typescript SYNTAX 

Need Visual Studio 2015 to compile and run this extension    
Almost all converting classes are in folder Translation, which for each file containing the convert method to convert one kind of
syntax (C#) to Typescript. For example ````ClassDeclarationSyntax```` in Roslyn will be ````ClassDeclarationTranslation```` in this project.

Let say you want convert :      
in C#: ````class A {}````    
typescript: ````class myA{}````      

you just need to navigate to class ClassDeclarationTranslation in project, then change this line:    
````
 return $@"{GetAttributeList()}export class {Syntax.Identifier}{TypeParameterList?.Translate()} {baseTranslation}
              {{
                {Members.Translate()} 
                }}";
 ````    
to:    
````
 return $@"{GetAttributeList()}export class my{Syntax.Identifier}{TypeParameterList?.Translate()} {baseTranslation}
                {{
                {Members.Translate()} 
                }}";
````    

For more information about Syntax Tree in Roslyn, you can use Syntax Visualizer which is included in VS 2015 (View -> Syntax Visualizer)
